using System;
using System.Collections.Generic;

namespace ThietBiDienTu.Models;

public partial class Coso
{
    public int Macs { get; set; }

    public string Tencs { get; set; } = null!;

    public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
}
