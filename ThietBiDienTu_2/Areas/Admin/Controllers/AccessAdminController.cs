using Microsoft.AspNetCore.Mvc;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    public class AccessAdminController : Controller
    {
        public IActionResult LogoutAdmin()
        {

            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Access");
        }
    }
}
