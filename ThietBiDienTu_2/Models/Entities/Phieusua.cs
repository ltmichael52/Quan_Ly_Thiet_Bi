using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models.Entities;

public partial class Phieusua
{
    public int Maps { get; set; }

    public int Matb { get; set; }

    public DateTime Ngaylap { get; set; }

    public DateTime? Ngayhoantat { get; set; }

    public decimal? Chiphi { get; set; }

    public string? Mota { get; set; }

    public virtual Thietbi MatbNavigation { get; set; } = null!;
}
