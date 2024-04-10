using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
//using ThietBiDienTu_2.Models.Entities;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeAdminController : Controller
    {

        private readonly ToolDbContext _context;
        public HomeAdminController(ToolDbContext context)
        {
            _context = context;
        }
        [AuthenticationM_S]
        public IActionResult Index()
        {
            // Đếm số lượng Thietbi
            int thietbiCount = _context.Thietbis.Count();
            ViewBag.ThietBiCount = thietbiCount; // Truyền tổng số lượng qua ViewBag để sử dụng trong View

            int dtbCount = _context.Dongthietbis.Count();
            ViewBag.DTBCount = dtbCount;

            int nvCount = _context.Nhanviens.Count();
            ViewBag.NvCount = nvCount;

            int svCount = _context.Sinhviens.Count();
            ViewBag.SvCount = svCount;

            //int cdpmCount = _context.Phieumuons.Count(t => t.Trangthai == "Chưa duyệt");

            return View();
        }
    }
}
