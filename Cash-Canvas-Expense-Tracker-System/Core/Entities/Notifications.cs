using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Core.Entities;

public class Notifications
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [StringLength(32)]
    public NotificationType Type { get; set; } = NotificationType.Other; // e.g. "reminder", "milestone", "other"

    [Required]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Required]
    public string Message { get; set; } = null!;

    [Required]
    public bool IsRead { get; set; } = false;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ModifyAt { get; set; }

    // For extensibility, any additional data
    [Column(TypeName = "jsonb")]
    public string Meta { get; set; } = string.Empty; // store as JSON string

    [Required]
    public bool IsDeleted { get; set; } = false;
}

