using TwitterPostingMusicBot.Models.Twitter;

namespace TwitterPostingMusicBot.Interfaces;

public interface ITwitterService
{
    Task UploadNewPostAsync(List<TwitterPost> posts);
}