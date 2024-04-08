using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThietBiDienTu_2.Models;

public partial class Dongthietbi
{
    public int Madongtb { get; set; }

    [Required(ErrorMessage = "Tên dòng thiết bị không được bỏ trống")]
    public string Tendongtb { get; set; } = null!;

    public string? Hinhanh { get; set; }

    [Required(ErrorMessage = "Số lượng thiết bị không được bỏ trống")]
    public int Soluong { get; set; }

    public string? Mota { get; set; }

    //public int SoLuongHoatDong { get; set; }

    public virtual ICollection<Thietbi> Thietbis { get; set; } = new List<Thietbi>();

    [NotMapped]
    public IFormFile hinhanh { get; set; }
}
