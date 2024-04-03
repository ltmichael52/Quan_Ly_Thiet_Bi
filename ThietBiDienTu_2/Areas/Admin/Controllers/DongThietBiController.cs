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
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DongThietBiController : Controller
    {
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
            var TooldbContext = _context.Dongthietbis.ToList();
            return View(TooldbContext);
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
            string stringhinhanh = UploadFile(dongthietbi);
            var dtb = new Dongthietbi
            {
                Madongtb = dongthietbi.Madongtb,
                Tendongtb = dongthietbi.Tendongtb,
                Soluong = dongthietbi.Soluong,
                Mota = dongthietbi.Mota,
                Hinhanh = stringhinhanh
            };
       
            // Kiểm tra xác nhận dữ liệu và thêm vào cơ sở dữ liệu
            _context.Dongthietbis.Add(dtb);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index)); // Chuyển hướng sau khi thêm thành công, bạn có thể chuyển hướng đến trang khác tùy ý
        }

        

        public IActionResult Edit(int id)
        {            
            string madongtb = (_context.Dongthietbis.Count() + 1).ToString();
            ViewBag.Madongtb = madongtb;
            Dongthietbi dtb = new Dongthietbi();
            Dongthietbi dongthietbi = _context.Dongthietbis.Single(x => x.Madongtb == id);
            return View(dongthietbi);
        }

        [HttpPost]
        public IActionResult Edit(Dongthietbi dongthietbi)
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

            if (ModelState.IsValid)
            {
                string stringhinhanh = UploadFile(dongthietbi);
                var dtb = new Dongthietbi
                {
                    Madongtb = dongthietbi.Madongtb,
                    Tendongtb = dongthietbi.Tendongtb,
                    Soluong = dongthietbi.Soluong,
                    Mota = dongthietbi.Mota,
                    Hinhanh = stringhinhanh
                };
                // Cập nhật thông tin và lưu vào cơ sở dữ liệu
                _context.Entry(dtb).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

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


        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    if (id == null || _context.Dongthietbis == null)
        //    {
        //        return NotFound();
        //    }
        //    var dongthietbi = await _context.Dongthietbis.Where(n => n.Madongtb == id).FirstOrDefaultAsync();
        //    if (dongthietbi == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(dongthietbi);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Dongthietbi dongthietbi)
        //{
        //    if (id != dongthietbi.Madongtb)
        //    {
        //        return NotFound();
        //    }

        //    string stringhinhanh = UploadFile(dongthietbi);
        //    dongthietbi.Hinhanh = stringhinhanh;

        //    _context.Update(dongthietbi);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        public IActionResult Delete(int id)
        {
            
            if (id == null || _context.Dongthietbis == null)
            {
                
                return NotFound();
            }

            var dongthietbi = _context.Dongthietbis
                .FirstOrDefault(m => m.Madongtb == id);
            if (dongthietbi == null)
            {
               
                return NotFound();
            }
            _context.Dongthietbis.Remove(dongthietbi);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
