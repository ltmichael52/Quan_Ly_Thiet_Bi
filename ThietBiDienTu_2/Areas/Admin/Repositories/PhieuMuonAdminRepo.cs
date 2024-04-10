using System.Diagnostics;
using System.Xml;
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

            List<Chitietphieumuon> ctpm = context.Chitietphieumuons.Where(x => x.Mapm == mapm).Select(x => new Chitietphieumuon
            {
                Mapm = x.Mapm,
                Matb = x.Matb,
                MatbNavigation = context.Thietbis.FirstOrDefault(a => a.Matb == x.Matb),
                Ngaytra = x.Ngaytra
            }).ToList();

            List<DongtbAndSeri> dongtbAndSeri = ctpm.Select(x => new DongtbAndSeri
            {
                madongtb = context.Dongthietbis.FirstOrDefault(a => a.Madongtb == x.MatbNavigation.Madongtb).Madongtb,
                seri = x.MatbNavigation.Seri,
                Ngaytra = x.Ngaytra ?? DateTime.Parse("2004-11-01"),
                Matb = x.Matb,
            }).ToList();

            List<ChitietPhieuMuonViewModel> ctpmView = new List<ChitietPhieuMuonViewModel>();
            foreach (DongtbAndSeri dongtbAndSeri1 in dongtbAndSeri)
            {

                ChitietPhieuMuonViewModel ctpmViewTemp = ctpmView.FirstOrDefault(x => x.Madongtb == dongtbAndSeri1.madongtb);
                if (ctpmViewTemp != null)
                {
                    ctpmViewTemp.Seri.Add(dongtbAndSeri1.seri);
                    ctpmViewTemp.Matb.Add(dongtbAndSeri1.Matb);
                    ctpmViewTemp.check.Add(dongtbAndSeri1.Ngaytra.Year > 2010);
                    ctpmViewTemp.Ngaytra.Add(dongtbAndSeri1.Ngaytra);
                    ctpmViewTemp.Soluong += 1;
                }
                else
                {
                    Dongthietbi dongtb = context.Dongthietbis.FirstOrDefault(x => x.Madongtb == dongtbAndSeri1.madongtb);
                    ctpmView.Add(new ChitietPhieuMuonViewModel
                    {
                        Madongtb = dongtbAndSeri1.madongtb,
                        Tendongthietbi = dongtb.Tendongtb,
                        Seri = new List<string> { dongtbAndSeri1.seri },
                        Matb = new List<int> { dongtbAndSeri1.Matb },
                        check = new List<bool> { dongtbAndSeri1.Ngaytra.Year > 2010},
                        Soluong = 1,
                        Hinhanh = dongtb.Hinhanh,
                        Ngaytra = new List<DateTime> { dongtbAndSeri1.Ngaytra },
                    });
                }
            }

            pmView.ctpmView = ctpmView;
            return pmView;
        }

        public void DuyetPm(int trangthai, PhieuMuonViewModel pmView)
        {
            Phieumuon pm = context.Phieumuons.Find(pmView.Mapm);
            pm.Trangthai = trangthai;
            pm.LydoTuChoi = pmView.LydoTuChoi;
            if (trangthai == 1 || trangthai == 3)
            {
                bool checkTrangthai=true;
                DateTime RecieveDay = DateTime.Now;
                foreach (ChitietPhieuMuonViewModel ctpmView in pmView.ctpmView)
                {
                    if (checkTrangthai == true)
                    {
                        checkTrangthai = ctpmView.check.TrueForAll(x => x);
                    }
                   
                    
                    for (int j = 0; j < ctpmView.Matb.Count; ++j)
                    {
                        Chitietphieumuon ctpm = context.Chitietphieumuons.FirstOrDefault(x => x.Mapm == pmView.Mapm && x.Matb == ctpmView.Matb[j]);
                        if (ctpmView.check[j] == true && ctpmView.Ngaytra[j].Year<2010)
                        {                          
                            ctpm.Ngaytra = RecieveDay;
                            
                        }                      
                        else if (ctpmView.check[j] == false)
                        {
                            ctpm.Ngaytra = null;
                        }
                        context.Chitietphieumuons.Update(ctpm);
                    }
                   
                }
                
                context.SaveChanges();
                if(checkTrangthai)
                {
                    pm.Trangthai = 3;
                }
                else
                {
                    pm.Trangthai = 1;
                }
                
            }
            


            context.Phieumuons.Update(pm);
            context.SaveChanges();
        }
        public List<int> AllStatePhieuMuonToday()
        {
            DateTime today = DateTime.Now.Date;
            List<int> trangthaiToday = context.Phieumuons.Where(x => x.Ngaymuon == today).Select(x => x.Trangthai).Distinct().ToList();

            return trangthaiToday;
           
        }
    }
}
