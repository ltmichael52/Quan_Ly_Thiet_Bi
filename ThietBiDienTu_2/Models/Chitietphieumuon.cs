using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Chitietphieumuon
{
    public int Mapm { get; set; }

    public string Seri { get; set; } = null!;

    public DateTime? Ngaytra { get; set; }

    public virtual Phieumuon MapmNavigation { get; set; } = null!;

    public virtual Chitietthietbi SeriNavigation { get; set; } = null!;
}
