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
    [Route("admin/[controller]/[action]/{id?}")]
    [AuthenticationManager]
    public class UserStudentController : Controller
    {
        ToolDbContext _context;
        public UserStudentController(ToolDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page, string? searchString, string? Course, string? Major)
        {
            int pageSize = 5;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Sinhvien> sv;
            var svList = _context.Sinhviens.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                svList = svList.Where(x => x.Tensv != null && x.Tensv.ToLower().Contains(searchString.ToLower()));
            }
            if (!string.IsNullOrEmpty(Course) && Course != "all")
            {
                svList = svList.Where(x => x.Khoa == Course);
            }
            if (!string.IsNullOrEmpty(Major) && Major != "all")
            {
                svList = svList.Where(x => x.Nganh == Major);
            }


            svList = svList.OrderBy(x => x.Tensv);
            sv = new PagedList<Sinhvien>(svList, pageNumber, pageSize);
            return View(sv);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Sinhvien sv)
        {

            if (!ModelState.IsValid)
            {
                return View(sv);
            }
            else if (_context.Sinhviens.Any(x => x.Masv == sv.Masv))
            {
                ModelState.AddModelError("Masv", "Mã nhân viên đã tồn tại");
                return View(sv);
            }
            else
            {
                Account newAcc = new Account
                {
                    Username = sv.Masv,
                    Password = "123456",
                    Loaiuser = 0
                };
                _context.Accounts.Add(newAcc);
                _context.Sinhviens.Add(sv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Update(int id)
        {
            Sinhvien _sv = _context.Sinhviens.FirstOrDefault(x => x.Masv == id);
            Account _acc = _context.Accounts.FirstOrDefault(x => x.Username == id);
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
            Debug.WriteLine("In update post");
            if (!ModelState.IsValid)
            {
                return View(stu_acc);
            }
            else if (_context.Sinhviens.Any(x => x.Masv == stu_acc.sv.Masv && x.Masv != stu_acc.acc.Username))
            {
                ModelState.AddModelError("sv.Masv", "Mã sinh viên đã tồn tại");
                return View(stu_acc);
            }
            else
            {
                //Update method doesn't track the change of the primary key 
                //So need to remove then add new one
                Account acc = _context.Accounts.FirstOrDefault(x => x.Username == stu_acc.acc.Username);
                Sinhvien stu = _context.Sinhviens.FirstOrDefault(x => x.Masv == stu_acc.acc.Username);

                //Remove stu first cuz in database stu has foreign key to acc
                _context.Sinhviens.Remove(stu);
                _context.Accounts.Remove(acc);

                stu_acc.acc.Username = stu_acc.sv.Masv;
                stu_acc.acc.Password = stu_acc.acc.Password;
                stu_acc.sv.MasvNavigation = stu_acc.acc;
                _context.Accounts.Add(stu_acc.acc);
                _context.Sinhviens.Add(stu_acc.sv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int id)
        {
            Sinhvien sv = _context.Sinhviens.Find(id);
            Account acc = _context.Accounts.Find(id);

            //Remove stu first cuz in database stu has foreign key to acc
            _context.Sinhviens.Remove(sv);
            _context.Accounts.Remove(acc);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
