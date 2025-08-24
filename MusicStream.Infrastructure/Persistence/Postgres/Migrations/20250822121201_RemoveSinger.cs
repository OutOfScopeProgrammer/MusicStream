using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStream.Infrastructure.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSinger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musics_Singers_SingerId",
                table: "Musics");

            migrationBuilder.DropTable(
                name: "Singers");

            migrationBuilder.DropIndex(
                name: "IX_Musics_SingerId",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "SingerId",
                table: "Musics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SingerId",
                table: "Musics",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Singers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Singers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Musics_SingerId",
                table: "Musics",
                column: "SingerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Musics_Singers_SingerId",
                table: "Musics",
                column: "SingerId",
                principalTable: "Singers",
                principalColumn: "Id");
        }
    }
}
