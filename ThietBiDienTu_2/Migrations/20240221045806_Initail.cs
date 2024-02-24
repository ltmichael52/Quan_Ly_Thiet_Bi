using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThietBiDienTu_2.Migrations
{
    /// <inheritdoc />
    public partial class Initail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HINHANH",
                table: "THIETBI",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HINHANH",
                table: "THIETBI");
        }
    }
}
