using InvestmentResearch;
using InvestmentResearch.Data;
using InvestmentResearch.Model;
using InvestmentResearch.Repository;
using InvestmentResearch.Service;
using InvestmentResearch.Service.Upstox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

// Add configuration from appsettings.json
builder.Configuration.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContextFactory<RssFeedDbContext>(options =>
    options.UseSqlite("Data Source=rssfeeds.db"));

// Register dependencies
builder.Services.AddOptions<AppSettings>()
    .BindConfiguration(AppSettings.SectionName);
builder.Services.AddSingleton<DatabaseInitializationService>();

builder.Services.AddSingleton<IRssService, RssService>();
builder.Services.AddSingleton<IRssFeedDatabaseService, RssFeedDatabaseService>();
builder.Services.AddSingleton<IFeedProcessorService, FeedProcessorService>();
builder.Services.AddSingleton<IGeminiService, GeminiService>();
builder.Services.AddSingleton<IRSSLinkService, RSSLinkService>();
builder.Services.AddHttpClient<IUpstoxIPOService, UpstoxIPOService>();
builder.Services.AddHttpClient<IUpstoxShareHoldingsService, UpstoxShareHoldingsService>();
builder.Services.AddHttpClient<IUpstoxKeyRatiosService, UpstoxKeyRatiosService>();
builder.Services.AddHttpClient<IUpstoxIncomeStatementService, UpstoxIncomeStatementService>();
builder.Services.AddHttpClient<IUpstoxCorporateActionsService, UpstoxCorporateActionsService>();
builder.Services.AddHttpClient<IUpstoxCashFlowService, UpstoxCashFlowService>();
builder.Services.AddHttpClient<IUpstoxBalanceSheetService, UpstoxBalanceSheetService>();
builder.Services.AddHttpClient<IUpstoxCompanyProfileService, UpstoxCompanyProfileService>();
builder.Services.AddSingleton<IUpstoxFundamentalsService, UpstoxFundamentalsService>();
builder.Services.AddSingleton<IIPOStorageService, IPOStorageService>();
builder.Services.AddSingleton<IIPOProcessorService, IPOProcessorService>();
builder.Services.AddSingleton<IDALAccess, DALAccess>();
builder.Services.AddSingleton<ICompanyData, CompanyData>();
builder.Services.AddSingleton<IInstrumentSearchService, InstrumentSearchService>();
builder.Services.AddSingleton<ICompanyResearchService, CompanyResearchService>();
builder.Services.AddSingleton<ILoadCompaniesService, LoadCompaniesService>();

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);
builder.Services.AddSingleton<IDailyEntry, DailyEntry>();
builder.Services.AddSingleton<IGitHubHelper, GitHubHelper>();
builder.Services.AddSingleton<IFileHelper, FileHelper>();
builder.Services.AddSingleton<IPipelineService, PipelineService>();
builder.Services.AddSingleton<IAICall, AICall>();
builder.Services.AddSingleton<RssFeedDatabaseService>();

// Register email service
builder.Services.AddSingleton<IEmailService, SmtpEmailService>();

builder.Services.AddHostedService<Worker>();
var host = builder.Build();

// Initialize database (creates tables if they don't exist)
var dbInitService = host.Services.GetRequiredService<DatabaseInitializationService>();
dbInitService.Initialize();

host.Run();