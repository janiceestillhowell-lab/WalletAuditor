using System;
using System.Collections.Generic;
using System.Linq;
using WalletAuditor.Core;

namespace WalletAuditor.Services
{
    /// <summary>
    /// Simple wallet service - manages wallet operations
    /// </summary>
    public class WalletService
    {
        private readonly List<Wallet> _wallets;

        public WalletService()
        {
            _wallets = new List<Wallet>();
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            _wallets.Add(new Wallet
            {
                Name = "Bitcoin Wallet",
                Address = "1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa",
                Balance = 0.5m,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            });

            _wallets.Add(new Wallet
            {
                Name = "Ethereum Wallet",
                Address = "0x742d35Cc6634C0532925a3b844Bc9e7595f0bEb",
                Balance = 2.75m,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            });
        }

        public List<Wallet> GetAllWallets()
        {
            return _wallets.ToList();
        }

        public Wallet? GetWalletByAddress(string address)
        {
            return _wallets.FirstOrDefault(w => w.Address.Equals(address, StringComparison.OrdinalIgnoreCase));
        }

        public bool AddWallet(Wallet wallet)
        {
            if (wallet == null || string.IsNullOrWhiteSpace(wallet.Address))
                return false;

            _wallets.Add(wallet);
            return true;
        }
    }
}
