using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Diagnostics;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Areas.Admin.Repositories;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PhieuMuonAdminController : Controller
    {
        IPhieuMuonAdmin pMAdmin;
        public PhieuMuonAdminController(IPhieuMuonAdmin _pMAdmin)
        {
            pMAdmin = _pMAdmin;
        }

        public IActionResult LichsuMuon(int? page,string? searchString,string? Khoa,string? Nganh,string? Trangthai)
        {
            int pageSize = 5;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Phieumuon> pm;
            List<Phieumuon> pmList = pMAdmin.GetAllPhieuMuon().OrderByDescending(x=>x.Ngaylap).ToList();

            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    pmList = pmList.Where()
            //}

            pm = new PagedList<Phieumuon>(pmList, pageNumber, pageSize);
            return View(pm);
        }

        public IActionResult ChitietPhieuMuon(int mapm)
        {
           
            PhieuMuonViewModel pm = pMAdmin.GetPhieumuonViewById(mapm);
            
            return View(pm);
        }

        [HttpPost] 
        public IActionResult ChitietPhieuMuon(PhieuMuonViewModel pm,int trangthaiduyet)
        {
            if (!ModelState.IsValid)
            {
                return View(pm);
            }
            return RedirectToAction("LichsuMuon");
        }
        
    }
}
