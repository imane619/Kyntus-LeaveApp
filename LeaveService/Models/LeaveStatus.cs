namespace LeaveService.Models;

public enum LeaveStatus
{
    Pending = 0,    // Par défaut : en attente (Employé vient de créer la demande)
    Approved = 1,   // Validée 
    Rejected = 2,   // Refusée 
    Canceled = 3    // Annulée 
}