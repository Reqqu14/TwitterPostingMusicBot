using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TwitterPostingMusicBot.Interfaces;
using TwitterPostingMusicBot.Models.Twitter;
using TimerInfo = Microsoft.Azure.Functions.Worker.TimerInfo;

namespace TwitterPostingMusicBot
{
    public class CheckNewSpotifySongFunction
    {
        private readonly ITwitterService _twitterService;
        private readonly ISpotifyService _spotifyService;
        private readonly ILogger _logger;

        public CheckNewSpotifySongFunction(
            ILoggerFactory loggerFactory,
            ITwitterService twitterService,
            ISpotifyService spotifyService)
        {
            _twitterService = twitterService;
            _spotifyService = spotifyService;
            _logger = loggerFactory.CreateLogger<CheckNewSpotifySongFunction>();
        }

        [Function("CheckNewSpotifySongFunction")]
        public async Task Run(
            [Microsoft.Azure.Functions.Worker.TimerTrigger("0 */3 * * * *", RunOnStartup = true)]
            TimerInfo myTimer)
        {
            _logger.LogInformation(
                $"C# Timer trigger function 'CheckNewSpotifySongFunction' executed at: {DateTime.Now}");
            await _spotifyService.GetNewSongsAsync();
            await _twitterService.UploadNewPostAsync(new TwitterPost());
        }
    }
}