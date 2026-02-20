using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace LeaveService.Services;

public class RabbitMqEventPublisher : IEventPublisher
{
    public async Task PublishLeaveValidated(Guid leaveId, string employeeId)
    {
        // 1. Configuration de la connexion
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        // 2. Déclaration de la file (Queue)
        await channel.QueueDeclareAsync(queue: "leave_notifications", 
                                      durable: false, 
                                      exclusive: false, 
                                      autoDelete: false);

        // 3. Préparation du message
        var messageData = new { LeaveId = leaveId, EmployeeId = employeeId, Status = "Validated" };
        var message = JsonSerializer.Serialize(messageData);
        var body = Encoding.UTF8.GetBytes(message);

        // 4. Envoi du message
        await channel.BasicPublishAsync(exchange: string.Empty, 
                                      routingKey: "leave_notifications", 
                                      body: body);

        Console.WriteLine($" [x] Message envoyé pour l'employé {employeeId}");
    }
}  