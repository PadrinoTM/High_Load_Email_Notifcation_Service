using ReventTask.Domain.DataTransferObjects.Dtos;
using ReventTask.Service.Helpers.Implementations;

namespace ReventTask.API.Controllers.v1
{
    public partial class EmailEventController : BaseController
    {
        private readonly IRabbitMQClientService publishService;

        public EmailEventController(IRabbitMQClientService publishService)
        {
            this.publishService = publishService;
        }
        [HttpPost("EmailRequest")]
        public async Task<IActionResult> GetEmailRequest([FromForm] MailRequestDto request)
        {
            _ = new Result<bool>
            {
                RequestTime = DateTime.UtcNow
            };
            var response = await publishService.PublishEvent(request);
            Result<bool>? result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }
    }
}
