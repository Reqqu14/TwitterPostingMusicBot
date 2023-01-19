using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterPostingMusicBot.Migrations
{
    /// <inheritdoc />
    public partial class ArtistLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArtistLanguage",
                table: "Artists",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtistLanguage",
                table: "Artists");
        }
    }
}
