using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

    var twitterConfig = new TwitterConfig();

    twitterConfig.TwitterToken = configuration["TwitterToken"];
    twitterConfig.TwitterAppKey = configuration["TwitterAppKey"];
    twitterConfig.TwitterAppSecret = configuration["TwitterAppSecret"];
    twitterConfig.TwitterOauthToken = configuration["TwitterOauthToken"];
    twitterConfig.TwitterOauthTokenSecret = configuration["TwitterOauthTokenSecret"];
    twitterConfig.TwitterBaseUrl = configuration["TwitterBaseUrl"];

    services.AddSingleton(twitterConfig);

    var spotifyConfig = new SpotifyConfig();

    spotifyConfig.SpotifyClientId = configuration["SpotifyClientId"];
    spotifyConfig.SpotifyClientSecret = configuration["SpotifyClientSecret"];

    services.AddSingleton(spotifyConfig);
}


host.Run();