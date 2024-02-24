using System.Drawing;
using System.Net.Http.Headers;

namespace ThietBiDienTu_2.Models
{
    public class CartItemModel
    {
        public string Matb { get; set; }
        public string Tenthietbi { get; set; } 
        public int Soluong { get; set; }
        public string Hinhanh { get; set; }
        public CartItemModel() { }//neu khong bam vao gio hang thi gio hang se trong
        public CartItemModel(Thietbi thietbi)
        {
            Matb = thietbi.Matb;
            Tenthietbi = thietbi.Tenthietbi;
            Soluong = 1;
            Hinhanh = thietbi.Hinhanh;
        } 
    }
}
