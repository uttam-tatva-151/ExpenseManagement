using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCanvas.Core.Entities
{
    /// <summary>
    /// Logs application errors and exception events.
    /// Each row stores error details, including the message, stack trace, type, time, status code, context, resolution status, and number of occurrences.
    /// </summary>
    [Table("ErrorLogs")]
    public class ErrorLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ErrorId { get; set; }

        [Required]
        public string ErrorMessage { get; set; } = null!;

        public string? StackTrace { get; set; }

        [Required]
        [StringLength(255)]
        public string ExceptionType { get; set; } = null!;

        [Required]
        public DateTime ErrorOccurAt { get; set; }

        [Required]
        [StringLength(20)]
        public string StatusCode { get; set; } = null!;

        [StringLength(255)]
        public string? ControllerName { get; set; }

        [StringLength(255)]
        public string? ActionName { get; set; }

        [Required]
        public bool IsSolved { get; set; } = false;

        public DateTime? SolvedAt { get; set; }

        public Guid? SolvedBy { get; set; }

        /// <summary>
        /// Counts how many times this specific error has occurred (duplicates are incremented by trigger).
        /// </summary>
        [Required]
        public int ErrorOccurCount { get; set; } = 1;
    }
}