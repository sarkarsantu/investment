using InvestmentResearch.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestmentResearch.Service;

public class DatabaseInitializationService
{
    private readonly IDbContextFactory<RssFeedDbContext> _contextFactory;

    public DatabaseInitializationService(IDbContextFactory<RssFeedDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// Initialize the database and create tables if they don't exist
    /// </summary>
    public void Initialize()
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            context.Database.EnsureCreated();
            Console.WriteLine("RSS Feed database initialized successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
            throw;
        }
    }

}
