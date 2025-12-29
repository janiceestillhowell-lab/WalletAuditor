using System;

namespace WalletAuditor.Core
{
    /// <summary>
    /// Represents a wallet data model containing financial account information.
    /// </summary>
    public class WalletModel
    {
        /// <summary>
        /// Gets or sets the wallet address identifier.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the wallet name or label.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the current balance in the wallet.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the currency type associated with the wallet.
        /// </summary>
        public string CurrencyType { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the wallet was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the wallet was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the wallet is valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the current status of the wallet.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletModel"/> class.
        /// </summary>
        public WalletModel()
        {
            CreatedDate = DateTime.UtcNow;
            LastUpdated = DateTime.UtcNow;
            IsValid = true;
            Status = "Active";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletModel"/> class with specified parameters.
        /// </summary>
        /// <param name="address">The wallet address.</param>
        /// <param name="name">The wallet name.</param>
        /// <param name="balance">The wallet balance.</param>
        /// <param name="currencyType">The currency type.</param>
        public WalletModel(string address, string name, decimal balance, string currencyType) : this()
        {
            Address = address;
            Name = name;
            Balance = balance;
            CurrencyType = currencyType;
        }
    }
}
