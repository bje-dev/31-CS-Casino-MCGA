using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqService.Abstractions;
using RabbitMqService.RabbitMq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Receiver
{
    public class DepositosReceiver<T> : BackgroundService where T : class
    {
        private readonly MessageManager messageManager;
        private readonly MessageManagerSettings messageManagerSettings;
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
        private readonly string queueName;

        public static JsonSerializerOptions Default { get; } = new(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        public DepositosReceiver(MessageManager messageManager, MessageManagerSettings messageManagerSettings, QueueSettings settings, ILogger<DepositosReceiver<T>> logger, IServiceProvider serviceProvider)
        {
            this.messageManager = messageManager;
            this.messageManagerSettings = messageManagerSettings;
            this.logger = logger;
            this.serviceProvider = serviceProvider;

            queueName = settings?.Queues?.FirstOrDefault(q => q.Name == "Depositos").Type?.Name;

            if (queueName is null)
                queueName = settings?.QueuesString?.FirstOrDefault(q => q == "Depositos");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug($"RabbitMQ Listener for {queueName} started");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug($"RabbitMQ Listener for {queueName} stopped");

            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            //Conectarme y me quedo conectado escuchando y esperando para procesar.
            var consumer = new EventingBasicConsumer(messageManager.Channel);
            consumer.Received += async (_, message) =>
            {
                try
                {
                    logger.LogDebug($"Messaged received: {Encoding.UTF8.GetString(message.Body.Span)}");

                    using var scope = serviceProvider.CreateScope();

                    var receiver = scope.ServiceProvider.GetRequiredService<IMessageReceiver<T>>();
                    var response = JsonSerializer.Deserialize<T>(message.Body.Span, messageManagerSettings.JsonSerializerOptions ?? Default);
                    await receiver.ReceiveAsync(response, stoppingToken);

                    messageManager.MarkAsComplete(message);

                    logger.LogDebug("Message processed");
                }
                catch (Exception ex)
                {
                    messageManager.MarkAsRejected(message);
                    logger.LogError(ex, "Unexpected error while processing message");
                }

                stoppingToken.ThrowIfCancellationRequested();
            };

            messageManager.Channel.BasicConsume(queueName, autoAck: false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            messageManager.Dispose();
            base.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
