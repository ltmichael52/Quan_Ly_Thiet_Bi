using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using ThietBiDienTu_2.Models.ViewModels;

namespace ThietBiDienTu_2.Controllers
{
    [AuthenticationCustomer]
    public class HomeController : Controller
    {
        private readonly ToolDbContext _dataContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ToolDbContext context)
        {
            _logger = logger;
            _dataContext = context;
        }


        public IActionResult Index(string? searchString, DateTime? NgayDat, string trangThai)
        {
            var dongthietbi = _dataContext.Dongthietbis.AsQueryable();
            List<Phong> phongList = _dataContext.Phongs.Where(x => x.Loaiphong == "Kho").ToList();
            List<string> mapKhoList = phongList.Select(id => id.Map).ToList();
            List<Thietbi> ctthietbi = _dataContext.Thietbis
                .Where(x => x.Trangthai == "Tồn kho") // Filter by TrangThai here
                .ToList();
            List<Dongthietbi> dongtbList = _dataContext.Dongthietbis.ToList();

            // Create a separate list for display
            List<Dongthietbi> displayList;

            foreach (Dongthietbi tb in dongtbList)
            {
                int soLuong = ctthietbi.Count(ct => ct.Madongtb == tb.Madongtb);
                tb.Soluong = soLuong;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                // If there's a search string, use the filtered thietbi list for display
                displayList = dongthietbi.Where(x => x.Tendongtb.Contains(searchString)).ToList();
            }
            else
            {
                // If no search string, use the original tbList for display
                displayList = dongtbList;
            }

            Debug.WriteLine("Ngay Dat out if: ", NgayDat);
            if (NgayDat != null)
            {
                // Query to get details for the specified date
                List<Chitietphieumuon> chiTietPhieuMuonList = _dataContext.Chitietphieumuons
                    .Include(ct => ct.MapmNavigation)
                    .Where(ct => ct.MapmNavigation.Ngaymuon.Date == NgayDat.Value.Date)
                    .ToList();

                // Lọc ra các mã thiết bị đã được mượn vào ngày đó
                List<string> maThietBiDaMuon = chiTietPhieuMuonList.Select(ct => ct.Matb.ToString()).ToList();

                // Lọc danh sách hiển thị để loại bỏ các thiết bị đã được mượn
                displayList = displayList.Where(tb => !maThietBiDaMuon.Contains(tb.Madongtb.ToString())).ToList();
                HttpContext.Session.SetString("NgayDat", NgayDat.ToString());

                // Update the ViewBag instead of using a ViewModel
                ViewBag.NgayDat = NgayDat;
                ViewBag.ChiTietPhieuMuonList = chiTietPhieuMuonList;

                return View(displayList);
            }

            if (trangThai == "Tồn Kho")
            {
                return View(displayList); // Trả về tất cả các mục
            }
            else
            {
                return View(displayList.Take(4).ToList()); // Trả về chỉ 4 mục đầu tiên
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
