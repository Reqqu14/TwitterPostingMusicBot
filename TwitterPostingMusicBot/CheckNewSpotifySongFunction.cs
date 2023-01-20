using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using System.Linq;
using System.Runtime.CompilerServices;
using TwitterPostingMusicBot.Helpers;
using TwitterPostingMusicBot.Interfaces;
using TwitterPostingMusicBot.Models.Domain;
using TwitterPostingMusicBot.Models.Twitter;

namespace TwitterPostingMusicBot
{
    public class CheckNewSpotifySongFunction
    {
        private readonly ITwitterService _twitterService;
        private readonly ISpotifyService _spotifyService;
        private readonly IArtistService _artistService;
        private readonly ILogger _logger;

        public CheckNewSpotifySongFunction(
            ILoggerFactory loggerFactory,
            ITwitterService twitterService,
            ISpotifyService spotifyService,
            IArtistService artistService)
        {
            _twitterService = twitterService;
            _spotifyService = spotifyService;
            _artistService = artistService;
            _logger = loggerFactory.CreateLogger<CheckNewSpotifySongFunction>();
        }

        [Function("CheckNewSpotifySongFunction")]
        public async Task Run(
            [TimerTrigger("0 0 */8 * * *")] MyInfo myTimer)
        {
            _logger.LogInformation(
                $"C# Timer trigger function 'CheckNewSpotifySongFunction' executed at: {DateTime.Now}");

            var artists = await _artistService.GetAllArtistsAsync();

            var newSongs = await _spotifyService.GetNewSongsAsync(artists);

            var postsToPublicate = await _twitterService.PreparePostsToPublicateAsync(newSongs, artists);

            if (postsToPublicate.Any())
            {
                await _twitterService.UploadNewPostAsync(postsToPublicate);
                await _artistService.UpdateArtistsInformationAsync(artists.Where(x => x.ToUpdate).ToList());
            }

            _logger.LogInformation(
                $"C# Timer trigger function 'CheckNewSpotifySongFunction' finished at: {DateTime.Now}, new posts publicated = {postsToPublicate.Count}");
        }

        #region timeModel

        public class MyInfo
        {
            public MyScheduleStatus ScheduleStatus { get; set; }

            public bool IsPastDue { get; set; }
        }

        public class MyScheduleStatus
        {
            public DateTime Last { get; set; }

            public DateTime Next { get; set; }

            public DateTime LastUpdated { get; set; }
        }

        #endregion
    }
}