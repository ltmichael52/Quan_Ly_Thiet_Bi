using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThietBiDienTu_2.Migrations
{
    /// <inheritdoc />
    public partial class v0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COSO",
                columns: table => new
                {
                    MACS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENCS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__COSO__603F183BED1C350F", x => x.MACS);
                });

            migrationBuilder.CreateTable(
                name: "NHANVIEN",
                columns: table => new
                {
                    MANV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENNV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PHAI = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    NGAYSINH = table.Column<DateTime>(type: "datetime", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DIACHI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NHANVIEN__603F5114894AC6CF", x => x.MANV);
                });

            migrationBuilder.CreateTable(
                name: "SINHVIEN",
                columns: table => new
                {
                    MASV = table.Column<int>(type: "int", nullable: false),
                    TENSV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    KHOA = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NGANH = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NGAYSINH = table.Column<DateTime>(type: "datetime", nullable: false),
                    DIACHI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SINHVIEN__60228A2838C40CEB", x => x.MASV);
                });

            migrationBuilder.CreateTable(
                name: "PHONG",
                columns: table => new
                {
                    MAP = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MACS = table.Column<int>(type: "int", nullable: false),
                    TENPHONG = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LOAIPHONG = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PHONG__C7907780ED66A93B", x => x.MAP);
                    table.ForeignKey(
                        name: "FK__PHONG__MACS__3D5E1FD2",
                        column: x => x.MACS,
                        principalTable: "COSO",
                        principalColumn: "MACS");
                });

            migrationBuilder.CreateTable(
                name: "PHIEUMUON",
                columns: table => new
                {
                    MAPM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NGAYLAP = table.Column<DateTime>(type: "datetime", nullable: false),
                    MANV = table.Column<int>(type: "int", nullable: true),
                    MASV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PHIEUMUO__603F61CDEBB7FD91", x => x.MAPM);
                    table.ForeignKey(
                        name: "FK_PHIEUMUON_NHANVIEN1",
                        column: x => x.MANV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MANV");
                    table.ForeignKey(
                        name: "FK_PHIEUMUON_SINHVIEN",
                        column: x => x.MASV,
                        principalTable: "SINHVIEN",
                        principalColumn: "MASV");
                });

            migrationBuilder.CreateTable(
                name: "THIETBI",
                columns: table => new
                {
                    MATB = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TENTHIETBI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MAP = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TRANGTHAI = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__THIETBI__6023721DD1D4DC00", x => x.MATB);
                    table.ForeignKey(
                        name: "FK__THIETBI__MAP__403A8C7D",
                        column: x => x.MAP,
                        principalTable: "PHONG",
                        principalColumn: "MAP");
                });

            migrationBuilder.CreateTable(
                name: "CHITIETPHIEUMUON",
                columns: table => new
                {
                    MACTPM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAPM = table.Column<int>(type: "int", nullable: false),
                    MATB = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CHITIETP__F50D75CA51DA8B28", x => x.MACTPM);
                    table.ForeignKey(
                        name: "FK_CHITIETPHIEUMUON_PHIEUMUON1",
                        column: x => x.MAPM,
                        principalTable: "PHIEUMUON",
                        principalColumn: "MAPM");
                    table.ForeignKey(
                        name: "FK_CHITIETPHIEUMUON_THIETBI",
                        column: x => x.MATB,
                        principalTable: "THIETBI",
                        principalColumn: "MATB");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETPHIEUMUON_MAPM",
                table: "CHITIETPHIEUMUON",
                column: "MAPM");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETPHIEUMUON_MATB",
                table: "CHITIETPHIEUMUON",
                column: "MATB");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUMUON_MANV",
                table: "PHIEUMUON",
                column: "MANV");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUMUON_MASV",
                table: "PHIEUMUON",
                column: "MASV");

            migrationBuilder.CreateIndex(
                name: "IX_PHONG_MACS",
                table: "PHONG",
                column: "MACS");

            migrationBuilder.CreateIndex(
                name: "IX_THIETBI_MAP",
                table: "THIETBI",
                column: "MAP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHITIETPHIEUMUON");

            migrationBuilder.DropTable(
                name: "PHIEUMUON");

            migrationBuilder.DropTable(
                name: "THIETBI");

            migrationBuilder.DropTable(
                name: "NHANVIEN");

            migrationBuilder.DropTable(
                name: "SINHVIEN");

            migrationBuilder.DropTable(
                name: "PHONG");

            migrationBuilder.DropTable(
                name: "COSO");
        }
    }
}
