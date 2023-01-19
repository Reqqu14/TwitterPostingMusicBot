using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private readonly string AlbumType = "single";

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
            [TimerTrigger("0 */30 * * * *", RunOnStartup = true)]
            MyInfo myTimer)
        {
            _logger.LogInformation(
                $"C# Timer trigger function 'CheckNewSpotifySongFunction' executed at: {DateTime.Now}");

            var artists = await _artistService.GetAllArtistsAsync();

            var newSongs = await _spotifyService.GetNewSongsAsync(artists);

            var postsToPublicate = PreparePostsToPublicate(newSongs, artists);

            if (postsToPublicate.Any())
            {
                await _twitterService.UploadNewPostAsync(postsToPublicate);
                await _artistService.UpdateArtistsInformationAsync(artists.Where(x => x.ToUpdate).ToList());
            }

            _logger.LogInformation(
                $"C# Timer trigger function 'CheckNewSpotifySongFunction' finished at: {DateTime.Now}, new posts publicated = {postsToPublicate.Count}");
        }

        private List<TwitterPost> PreparePostsToPublicate(List<SimpleAlbum> newSongs, List<Artist> artists)
        {
            var posts = new List<TwitterPost>();

            foreach (var song in newSongs.DistinctBy(x => x.Name))
            {
                var post = new TwitterPost();

                var twitterArtistsAccounts = artists.Where(x =>
                        song.Artists.Select(y => y.Name).Contains(x.ArtistName) &&
                        !string.IsNullOrEmpty(x.ArtistTwitterName))
                    .Select(x => x.ArtistTwitterName);

                post.Text = (song.AlbumType == AlbumType ? "New song from " : "New album from ") +
                            $" {string.Join(", ", song.Artists.Select(x => x.Name))}: " +
                            $"{song.Name}" +
                            $"{Environment.NewLine} {string.Join(", ", twitterArtistsAccounts)}" +
                            $"{Environment.NewLine} {song.ExternalUrls.FirstOrDefault().Value}";

                posts.Add(post);
            }

            return posts;
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