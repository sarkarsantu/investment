using InvestmentResearch.Data;
using InvestmentResearch.Model;
using Microsoft.EntityFrameworkCore;

namespace InvestmentResearch.Service;

public class RssFeedDatabaseService : IRssFeedDatabaseService
{
    private readonly IDbContextFactory<RssFeedDbContext> _contextFactory;
    
    public RssFeedDatabaseService(IDbContextFactory<RssFeedDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
        
    }

    /// <summary>
    /// Get all RSS feeds from the database in real-time
    /// </summary>
    public async Task<List<RssFeedConfig>> GetAllFeedsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        // AsNoTracking ensures fresh data is always fetched from the database
        var entities = context.RssFeeds.AsNoTracking().ToList();
        return ConvertToRssFeedConfigs(entities);
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
