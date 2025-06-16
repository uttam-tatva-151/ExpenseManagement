namespace CashCanvas.Core.DTOs;

public class CategoryViewModel
{
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; } // FK to User
    public string CategoryName { get; set; } = null!;// Category Name
    public string? Description { get; set; }
    public string Type { get; set; } = null!;// "Income" or "Expense"

}
