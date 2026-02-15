using System.Text.Json.Serialization;
using SecureTodo.Api.Extensions;
using SecureTodo.Api.Services;
using SecureTodo.Application.Extensions;
using SecureTodo.Application.Services;
using SecureTodo.Infrastructure.Extensions;
using Serilog;

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog((context, configuration) => 
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddOpenApi();
    builder.Services.AddHttpContextAccessor();

    // Cross cutting services.
    builder.Services.AddTransient<IAuthService, AuthService>();

    builder.Services.AddMediator(options => 
        options.ServiceLifetime = ServiceLifetime.Scoped);
    
    builder.Services.AddApplicationServices();
    
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddRepositories();
    
    builder.Services.AddEndpoints(typeof(Program).Assembly);
    
    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseEndpoints();

    app.Run();

}
catch (Exception ex) when (ex is not HostAbortedException)
{
    logger.Fatal(ex, "SecureTodo.Api terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}

namespace SecureTodo.Api
{
    public partial class Program; // Handler for testing purposes
}