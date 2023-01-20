using SpotifyAPI.Web;
using TwitterPostingMusicBot.Models.Domain;
using TwitterPostingMusicBot.Models.Twitter;

namespace TwitterPostingMusicBot.Interfaces;

public interface ITwitterService
{
    Task UploadNewPostAsync(List<TwitterPost> posts);

    Task<List<TwitterPost>> PreparePostsToPublicateAsync(
        List<SimpleAlbum> newSongs,
        List<Artist> artists);
}