using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitterPostingMusicBot.Infrastructure;
using TwitterPostingMusicBot.Interfaces;
using TwitterPostingMusicBot.Models.Spotify;
using TwitterPostingMusicBot.Models.Twitter;
using TwitterPostingMusicBot.Services;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("local.settings.json", optional: true);
        var settings = builder.Build();
        var connectionString = settings.GetValue<string>("AppConfigCs");
        builder.AddAzureAppConfiguration(connectionString);
    })
    .ConfigureServices((context, services) => { ConfigureServices(context.Configuration, services); })
    .Build();


static void ConfigureServices(IConfiguration configuration,
    IServiceCollection services)
{
    services.AddTransient<ITwitterService, TwitterService>();
    services.AddTransient<ISpotifyService, SpotifyService>();
    services.AddTransient<IArtistService, ArtistService>();
    services.AddTransient<IOpenAiService, OpenAiService>();
    services.AddDbContext<TwitterPostingMusicDbContext>(x =>
        x.UseSqlServer(configuration["DatabaseCS"]));

    var twitterConfig = PrepareTwitterConfig(configuration);

    services.AddSingleton(twitterConfig);

    var spotifyConfig = PrepareSporifyConfig(configuration);

    services.AddSingleton(spotifyConfig);
}


host.Run();


static TwitterConfig PrepareTwitterConfig(IConfiguration configuration)
{
    var twitterConfig = new TwitterConfig();

    twitterConfig.TwitterToken = configuration["TwitterToken"];
    twitterConfig.TwitterAppKey = configuration["TwitterAppKey"];
    twitterConfig.TwitterAppSecret = configuration["TwitterAppSecret"];
    twitterConfig.TwitterOauthToken = configuration["TwitterOauthToken"];
    twitterConfig.TwitterOauthTokenSecret = configuration["TwitterOauthTokenSecret"];
    twitterConfig.TwitterBaseUrl = configuration["TwitterBaseUrl"];

    return twitterConfig;
}

static SpotifyConfig PrepareSporifyConfig(IConfiguration configuration)
{
    var spotifyConfig = new SpotifyConfig();

    spotifyConfig.SpotifyClientId = configuration["SpotifyClientId"];
    spotifyConfig.SpotifyClientSecret = configuration["SpotifyClientSecret"];

    return spotifyConfig;
}