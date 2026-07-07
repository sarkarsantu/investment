using InvestmentResearch;
using InvestmentResearch.Model;
using InvestmentResearch.Repository;
using InvestmentResearch.Service;
using System.Text.Json;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

// Add configuration from appsettings.json
builder.Configuration.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

// Register dependencies
builder.Services.AddOptions<AppSettings>()
    .BindConfiguration(AppSettings.SectionName);

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);
builder.Services.AddSingleton<IDailyEntry, DailyEntry>();
builder.Services.AddSingleton<IGitHubHelper, GitHubHelper>();
builder.Services.AddSingleton<IFileHelper, FileHelper>();
builder.Services.AddSingleton<IPipelineService, PipelineService>();
builder.Services.AddSingleton<IAICall, AICall>();

builder.Services.AddHostedService<Worker>();
var host = builder.Build();

host.Run();