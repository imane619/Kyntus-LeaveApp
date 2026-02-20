namespace LeaveService.Models;


public class LeaveRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string EmployeeId { get; set; } = string.Empty;

    public string CellId { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public string Type { get; set; } = string.Empty;

    // Le compilateur ira chercher LeaveStatus dans le fichier LeaveStatus.cs automatiquement
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending; 
    
    public string? Justification { get; set; }
    public string? AttachmentUrl { get; set; } 
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}