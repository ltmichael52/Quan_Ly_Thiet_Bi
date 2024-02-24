using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Models.Authentication;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
	[Area("admin")]
	[Route("admin/[controller]/[action]/{id?}")]

	public class RentController : Controller
	{

		public IActionResult Index()
		{
			return View();
		}
	}
}
