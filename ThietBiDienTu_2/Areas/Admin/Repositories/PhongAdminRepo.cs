using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.Repositories
{
    public class PhongAdminRepo : IPhongAdmin
    {
        ToolDbContext _toolDbContext;
        public PhongAdminRepo(ToolDbContext toolDbContext)
        {
            _toolDbContext = toolDbContext;
        }

        public List<Phong> GetPhongList()
        {
            List<Phong> pList = _toolDbContext.Phongs.ToList();
            return pList;
        }
    }
}
