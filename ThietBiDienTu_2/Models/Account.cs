namespace ThietBiDienTu_2.Models
{
    public partial class Account
    {
        public int Username {  get; set; }
        public string Password { get; set; }
        public Int16 LoaiUser {  get; set; }

        public virtual Nhanvien? ManvNavigation { get; set; }
        public virtual Sinhvien? MasvNavigation { get; set; }
    }
}
