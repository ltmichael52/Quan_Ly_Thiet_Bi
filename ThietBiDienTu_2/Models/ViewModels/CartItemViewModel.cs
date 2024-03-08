namespace ThietBiDienTu_2.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; }
        public decimal GrandTotal { get; set; } //Tinh tong so luong
    }
}
