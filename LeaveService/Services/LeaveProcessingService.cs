namespace LeaveService.Services;
public class LeaveProcessingService : ILeaveProcessingService {
    private readonly IEventPublisher _publisher;
    public LeaveProcessingService(IEventPublisher publisher) => _publisher = publisher;

    public async Task ValidateLeaveAsync(Guid leaveId, string employeeId, DateTime start, DateTime end) 
        => await _publisher.PublishLeaveValidated(leaveId, employeeId);
}