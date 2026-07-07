using InvestmentResearch.Data;
using InvestmentResearch.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace InvestmentResearch.Service;

public class RssFeedDatabaseService : IRssFeedDatabaseService
{
    private readonly IDbContextFactory<RssFeedDbContext> _contextFactory;
    private readonly AppSettings _appSettings;

    public RssFeedDatabaseService(IDbContextFactory<RssFeedDbContext> contextFactory, IOptions<AppSettings> appSettings)
    {
        _contextFactory = contextFactory;
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
    }

    /// <summary>
    /// Get all RSS feeds from the database (including skipped ones)
    /// </summary>
    public async Task<List<RssFeedConfig>> GetAllFeedsAsync()
    {
        using var context =await _contextFactory.CreateDbContextAsync();
        var entities = context.RssFeeds.ToList();
        return ConvertToRssFeedConfigs(entities);
    }

    /// <summary>
    /// Seed the database with RSS feeds from AppConfig
    /// </summary>
    public async Task SeedFromAppConfigAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        if (context.RssFeeds.Any())
        {
            return; // Database already seeded
        }

        foreach (var feed in _appSettings.RssFeeds ?? new List<RssFeedConfig>())
        {
            var entity = new RssFeedEntity
            {
                Name = feed.Name,
                Theme = feed.Theme,
                Url = feed.Url,
            };
            context.RssFeeds.Add(entity);
        }

        context.SaveChanges();
        Console.WriteLine($"Seeded database with {_appSettings.RssFeeds?.Count ?? 0} RSS feeds from appsettings.json");
    }

    private List<RssFeedConfig> ConvertToRssFeedConfigs(List<RssFeedEntity> entities)
    {
        return entities.Select(e => new RssFeedConfig
        {
            Name = e.Name,
            Theme = e.Theme,
            Url = e.Url
        }).ToList();
    }
}
