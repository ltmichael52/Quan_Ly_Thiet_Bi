using System;
using System.Collections.Generic;

namespace ThietBiDienTu.Models;

public partial class Sinhvien
{
    public int Masv { get; set; }

    public string Tensv { get; set; } = null!;

    public string Khoa { get; set; } = null!;

    public string Nganh { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public DateTime Ngaysinh { get; set; }

    public string? Diachi { get; set; }
}
