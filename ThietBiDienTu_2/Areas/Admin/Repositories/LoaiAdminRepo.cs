using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Repositories
{
    public class LoaiAdminRepo : ILoaiAdmin
    {
        ToolDbContext context;

        public LoaiAdminRepo(ToolDbContext _context)
        {
            this.context = _context;
        }

        public List<Loaithietbi> GetAllLoai()
        {
            List<Loaithietbi> loaiList = context.Loaithietbis.ToList();
            return loaiList;
        }
    }
}
