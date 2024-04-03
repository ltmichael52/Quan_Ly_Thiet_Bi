using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Thietbi
{
    public int Matb { get; set; }

    public string Seri { get; set; } = null!;

    public string Map { get; set; } = null!;

    public int Madongtb { get; set; }

    public string? Trangthai { get; set; }

    public virtual ICollection<Chitietphieumuon> Chitietphieumuons { get; set; } = new List<Chitietphieumuon>();

    public virtual Dongthietbi MadongtbNavigation { get; set; } = null!;

    public virtual Phong MapNavigation { get; set; } = null!;

    public virtual ICollection<Phieusua> Phieusuas { get; set; } = new List<Phieusua>();
}
