using TwitterPostingMusicBot.Models.Domain;

namespace TwitterPostingMusicBot.Interfaces;

public interface IArtistService
{
    Task<List<Artist>> GetAllArtistsAsync();
    Task UpdateArtistsInformationAsync(List<Artist> artists);
}