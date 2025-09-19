using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStream.Infrastructure.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class PlaylistTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Playlists",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Playlists");
        }
    }
}
