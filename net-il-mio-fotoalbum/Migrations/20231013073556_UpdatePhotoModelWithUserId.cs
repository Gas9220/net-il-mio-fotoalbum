using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net_il_mio_fotoalbum.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhotoModelWithUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Photos");
        }
    }
}
