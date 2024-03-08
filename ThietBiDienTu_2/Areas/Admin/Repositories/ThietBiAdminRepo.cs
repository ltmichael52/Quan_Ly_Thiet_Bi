using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Repositories
{
    public class ThietBiAdminRepo : IThietBiAdmin
    {
        ToolDbContext context;
        public ThietBiAdminRepo(ToolDbContext _context)
        {
            this.context = _context;
        }

        public List<Thietbi> GetAllThietBi()
        {
            List<Thietbi> tbList = context.Thietbis.ToList();
            return tbList;
        }
    }
}
