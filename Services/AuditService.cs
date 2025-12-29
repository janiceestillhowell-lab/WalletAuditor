using System;
using System.Threading.Tasks;
using WalletAuditor.Core;

namespace WalletAuditor.Services
{
    /// <summary>
    /// Simple audit service - performs wallet audits
    /// </summary>
    public class AuditService
    {
        public async Task<AuditResult> AuditWalletAsync(Wallet wallet)
        {
            var result = new AuditResult();

            // Perform basic wallet audit
            if (wallet == null)
            {
                result.IsSuccessful = false;
                result.Message = "Wallet is null";
                return result;
            }

            if (string.IsNullOrWhiteSpace(wallet.Address))
            {
                result.IsSuccessful = false;
                result.Message = "Wallet address is empty";
                result.Findings.Add("Missing wallet address");
                return result;
            }

            if (wallet.Balance < 0)
            {
                result.IsSuccessful = false;
                result.Message = "Invalid balance";
                result.Findings.Add("Wallet balance cannot be negative");
                return result;
            }

            result.IsSuccessful = true;
            result.Message = $"Wallet '{wallet.Name}' audit completed successfully";
            result.Findings.Add($"Wallet address: {wallet.Address}");
            result.Findings.Add($"Balance: {wallet.Balance}");

            return await Task.FromResult(result);
        }
    }
}
