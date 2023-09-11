using EmailConsumerWorkerService;
using EmailSenders.Services;
using RYTNotificationService.API.Services.Implementation;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices(services =>
    {
        services.AddSingleton(Log.Logger);
        services.AddSingleton<IMailService, MailService>();
        services.AddHostedService<Worker>();
        services.AddHostedService<EmailWorkerService>();
    })
    .Build();
host.RunAsync();
