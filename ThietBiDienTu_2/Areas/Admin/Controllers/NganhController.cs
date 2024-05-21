using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/[controller]/[action]")]
    [AuthenticationManager]
    public class NganhController : Controller
    {
        private readonly IHttpContextAccessor contextAccess;
        private readonly ToolDbContext _context;

        public NganhController(ToolDbContext context, IHttpContextAccessor contextAccess)
        {
            _context = context;
            this.contextAccess = contextAccess;
        }

        public IActionResult Index(string? searchString, int? page)
        {
            var nganhs = string.IsNullOrEmpty(searchString)
                ? _context.Nganhs.ToList()
                : _context.Nganhs.Where(n => n.Tennganh.Contains(searchString)).ToList();
            ViewBag.searchString = searchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            var pagedNganhs = nganhs.ToPagedList(pageNumber, pageSize);
            if (IsAjaxRequest())
            {
                return PartialView("PartialViewNganh", pagedNganhs);
            }
            return View(pagedNganhs);
        }

        private bool IsAjaxRequest()
        {
            var request = contextAccess.HttpContext.Request;
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        [HttpPost]
        public async Task<IActionResult> Search(string? searchString)
        {
            var nganhs = string.IsNullOrEmpty(searchString)
                ? await _context.Nganhs.ToListAsync()
                : await _context.Nganhs.Where(n => n.Tennganh.Contains(searchString)).ToListAsync();

            return PartialView("PartialViewNganh", nganhs);
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
        public IActionResult Create([Bind("Tennganh")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nganh);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(nganh);
        }

        public IActionResult Edit(int id)
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
        public IActionResult Edit(int id, [Bind("Manganh, TenNganh")] Nganh nganh)
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
                    TempData["SuccessMessage"] = "Đã cập nhật ngành thành công";
                    return RedirectToAction(nameof(Index));
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
