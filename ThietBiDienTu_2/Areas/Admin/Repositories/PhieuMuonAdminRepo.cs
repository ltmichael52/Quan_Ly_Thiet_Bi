using System.Diagnostics;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Repositories
{
    public class PhieuMuonAdminRepo : IPhieuMuonAdmin
    {
        ToolDbContext context;
        public PhieuMuonAdminRepo(ToolDbContext _context)
        {
            context = _context;
        }

        public List<Phieumuon> GetAllPhieuMuon()
        {

            List<Phieumuon> phieumuons = context.Phieumuons.Select(x => new Phieumuon
            {
                Mapm = x.Mapm,
                Ngaymuon = x.Ngaymuon,
                Ngaylap = x.Ngaylap,
                Manv = x.Manv,
                Masv = x.Masv,
                Trangthai = x.Trangthai,
                Lydomuon = x.Lydomuon,
                MasvNavigation = context.Sinhviens.FirstOrDefault(y => y.Masv == x.Masv),
                ManvNavigation = context.Nhanviens.FirstOrDefault(y=>y.Manv==x.Manv)
            }).ToList();

            for(int i=0;i<phieumuons.Count;++i)
            {
                phieumuons.ElementAt(i).MasvNavigation.MakhoaNavigation = context.Khoas.FirstOrDefault(x => phieumuons.ElementAt(i).MasvNavigation.Makhoa == x.Makhoa);
                phieumuons.ElementAt(i).MasvNavigation.ManganhNavigation = context.Nganhs.FirstOrDefault(x => phieumuons.ElementAt(i).MasvNavigation.Manganh == x.Manganh);
            }
            
            
            return phieumuons;
        }
        
        public PhieuMuonViewModel GetPhieumuonViewById(int mapm)
        {
            Phieumuon pm = context.Phieumuons.FirstOrDefault(x=>x.Mapm == mapm);
            Sinhvien sv = context.Sinhviens.FirstOrDefault(x => x.Masv == pm.Masv);
            PhieuMuonViewModel pmView = new PhieuMuonViewModel
            {
                Manv = pm.Manv,
                Masv = pm.Masv,
                Mapm = pm.Mapm,
                Tensv = sv.Tensv,
                TenKhoa = context.Khoas.FirstOrDefault(x => x.Makhoa == sv.Makhoa).Tenkhoa,
                TenNganh = context.Nganhs.FirstOrDefault(x => x.Manganh == sv.Manganh).Tennganh,
                Trangthai =pm.Trangthai,
                Lydomuon = pm.Lydomuon,
                LydoTuChoi = pm.LydoTuChoi,
                Ngaylap = pm.Ngaylap,
                Ngaymuon = pm.Ngaymuon
            };

            return pmView;
        }
    }
}
