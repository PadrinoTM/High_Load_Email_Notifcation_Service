using FluentValidation.AspNetCore;
using ReventTask.Domain.DataTransferObjects.DtoValidators;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, config) =>
{
    config.Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration);

});

builder.Services.AddControllers();
builder.Services.AddControllers().AddFluentValidation( fv => {

    fv.RegisterValidatorsFromAssembly((Assembly.GetAssembly(typeof(MailRequestDtoValidator))));
    }); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Email Notification Service", Version = "v1" });
        }
);


builder.Services.AddHealthChecks()
   .AddUrlGroup(new Uri
            ("https://www.google.com"),
             name: "Google",
             failureStatus: HealthStatus.Degraded)
;

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));


builder.Services.AddDataDependencies(builder.Configuration);
builder.Services.AddServiceDependencies(builder.Configuration);

builder.Services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
                //x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");  
            });

builder.Services.AddHealthChecksUI(opt =>
{
    opt.SetEvaluationTimeInSeconds(builder.Configuration.GetValue<int>("HealthCheckConfig:EvaluationTimeInSeconds")); //time in seconds between check    
    opt.MaximumHistoryEntriesPerEndpoint(builder.Configuration.GetValue<int>("HealthCheckConfig:MaxHistoryPerEndpoint")); //maximum history of checks    
    opt.SetApiMaxActiveRequests(builder.Configuration.GetValue<int>("HealthCheckConfig:ApiMaxActiveRequest")); //api requests concurrency    
    opt.AddHealthCheckEndpoint("default api", builder.Configuration.GetValue<string>("HealthCheckConfig:HealthCheckEndpoint")); //map health check api    
})
.AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseCors("corsapp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecksUI();

app.MapHealthChecks(builder.Configuration.GetValue<string>("HealthCheckConfig:HealthCheckEndpoint"), new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

