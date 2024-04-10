using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Diagnostics;
using System.Linq;
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
        IPhieuMuonAdmin pMAdmin; IHttpContextAccessor contextAcc; IKhoa kRepo;
        public PhieuMuonAdminController(IPhieuMuonAdmin _pMAdmin,IHttpContextAccessor contextAcc,IKhoa kRepo)
        {
            pMAdmin = _pMAdmin;
            this.contextAcc = contextAcc;
            this.kRepo = kRepo;
        }

        public IActionResult DanhsachPhieuMuon(int? page, string? searchString,int? Makhoa, string? Trangthai,int? indexPartial,DateTime? From,DateTime? To)
        {
            int pageSize = 5;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Phieumuon> pm;
            List<Phieumuon> pmList = pMAdmin.GetAllPhieuMuon().OrderBy(x => x.Trangthai).ToList();
            CreateData(searchString, Trangthai,From,To,Makhoa.ToString());

            if (IsAjaxRequest())
            {
                DateTime today = DateTime.Now.Date;
                if (!string.IsNullOrEmpty(searchString))
                {
                    ViewBag.searchString = searchString;
                    pmList = pmList.Where(x => x.MasvNavigation.Tensv.ToLower().Contains(searchString.ToLower())).ToList();
                }
                if(Makhoa != -1 && Makhoa!=null)
                {
                    pmList = pmList.Where(x => x.MasvNavigation.Makhoa == Makhoa).ToList();
                }
                if(From.HasValue)
                {
                    pmList = pmList.Where(x => x.Ngaymuon >= From).ToList();
                }
                if(To.HasValue)
                {

                    pmList = pmList.Where(x => x.Ngaymuon.Date <=To.Value.Date).ToList();
                }
                if(!string.IsNullOrEmpty(Trangthai) && Trangthai!="-1")
                {
                    pmList = pmList.Where(x => x.Trangthai.ToString() == Trangthai).ToList();
                }

                if (indexPartial == 1)
                {
                    pmList = pmList.Where(x => x.Ngaymuon == today).ToList();
                    pm = new PagedList<Phieumuon>(pmList, pageNumber, pageSize);
                    return PartialView("_PartialTableToday",pm);
                }
                if (indexPartial == 2) //Not checked - not give back
                {
                    pm = new PagedList<Phieumuon>(pmList, pageNumber, pageSize);
                    return PartialView("_PartialTableAnother",pm);
                }
            }
            
            return View(pmList);
        }

        public IActionResult ChitietPhieuMuon(int mapm)
        {

            PhieuMuonViewModel pm = pMAdmin.GetPhieumuonViewById(mapm);

            return View(pm);
        }

        [HttpPost]
        public IActionResult ChitietPhieuMuon(PhieuMuonViewModel pm, int trangthai)
        {
            pMAdmin.DuyetPm(trangthai, pm);
            return RedirectToAction("DanhsachPhieuMuon");
        }

        public void CreateData(string? searchString,string? Trangthai,DateTime? From,DateTime? To,string? Makhoa)
        {
            List<int> trangthaiToday = pMAdmin.AllStatePhieuMuonToday();
            List<SelectListItem> selectState = trangthaiToday.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x == 0 ? "Chưa duyệt" : x == 1 ? "Chưa trả" : x == 2 ? "Đã duyệt" : x == 3 ? "Đã trả" : x == 4 ? "Từ chối" : x == 5 ? "Hủy phiếu" : ""
            }).ToList();
            ViewBag.StateSelect = selectState;
            ViewBag.searchString1 = searchString;
            ViewBag.selectState = Trangthai;
            ViewBag.borrowFrom = From.HasValue ? From.Value.ToString("yyyy-MM-dd") : "";
            ViewBag.borrowTo = To.HasValue ? To.Value.ToString("yyyy-MM-dd") : "";
            ViewBag.selectKhoa = Makhoa;

            List<SelectListItem> selectKhoa = kRepo.getAllKhoa().Select(x => new SelectListItem {
                Value = x.Makhoa.ToString(),
                Text = x.Tenkhoa
            }).ToList();

            ViewBag.selectKhoa = selectKhoa;
        }

        private bool IsAjaxRequest()
        {
            var request = contextAcc.HttpContext.Request;
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
