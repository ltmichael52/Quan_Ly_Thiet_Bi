using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Controllers;
using ThietBiDienTu_2.Models.Authentication;
using ThietBiDienTu_2.Models.ViewModels;
using ThietBiDienTu_2.Models;
using X.PagedList;

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
        // Lấy mã số sinh viên từ Session
        int masv = HttpContext.Session.GetInt32("UserName") ?? 0;

        // Lọc danh sách phiếu mượn chỉ hiển thị của sinh viên hiện tại
        // Lọc danh sách phiếu mượn chỉ hiển thị của sinh viên hiện tại
        var phieuMuonList = _dataContext.Phieumuons.Where(x => x.Masv == masv).ToList();

        // Chuyển đổi danh sách thành IEnumerable trước khi sử dụng ToPagedList()
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
        }).AsEnumerable(); // Chuyển đổi thành IEnumerable

        // Sử dụng phương thức Count() để đếm số lượng phần tử trong danh sách
        for (int i = 0; i < pagedList.Count(); ++i)
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
        // Lấy mã số sinh viên từ Session
        int masv = HttpContext.Session.GetInt32("UserName") ?? 0;

        Phieumuon pm = _dataContext.Phieumuons.FirstOrDefault(x => x.Mapm == id && x.Masv == masv);
        if (pm == null)
        {
            // Xử lý trường hợp không tìm thấy Phieumuon có Mapm tương ứng với id hoặc không phải của sinh viên hiện tại
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