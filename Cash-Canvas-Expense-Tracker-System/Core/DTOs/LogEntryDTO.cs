namespace CashCanvas.Core.DTOs;

public class LogEntryDTO
{
    public string TIMESTAMP { get; set; } = DateTime.UtcNow.ToString("o");
    public string USERNAME { get; set; } = string.Empty;
    public string CONTROLLER { get; set; } = string.Empty;
    public string ACTION { get; set; } = string.Empty;
    public string METHOD { get; set; } = string.Empty;
    public string PATH { get; set; } = string.Empty;
    public int STATUS { get; set; }
    public double DURATION_MS { get; set; }
    public string SUMMARY { get; set; } = string.Empty;
}
