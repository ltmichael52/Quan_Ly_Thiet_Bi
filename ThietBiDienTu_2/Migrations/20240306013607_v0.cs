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

            migrationBuilder.CreateTable(
                name: "COSO",
                columns: table => new
                {
                    MACS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENCS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DIACHI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__COSO__603F183B80D63A70", x => x.MACS);
                });

            migrationBuilder.CreateTable(
                name: "LOAITHIETBI",
                columns: table => new
                {
                    MALOAI = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    TENLOAI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAITHIETBI", x => x.MALOAI);
                });

            migrationBuilder.CreateTable(
                name: "NHANVIEN",
                columns: table => new
                {
                    MANV = table.Column<int>(type: "int", nullable: false),
                    TENNV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GIOITINH = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    NGAYSINH = table.Column<DateTime>(type: "datetime", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DIACHI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NHANVIEN__603F511441B78137", x => x.MANV);
                    table.ForeignKey(
                        name: "FK__ACCOUNT__NV",
                        column: x => x.MANV,
                        principalTable: "ACCOUNT",
                        principalColumn: "USERNAME");
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
                    DIACHI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GIOITINH = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValueSql: "(N'')"),
                    DIEMRENLUYEN = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SINHVIEN__60228A2812E98CDF", x => x.MASV);
                    table.ForeignKey(
                        name: "FK__ACCOUNT__SV",
                        column: x => x.MASV,
                        principalTable: "ACCOUNT",
                        principalColumn: "USERNAME");
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
                    table.PrimaryKey("PK__PHONG__C790778043DD78DD", x => x.MAP);
                    table.ForeignKey(
                        name: "FK__PHONG__MACS__5165187F",
                        column: x => x.MACS,
                        principalTable: "COSO",
                        principalColumn: "MACS");
                });

            migrationBuilder.CreateTable(
                name: "THIETBI",
                columns: table => new
                {
                    MATB = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TENTHIETBI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HINHANH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MALOAI = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    SOLUONG = table.Column<int>(type: "int", nullable: false),
                    MOTA = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__THIETBI__6023721DFE47CEB0", x => x.MATB);
                    table.ForeignKey(
                        name: "FK_THIETBI_LOAITHIETBI",
                        column: x => x.MALOAI,
                        principalTable: "LOAITHIETBI",
                        principalColumn: "MALOAI");
                });

            migrationBuilder.CreateTable(
                name: "PHIEUMUON",
                columns: table => new
                {
                    MAPM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NGAYMUON = table.Column<DateTime>(type: "datetime", nullable: true),
                    NGAYLAP = table.Column<DateTime>(type: "datetime", nullable: false),
                    MANV = table.Column<int>(type: "int", nullable: false),
                    MASV = table.Column<int>(type: "int", nullable: false),
                    TRANGTHAI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
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
                name: "CHITIETTHIETBI",
                columns: table => new
                {
                    SERI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MAP = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MATB = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TRANGTHAI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MALOAI = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHITIETTHIETBI", x => x.SERI);
                    table.ForeignKey(
                        name: "FK_CHITIETTHIETBI_PHONG",
                        column: x => x.MAP,
                        principalTable: "PHONG",
                        principalColumn: "MAP");
                    table.ForeignKey(
                        name: "FK_CHITIETTHIETBI_THIETBI",
                        column: x => x.MATB,
                        principalTable: "THIETBI",
                        principalColumn: "MATB");
                });

            migrationBuilder.CreateTable(
                name: "CHITIETPHIEUMUON",
                columns: table => new
                {
                    MAPM = table.Column<int>(type: "int", nullable: false),
                    SERI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NGAYTRA = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHITIETPHIEUMUON", x => new { x.MAPM, x.SERI });
                    table.ForeignKey(
                        name: "FK_CHITIETPHIEUMUON_CHITIETTHIETBI",
                        column: x => x.SERI,
                        principalTable: "CHITIETTHIETBI",
                        principalColumn: "SERI");
                    table.ForeignKey(
                        name: "FK_CHITIETPHIEUMUON_PHIEUMUON1",
                        column: x => x.MAPM,
                        principalTable: "PHIEUMUON",
                        principalColumn: "MAPM");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETPHIEUMUON_SERI",
                table: "CHITIETPHIEUMUON",
                column: "SERI");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETTHIETBI_MAP",
                table: "CHITIETTHIETBI",
                column: "MAP");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETTHIETBI_MATB",
                table: "CHITIETTHIETBI",
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
                name: "IX_THIETBI_MALOAI",
                table: "THIETBI",
                column: "MALOAI");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHITIETPHIEUMUON");

            migrationBuilder.DropTable(
                name: "CHITIETTHIETBI");

            migrationBuilder.DropTable(
                name: "PHIEUMUON");

            migrationBuilder.DropTable(
                name: "PHONG");

            migrationBuilder.DropTable(
                name: "THIETBI");

            migrationBuilder.DropTable(
                name: "NHANVIEN");

            migrationBuilder.DropTable(
                name: "SINHVIEN");

            migrationBuilder.DropTable(
                name: "COSO");

            migrationBuilder.DropTable(
                name: "LOAITHIETBI");

            migrationBuilder.DropTable(
                name: "ACCOUNT");
        }
    }
}
