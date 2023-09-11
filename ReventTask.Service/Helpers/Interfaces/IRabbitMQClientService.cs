
using ReventTask.Domain.DataTransferObjects.Dtos;

namespace ReventTask.Service.Helpers.Implementations
{
    public interface IRabbitMQClientService
    {
        Task<Result<bool>> PublishEvent(MailRequestDto request);
    }
}