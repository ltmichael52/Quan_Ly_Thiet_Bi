using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;

namespace ThietBiDienTu_2.Controllers
{

    public class HomeController : Controller
    {

		private readonly ToolDbContext _dataContext;

		public HomeController( ToolDbContext context)
		{
			_dataContext = context;
		}

		public IActionResult Index()
		{
			var product = _dataContext.Thietbis.ToList(); //hien thi them trang thai cua thiet bi
			return View(product);
		}

	}
}