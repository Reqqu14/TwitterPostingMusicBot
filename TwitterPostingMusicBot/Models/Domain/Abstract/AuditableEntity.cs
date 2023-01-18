namespace TwitterPostingMusicBot.Models.Domain.Abstract;

public abstract class AuditableEntity
{
    public DateTime LastModifiedAt { get; set; }
}