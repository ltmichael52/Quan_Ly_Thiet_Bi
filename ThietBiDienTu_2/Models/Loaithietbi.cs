using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Loaithietbi
{
    public string Maloai { get; set; } = null!;

    public string Tenloai { get; set; } = null!;

    public virtual ICollection<Thietbi> Thietbis { get; set; } = new List<Thietbi>();
}
