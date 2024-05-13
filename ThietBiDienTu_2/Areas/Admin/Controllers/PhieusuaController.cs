using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Repository;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhieusuaController : Controller
    {
        IPhieuSuaAdmin psRepo; IThietBiAdmin tbRepo;

        public PhieusuaController(IPhieuSuaAdmin psRepo, IThietBiAdmin tbRepo)
        {
            this.psRepo = psRepo;
            this.tbRepo = tbRepo;
        }

        public IActionResult DanhsachPhieuSua(int? page,string? searchString,DateTime? NgaylapTu,DateTime? NgaylapDen)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            List<Phieusua> psList = psRepo.GetAllPs();
            PagedList<Phieusua> psPaged = new PagedList<Phieusua>(psList,pageNumber,pageSize);


            return View(psPaged);
        }

        public IActionResult CreatePs()
        {
            List<TbFixAndCheck> tbFixList =tbRepo.GetTbFixAndCheckList();
            PagedList<TbFixAndCheck> pageTbFix = new PagedList<TbFixAndCheck>(tbFixList,1,5);
            List<TbFixAndCheck> tbChoosenList = HttpContext.Session.GetJson<List<TbFixAndCheck>>("Fixcart") ?? new List<TbFixAndCheck>();
            PagedList<TbFixAndCheck> pageTbChoosen = new PagedList<TbFixAndCheck>(tbChoosenList, 1, 5);

            CreatePSViewModel createPs = new CreatePSViewModel
            {
                tbList = pageTbFix,
                tbChoosen = pageTbChoosen,
                Tongchiphi = 0
            };
            return View(createPs);
        }

       

        public void AddFix(int matb)
        {
            List<TbFixAndCheck> tbFixList = HttpContext.Session.GetJson<List<TbFixAndCheck>>("Fixcart") ?? new List<TbFixAndCheck>();
            Thietbi tb = tbRepo.GetTBById(matb);
            tbFixList.Add(new TbFixAndCheck
            {
                Hinhanh = tb.MadongtbNavigation.Hinhanh,
                TenKho = tb.MapNavigation.Tenphong,
                Matb = matb,
                Seri = tb.Seri,
                Tentb = tb.MadongtbNavigation.Tendongtb,
                CheckFix = true,
                Chiphi = 0,
                Mota = ""
            });
            HttpContext.Session.SetJson("Fixcart", tbFixList);
        }
    }
}
