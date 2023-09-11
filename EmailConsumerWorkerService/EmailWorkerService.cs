using EmailSenders.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReventTask.Domain.Config;
using ReventTask.Domain.Entities;
using System.Text;
using ILogger = Serilog.ILogger;

namespace EmailConsumerWorkerService
{
    public class EmailWorkerService : BackgroundService
    {
        private readonly ILogger logger;
        private readonly IModel channel;
        private readonly string queueName;
        private readonly IMailService emailService;
        private readonly IConnection connection;
        private readonly IConfiguration configuration;

        public EmailWorkerService(ILogger logger,
            IMailService emailService,
            IConfiguration configuration)

        {
            this.logger = logger;
            this.emailService = emailService;
            this.configuration = configuration;

            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQSettings:HostName"],
                Port = int.Parse(configuration["RabbitMQSettings:Port"]),
                UserName = configuration["RabbitMQSettings:UserName"],
                Password = configuration["RabbitMQSettings:Password"]
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            queueName = configuration["RabbitMQSettings:QueueName"];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.Information("Consumer operation Statrted");

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);



            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    var message = GetMessageFromQueue();

                    if (message == null)
                    {
                        logger.Information("No more messages in the queue. Exiting worker.");
                        break; // Exit the loop when the queue is empty
                    }

                    logger.Information($"Received message: {message}");

        
                    var messageBody = Encoding.UTF8.GetString(message.Body.ToArray());
                    await ProcessMessageAsync(messageBody);

                    channel.BasicAck(message.DeliveryTag, false);
                }
                catch (OperationCanceledException)
                {
                    // Graceful shutdown requested
                    logger.Information("Worker shutdown requested. Exiting worker.");
                    break;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while processing messages.");
                }
            }

            logger.Information("Email Worker stopped.");
        }

        private BasicGetResult GetMessageFromQueue()
        {
            try
            {
                return channel.BasicGet(queueName, autoAck: true);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while getting a message from the queue.");
                return null;
            }
        }

        private async Task ProcessMessageAsync(string messageBody)
        {
             var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
         {
             var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
             logger.Information($"Received message: {message}");

             var emailMessage = JsonConvert.DeserializeObject<EmailRequest>(message);

             emailService.SendEmailAsync(emailMessage).GetAwaiter().GetResult();

             channel.BasicAck(eventArgs.DeliveryTag, false);
             Console.WriteLine("Finished here");
         };
            await Task.Delay(TimeSpan.FromSeconds(2));

            logger.Information($"Processed message: {messageBody}");
        }



        public override void Dispose()
        {
            connection.Close();
            channel.Close();
            base.Dispose();
        }
    }

}





