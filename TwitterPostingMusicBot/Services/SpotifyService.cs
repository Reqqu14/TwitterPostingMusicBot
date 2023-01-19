using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using TwitterPostingMusicBot.Interfaces;
using TwitterPostingMusicBot.Models.Domain;
using TwitterPostingMusicBot.Models.Spotify;
using System.Linq;

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

    public async Task<List<SimpleAlbum>> GetNewSongsAsync(List<Artist> artists)
    {
        var newSongs = new List<SimpleAlbum>();

        var config = SpotifyClientConfig.CreateDefault();

        var request =
            new ClientCredentialsRequest(_spotifyConfig.SpotifyClientId, _spotifyConfig.SpotifyClientSecret);

        var response = await new OAuthClient(config).RequestToken(request);

        var spotifyClient = new SpotifyClient(config.WithToken(response.AccessToken));

        foreach (var artist in artists)
        {
            try
            {
                var albums = await spotifyClient.Artists.GetAlbums(artist.ArtistId);

                if (albums.Items != null)
                {
                    var newRelease = albums.Items.MaxBy(x => x.ReleaseDate);

                    var releaseDate = DateTimeOffset.Parse(newRelease.ReleaseDate);

                    if (releaseDate > artist.LastReleasedSongDate &&
                        releaseDate.Day <= DateTimeOffset.Now.Day)
                    {
                        newSongs.Add(newRelease);
                        artist.ToUpdate = true;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error occured while getting artist, message: '{e.Message}', artist: '{artists}'");
            }
        }

        return newSongs;
    }
}