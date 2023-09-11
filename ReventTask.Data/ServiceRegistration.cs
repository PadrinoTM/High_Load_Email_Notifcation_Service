namespace ReventTask.Core.Data;
public static class ServiceRegistration
{
    public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(configuration["MongoDbSettings:ConnectionString"]));

        services.AddSingleton<IMongoDbConfig, MongoDbConfig>(
            sp => new MongoDbConfig(configuration.GetSection("MongoDbSettings:ConnectionString").Value,
        configuration.GetSection("MongoDbSettings:DatabaseName").Value));

        services.AddScoped<IReventDBContext, ReventDBContext>();


        services.AddTransient<IInboundLogRepository, InboundLogRepository>();


        return services;

    }

}
