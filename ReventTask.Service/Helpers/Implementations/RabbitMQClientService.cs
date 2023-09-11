using RabbitMQ.Client;
using ReventTask.Domain.DataTransferObjects.Dtos;

namespace ReventTask.Service.Helpers.Implementations;

public class RabbitMQClientService : IRabbitMQClientService
{
    private readonly RabbitMQSettings rbqSettings;
    private readonly ILogger logger;
    private readonly IInboundLogRepository inbound;
    public RabbitMQClientService(IOptions<RabbitMQSettings> rbqSettings, IInboundLogRepository inbound, ILogger logger)
    {
        this.rbqSettings = rbqSettings.Value;
        this.inbound = inbound;
        this.logger = logger;
    }
    public async Task<Result<bool>> PublishEvent(MailRequestDto req)
    {
        var result = new Result<bool>();
        var factory = new ConnectionFactory
        {
            HostName = rbqSettings.HostName, // RabbitMQ server address
            Port = rbqSettings.Port, // RabbitMQ port
            UserName = rbqSettings.UserName, // RabbitMQ username
            Password = rbqSettings.Password // RabbitMQ password
        };

        var request = new EmailRequest
        {
            RecepientEmail = req.RecepientEmail,
            Attachments = req.Attachments,
            Subject = req.Subject,
            Body = req.Body
        };


        var inbLogs = new InboundLog
        {
            InboundLogId = request.EmailId,
            RequestDateTime = DateTime.Now,
            APIMethod = "Call Publisher",
            RequestDetails = request.ToString(),
            APICalled = "Email Service"
        };
        _ = await inbound.CreateInboundLog(inbLogs);
        var queueName = rbqSettings.QueueName;
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        try
        {
            channel.QueueDeclare(queue: queueName, durable: true, false, false, null);

            var message = JsonConvert.SerializeObject(request);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
            result.IsSuccess = true;
            result.Content = true;
            result.Message = "Successfully Queued";
            result.ResponseTime = DateTime.Now;

            return result;
        }

        catch (Exception ex)
        {
            result.Content = false;
            result.IsSuccess = false;
            result.Message = ex.ToString();
            return result;

        }
    }
}
