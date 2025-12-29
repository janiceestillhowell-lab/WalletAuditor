using System;
using System.Collections.Generic;

namespace WalletAuditor.Core
{
    /// <summary>
    /// Represents the result of a wallet audit operation.
    /// </summary>
    public class AuditResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the audit was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the audit timestamp.
        /// </summary>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// Gets or sets the message describing the audit result.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error details if the audit failed.
        /// </summary>
        public string ErrorDetails { get; set; }

        /// <summary>
        /// Gets or sets the list of audit findings.
        /// </summary>
        public List<string> Findings { get; set; }

        /// <summary>
        /// Initializes a new instance of the AuditResult class.
        /// </summary>
        public AuditResult()
        {
            Findings = new List<string>();
            AuditTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the AuditResult class with success status and message.
        /// </summary>
        /// <param name="isSuccessful">Whether the audit was successful.</param>
        /// <param name="message">The result message.</param>
        public AuditResult(bool isSuccessful, string message)
            : this()
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }

        /// <summary>
        /// Creates a successful audit result.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <returns>A successful AuditResult instance.</returns>
        public static AuditResult Success(string message)
        {
            return new AuditResult(true, message);
        }

        /// <summary>
        /// Creates a failed audit result.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <param name="errorDetails">Additional error details.</param>
        /// <returns>A failed AuditResult instance.</returns>
        public static AuditResult Failure(string message, string errorDetails = null)
        {
            return new AuditResult(false, message)
            {
                ErrorDetails = errorDetails
            };
        }

        /// <summary>
        /// Adds a finding to the audit result.
        /// </summary>
        /// <param name="finding">The finding to add.</param>
        public void AddFinding(string finding)
        {
            if (!string.IsNullOrWhiteSpace(finding))
            {
                Findings.Add(finding);
            }
        }
    }
}
