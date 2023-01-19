using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterPostingMusicBot.Migrations
{
    /// <inheritdoc />
    public partial class TwitterArtistName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArtistTwitterName",
                table: "Artists",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtistTwitterName",
                table: "Artists");
        }
    }
}
