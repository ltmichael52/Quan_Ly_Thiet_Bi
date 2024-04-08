using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models.Entities;

public partial class Sinhvien
{
    public int Masv { get; set; }

    public string Tensv { get; set; } = null!;

    public int Makhoa { get; set; }

    public int Manganh { get; set; }

    public string Email { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public DateTime Ngaysinh { get; set; }

    public string? Diachi { get; set; }

    public string Gioitinh { get; set; } = null!;

    public int? Diemrenluyen { get; set; }

    public virtual Khoa MakhoaNavigation { get; set; } = null!;

    public virtual Nganh ManganhNavigation { get; set; } = null!;

    public virtual Taikhoan MasvNavigation { get; set; } = null!;

    public virtual ICollection<Phieumuon> Phieumuons { get; set; } = new List<Phieumuon>();
}
