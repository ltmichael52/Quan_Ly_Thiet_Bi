using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

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

    public string Gioitinh { get; set; } = null!;

    public int? Diemrenluyen { get; set; }

    public virtual Account MasvNavigation { get; set; } = null!;

    public virtual ICollection<Phieumuon> Phieumuons { get; set; } = new List<Phieumuon>();
}
