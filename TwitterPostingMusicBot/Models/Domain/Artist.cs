using TwitterPostingMusicBot.Models.Domain.Abstract;

namespace TwitterPostingMusicBot.Models.Domain;

public class Artist : AuditableEntity
{
    public Guid Id { get; set; }
    public string ArtistId { get; set; }
    public string ArtistName { get; set; }
    public DateTimeOffset? LastReleasedSongDate { get; set; }
    public bool ToUpdate { get; set; }
}