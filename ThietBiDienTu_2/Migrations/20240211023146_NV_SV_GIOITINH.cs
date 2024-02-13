using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThietBiDienTu_2.Migrations
{
    /// <inheritdoc />
    public partial class NV_SV_GIOITINH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PHAI",
                table: "NHANVIEN",
                newName: "GIOITINH");

            migrationBuilder.AddColumn<string>(
                name: "GIOITINH",
                table: "SINHVIEN",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GIOITINH",
                table: "SINHVIEN");

            migrationBuilder.RenameColumn(
                name: "GIOITINH",
                table: "NHANVIEN",
                newName: "PHAI");
        }
    }
}
