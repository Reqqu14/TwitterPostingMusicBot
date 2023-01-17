using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using TwitterPostingMusicBot.Interfaces;
using TwitterPostingMusicBot.Models.Spotify;

namespace TwitterPostingMusicBot.Services;

public class SpotifyService : ISpotifyService
{
    private readonly SpotifyConfig _spotifyConfig;
    private readonly ILogger _logger;

    public SpotifyService(
        SpotifyConfig spotifyConfig,
        ILoggerFactory loggerFactory)
    {
        _spotifyConfig = spotifyConfig;
        _logger = loggerFactory.CreateLogger<SpotifyService>();
    }

    public async Task GetNewSongsAsync()
    {
        var newSongs = new List<SimpleAlbum>();

        var config = SpotifyClientConfig.CreateDefault();

        var request =
            new ClientCredentialsRequest(_spotifyConfig.SpotifyClientId, _spotifyConfig.SpotifyClientSecret);

        var response = await new OAuthClient(config).RequestToken(request);

        var spotifyClient = new SpotifyClient(config.WithToken(response.AccessToken));

        var artist = await spotifyClient.Artists.GetAlbums("4Q3xLVaD2uBZGVxmCYuSkt");

        if (artist.Items != null)
        {
            var newRelease = artist.Items.MaxBy(x => x.ReleaseDate);
        }

        var zm = 1;
    }
}