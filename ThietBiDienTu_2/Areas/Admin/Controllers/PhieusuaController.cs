using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Models;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhieusuaController : Controller
    {
        IPhieuSuaAdmin psRepo;

        public PhieusuaController(IPhieuSuaAdmin psRepo)
        {
            this.psRepo = psRepo;
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
            return View();
        }
    }
}
