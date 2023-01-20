using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using SpotifyAPI.Web;
using TwitterPostingMusicBot.Helpers;
using TwitterPostingMusicBot.Interfaces;
using TwitterPostingMusicBot.Models.Domain;
using TwitterPostingMusicBot.Models.Twitter;

namespace TwitterPostingMusicBot.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly TwitterConfig _twitterConfig;
        private readonly IOpenAiService _openAiService;
        private readonly ILogger _logger;
        private readonly string _albumType = "single";

        public TwitterService(
            TwitterConfig twitterConfig,
            ILoggerFactory loggerFactory,
            IOpenAiService openAiService)
        {
            _twitterConfig = twitterConfig;
            _openAiService = openAiService;
            _logger = loggerFactory.CreateLogger<TwitterService>();
        }

        public async Task UploadNewPostAsync(List<TwitterPost> posts)
        {
            RestClient client = new RestClient();

            client.Authenticator =
                OAuth1Authenticator.ForProtectedResource(_twitterConfig.TwitterAppKey, _twitterConfig.TwitterAppSecret,
                    _twitterConfig.TwitterOauthToken, _twitterConfig.TwitterOauthTokenSecret);

            foreach (var post in posts)
            {
                RestRequest postRequest = new RestRequest($"{_twitterConfig.TwitterBaseUrl}/tweets");

                postRequest.AddHeader("Authorization", $"Bearer {_twitterConfig.TwitterToken}");

                postRequest.AddBody(post);

                try
                {
                    var postResponse = await client.ExecutePostAsync(postRequest);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error occured during post publishing, message: '{e.Message}', post: '{post}'");
                }
            }
        }

        public async Task<List<TwitterPost>> PreparePostsToPublicateAsync(List<SimpleAlbum> newSongs,
            List<Artist> artists)
        {
            var posts = new List<TwitterPost>();

            foreach (var song in newSongs.DistinctBy(x => x.Name))
            {
                var post = new TwitterPost();

                var artist = artists.Where(x =>
                        song.Artists.Select(y => y.Id).Contains(x.ArtistId))
                    .Select(x => x);

                if (!artist.Any())
                {
                    continue;
                }

                var language = artist.FirstOrDefault().ArtistLanguage == "PL"
                    ? LanguageEnum.Polish
                    : LanguageEnum.English;

                var postText = song.AlbumType == _albumType
                    ? await _openAiService.GetMessage(
                        true,
                        language,
                        string.Join(", ", song.Artists.Select(x => x.Name)),
                        song.Name)
                    : await _openAiService.GetMessage(
                        false,
                        language,
                        string.Join(", ", song.Artists.Select(x => x.Name)),
                        song.Name);

                post.Text = postText +
                            $"{Environment.NewLine} {string.Join(", ", artist.Where(x => !string.IsNullOrEmpty(x.ArtistTwitterName)).Select(x => x.ArtistTwitterName))}" +
                            $"{Environment.NewLine} {song.ExternalUrls.FirstOrDefault().Value}";

                posts.Add(post);
            }

            return posts;
        }
    }
}