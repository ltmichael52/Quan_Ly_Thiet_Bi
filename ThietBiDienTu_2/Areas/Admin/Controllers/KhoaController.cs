using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    [AuthenticationManager]
    public class KhoaController : Controller
    {
        private readonly ToolDbContext _context;

        public KhoaController(ToolDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? page)
        {
            ViewBag.CurrentFilter = searchString;

            var pageNumber = page ?? 1; // nếu page không có giá trị, mặc định là 1
            var pageSize = 10; // số lượng phần tử trên mỗi trang

                var khoas = string.IsNullOrEmpty(searchString)
              ? await _context.Khoas.ToPagedListAsync(pageNumber, pageSize)
              : await _context.Khoas.Where(k => k.Tenkhoa.Contains(searchString)).ToPagedListAsync(pageNumber, pageSize);
            
          

            if (IsAjaxRequest())
            {
                return PartialView("PartialViewKhoa", khoas);
            }
            return View(khoas);
        }

        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }


        public IActionResult Details(int id)
        {
            var khoa = _context.Khoas.FirstOrDefault(m => m.Makhoa == id);
            if (khoa == null)
            {
                return NotFound();
            }

            return View(khoa);
        }
        [HttpPost]
        public IActionResult Search(string searchString)
        {
            var khoas = _context.Khoas.Where(k => k.Tenkhoa.Contains(searchString)).ToList();
            if (IsAjaxRequest())
            {
                return PartialView("PartialViewKhoa", khoas);
            }
            return View(khoas);
        }




        // KhoaController
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Tenkhoa")] Khoa khoa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khoa);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(khoa);
        }

        public IActionResult Edit(int id)
        {
            var khoa = _context.Khoas.Find(id);
            if (khoa == null)
            {
                return NotFound();
            }
            return View(khoa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("MaKhoa,TenKhoa")] Khoa khoa)
        {
            if (id != khoa.Makhoa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khoa);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhoaExists(khoa.Makhoa))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khoa);
        }

        public IActionResult Delete(int id)
        {
            var khoa = _context.Khoas.Find(id);
            if (khoa == null)
            {
                return NotFound();
            }

            _context.Khoas.Remove(khoa);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool KhoaExists(int id)
        {
            return _context.Khoas.Any(e => e.Makhoa == id);
        }

    }
}