using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    [AuthenticationManager]
    public class UserStudentController : Controller
    {
        ToolDbContext _context;
        public UserStudentController(ToolDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page, string? searchString, int? Course, int? Major)
        {
            CreateData();
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Sinhvien> sv;
            var svList = _context.Sinhviens.Select(x => new Sinhvien
            {
                Masv = x.Masv,
                Tensv = x.Tensv,
                Makhoa = x.Makhoa,
                Manganh = x.Manganh,
                Email = x.Email,
                Sdt = x.Sdt,
                Ngaysinh = x.Ngaysinh,
                Gioitinh = x.Gioitinh,
                MakhoaNavigation = _context.Khoas.FirstOrDefault(a=>a.Makhoa ==x.Makhoa),
                ManganhNavigation = _context.Nganhs.FirstOrDefault(a => a.Manganh == x.Makhoa),

            }).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                svList = svList.Where(x => x.Tensv != null && x.Tensv.ToLower().Contains(searchString.ToLower()));
            }
            if (Course != null && Course != 0)
            {
                svList = svList.Where(x => x.Makhoa == Course);
            }
            if (Major!= null &&Major!=0)
            {
                svList = svList.Where(x => x.Manganh == Major);
            }


            svList = svList.OrderBy(x => x.Tensv);
            sv = new PagedList<Sinhvien>(svList, pageNumber, pageSize);
            return View(sv);
        }
        public IActionResult Create()
        {
            CreateData();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Sinhvien sv)
        {
            CreateData();

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
            
            return View(sv);
            }
            else if (_context.Sinhviens.Any(x => x.Masv == sv.Masv))
            {
                ModelState.AddModelError("Masv", "Mã sinh viên đã tồn tại");
                return View(sv);
            }
            else
            {
                Taikhoan newAcc = new Taikhoan
                {
                    Matk = sv.Masv,
                    Matkhau = "123456",
                    Loaitk = 0
                };
                _context.Taikhoans.Add(newAcc);

                sv.MakhoaNavigation = null;
                sv.ManganhNavigation = null;
                sv.MasvNavigation = newAcc;
                _context.Sinhviens.Add(sv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }

        public IActionResult Update(int id)
        {
            CreateData();
            Sinhvien _sv = _context.Sinhviens.FirstOrDefault(x => x.Masv == id);
            Taikhoan _acc = _context.Taikhoans.FirstOrDefault(x => x.Matk == id);
            StuAcc stuacc = new StuAcc
            {
                sv = _sv,
                acc = _acc
            };
            return View(stuacc);
        }

        [HttpPost]
        public IActionResult Update(StuAcc stu_acc)
        {
            CreateData();
            Debug.WriteLine("In update post");
            if (!ModelState.IsValid)
            {
                return View(stu_acc);
            }
            else if (_context.Sinhviens.Any(x => x.Masv == stu_acc.sv.Masv && x.Masv != stu_acc.acc.Matk))
            {
                ModelState.AddModelError("sv.Masv", "Mã sinh viên đã tồn tại");
                return View(stu_acc);
            }
            else
            {
                //Update method doesn't track the change of the primary key 
                //So need to remove then add new one
                Taikhoan acc = _context.Taikhoans.FirstOrDefault(x => x.Matk == stu_acc.acc.Matk);
                Sinhvien stu = _context.Sinhviens.FirstOrDefault(x => x.Masv == stu_acc.acc.Matk);

                //Remove stu first cuz in database stu has foreign key to acc
                _context.Sinhviens.Remove(stu);
                _context.Taikhoans.Remove(acc);

                stu_acc.acc.Matk = stu_acc.sv.Masv;
                stu_acc.acc.Matkhau = stu_acc.acc.Matkhau;
                stu_acc.sv.MasvNavigation = stu_acc.acc;
                _context.Taikhoans.Add(stu_acc.acc);
                _context.Sinhviens.Add(stu_acc.sv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int id)
        {
            Sinhvien sv = _context.Sinhviens.Find(id);
            Taikhoan acc = _context.Taikhoans.Find(id);

            //Remove stu first cuz in database stu has foreign key to acc
            _context.Sinhviens.Remove(sv);
            _context.Taikhoans.Remove(acc);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public void CreateData()
        {
            List<SelectListItem> khoaType = _context.Khoas.Select(x => new SelectListItem
            {
                Value = x.Makhoa.ToString(),
                Text = x.Tenkhoa
            }).ToList();
            ViewBag.khoaType = khoaType;
            List<SelectListItem> nganhType = _context.Nganhs.Select(x => new SelectListItem
            {
                Value = x.Manganh.ToString(),
                Text = x.Tennganh
            }).ToList();
            ViewBag.nganhType = nganhType;
        }
    }
}
