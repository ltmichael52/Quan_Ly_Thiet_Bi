using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
	[Area("admin")]
	[AuthenticationM_S]
	public class DongThietBiController : Controller
	{//Test
		private readonly ToolDbContext _context;
		private readonly IWebHostEnvironment _webHost;
		private readonly IDongThietBiAdmin _idongthietbirepo;

		public DongThietBiController(ToolDbContext context, IWebHostEnvironment webHost, IDongThietBiAdmin idongthietbirepo)
		{
			_context = context;
			_webHost = webHost;
			_idongthietbirepo = idongthietbirepo;
		}


		public async Task<IActionResult> Index()
		{
			List<DongThietBiViewModel> dongthietbiList = new List<DongThietBiViewModel>();

			// Lấy danh sách các dòng thiết bị từ cơ sở dữ liệu
			var allDongThietBi = _context.Dongthietbis.Include(d => d.Thietbis).ToList();

			foreach (var dongthietbi in allDongThietBi)
			{
				// Tính số lượng thiết bị hoạt động của đối tượng DongThietBi
				int soLuongHoatDong = dongthietbi.Thietbis.Count(t => t.Trangthai == "Hoạt động");

				// Tính số lượng thiết bị hỏng của đối tượng DongThietBi
				int soLuongHu = dongthietbi.Thietbis.Count(t => t.Trangthai == "Hư");

				// Tính số lượng thiết bị Sẵn sàng của đối tượng DongThietBi
				int soLuongTonKho = dongthietbi.Thietbis.Count(t => t.Trangthai == "Sẵn sàng");

				// Tạo một đối tượng ViewModel mới để chứa thông tin
				var dongThietBiViewModel = new DongThietBiViewModel
				{
					Madongtb = dongthietbi.Madongtb,
					Tendongtb = dongthietbi.Tendongtb,
					Soluong = dongthietbi.Soluong,
					Mota = dongthietbi.Mota,
					Hinhanh = dongthietbi.Hinhanh,
					SoLuongHoatDong = soLuongHoatDong,
					SoLuongHu = soLuongHu,
					SoLuongTonKho = soLuongTonKho
				};

				// Thêm đối tượng ViewModel vào danh sách
				dongthietbiList.Add(dongThietBiViewModel);
			}

			return View(dongthietbiList); // Truyền đúng kiểu dữ liệu cho View
		}


		public IActionResult CreateTool()
		{
			ViewData["YourDropdownData"] = _context.Dongthietbis.ToList();
			string madongtb = _context.Dongthietbis.Select(x => x.Madongtb).Count() + 1.ToString();
			ViewBag.Madongtb = madongtb;
			Dongthietbi dongthietbi = new Dongthietbi();
			return View(dongthietbi);
		}
		[HttpPost]
		public IActionResult CreateTool(Dongthietbi dongthietbi)
		{
			if (ModelState.IsValid)
			{
				// Kiểm tra các điều kiện trước khi thêm vào cơ sở dữ liệu
				if (string.IsNullOrEmpty(dongthietbi.Tendongtb))
				{
					ModelState.AddModelError("Tendongtb", "Tên dòng thiết bị không được bỏ trống");
				}
				else if (_context.Dongthietbis.Any(d => d.Tendongtb == dongthietbi.Tendongtb))
				{
					ModelState.AddModelError("Tendongtb", "Tên dòng thiết bị đã tồn tại");
				}

				if (dongthietbi.Soluong <= 0)
				{
					ModelState.AddModelError("Soluong", "Số lượng phải lớn hơn 0");
				}

				if (ModelState.IsValid)
				{
					// Tiến hành thêm vào cơ sở dữ liệu
					string stringhinhanh = UploadFile(dongthietbi);
					
					var dtb = new Dongthietbi
					{
						Madongtb = dongthietbi.Madongtb,
						Tendongtb = dongthietbi.Tendongtb,
						Soluong = dongthietbi.Soluong,
						Mota = dongthietbi.Mota,
						Hinhanh = stringhinhanh
					};

					_context.Dongthietbis.Add(dtb);
					_context.SaveChanges();
					TempData["AlertMessage"] = "Đã thêm dòng thiết bị thành công";
					return RedirectToAction(nameof(Index)); // Chuyển hướng sau khi thêm thành công
				}
			}

			return View(dongthietbi); // Hiển thị lại form với thông báo lỗi nếu có
		}


		public IActionResult Edit(int id)
		{
			Dongthietbi dongthietbi = _idongthietbirepo.GetDtbById(id);
			ViewBag.Hinhanh = dongthietbi.Hinhanh;
			return View(dongthietbi);
		}

		[HttpPost]
		public IActionResult Edit(Dongthietbi dongthietbi,string OldHinhAnh)
		{
			if (!ModelState.IsValid)
			{
				// Print out model state errors
				foreach (var modelState in ModelState.Values)
				{
					foreach (var error in modelState.Errors)
					{
						// You can print out the error message or handle it as you like
						string errorMessage = error.ErrorMessage;
						string exceptionMessage = error.Exception?.Message;

						// Print error message to the console or log it
						Debug.WriteLine($"Error: {errorMessage}");

						// Handle exception message if there's an exception associated with the error
						if (exceptionMessage != null)
						{
							Debug.WriteLine($"Exception: {exceptionMessage}");
						}
					}
				}
			}

            string stringhinhanh = UploadFile(dongthietbi);

            dongthietbi.Hinhanh = stringhinhanh ?? OldHinhAnh;
            // Cập nhật thông tin và lưu vào cơ sở dữ liệu
            _idongthietbirepo.Updatedtb(dongthietbi);
            TempData["AlertMessage"] = "Đã chỉnh sửa dòng thiết bị thành công";
            return RedirectToAction("Index");

            // Nếu dữ liệu không hợp lệ, trả về view và giữ nguyên các giá trị đã nhập
            string madongtb = (_context.Dongthietbis.Count() + 1).ToString();
			ViewBag.Madongtb = madongtb;
			return View(dongthietbi);
		}


		private string UploadFile(Dongthietbi dongthietbi)
		{
			string fileName = null;
			if (dongthietbi.hinhanh != null)
			{
				string uploadDir = Path.Combine(_webHost.WebRootPath, "images");
				fileName = Guid.NewGuid().ToString() + "-" + dongthietbi.hinhanh.FileName;
				string filePath = Path.Combine(uploadDir, fileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					dongthietbi.hinhanh.CopyTo(fileStream);
				}
			}
			return fileName;
		}

        public IActionResult Delete(int id)
        {
            if (id == null || _context.Dongthietbis == null)
            {
                return NotFound();
            }

            var dongthietbi = _context.Dongthietbis
                .Include(d => d.Thietbis) // Load các Thietbi thuộc về Dongthietbi
                .FirstOrDefault(m => m.Madongtb == id);

            if (dongthietbi == null)
            {
                return NotFound();
            }

            if (dongthietbi.Thietbis != null && dongthietbi.Thietbis.Any())
            {
                TempData["DangerMessage"] = "Không thể xóa dòng thiết bị vì còn tồn tại thiết bị thuộc dòng này.";
                return RedirectToAction("Index");
            }

            _context.Dongthietbis.Remove(dongthietbi);
            _context.SaveChanges();
            TempData["AlertMessage"] = "Đã xóa dòng thiết bị thành công";
            return RedirectToAction("Index");
        }


        public ActionResult Details(int id)
		{
			var dtb = _context.Dongthietbis.Find(id);
			return View(dtb);
		}
	}
}
