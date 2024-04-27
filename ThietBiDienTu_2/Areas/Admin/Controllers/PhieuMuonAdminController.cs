using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Areas.Admin.Repositories;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using ThietBiDienTu_2.Repository;
using X.PagedList;

namespace ThietBiDienTu_2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PhieuMuonAdminController : Controller
    {
        IPhieuMuonAdmin pMAdmin; IHttpContextAccessor contextAcc; IKhoa kRepo;INhanVien nvRepo;
        ISinhvienAdmin svRepo; IDongThietBiAdmin dongtbRepo;
        public PhieuMuonAdminController(IPhieuMuonAdmin _pMAdmin,IHttpContextAccessor contextAcc,IKhoa kRepo,
            INhanVien nvRepo,ISinhvienAdmin svRepo,IDongThietBiAdmin dongtbRepo)
        {
            pMAdmin = _pMAdmin;
            this.contextAcc = contextAcc;
            this.kRepo = kRepo;
            this.nvRepo= nvRepo;
            this.svRepo = svRepo;
            this.dongtbRepo = dongtbRepo;
        }

        public IActionResult DanhsachPhieuMuon(int? page, string? searchString,string? Makhoa, string? Trangthai,int? indexPartial,DateTime? From,DateTime? To)
        {
            int pageSize = 5;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Phieumuon> pm;
            List<Phieumuon> pmList = pMAdmin.GetAllPhieuMuon().OrderBy(x => x.Trangthai).ToList();
            CreateData(searchString, Trangthai,From,To,Makhoa);

            if (IsAjaxRequest())
            {
                DateTime today = DateTime.Now.Date;
                if (!string.IsNullOrEmpty(searchString))
                {
                    ViewBag.searchString = searchString;
                    pmList = pmList.Where(x => x.MasvNavigation.Tensv.ToLower().Contains(searchString.ToLower())).ToList();
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
                if(!string.IsNullOrEmpty(Makhoa) && Makhoa != "-1")
                {
                    pmList = pmList.Where(x => x.MasvNavigation.Makhoa.ToString() == Makhoa).ToList();
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
         
            pm.Manv = HttpContext.Session.GetInt32("UserName") ?? 0;
			pMAdmin.DuyetPm(trangthai, pm);
            TempData["Duyet"] = "Duyệt thành công";
            return RedirectToAction("DanhsachPhieuMuon");
        }

        public IActionResult CreatePm(int? page, string? searchString,int? masv,DateTime Ngaymuon,string? Lydomuon,bool checkInBorrow=false)
        {
            int pageSize = 9;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            List<DongTbAndAmount> dongtbList = countDongTbAndAmount(Ngaymuon);
            
            if (checkInBorrow == false)
            {
                HttpContext.Session.Remove("BorrowCart");
            }
           
            PagedList<DongTbAndAmount> pagedongtb = new PagedList<DongTbAndAmount>(dongtbList, pageNumber, pageSize);
            CreatePmViewModel crPmview = new CreatePmViewModel();
            crPmview.pagedongtb = pagedongtb;

            if (IsAjaxRequest())
            {

                if (masv != null && masv != 0)
                {
                    Sinhvien sv = svRepo.GetSvById(masv ?? 0);
                    if (sv != null)
                    {
                        crPmview.TenNganh = sv.ManganhNavigation.Tennganh;
                        crPmview.TenKhoa = sv.MakhoaNavigation.Tenkhoa;
                        crPmview.Tensv = sv.Tensv;
                    }

                    return PartialView("_PartialViewCreatePm", crPmview);
                }

                checkNgayMuonSame(Ngaymuon);
                return PartialView("_PartialViewChooseDevices", pagedongtb);
            }
            return View(crPmview);
        }


        public IActionResult AddBorrowDevices(int madongtb, int? page, string? searchString, DateTime Ngaymuon)
        {
            int pageSize = 9;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;


            checkNgayMuonSame(Ngaymuon);



            List<DongTbAndAmount> dongtbAmountList = countDongTbAndAmount(Ngaymuon);
            List<DongTbAndAmount> BorrowCart = HttpContext.Session.GetJson<List<DongTbAndAmount>>("BorrowCart") ?? new List<DongTbAndAmount>();

            if(madongtb !=0)
            {
                DongTbAndAmount dongtbAmount = BorrowCart.FirstOrDefault(x => x.madongtb == madongtb);

                if (dongtbAmount != null)
                {
                    dongtbAmount.amount += 1;
                }
                else
                {
                    Dongthietbi dongtb = dongtbRepo.GetDtbById(madongtb);
                    dongtbAmount = new DongTbAndAmount()
                    {
                        madongtb = madongtb,
                        tendongtb = dongtb.Tendongtb,
                        hinhanh = dongtb.Hinhanh,
                        amount = 1
                    };
                    BorrowCart.Add(dongtbAmount);

                }
                HttpContext.Session.SetJson("BorrowCart", BorrowCart);
                    dongtbAmountList.FirstOrDefault(x => x.madongtb == dongtbAmount.madongtb).amount -= 1;
            }
           
            PagedList<DongTbAndAmount> pagedongtb = new PagedList<DongTbAndAmount>(dongtbAmountList, pageNumber, pageSize);

            return PartialView("_PartialViewChooseDevices",pagedongtb);
        }

        public void checkNgayMuonSame(DateTime Ngaymuon)
        {
            DateTime NgayMuonOld = DateTime.Parse(HttpContext.Session.GetString("NgaymuonPm") ?? "2004-11-01");
            if (Ngaymuon != NgayMuonOld)
            {
                if(Ngaymuon.Year < 2010)
                {
                    HttpContext.Session.SetString("NgaymuonPm", "2004-11-01");
                }
                else
                {
                    HttpContext.Session.SetString("NgaymuonPm", Ngaymuon.ToString("dd-MM-yyyy"));
                }
                HttpContext.Session.Remove("BorrowCart");
            }
        }

        public List<DongTbAndAmount> countDongTbAndAmount(DateTime Ngaymuon)
        {
            List<DongTbAndAmount> dongtbList;
            List<DongTbAndAmount> BorrowCart = HttpContext.Session.GetJson<List<DongTbAndAmount>>("BorrowCart") ?? new List<DongTbAndAmount>();
            if (Ngaymuon.Year > 2010)
            {
                dongtbList = dongtbRepo.DongTbAndAmountTbInDay(Ngaymuon);
                foreach (DongTbAndAmount bCart in BorrowCart)
                {
                    dongtbList.FirstOrDefault(x => x.madongtb == bCart.madongtb).amount -= bCart.amount;
                }
            }
            else
            {
                dongtbList = new List<DongTbAndAmount>();
            }
            return dongtbList;
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
            ViewBag.selectState = Trangthai ?? "-1";
            ViewBag.borrowFrom = From.HasValue ? From.Value.ToString("yyyy-MM-dd") : "";
            ViewBag.borrowTo = To.HasValue ? To.Value.ToString("yyyy-MM-dd") : "";
            ViewBag.selectKhoaChoose = Makhoa ?? "-1";

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
