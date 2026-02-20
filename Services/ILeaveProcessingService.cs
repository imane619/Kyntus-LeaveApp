namespace LeaveService.Services;

public interface ILeaveProcessingService
{
    Task ValidateLeaveAsync(Guid leaveId, string employeeId, DateTime start, DateTime end);
}