using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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

    // Setup logging
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Configure Kestrel for NetTcpBinding
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5000, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        });

        options.ListenAnyIP(1001, listenOptions =>
        {
            // This is sufficient — do NOT call UseCoreWCFNetTCP
            listenOptions.Protocols = HttpProtocols.Http2;
        });
    });


    builder.Services.AddControllers();

    // Add CoreWCF services
    builder.Services.AddServiceModelServices();
    builder.Services.AddServiceModelMetadata();
    
    builder.Services.AddSingleton<IUserRepository>(sp =>
    {
        var config = GetConfig.Configuration;
        return new UserRepository(config);
    });
    builder.Services.AddSingleton<IUserService, UserService>();

    var app = builder.Build();

    app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    app.UseAuthorization();
    app.MapControllers();

    // CoreWCF service mapping
    app.UseServiceModel(serviceBuilder =>
    {
        serviceBuilder.AddService<UserService>();

        var binding = new NetTcpBinding(SecurityMode.None);
        serviceBuilder.AddServiceEndpoint<UserService, IUserService>(binding, "/UserService");

        var smb = app.Services.GetRequiredService<ServiceMetadataBehavior>();
        smb.HttpGetEnabled = false;
    });

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
