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
    public class ProcductManagementController : Controller
    {
        private readonly ToolDbContext _dataContext;
        private readonly ILogger<ProcductManagementController> _logger; IHttpContextAccessor contextAcc;

        public ProcductManagementController(ILogger<ProcductManagementController> logger, ToolDbContext context, IHttpContextAccessor contextAcc)
        {
            _logger = logger;
            _dataContext = context;
            this.contextAcc = contextAcc;
        }

        public IActionResult Index(string? searchString, DateTime? NgayDat, string trangThai)
        {
            var viewModel = new HomeViewModel();
            var dongthietbi = _dataContext.Dongthietbis.AsQueryable();
            List<Dongthietbi> dongtbList = _dataContext.Dongthietbis.ToList();

            // Create a separate list for display
            List<Dongthietbi> displayList = new List<Dongthietbi>(); // Initialize empty list

            foreach (Dongthietbi tb in dongtbList)
            {
                int soLuong = _dataContext.Thietbis
                    .Count(ct => ct.Madongtb == tb.Madongtb && ct.Trangthai == "Sẵn sàng");
                tb.Soluong = soLuong;
            }
            //Lấy hết thiết bị trong kho với tổng số lượng sẵn sàng
            displayList = dongtbList;
            viewModel.DongThietBiList = displayList.Take(4).ToList();

            if (IsAjaxRequest())
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    // If there's a search string, use the filtered thietbi list for display
                    displayList = dongthietbi.Where(x => x.Tendongtb.Contains(searchString)).ToList();
                }


                if (NgayDat != null)
                {
                    HttpContext.Session.SetString("NgayDat", NgayDat?.ToString("dd-MM-yyyy"));
                    // Query to get details for the specified date
                    List<Chitietphieumuon> chiTietPhieuMuonList = _dataContext.Chitietphieumuons
                        .Include(ct => ct.MapmNavigation)
                        .Where(ct => ct.MapmNavigation.Ngaymuon.Date == NgayDat.Value.Date)
                        .ToList();

                    // Lọc ra các mã thiết bị đã được mượn vào ngày đó
                    List<string> maThietBiDaMuon = chiTietPhieuMuonList.Select(ct => ct.Matb.ToString()).ToList();

                    // Lọc danh sách hiển thị để loại bỏ các thiết bị đã được mượn
                    displayList = displayList.Where(tb => !maThietBiDaMuon.Contains(tb.Madongtb.ToString())).ToList();

                    // Gán dữ liệu vào ViewModel
                    viewModel.DongThietBiList = displayList;
                    viewModel.NgayDat = NgayDat;
                    viewModel.ChiTietPhieuMuonList = chiTietPhieuMuonList;
                }

                if (trangThai == "Sẵn sàng")
                {
                    // Trả về tất cả các mục
                    viewModel.DongThietBiList = displayList;
                }


                return PartialView("_PartialShowProduct", viewModel);
            }

            return View(viewModel);
        }

        bool IsAjaxRequest()
        {
            var request = contextAcc.HttpContext.Request;
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
