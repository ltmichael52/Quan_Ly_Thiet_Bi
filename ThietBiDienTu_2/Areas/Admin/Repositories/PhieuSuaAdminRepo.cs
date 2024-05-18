using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Repositories
{
    public class PhieuSuaAdminRepo : IPhieuSuaAdmin
    {
        ToolDbContext context;
        public PhieuSuaAdminRepo(ToolDbContext context)
        {
            this.context = context;
        }

        public bool TbHasPhieuSua(int matb)
        {
            return context.Chitietphieusuas.FirstOrDefault(x => x.Matb == matb) == null;
        }

        public List<Phieusua> GetAllPs()
        {
            return context.Phieusuas.ToList();
        }

       public int CreatePs(List<TbFixAndCheck> tbChoosenList, decimal tongchiphi)
        {
            DateTime today =  DateTime.Now;
            Phieusua ps = new Phieusua()
            {
                Ngaylap = today,
                Trangthai = 0,
                Tongchiphi = tongchiphi,
            };
            context.Phieusuas.Add(ps);
            context.SaveChanges();

            foreach (TbFixAndCheck tbFix in tbChoosenList)
            {
                Chitietphieusua ctps = new Chitietphieusua
                {
                    Maps = ps.Maps,
                    Matb = tbFix.Matb,
                    Chiphi = tbFix.Chiphi,
                    Mota = tbFix.Mota,
                };
                context.Chitietphieusuas.Add(ctps);
                context.SaveChanges();

                Thietbi tb = context.Thietbis.Find(tbFix.Matb);
                tb.Trangthai = "Đang sửa";
                context.Thietbis.Update(tb);
                context.SaveChanges();
            }
            
            return ps.Maps;
        }

    }
}
