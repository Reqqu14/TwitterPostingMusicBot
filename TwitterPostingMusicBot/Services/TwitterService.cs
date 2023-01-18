using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using TwitterPostingMusicBot.Interfaces;
using TwitterPostingMusicBot.Models.Twitter;

namespace TwitterPostingMusicBot.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly TwitterConfig _twitterConfig;
        private readonly ILogger _logger;

        public TwitterService(
            TwitterConfig twitterConfig,
            ILoggerFactory loggerFactory)
        {
            _twitterConfig = twitterConfig;
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
    }
}