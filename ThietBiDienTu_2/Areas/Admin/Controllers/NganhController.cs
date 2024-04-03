using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/[controller]/[action]")]
    public class NganhController : Controller
    {
        private readonly ToolDbContext _context;

        public NganhController(ToolDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Nganhs.ToList());
        }

        public IActionResult Details(int id)
        {
            var nganh = _context.Nganhs.FirstOrDefault(m => m.Manganh == id);
            if (nganh == null)
            {
                return NotFound();
            }

            return View(nganh);
        }
        // NganhController
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TenNganh")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nganh);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(nganh);
        }

        public IActionResult Edit(string id)
        {
            var nganh = _context.Nganhs.Find(id);
            if (nganh == null)
            {
                return NotFound();
            }
            return View(nganh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("TenNganh")] Nganh nganh)
        {
            if (id != nganh.Manganh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nganh);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NganhExists(nganh.Manganh))
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
            return View(nganh);
        }

        public IActionResult Delete(int id)
        {
            var nganh = _context.Nganhs.Find(id);
            if (nganh == null)
            {
                return NotFound();
            }

            _context.Nganhs.Remove(nganh);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool NganhExists(int id)
        {
            return _context.Nganhs.Any(e => e.Manganh == id);
        }

    }
}
