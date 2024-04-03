using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
   
    public class KhoaController : Controller
    {
        private readonly ToolDbContext _context;

        public KhoaController(ToolDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Khoas.ToList());
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
        // KhoaController
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TenKhoa")] Khoa khoa)
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
        public IActionResult Edit(int id, [Bind("TenKhoa")] Khoa khoa)
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