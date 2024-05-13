using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using ThietBiDienTu_2.Models.Authentication;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    [AuthenticationM_S]
    public class PhongController : Controller
    {
        private readonly ICoSoAdmin csRepo;
        private readonly IPhongAdmin phongRepo;
        private readonly IHttpContextAccessor contextAccess;

        public PhongController(ICoSoAdmin csRepo, IPhongAdmin phongRepo, IHttpContextAccessor contextAccess)
        {
            this.csRepo = csRepo;
            this.phongRepo = phongRepo;
            this.contextAccess = contextAccess;
        }

        private void GetData()
        {
            List<SelectListItem> loaiphong = new List<SelectListItem>
            {
                new SelectListItem { Text = "Phòng Kho", Value = "kho" },
                new SelectListItem { Text = "Phòng Nhân Viên", Value = "nhanvien" },
                new SelectListItem { Text = "Phòng Học", Value = "Phòng học" },
                new SelectListItem { Text = "Phòng Chung", Value = "chung" }
            };
            ViewBag.RoomTypes = loaiphong;

            List<SelectListItem> coso = csRepo.GetCoSoList().Select(x => new SelectListItem
            {
                Value = x.Macs.ToString(),
                Text = x.Tencs
            }).ToList();
            ViewBag.Coso = coso;
        }

        // GET: Phongs
        public async Task<IActionResult> Index(string? searchStringController, string? filterLoaiphongController, string? filterCosoController, int? page)
        {
            GetData();

            var phongs = phongRepo.GetPhongList();

            if (!string.IsNullOrEmpty(searchStringController))
            {
                phongs = phongs.Where(x => x.Tenphong.ToLower().Contains(searchStringController.ToLower())).ToList();
                ViewBag.searchStringController = searchStringController;
            }
            if (!string.IsNullOrEmpty(filterLoaiphongController))
            {
                phongs = phongs.Where(x => x.Loaiphong == filterLoaiphongController).ToList();
                ViewBag.filterLoaiphongController = filterLoaiphongController;
            }
            if (!string.IsNullOrEmpty(filterCosoController))
            {
                phongs = phongs.Where(x => x.Macs.ToString() == filterCosoController).ToList();
                ViewBag.filterCosoController = filterCosoController;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var pagedPhongs = phongs.ToPagedList(pageNumber, pageSize);

            if (IsAjaxRequest())
            {
                return PartialView("PartialViewPhong", pagedPhongs);
            }
            return View(pagedPhongs);
        }

        private bool IsAjaxRequest()
        {
            var request = contextAccess.HttpContext.Request;
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        // GET: Phongs/Create
        public IActionResult Create()
        {
            GetData();
            return View();
        }

        // POST: Phongs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Map,Macs,Tenphong,Loaiphong")] Phong phong)
        {

            phongRepo.Add(phong);
            TempData["Action"] = "Tạo thành công";
            return RedirectToAction(nameof(Index));

            GetData();
            return View(phong);
        }



        // GET: Phongs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phong = phongRepo.FindPhong(id);
            if (phong == null)
            {
                return NotFound();
            }

            GetData();
            return View(phong);
        }

        // POST: Phongs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Map,Macs,Tenphong,Loaiphong,Douutien")] Phong phong)
        {
            if (id != phong.Map)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    phongRepo.Update(phong);
                    TempData["Action"] = "Chỉnh sửa thành công";
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
            GetData();
            return View(phong);
        }

        // GET: Phongs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phong = phongRepo.FindPhong(id);
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
            var phong = phongRepo.FindPhong(id);

            try
            {
                // Kiểm tra xem có thiết bị nào được gắn với phòng hay không
                bool phongContainsDevices = phongRepo.CheckIfPhongHasDevices(id);

                if (phongContainsDevices)
                {
                    TempData["ErrorMessage"] = "Không thể xóa phòng có chứa thiết bị.";
                    return RedirectToAction(nameof(Index));
                }

                phongRepo.Delete(phong);
                TempData["Action"] = "Xóa thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Xử lý bất kỳ lỗi nào xảy ra trong quá trình xóa phòng
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa phòng: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool PhongExists(string id)
        {
            return phongRepo.FindPhong(id) == null;

        }
    }
}