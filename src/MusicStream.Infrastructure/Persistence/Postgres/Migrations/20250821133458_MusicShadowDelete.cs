using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStream.Infrastructure.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class MusicShadowDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Musics",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Musics");
        }
    }
}
