using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThietBiDienTu_2.Models;

public partial class Nhanvien
{
    public int Manv { get; set; }

    public string Tennv { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public string Phai { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date, ErrorMessage = "Ngày sinh không hợp lệ")]
    public DateTime Ngaysinh { get; set; }

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;

    public string? Diachi { get; set; }

    public virtual ICollection<Phieumuon> Phieumuons { get; set; } = new List<Phieumuon>();
    public virtual Account? Account { get; set; }
}
