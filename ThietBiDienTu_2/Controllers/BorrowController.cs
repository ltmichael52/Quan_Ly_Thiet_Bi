using Microsoft.AspNetCore.Mvc;

namespace DateBorrowTest.Controllers
{
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
