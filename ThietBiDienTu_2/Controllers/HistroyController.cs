using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using ThietBiDienTu_2.Models.ViewModels;
using X.PagedList;

namespace ThietBiDienTu_2.Controllers
{
    [AuthenticationCustomer]
    public class HistroyController : Controller
    {

        private readonly ToolDbContext _dataContext;
        private readonly ILogger<ProcductManagementController> _logger;

        public HistroyController(ILogger<ProcductManagementController> logger, ToolDbContext context)
        {
            _logger = logger;
            _dataContext = context;
        }

        public IActionResult Index(int? page, string trangThai)
        {
            int pageSize = 10; // Số lượng mục trong mỗi trang
            int pageNumber = page ?? 1; // Trang mặc định
            var phieuMuonList = _dataContext.Phieumuons.ToList();

            // Sử dụng thư viện X.PagedList để phân trang danh sách
            var pagedList = phieuMuonList.Select(x => new Phieumuon
            {
                Mapm = x.Mapm,
                Ngaymuon = x.Ngaymuon,
                Ngaylap = x.Ngaylap,
                Manv = x.Manv,
                Masv = x.Masv,
                Trangthai = x.Trangthai,
                Lydomuon = x.Lydomuon,
                MasvNavigation = _dataContext.Sinhviens.FirstOrDefault(y => y.Masv == x.Masv),
                ManvNavigation = _dataContext.Nhanviens.FirstOrDefault(y => y.Manv == x.Manv)
            }).ToList();

            for (int i = 0; i < pagedList.Count; ++i)
            {
                pagedList.ElementAt(i).MasvNavigation.MakhoaNavigation = _dataContext.Khoas.FirstOrDefault(x => pagedList.ElementAt(i).MasvNavigation.Makhoa == x.Makhoa);
                pagedList.ElementAt(i).MasvNavigation.ManganhNavigation = _dataContext.Nganhs.FirstOrDefault(x => pagedList.ElementAt(i).MasvNavigation.Manganh == x.Manganh);
            }
            // Lấy danh sách các phiếu mượn từ database

            return View(pagedList.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> ChiTietPhieuMuon(int id)
        {
            Phieumuon pm = _dataContext.Phieumuons.FirstOrDefault(x => x.Mapm == id);
            if (pm == null)
            {
                // Xử lý trường hợp không tìm thấy Phieumuon có Mapm tương ứng với id
                return NotFound();
            }

            Sinhvien sv = _dataContext.Sinhviens.FirstOrDefault(x => x.Masv == pm.Masv);
            if (sv == null)
            {
                // Xử lý trường hợp không tìm thấy Sinhvien có Masv tương ứng với Masv trong Phieumuon
                return NotFound();
            }

            HomeViewModel pmView = new HomeViewModel
            {
                Manv = pm.Manv,
                Masv = pm.Masv,
                Mapm = pm.Mapm,
                Tensv = sv.Tensv,
                TenKhoa = _dataContext.Khoas.FirstOrDefault(x => x.Makhoa == sv.Makhoa)?.Tenkhoa,
                TenNganh = _dataContext.Nganhs.FirstOrDefault(x => x.Manganh == sv.Manganh)?.Tennganh,
                Trangthai = pm.Trangthai,
                Lydomuon = pm.Lydomuon,
                LydoTuChoi = pm.LydoTuChoi,
                Ngaylap = pm.Ngaylap,
                NgayDat = pm.Ngaymuon
            };

            return View(pmView);
        }

    }
}
