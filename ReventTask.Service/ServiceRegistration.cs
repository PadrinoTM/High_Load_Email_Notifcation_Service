using Polly;
using Polly.Contrib.WaitAndRetry;
using ReventTask.Service.Helpers.Implementations;
using RYTNotificationService.API.Services.Implementation;

namespace ReventTask.Core.Services;
public static class ServiceRegistration
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IRabbitMQClientService, RabbitMQClientService>();
        services.AddAutoMapper(typeof(ServiceRegistration));
        services.AddControllersWithViews();
        services.AddTransient<ISoapHelper, SoapHelper>();
        services.AddTransient<IRestHelper, RestHelper>();
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

        // Add IOptions to the DI container
        services.AddScoped(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);
        services.AddScoped(sp => sp.GetRequiredService<IOptions<MailSettings>>().Value);

        services.AddHttpClient("Client", client =>
        {
            client.Timeout = new TimeSpan(0, 0, 60);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }).AddTransientHttpErrorPolicy(x => x.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(configuration.GetValue<double>("PollyConfig:RetryTime")), retryCount: configuration.GetValue<int>("PollyConfig:RetryCount"))))
            .AddTransientHttpErrorPolicy(x => x.CircuitBreakerAsync(configuration.GetValue<int>("PollyConfig:HandledEventsAllowedBeforeBreaking"), TimeSpan.FromSeconds(configuration.GetValue<double>("PollyConfig:breakerTime"))));

        return services;

    }

}
