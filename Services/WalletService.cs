using System;
using System.Collections.Generic;
using System.Linq;

namespace WalletAuditor.Services
{
    /// <summary>
    /// WalletService manages all wallet operations including CRUD operations,
    /// validation, and wallet queries.
    /// </summary>
    public class WalletService
    {
        private List<Wallet> _wallets;

        public WalletService()
        {
            _wallets = new List<Wallet>();
            InitializeSampleWallets();
        }

        /// <summary>
        /// Initializes the service with sample Bitcoin and Ethereum wallets.
        /// </summary>
        private void InitializeSampleWallets()
        {
            _wallets.Add(new Wallet
            {
                Id = Guid.NewGuid(),
                Name = "Bitcoin Wallet",
                Address = "1A1z7agoat0Rm16qSrx2RWQa8NFGSLJUxQ",
                Currency = "Bitcoin",
                CurrencySymbol = "BTC",
                Balance = 0.5m,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            });

            _wallets.Add(new Wallet
            {
                Id = Guid.NewGuid(),
                Name = "Ethereum Wallet",
                Address = "0x32Be343B94f860124dC4fEe278FADBD03915C7FF",
                Currency = "Ethereum",
                CurrencySymbol = "ETH",
                Balance = 2.75m,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            });
        }

        /// <summary>
        /// Retrieves all wallets in the system.
        /// </summary>
        /// <returns>List of all wallets</returns>
        public List<Wallet> GetAllWallets()
        {
            return _wallets.ToList();
        }

        /// <summary>
        /// Retrieves a wallet by its address.
        /// </summary>
        /// <param name="address">The wallet address to search for</param>
        /// <returns>Wallet if found, null otherwise</returns>
        public Wallet GetWalletByAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return null;

            return _wallets.FirstOrDefault(w => w.Address.Equals(address, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves a wallet by its name.
        /// </summary>
        /// <param name="name">The wallet name to search for</param>
        /// <returns>Wallet if found, null otherwise</returns>
        public Wallet GetWalletByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return _wallets.FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Adds a new wallet to the system.
        /// </summary>
        /// <param name="wallet">The wallet to add</param>
        /// <returns>True if wallet was added successfully, false otherwise</returns>
        public bool AddWallet(Wallet wallet)
        {
            if (wallet == null)
                return false;

            if (!ValidateWalletAddress(wallet.Address))
                return false;

            if (GetWalletByAddress(wallet.Address) != null)
                return false; // Wallet with this address already exists

            wallet.Id = Guid.NewGuid();
            wallet.CreatedDate = DateTime.UtcNow;
            _wallets.Add(wallet);
            return true;
        }

        /// <summary>
        /// Updates an existing wallet.
        /// </summary>
        /// <param name="wallet">The wallet with updated information</param>
        /// <returns>True if wallet was updated successfully, false otherwise</returns>
        public bool UpdateWallet(Wallet wallet)
        {
            if (wallet == null || wallet.Id == Guid.Empty)
                return false;

            var existingWallet = _wallets.FirstOrDefault(w => w.Id == wallet.Id);
            if (existingWallet == null)
                return false;

            existingWallet.Name = wallet.Name;
            existingWallet.Balance = wallet.Balance;
            existingWallet.IsActive = wallet.IsActive;
            existingWallet.LastModifiedDate = DateTime.UtcNow;

            return true;
        }

        /// <summary>
        /// Deletes a wallet from the system.
        /// </summary>
        /// <param name="walletId">The ID of the wallet to delete</param>
        /// <returns>True if wallet was deleted successfully, false otherwise</returns>
        public bool DeleteWallet(Guid walletId)
        {
            if (walletId == Guid.Empty)
                return false;

            var wallet = _wallets.FirstOrDefault(w => w.Id == walletId);
            if (wallet == null)
                return false;

            return _wallets.Remove(wallet);
        }

        /// <summary>
        /// Validates a wallet address format (basic validation for common cryptocurrencies).
        /// </summary>
        /// <param name="address">The address to validate</param>
        /// <returns>True if address is valid, false otherwise</returns>
        public bool ValidateWalletAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;

            // Basic validation: address should be between 26 and 42 characters
            // and contain only alphanumeric characters
            if (address.Length < 26 || address.Length > 42)
                return false;

            if (!address.All(c => char.IsLetterOrDigit(c)))
                return false;

            return true;
        }

        /// <summary>
        /// Retrieves all wallets for a specific currency.
        /// </summary>
        /// <param name="currency">The currency to filter by</param>
        /// <returns>List of wallets matching the currency</returns>
        public List<Wallet> GetWalletsByCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                return new List<Wallet>();

            return _wallets
                .Where(w => w.Currency.Equals(currency, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Calculates the total balance across all wallets.
        /// </summary>
        /// <returns>Total balance across all wallets</returns>
        public decimal GetTotalBalance()
        {
            return _wallets.Sum(w => w.Balance);
        }

        /// <summary>
        /// Gets the total count of wallets in the system.
        /// </summary>
        /// <returns>Number of wallets</returns>
        public int GetWalletCount()
        {
            return _wallets.Count;
        }
    }

    /// <summary>
    /// Represents a cryptocurrency wallet.
    /// </summary>
    public class Wallet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
