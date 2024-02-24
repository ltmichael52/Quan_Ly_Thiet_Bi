using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;

namespace ThietBiDienTu_2.Controllers
{

    public class HomeController : Controller
    {
        private readonly ToolDbContext _dataContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ToolDbContext context)
        {
            _logger = logger;
            _dataContext = context;
        }
        
        public IActionResult Index()
        {
            var product = _dataContext.Thietbis.ToList(); //hien thi them trang thai cua thiet bi
            return View(product);
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