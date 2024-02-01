using System;
using System.Collections.Generic;

namespace ThietBiDienTu.Models;

public partial class Thietbi
{
    public string Matb { get; set; } = null!;

    public string Tenthietbi { get; set; } = null!;

    public string Map { get; set; } = null!;

    public string Trangthai { get; set; } = null!;

    public virtual Phong MapNavigation { get; set; } = null!;
}
