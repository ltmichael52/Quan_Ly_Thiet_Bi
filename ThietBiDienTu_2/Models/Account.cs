using System;
using System.Collections.Generic;

namespace ThietBiDienTu_2.Models;

public partial class Account
{
    public int Username { get; set; }

    public string Password { get; set; } = null!;

    public short Loaiuser { get; set; }

    public virtual Nhanvien? Nhanvien { get; set; }

    public virtual Sinhvien? Sinhvien { get; set; }
}
