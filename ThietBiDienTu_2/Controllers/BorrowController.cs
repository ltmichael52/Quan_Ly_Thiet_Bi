using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Models.Authentication;

namespace DateBorrowTest.Controllers
{
    [AuthenticationCustomer]
    public class BorrowController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("NgayDat") !=null)
            {
                ViewBag.NgayDat = HttpContext.Session.GetString("NgayDat");
            }
            return View();
        }
    }
}
