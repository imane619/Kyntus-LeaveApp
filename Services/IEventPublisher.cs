namespace LeaveService.Services;

public interface IEventPublisher
{
    Task PublishLeaveValidated(Guid leaveId, string employeeId);
}