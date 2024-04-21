using System.Diagnostics;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Repositories
{
    public class DongThietBiAdminRepo : IDongThietBiAdmin
    {
        ToolDbContext context;
        public DongThietBiAdminRepo(ToolDbContext _context)
        {
            this.context = _context;
        }

        public List<Dongthietbi> GetAllDongThietBi()
        {
            List<Dongthietbi> tbList = context.Dongthietbis.ToList();
            return tbList;
        }

        public bool CountSlFullOrNot(int MaDongTb)
        {
            int Soluongtb = context.Dongthietbis.FirstOrDefault(x => x.Madongtb == MaDongTb).Soluong;
            int count = context.Thietbis.Where(x => x.Madongtb == MaDongTb).Count();
            return count >= Soluongtb ? true : false;
        }

        public void Updatedtb(Dongthietbi dongthietbi)
        {
           //Dongthietbi dtb = context.Dongthietbis.Find(dongthietbi.Madongtb);
            //dtb.Hinhanh = dongthietbi.Hinhanh;
            //dtb.Soluong = dongthietbi.Soluong;
            //dtb.Mota = dongthietbi.Mota;
            context.Update(dongthietbi);
            context.SaveChanges();
        }

        public Dongthietbi GetDtbById(int id)
        {
            Dongthietbi dtb = context.Dongthietbis.Find(id);
            return dtb;
        }

    }
}
