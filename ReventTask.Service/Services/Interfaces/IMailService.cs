
namespace EmailSenders.Services;
    public interface IMailService
    {
        Task<Response<string>> SendEmailAsync(EmailRequest mailRequest);
    }
