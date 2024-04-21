using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
    }
}
