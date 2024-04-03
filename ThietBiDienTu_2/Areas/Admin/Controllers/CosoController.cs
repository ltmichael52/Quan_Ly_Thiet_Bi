using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
	[Area("admin")]
	
	public class CosoController : Controller
	{
		private readonly ToolDbContext _context;

		public CosoController(ToolDbContext context)
		{
			_context = context;
		}

		// GET: Cosos
		public async Task<IActionResult> Index()
		{
			return View(await _context.Cosos.ToListAsync());
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
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(coso);
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
		public async Task<IActionResult> Edit(int id, [Bind("Tencs,Diachi")] Coso coso)
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
			_context.Cosos.Remove(coso);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CosoExists(int id)
		{
			return _context.Cosos.Any(e => e.Macs == id);
		}
	}

}
