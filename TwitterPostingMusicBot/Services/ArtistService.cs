using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using TwitterPostingMusicBot.Infrastructure;
using TwitterPostingMusicBot.Interfaces;
using TwitterPostingMusicBot.Models.Domain;

namespace TwitterPostingMusicBot.Services;

public class ArtistService : IArtistService
{
    private readonly TwitterPostingMusicDbContext _context;

    public ArtistService(TwitterPostingMusicDbContext context)
    {
        _context = context;
    }

    public async Task<List<Artist>> GetAllArtistsAsync()
    {
        return await _context.Artists
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateArtistsInformationAsync(List<Artist> artists)
    {
        artists.ForEach(x =>
        {
            x.ToUpdate = false;
            x.LastReleasedSongDate = DateTimeOffset.Now;
        });

        _context.Artists.UpdateRange(artists);
        await _context.SaveChangesAsync(default);
    }
}