using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ToolDetailController : Controller
    {
        ICoSoAdmin coso;  IPhongAdmin pRepo; ILoaiAdmin loai; IThietBiAdmin tb;

        public ToolDetailController(ICoSoAdmin CS, IPhongAdmin p, ILoaiAdmin l, IThietBiAdmin _tb)
        {
            coso = CS;
            pRepo = p;
            loai = l;
            tb = _tb;
        }

        public IActionResult ToolDetailList(int? page, string? searchString, string? Course, string? Major)
        {
            int pageSize = 5;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<ToolDetailAdmin> toolDetail;
            List<ToolDetailAdmin> dataList = new List<ToolDetailAdmin>();

            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    svList = svList.Where(x => x.Tensv != null && x.Tensv.ToLower().Contains(searchString.ToLower()));
            //}
            //if (!string.IsNullOrEmpty(Course) && Course != "all")
            //{
            //    svList = svList.Where(x => x.Khoa == Course);
            //}
            //if (!string.IsNullOrEmpty(Major) && Major != "all")
            //{
            //    svList = svList.Where(x => x.Nganh == Major);
            //}

            List<Coso> csList = coso.GetCoSoList();
            List<SelectListItem> csListItem = csList.Select(cs => new SelectListItem { Value = cs.Macs.ToString(),Text = cs.Tencs}).ToList();
            ViewBag.CoSoList = csListItem;


            List<Phong> roomList = pRepo.GetPhongList();
            List<SelectListItem> phongListItem = roomList.Select(p => new SelectListItem
            {
                Value = p.Map.ToString(),
                Text = p.Map.ToString() + "," +p.Macs.ToString(),
            }).ToList();
            ViewBag.PhongList = phongListItem;

            //chitietThietbiList = chitietThietbiList.OrderBy(x => x.Tensv);
            toolDetail = new PagedList<ToolDetailAdmin>(dataList, pageNumber, pageSize);
            return View(toolDetail);
        }

        public IActionResult CreateToolDetail()
        {
            List<Loaithietbi> loaiList = loai.GetAllLoai();
            List<SelectListItem> loaiSelect = loaiList.Select(l=>new SelectListItem
            {
                Value = l.Maloai,
                Text = l.Tenloai
            }).ToList();
            ViewBag.LoaiList = loaiSelect;


            List<Thietbi> tbList = tb.GetAllThietBi();
            List<SelectListItem> tbSelect = tbList.Select(tb => new SelectListItem
            {
                Value = tb.Matb,
                Text = tb.Tenthietbi
            }).ToList();
            ViewBag.TbList = tbSelect;


            List<Coso> csList = coso.GetCoSoList();
            List<SelectListItem> csListItem = csList.Select(cs => new SelectListItem { Value = cs.Macs.ToString(), Text = cs.Tencs }).ToList();
            ViewBag.CoSoList = csListItem;


            List<Phong> roomList = pRepo.GetPhongList();
            List<SelectListItem> phongListItem = roomList.Select(p => new SelectListItem
            {
                Value = p.Map.ToString(),
                Text = p.Map.ToString() + "-" + p.Tenphong + "," + p.Macs.ToString(),
            }).ToList();
            ViewBag.PhongList = phongListItem;
            return View();
        }

        //[HttpPost]
        //public IActionResult Create(Sinhvien sv)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View(sv);
        //    }
        //    else if (_context.Sinhviens.Any(x => x.Masv == sv.Masv))
        //    {
        //        ModelState.AddModelError("Masv", "Mã nhân viên đã tồn tại");
        //        return View(sv);
        //    }
        //    else
        //    {
        //        Account newAcc = new Account
        //        {
        //            Username = sv.Masv,
        //            Password = "123456",
        //            Loaiuser = 0
        //        };
        //        _context.Accounts.Add(newAcc);
        //        _context.Sinhviens.Add(sv);
        //        _context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //}
    }
}
