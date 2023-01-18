using Microsoft.EntityFrameworkCore;
using TwitterPostingMusicBot.Infrastructure.Configurations;
using TwitterPostingMusicBot.Models.Domain;
using TwitterPostingMusicBot.Models.Domain.Abstract;

namespace TwitterPostingMusicBot.Infrastructure;

public class TwitterPostingMusicDbContext : DbContext
{
    public TwitterPostingMusicDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArtistConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((AuditableEntity)entityEntry.Entity).LastModifiedAt = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}