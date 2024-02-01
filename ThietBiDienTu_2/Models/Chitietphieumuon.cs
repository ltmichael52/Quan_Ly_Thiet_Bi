using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Chitietphieumuon
{
    public int Mactpm { get; set; }

    public int Mapm { get; set; }

    public string Matb { get; set; } = null!;

    public virtual Phieumuon MapmNavigation { get; set; } = null!;

    public virtual Thietbi MatbNavigation { get; set; } = null!;
}
