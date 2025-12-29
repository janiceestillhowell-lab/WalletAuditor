using System;

namespace WalletAuditor.Core
{
    /// <summary>
    /// Simple wallet data model
    /// </summary>
    public class Wallet
    {
        public string Address { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
