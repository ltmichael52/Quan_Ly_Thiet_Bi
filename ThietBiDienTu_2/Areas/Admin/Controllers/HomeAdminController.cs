using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Models.Authentication;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class HomeAdminController : Controller
    {
        [AuthenticationM_S]
        public IActionResult Index()
        {
            return View();
        }
    }
}
