using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    [AuthenticationManager]
    public class CosoController : Controller
    {
        private readonly ICoSoAdmin csRepo;
        private readonly ToolDbContext _context;

        public CosoController(ToolDbContext context)
        {
            _context = context;
        }

        // GET: Cosos
        public async Task<IActionResult> Index(string? searchString, int? page)
        {

            var cosos = _context.Cosos.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                cosos = cosos.Where(x => x.Tencs.ToLower().Contains(searchString.ToLower()));
                ViewBag.searchString = searchString;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var pagedCosos = await cosos.ToPagedListAsync(pageNumber, pageSize);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("PartialViewCoSo", pagedCosos);
            }

            return View(pagedCosos);
        }


        private bool IsAjaxRequest()
        {
            throw new NotImplementedException();
        }


        // GET: Cosos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cosos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Tencs,Diachi")] Coso coso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coso);
                TempData["AlertMessage"] = "Đã tạo cơ sở thành công";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coso);
        }

        [HttpPost]
        public IActionResult Search(string? searchString)
        {
            var cosos = _context.Cosos.Where(c => c.Tencs.ToLower().Contains(searchString.ToLower())).ToList();
            return PartialView("PartialViewCoSo", cosos);
        }


        // GET: Cosos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coso = await _context.Cosos.FindAsync(id);
            if (coso == null)
            {
                return NotFound();
            }
            return View(coso);
        }

        // POST: Cosos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Macs,Tencs,Diachi")] Coso coso)
        {
            if (id != coso.Macs)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coso);
                    TempData["AlertMessage"] = "Đã chỉnh sửa cơ sở thành công";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CosoExists(coso.Macs))
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
            return View(coso);
        }

        private bool CosoExists(int macs)
        {
            return _context.Cosos.Any(e => e.Macs == macs);
        }


        // GET: Cosos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coso = await _context.Cosos
                .FirstOrDefaultAsync(m => m.Macs == id);
            if (coso == null)
            {
                return NotFound();
            }

            return View(coso);
        }

        // POST: Cosos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coso = await _context.Cosos.FindAsync(id);

            // Kiểm tra xem cơ sở có chứa phòng hay không
            var hasRooms = _context.Phongs.Any(p => p.Macs == id);

            if (hasRooms)
            {
                TempData["AlertMessage"] = "Cơ sở này đã có phòng";
                return RedirectToAction(nameof(Index));
            }

            _context.Cosos.Remove(coso);
            TempData["AlertMessage"] = "Đã xóa cơ sở thành công";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }

}
