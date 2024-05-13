﻿using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
//using ThietBiDienTu_2.Models.Entities;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeAdminController : Controller
    {

        private readonly ToolDbContext _context; IPhieuMuonAdmin pmRepo;
        public HomeAdminController(ToolDbContext context, IPhieuMuonAdmin pmRepo)
        {
            _context = context;
            this.pmRepo = pmRepo;
        }
        [AuthenticationM_S]
        public IActionResult Index()
        {

            pmRepo.CheckPmToday();
            // Đếm số lượng Thietbi
            int thietbiCount = _context.Thietbis.Count();
            ViewBag.ThietBiCount = thietbiCount; // Truyền tổng số lượng qua ViewBag để sử dụng trong View

            int dtbCount = _context.Dongthietbis.Count();
            ViewBag.DTBCount = dtbCount;

            int nvCount = _context.Nhanviens.Count();
            ViewBag.NvCount = nvCount;

            int svCount = _context.Sinhviens.Count();
            ViewBag.SvCount = svCount;

            DateTime today = DateTime.Now.Date;
            string formattedDate = today.ToString("dd/MM/yyyy"); // Định dạng ngày/tháng/năm
            ViewBag.Today = formattedDate;

            //Tổng chưa duyệt hôm nay
            int todaychuaduyetCount = _context.Phieumuons.Count(p => p.Trangthai == 0 && p.Ngaymuon.Date == today);
            ViewBag.todayCDCount = todaychuaduyetCount;

            //tổng phiếu chưa duyệt
            int tongchuaduyetCount = _context.Phieumuons.Count(p => p.Trangthai == 0);
            ViewBag.tongCDCount = tongchuaduyetCount;

            //tổng phiếu chưa trả
            int chuatraCount = _context.Phieumuons.Count(p => p.Trangthai == 1);
            ViewBag.CTCount = chuatraCount;

            //Tổng phieu hôm nay
            int phieutodayCount = _context.Phieumuons.Count(p => p.Ngaymuon.Date == today);
            ViewBag.PhieutodayCount = phieutodayCount;

            int thietbihuCount = _context.Thietbis.Count(p => p.Trangthai == "Đang hư");
            ViewBag.ThietBiHuCount = thietbihuCount;

            return View();
        }

        [HttpGet]
        public IActionResult GetChartData()
        {
            List<int> trangthaiList = _context.Phieumuons.Select(t => t.Trangthai).ToList();

            Dictionary<int, string> trangthaiNames = new Dictionary<int, string>
            {
                { 0, "Chưa duyệt" },
                { 1, "Chưa trả" },
                { 2, "Đã duyệt" },
                { 3, "Đã trả" },
                { 4, "Từ chối" },
                { 5, "Hủy phiếu" },
                { 6, "Không mượn" }
            };

            List<int> counts = new List<int>();
            foreach (var trangthai in Enumerable.Range(0, trangthaiNames.Count))
            {
                counts.Add(trangthaiList.Count(t => t == trangthai));
            }

            var data = new
            {
                TrangthaiNames = trangthaiNames.Values.ToList(),
                Counts = counts
            };

            return Json(data); // Trả về dữ liệu JSON
        }


    }
}
