using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterPostingMusicBot.Models.Domain;

namespace TwitterPostingMusicBot.Infrastructure.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ArtistName).HasMaxLength(50);

        builder.Property(x => x.LastReleasedSongDate).IsRequired(false);
    }
}