using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models.Entities;

public partial class Dongthietbi
{
    public int Madongtb { get; set; }

    public string Tendongtb { get; set; } = null!;

    public string Hinhanh { get; set; } = null!;

    public int Soluong { get; set; }

    public string? Mota { get; set; }

    public int? Soluonghoatdong { get; set; }

    public virtual ICollection<Thietbi> Thietbis { get; set; } = new List<Thietbi>();
}
