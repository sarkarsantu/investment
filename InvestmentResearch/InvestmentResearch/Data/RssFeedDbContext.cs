using InvestmentResearch.Model;
using Microsoft.EntityFrameworkCore;

namespace InvestmentResearch.Data;

public class RssFeedDbContext : DbContext
{
    public DbSet<RssFeedEntity> RssFeeds { get; set; }

    public RssFeedDbContext(DbContextOptions<RssFeedDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=rssfeeds.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RssFeedEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Theme).HasMaxLength(100);
            entity.Property(e => e.Url).IsRequired();
        });
    }
}
