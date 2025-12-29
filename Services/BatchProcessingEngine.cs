using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WalletAuditor.Services
{
    /// <summary>
    /// BatchProcessingEngine provides efficient batch processing with smart threading
    /// for handling large datasets (10K+ items) with optimal performance.
    /// </summary>
    public class BatchProcessingEngine<T> where T : class
    {
        private readonly int _batchSize;
        private readonly int _maxDegreeOfParallelism;
        private readonly int _timeoutMs;
        private CancellationTokenSource _cancellationTokenSource;

        public event EventHandler<BatchProgressEventArgs> OnProgress;
        public event EventHandler<BatchErrorEventArgs> OnError;
        public event EventHandler<BatchCompletedEventArgs> OnCompleted;

        public BatchProcessingEngine(int batchSize = 100, int maxThreads = 0, int timeoutMs = 30000)
        {
            _batchSize = batchSize > 0 ? batchSize : 100;
            _maxDegreeOfParallelism = maxThreads > 0 ? maxThreads : GetOptimalThreadCount();
            _timeoutMs = timeoutMs > 0 ? timeoutMs : 30000;
        }

        /// <summary>
        /// Determines optimal thread count based on system resources
        /// </summary>
        private int GetOptimalThreadCount()
        {
            int processorCount = Environment.ProcessorCount;
            // Use 75% of available cores, minimum 2, maximum of core count
            return Math.Max(2, (int)(processorCount * 0.75));
        }

        /// <summary>
        /// Processes a collection of items in batches with smart threading
        /// </summary>
        public async Task<BatchProcessingResult<T>> ProcessAsync(
            IEnumerable<T> items,
            Func<T, CancellationToken, Task> processItemAsync,
            int? customBatchSize = null,
            CancellationToken externalToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new BatchProcessingResult<T>();
            var batchSize = customBatchSize ?? _batchSize;
            var itemsList = items.ToList();

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            var token = _cancellationTokenSource.Token;

            try
            {
                var batches = itemsList
                    .Select((item, index) => new { item, index })
                    .GroupBy(x => x.index / batchSize)
                    .Select(g => g.Select(x => x.item).ToList())
                    .ToList();

                result.TotalItems = itemsList.Count;
                result.TotalBatches = batches.Count;

                int processedCount = 0;
                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = _maxDegreeOfParallelism,
                    CancellationToken = token
                };

                await Parallel.ForEachAsync(
                    batches,
                    parallelOptions,
                    async (batch, ct) =>
                    {
                        try
                        {
                            await ProcessBatchAsync(batch, processItemAsync, result, ct);
                            
                            Interlocked.Add(ref processedCount, batch.Count);
                            RaiseProgress(result, processedCount);
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex)
                        {
                            RaiseError(new BatchErrorEventArgs { Exception = ex, ItemsProcessed = processedCount });
                            Interlocked.Increment(ref result.FailedItems);
                        }
                    });

                result.ProcessedItems = processedCount;
                result.IsSuccess = result.FailedItems == 0;
            }
            catch (OperationCanceledException)
            {
                result.IsCancelled = true;
                result.IsSuccess = false;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex;
                RaiseError(new BatchErrorEventArgs { Exception = ex });
            }
            finally
            {
                stopwatch.Stop();
                result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                result.ItemsPerSecond = result.ProcessedItems > 0 
                    ? Math.Round((result.ProcessedItems * 1000.0) / stopwatch.ElapsedMilliseconds, 2)
                    : 0;

                _cancellationTokenSource?.Dispose();
                RaiseCompleted(result);
            }

            return result;
        }

        /// <summary>
        /// Processes a batch with timeout protection
        /// </summary>
        private async Task ProcessBatchAsync(
            List<T> batch,
            Func<T, CancellationToken, Task> processItemAsync,
            BatchProcessingResult<T> result,
            CancellationToken cancellationToken)
        {
            var batchTasks = batch.Select(item =>
                ProcessItemWithTimeoutAsync(item, processItemAsync, result, cancellationToken)
            ).ToList();

            await Task.WhenAll(batchTasks);
        }

        /// <summary>
        /// Processes individual item with timeout
        /// </summary>
        private async Task ProcessItemWithTimeoutAsync(
            T item,
            Func<T, CancellationToken, Task> processItemAsync,
            BatchProcessingResult<T> result,
            CancellationToken cancellationToken)
        {
            using (var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                timeoutCts.CancelAfter(_timeoutMs);

                try
                {
                    await processItemAsync(item, timeoutCts.Token);
                    Interlocked.Increment(ref result.SuccessfulItems);
                }
                catch (OperationCanceledException)
                {
                    Interlocked.Increment(ref result.TimeoutItems);
                    throw;
                }
                catch (Exception)
                {
                    Interlocked.Increment(ref result.FailedItems);
                    throw;
                }
            }
        }

        /// <summary>
        /// Cancels ongoing batch processing
        /// </summary>
        public void Cancel()
        {
            _cancellationTokenSource?.Cancel();
        }

        private void RaiseProgress(BatchProcessingResult<T> result, int processed)
        {
            OnProgress?.Invoke(this, new BatchProgressEventArgs
            {
                ProcessedItems = processed,
                TotalItems = result.TotalItems,
                ProgressPercentage = result.TotalItems > 0 ? (processed * 100.0) / result.TotalItems : 0
            });
        }

        private void RaiseError(BatchErrorEventArgs args)
        {
            OnError?.Invoke(this, args);
        }

        private void RaiseCompleted(BatchProcessingResult<T> result)
        {
            OnCompleted?.Invoke(this, new BatchCompletedEventArgs
            {
                TotalItems = result.TotalItems,
                SuccessfulItems = result.SuccessfulItems,
                FailedItems = result.FailedItems,
                TimeoutItems = result.TimeoutItems,
                ElapsedMilliseconds = result.ElapsedMilliseconds,
                ItemsPerSecond = result.ItemsPerSecond,
                IsSuccess = result.IsSuccess,
                IsCancelled = result.IsCancelled
            });
        }
    }

    /// <summary>
    /// Result of batch processing operation
    /// </summary>
    public class BatchProcessingResult<T> where T : class
    {
        public int TotalItems { get; set; }
        public int TotalBatches { get; set; }
        public int ProcessedItems { get; set; }
        public int SuccessfulItems { get; set; }
        public int FailedItems { get; set; }
        public int TimeoutItems { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public double ItemsPerSecond { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsCancelled { get; set; }
        public Exception Exception { get; set; }

        public override string ToString()
        {
            return $"Total: {TotalItems}, Processed: {ProcessedItems}, " +
                   $"Successful: {SuccessfulItems}, Failed: {FailedItems}, " +
                   $"Timeouts: {TimeoutItems}, Speed: {ItemsPerSecond} items/sec, " +
                   $"Duration: {ElapsedMilliseconds}ms";
        }
    }

    /// <summary>
    /// Progress event arguments
    /// </summary>
    public class BatchProgressEventArgs : EventArgs
    {
        public int ProcessedItems { get; set; }
        public int TotalItems { get; set; }
        public double ProgressPercentage { get; set; }
    }

    /// <summary>
    /// Error event arguments
    /// </summary>
    public class BatchErrorEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public int ItemsProcessed { get; set; }
    }

    /// <summary>
    /// Completion event arguments
    /// </summary>
    public class BatchCompletedEventArgs : EventArgs
    {
        public int TotalItems { get; set; }
        public int SuccessfulItems { get; set; }
        public int FailedItems { get; set; }
        public int TimeoutItems { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public double ItemsPerSecond { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsCancelled { get; set; }
    }
}
