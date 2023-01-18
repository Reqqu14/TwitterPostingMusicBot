using SpotifyAPI.Web;
using TwitterPostingMusicBot.Models.Domain;

namespace TwitterPostingMusicBot.Interfaces;

public interface ISpotifyService
{
    Task<List<SimpleAlbum>> GetNewSongsAsync(List<Artist> artists);
}