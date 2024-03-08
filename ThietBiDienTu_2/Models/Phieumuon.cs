using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Phieumuon
{
    public int Mapm { get; set; }

    public DateTime? Ngaymuon { get; set; }

    public DateTime Ngaylap { get; set; }

    public int Manv { get; set; }

    public int Masv { get; set; }

    public string? Trangthai { get; set; }

    public virtual ICollection<Chitietphieumuon> Chitietphieumuons { get; set; } = new List<Chitietphieumuon>();

    public virtual Nhanvien ManvNavigation { get; set; } = null!;

    public virtual Sinhvien MasvNavigation { get; set; } = null!;
}
