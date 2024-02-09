using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class UserEmployeeController : Controller
    {
        ToolDbContext _context;
        public UserEmployeeController(ToolDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Nhanvien> nvList= _context.Nhanviens.ToList();
            return View(nvList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Nhanvien nv)
        {
            if (!ModelState.IsValid)
            {               
                return View();
            }
            else
            {
                Account nacc = new Account
                {
                    Username = nv.Manv,
                    Password = "123456",
                    LoaiUser = 1
                };
                nv.Account = nacc;
                _context.Accounts.Add(nacc);
                _context.Nhanviens.Add(nv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Update(int id)
        {
            Debug.WriteLine("Id in update: ", id);
            Nhanvien nv = _context.Nhanviens.FirstOrDefault(x=> x.Manv == id);
            return View(nv);
        }
    }
}
