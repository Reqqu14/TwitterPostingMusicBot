using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace TwitterPostingMusicBot.Infrastructure;

public class TwitterPostingMusicDbContextFactory : IDesignTimeDbContextFactory<TwitterPostingMusicDbContext>
{
    public TwitterPostingMusicDbContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json")
            .Build();

        var connectionString = builder.GetValue<string>("AppConfigCs");

        var configuration = new ConfigurationBuilder()
            .AddAzureAppConfiguration(connectionString)
            .Build();

        connectionString = configuration.GetValue<string>("DatabaseCS");

        var optionsBuilder = new DbContextOptionsBuilder<TwitterPostingMusicDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new TwitterPostingMusicDbContext(optionsBuilder.Options);
    }
}