using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStream.Infrastructure.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class NewMusicFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Musics");

            migrationBuilder.AddColumn<string>(
                name: "Artist",
                table: "Musics",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Musics",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Musics",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Musics",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Artist",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Musics");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Musics",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
