using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models.Entities;

public partial class Phong
{
    public string Map { get; set; } = null!;

    public int Macs { get; set; }

    public string Tenphong { get; set; } = null!;

    public string Loaiphong { get; set; } = null!;

    public int? Douutien { get; set; }

    public virtual Coso MacsNavigation { get; set; } = null!;

    public virtual ICollection<Thietbi> Thietbis { get; set; } = new List<Thietbi>();
}
