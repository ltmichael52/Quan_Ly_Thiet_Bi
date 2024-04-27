using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Repositories
{
    public class SinhvienAdminRepo : ISinhvienAdmin
    {
        ToolDbContext context;
        public SinhvienAdminRepo(ToolDbContext context)
        {
            this.context = context;
        }

        public Sinhvien GetSvById(int id)
        {
            Sinhvien sv = null;
            if(context.Sinhviens.Any(x=>x.Masv == id))
            {
                sv = context.Sinhviens.Find(id);
                sv.MakhoaNavigation = context.Khoas.Find(sv.Makhoa);
                sv.ManganhNavigation = context.Nganhs.Find(sv.Manganh);
            }
            
            return sv;
        }
    }
}
