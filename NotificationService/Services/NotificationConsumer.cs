using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService.Services;

public class NotificationConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "leave_notifications", durable: false, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            // Ici, on simule l'envoi d'un mail ou d'une notification
            Console.WriteLine($" [x] Notification reçue : {message}");
            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queue: "leave_notifications", autoAck: true, consumer: consumer);

        // Garde le service en vie
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}