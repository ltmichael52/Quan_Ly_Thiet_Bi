using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThietBiDienTu_2.Migrations
{
    /// <inheritdoc />
    public partial class account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MANV",
                table: "NHANVIEN",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ACCOUNT",
                columns: table => new
                {
                    USERNAME = table.Column<int>(type: "int", nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LOAIUSER = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ACCOUNT__6029121DD1D4BE32", x => x.USERNAME);
                });

            migrationBuilder.AddForeignKey(
                name: "FK__ACCOUNT__NV",
                table: "NHANVIEN",
                column: "MANV",
                principalTable: "ACCOUNT",
                principalColumn: "USERNAME");

            migrationBuilder.AddForeignKey(
                name: "FK__ACCOUNT__SV",
                table: "SINHVIEN",
                column: "MASV",
                principalTable: "ACCOUNT",
                principalColumn: "USERNAME");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ACCOUNT__NV",
                table: "NHANVIEN");

            migrationBuilder.DropForeignKey(
                name: "FK__ACCOUNT__SV",
                table: "SINHVIEN");

            migrationBuilder.DropTable(
                name: "ACCOUNT");

            migrationBuilder.AlterColumn<int>(
                name: "MANV",
                table: "NHANVIEN",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
