using System;
using System.Collections.Generic;

namespace WalletAuditor.Core
{
    /// <summary>
    /// Simple audit result model
    /// </summary>
    public class AuditResult
    {
        public bool IsSuccessful { get; set; }
        public DateTime AuditTime { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Findings { get; set; }

        public AuditResult()
        {
            Findings = new List<string>();
            AuditTime = DateTime.UtcNow;
        }
    }
}
