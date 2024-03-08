using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    [AuthenticationManager]
    public class UserEmployeeController : Controller
    {
        ToolDbContext _context;
        public UserEmployeeController(ToolDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page, string? searchString)
        {
            int pageSize = 5;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Nhanvien> nv;
            if (searchString != null)
            {
                ViewData["CurrentFilter"] = searchString;
                var nvList1 = _context.Nhanviens
                               .Where(emp => emp.Tennv.ToLower().Contains(searchString.ToLower()))
                               .Select(emp => new Nhanvien
                               {
                                   Manv = emp.Manv,
                                   Tennv = emp.Tennv,
                                   Diachi = emp.Diachi,
                                   Email = emp.Email,
                                   Ngaysinh = emp.Ngaysinh,
                                   Gioitinh = emp.Gioitinh,
                                   Sdt = emp.Sdt,
                                   ManvNavigation = emp.ManvNavigation
                               })
                               .OrderBy(emp => emp.Tennv);
                nv = new PagedList<Nhanvien>(nvList1, pageNumber, pageSize);
            }
            else
            {
                var nvList2 = _context.Nhanviens.AsNoTracking().OrderBy(x => x.Tennv);
                nv = new PagedList<Nhanvien>(nvList2, pageNumber, pageSize);
            }


            return View(nv);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Nhanvien nv)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (_context.Nhanviens.Any(x => x.Manv == nv.Manv))
            {
                ModelState.AddModelError("Manv", "Mã nhân viên đã tồn tại");
                return View(nv);
            }
            else
            {
                Account nacc = new Account
                {
                    Username = nv.Manv,
                    Password = "123456",
                    Loaiuser = 1
                };
                nv.ManvNavigation = nacc;
                _context.Accounts.Add(nacc);
                _context.Nhanviens.Add(nv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Update(int id)
        {
            Nhanvien _nv = _context.Nhanviens.FirstOrDefault(x => x.Manv == id);
            Account _acc = _context.Accounts.FirstOrDefault(x => x.Username == id);
            EmpAcc empacc = new EmpAcc
            {
                nv = _nv,
                acc = _acc
            };
            return View(empacc);
        }

        [HttpPost]
        public IActionResult Update(EmpAcc emp_acc)
        {
            if (!ModelState.IsValid)
            {
                return View(emp_acc);
            }
            else if (_context.Nhanviens.Any(x => x.Manv == emp_acc.nv.Manv && x.Manv != emp_acc.acc.Username))
            {
                ModelState.AddModelError("nv.Manv", "Mã nhân viên đã tồn tại");
                return View(emp_acc);
            }
            else
            {
                //Update method doesn't track the change of the primary key 
                //So need to remove then add new one
                Account acc = _context.Accounts.FirstOrDefault(x => x.Username == emp_acc.acc.Username);
                Nhanvien emp = _context.Nhanviens.FirstOrDefault(x => x.Manv == emp_acc.acc.Username);

                //Remove emp first cuz in database emp has foreign key to acc
                _context.Nhanviens.Remove(emp);
                _context.Accounts.Remove(acc);

                emp_acc.acc.Username = emp_acc.nv.Manv;
                emp_acc.acc.Password = emp_acc.acc.Password;
                emp_acc.nv.ManvNavigation = emp_acc.acc;
                _context.Accounts.Add(emp_acc.acc);
                _context.Nhanviens.Add(emp_acc.nv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int id)
        {
            Nhanvien emp = _context.Nhanviens.Find(id);
            Account acc = _context.Accounts.Find(id);

            //Remove emp first cuz in database emp has foreign key to acc
            _context.Nhanviens.Remove(emp);
            _context.Accounts.Remove(acc);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
