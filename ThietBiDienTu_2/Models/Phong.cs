using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Phong
{
    public string Map { get; set; } = null!;

    public int Macs { get; set; }

    public string Tenphong { get; set; } = null!;

    public string Loaiphong { get; set; } = null!;

    public virtual ICollection<Chitietthietbi> Chitietthietbis { get; set; } = new List<Chitietthietbi>();

    public virtual Coso MacsNavigation { get; set; } = null!;
}
