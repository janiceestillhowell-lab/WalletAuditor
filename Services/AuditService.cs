using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletAuditor.Services
{
    /// <summary>
    /// Service for performing wallet audits, validating wallet data, and logging results
    /// </summary>
    public class AuditService
    {
        private readonly List<AuditLog> _auditLogs;

        public AuditService()
        {
            _auditLogs = new List<AuditLog>();
        }

        /// <summary>
        /// Performs a comprehensive audit on wallet data
        /// </summary>
        /// <param name="walletId">The wallet identifier</param>
        /// <param name="walletData">The wallet data to audit</param>
        /// <returns>AuditResult containing the audit findings</returns>
        public async Task<AuditResult> PerformWalletAuditAsync(string walletId, WalletData walletData)
        {
            var auditResult = new AuditResult
            {
                WalletId = walletId,
                AuditStartTime = DateTime.UtcNow,
                IsValid = true,
                Errors = new List<string>(),
                Warnings = new List<string>()
            };

            try
            {
                // Validate wallet data
                ValidateWalletData(walletData, auditResult);

                // Check for data consistency
                CheckDataConsistency(walletData, auditResult);

                // Verify transaction integrity
                VerifyTransactionIntegrity(walletData, auditResult);

                auditResult.AuditEndTime = DateTime.UtcNow;
                auditResult.Status = auditResult.IsValid ? AuditStatus.Passed : AuditStatus.Failed;

                // Log the audit result
                LogAuditResult(auditResult);

                return auditResult;
            }
            catch (Exception ex)
            {
                auditResult.IsValid = false;
                auditResult.Status = AuditStatus.Error;
                auditResult.Errors.Add($"Audit process failed: {ex.Message}");
                LogAuditResult(auditResult);
                throw;
            }
        }

        /// <summary>
        /// Validates wallet data for required fields and format
        /// </summary>
        private void ValidateWalletData(WalletData walletData, AuditResult auditResult)
        {
            if (walletData == null)
            {
                auditResult.IsValid = false;
                auditResult.Errors.Add("Wallet data is null");
                return;
            }

            if (string.IsNullOrWhiteSpace(walletData.WalletId))
            {
                auditResult.IsValid = false;
                auditResult.Errors.Add("Wallet ID is missing or empty");
            }

            if (walletData.Balance < 0)
            {
                auditResult.IsValid = false;
                auditResult.Errors.Add("Wallet balance cannot be negative");
            }

            if (walletData.CreatedDate > DateTime.UtcNow)
            {
                auditResult.IsValid = false;
                auditResult.Errors.Add("Wallet creation date is in the future");
            }

            if (walletData.Transactions == null)
            {
                auditResult.Warnings.Add("Wallet transactions list is null");
            }
        }

        /// <summary>
        /// Checks for data consistency in wallet information
        /// </summary>
        private void CheckDataConsistency(WalletData walletData, AuditResult auditResult)
        {
            if (walletData?.Transactions == null || walletData.Transactions.Count == 0)
                return;

            // Verify transaction sum matches balance
            decimal transactionSum = walletData.Transactions.Sum(t => t.Amount);
            if (Math.Abs(transactionSum - walletData.Balance) > 0.01m)
            {
                auditResult.Warnings.Add($"Balance mismatch: Expected {transactionSum}, but found {walletData.Balance}");
            }

            // Check for duplicate transactions
            var duplicateTransactions = walletData.Transactions
                .GroupBy(t => t.TransactionId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateTransactions.Any())
            {
                auditResult.IsValid = false;
                auditResult.Errors.Add($"Found {duplicateTransactions.Count} duplicate transaction IDs");
            }
        }

        /// <summary>
        /// Verifies the integrity of wallet transactions
        /// </summary>
        private void VerifyTransactionIntegrity(WalletData walletData, AuditResult auditResult)
        {
            if (walletData?.Transactions == null || walletData.Transactions.Count == 0)
                return;

            foreach (var transaction in walletData.Transactions)
            {
                if (string.IsNullOrWhiteSpace(transaction.TransactionId))
                {
                    auditResult.IsValid = false;
                    auditResult.Errors.Add("Transaction with missing ID found");
                }

                if (transaction.Amount == 0)
                {
                    auditResult.Warnings.Add($"Zero-amount transaction detected: {transaction.TransactionId}");
                }

                if (transaction.Timestamp > DateTime.UtcNow)
                {
                    auditResult.IsValid = false;
                    auditResult.Errors.Add($"Future-dated transaction found: {transaction.TransactionId}");
                }
            }
        }

        /// <summary>
        /// Logs the audit result
        /// </summary>
        private void LogAuditResult(AuditResult auditResult)
        {
            var auditLog = new AuditLog
            {
                LogId = Guid.NewGuid().ToString(),
                WalletId = auditResult.WalletId,
                AuditStatus = auditResult.Status,
                ErrorCount = auditResult.Errors.Count,
                WarningCount = auditResult.Warnings.Count,
                Timestamp = DateTime.UtcNow,
                DurationMs = (auditResult.AuditEndTime - auditResult.AuditStartTime).TotalMilliseconds
            };

            _auditLogs.Add(auditLog);
        }

        /// <summary>
        /// Retrieves all audit logs
        /// </summary>
        public IReadOnlyList<AuditLog> GetAuditLogs() => _auditLogs.AsReadOnly();

        /// <summary>
        /// Clears all audit logs
        /// </summary>
        public void ClearAuditLogs() => _auditLogs.Clear();
    }

    /// <summary>
    /// Represents the result of a wallet audit
    /// </summary>
    public class AuditResult
    {
        public string WalletId { get; set; }
        public DateTime AuditStartTime { get; set; }
        public DateTime AuditEndTime { get; set; }
        public bool IsValid { get; set; }
        public AuditStatus Status { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"Audit for Wallet {WalletId}: Status={Status}, Valid={IsValid}, " +
                   $"Errors={Errors.Count}, Warnings={Warnings.Count}";
        }
    }

    /// <summary>
    /// Represents an audit log entry
    /// </summary>
    public class AuditLog
    {
        public string LogId { get; set; }
        public string WalletId { get; set; }
        public AuditStatus AuditStatus { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public DateTime Timestamp { get; set; }
        public double DurationMs { get; set; }

        public override string ToString()
        {
            return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] WalletId={WalletId}, Status={AuditStatus}, " +
                   $"Errors={ErrorCount}, Warnings={WarningCount}, Duration={DurationMs}ms";
        }
    }

    /// <summary>
    /// Enumeration for audit status
    /// </summary>
    public enum AuditStatus
    {
        Pending,
        Passed,
        Failed,
        Error
    }

    /// <summary>
    /// Represents wallet data structure for auditing
    /// </summary>
    public class WalletData
    {
        public string WalletId { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    /// <summary>
    /// Represents a transaction in a wallet
    /// </summary>
    public class Transaction
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } // e.g., "Credit", "Debit"
        public string Description { get; set; }
    }
}
