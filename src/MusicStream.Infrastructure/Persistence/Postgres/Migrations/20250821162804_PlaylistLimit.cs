using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStream.Infrastructure.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class PlaylistLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MusicLimits",
                table: "Playlists",
                type: "integer",
                nullable: false,
                defaultValue: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MusicLimits",
                table: "Playlists");
        }
    }
}
