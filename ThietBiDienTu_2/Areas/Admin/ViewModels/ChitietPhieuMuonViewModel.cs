﻿namespace ThietBiDienTu_2.Areas.Admin.ViewModels
{
    public class ChitietPhieuMuonViewModel
    {
        public int Madongtb {  get; set; }
        public string Tendongthietbi {  get; set; }
        public List<string> Seri {  get; set; }
        public List<int> Matb {  get; set; }
        public int Soluong {  get; set; }
        public string Hinhanh {  get; set; }
        public List<DateTime> Ngaytra {  get; set; }
        public List<bool> check {  get; set; }
        public string TenKho {  get; set; }
        //public int matb {  get; set; }
    }
}
