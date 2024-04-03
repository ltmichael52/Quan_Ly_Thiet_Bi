using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
	[Area("admin")]
	public class PhongController : Controller
	{
		private readonly ToolDbContext _context;

		public PhongController(ToolDbContext context)
		{
			_context = context;
		}

		// GET: Phongs
		public async Task<IActionResult> Index()
		{
			var phongs = _context.Phongs.Include(p => p.MacsNavigation);
			return View(await phongs.ToListAsync());
		}

		// GET: Phongs/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Phongs/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Map,Macs,Tenphong,Loaiphong")] Phong phong)
		{
			if (ModelState.IsValid)
			{
				_context.Add(phong);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(phong);
		}

		// GET: Phongs/Edit/5
		public async Task<IActionResult> Edit(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var phong = await _context.Phongs.FindAsync(id);
			if (phong == null)
			{
				return NotFound();
			}
			return View(phong);
		}

		// POST: Phongs/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("Map,Macs,Tenphong,Loaiphong")] Phong phong)
		{
			if (id != phong.Map)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(phong);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PhongExists(phong.Map))
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
			return View(phong);
		}

		// GET: Phongs/Delete/5
		public async Task<IActionResult> Delete(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var phong = await _context.Phongs
				.FirstOrDefaultAsync(m => m.Map == id);
			if (phong == null)
			{
				return NotFound();
			}

			return View(phong);
		}

		// POST: Phongs/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var phong = await _context.Phongs.FindAsync(id);
			_context.Phongs.Remove(phong);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool PhongExists(string id)
		{
			return _context.Phongs.Any(e => e.Map == id);
		}
	}

}
