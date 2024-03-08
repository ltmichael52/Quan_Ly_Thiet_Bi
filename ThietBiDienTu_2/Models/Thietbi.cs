using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Thietbi
{
    public string Matb { get; set; } = null!;

    public string Tenthietbi { get; set; } = null!;

    public string Hinhanh { get; set; } = null!;

    public string? Maloai { get; set; }

    public int Soluong { get; set; }

    public string? Mota { get; set; }

    public virtual ICollection<Chitietthietbi> Chitietthietbis { get; set; } = new List<Chitietthietbi>();

    public virtual Loaithietbi? MaloaiNavigation { get; set; }
}
