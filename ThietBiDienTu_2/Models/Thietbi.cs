using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Thietbi
{
    public string Matb { get; set; } = null!;

    public string Tenthietbi { get; set; } = null!;

    public string Map { get; set; } = null!;

    public string Trangthai { get; set; } = null!;

    public string Hinhanh {  get; set; } = null!;

    public virtual ICollection<Chitietphieumuon> Chitietphieumuons { get; set; } = new List<Chitietphieumuon>();

    public virtual Phong MapNavigation { get; set; } = null!;
}
