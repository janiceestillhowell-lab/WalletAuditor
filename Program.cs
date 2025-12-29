using System;
using System.Linq;
using WalletAuditor.Services;

namespace WalletAuditor
{
    /// <summary>
    /// Main entry point for the WalletAuditor console application.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("    Wallet Auditor - Console Edition");
            Console.WriteLine("===========================================");
            Console.WriteLine();

            try
            {
                // Initialize services
                var walletService = new WalletService();
                var auditService = new AuditService();

                // Display all wallets
                Console.WriteLine("Loading wallets...");
                var wallets = walletService.GetAllWallets();
                
                Console.WriteLine($"\nFound {wallets.Count} wallet(s):");
                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"{"Name",-20} {"Address",-35} {"Currency",-10} {"Balance",12}");
                Console.WriteLine(new string('-', 80));

                foreach (var wallet in wallets)
                {
                    Console.WriteLine($"{wallet.Name,-20} {wallet.Address,-35} {wallet.Currency,-10} {wallet.Balance,12:F8}");
                }

                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"Total Balance: {walletService.GetTotalBalance():F8}");
                Console.WriteLine();

                // Perform audit on each wallet
                Console.WriteLine("Performing wallet audits...");
                Console.WriteLine();

                foreach (var wallet in wallets)
                {
                    var walletData = new WalletData
                    {
                        WalletId = wallet.Name,
                        Balance = wallet.Balance,
                        CreatedDate = wallet.CreatedDate,
                        Transactions = new System.Collections.Generic.List<Transaction>()
                    };

                    var auditResult = auditService.PerformWalletAuditAsync(wallet.Name, walletData).Result;
                    
                    Console.WriteLine($"Audit for {wallet.Name}:");
                    Console.WriteLine($"  Status: {auditResult.Status}");
                    Console.WriteLine($"  Valid: {auditResult.IsValid}");
                    Console.WriteLine($"  Errors: {auditResult.Errors.Count}");
                    Console.WriteLine($"  Warnings: {auditResult.Warnings.Count}");
                    
                    if (auditResult.Errors.Any())
                    {
                        Console.WriteLine("  Error Details:");
                        foreach (var error in auditResult.Errors)
                        {
                            Console.WriteLine($"    - {error}");
                        }
                    }
                    
                    if (auditResult.Warnings.Any())
                    {
                        Console.WriteLine("  Warnings:");
                        foreach (var warning in auditResult.Warnings)
                        {
                            Console.WriteLine($"    - {warning}");
                        }
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("===========================================");
                Console.WriteLine("Audit completed successfully!");
                Console.WriteLine("===========================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Environment.Exit(1);
            }

            if (args.Contains("--wait") || args.Contains("-w"))
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
