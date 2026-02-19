using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SecureTodo.Api.ExceptionHandlers;
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

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddOpenApi();
    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
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
    
    builder.Services
        .AddAuthorization()
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.RequireHttpsMetadata = false;
            options.Authority = builder.Configuration["Identity:Authority"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Identity:Issuer"],
                ValidAudiences = builder.Configuration["Identity:Audiences"]!.Split(",", StringSplitOptions.RemoveEmptyEntries),
                ValidateLifetime = true,
            };
            options.MapInboundClaims = true;
        });
    
    const string corsPolicyName = "CorsPolicy";
    var crossOrigins = builder.Configuration.GetSection("CrossOrigins").Get<string>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: corsPolicyName,
            policy =>
            {
                policy.WithOrigins(crossOrigins!.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });
    
    var app = builder.Build();
    app.UseExceptionHandler();
    
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference("/docs");
    }
    
    app.UseCors(corsPolicyName);

    app.UseAuthentication();
    app.UseAuthorization();
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