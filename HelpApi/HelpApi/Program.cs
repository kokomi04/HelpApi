using HelpApi;
using Infrastructures.AppConfigs;

var builder = WebApplication.CreateBuilder(args);
var appSetting = AppConfigSetting.Config();
var startup = new Startup(appSetting); // My custom startup class.

startup.ConfigureServices(builder.Services); // Add services to the container.

var app = builder.Build();

startup.Configure(app, app.Environment); // Configure the HTTP request pipeline.

app.Run();