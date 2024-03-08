using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Chitietthietbi
{
    public string Seri { get; set; } = null!;

    public string Map { get; set; } = null!;

    public string Matb { get; set; } = null!;

    public string? Trangthai { get; set; }

    public virtual ICollection<Chitietphieumuon> Chitietphieumuons { get; set; } = new List<Chitietphieumuon>();

    public virtual Phong MapNavigation { get; set; } = null!;

    public virtual Thietbi MatbNavigation { get; set; } = null!;
}
