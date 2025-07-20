using NLog;
using NLog.Web;
using Savio.Core;
using User;
using User.Contract;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config")
    .GetCurrentClassLogger();

try
{
    logger.Info("Starting up the app...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();

    builder.Services.AddSingleton<IUserRepository>(_ =>
    {
        var config = GetConfig.Configuration;
        return new UserRepository(config);
    });

    builder.Services.AddSingleton<IUserService, UserService>();

    var app = builder.Build();

    app.UseCors(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());

    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application stopped due to an exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
