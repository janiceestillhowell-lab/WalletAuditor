using System;

namespace WalletAuditor.Core
{
    /// <summary>
    /// Represents a wallet model containing wallet information and metadata.
    /// </summary>
    public class WalletModel
    {
        /// <summary>
        /// Gets or sets the wallet address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the wallet name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the wallet balance.
        /// </summary>
        public decimal Balance { get; set; }

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
    }
}
