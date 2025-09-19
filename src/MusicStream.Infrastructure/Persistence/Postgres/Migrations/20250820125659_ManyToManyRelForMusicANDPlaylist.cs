using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStream.Infrastructure.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyRelForMusicANDPlaylist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musics_Playlists_PlaylistId",
                table: "Musics");

            migrationBuilder.DropIndex(
                name: "IX_Musics_PlaylistId",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Musics");

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionType",
                table: "Subscriptions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MusicPlaylist",
                columns: table => new
                {
                    MusicsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicPlaylist", x => new { x.MusicsId, x.PlaylistId });
                    table.ForeignKey(
                        name: "FK_MusicPlaylist_Musics_MusicsId",
                        column: x => x.MusicsId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicPlaylist_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MusicPlaylist_PlaylistId",
                table: "MusicPlaylist",
                column: "PlaylistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicPlaylist");

            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                table: "Subscriptions");

            migrationBuilder.AddColumn<Guid>(
                name: "PlaylistId",
                table: "Musics",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Musics_PlaylistId",
                table: "Musics",
                column: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Musics_Playlists_PlaylistId",
                table: "Musics",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id");
        }
    }
}
