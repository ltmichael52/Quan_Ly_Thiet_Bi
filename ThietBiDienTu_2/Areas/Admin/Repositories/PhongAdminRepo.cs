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
        public void Add(Phong phong)
        {
            _toolDbContext.Add(phong);
            _toolDbContext.SaveChangesAsync();
        }
        public void Update(Phong phong)
        {
            _toolDbContext.Update(phong);
            _toolDbContext.SaveChangesAsync();
        }
        public void Delete(Phong phong)
        {
            _toolDbContext.Phongs.Remove(phong);
            _toolDbContext.SaveChangesAsync();
        }
        public Phong FindPhong(string Map)
        {
            var phong = _toolDbContext.Phongs.Find(Map);
            return phong;
        }

        public List<Phong> phongKhoListOfTbList(List<Thietbi> tbList)
        {
            List<Phong> khoList = tbList.Select(x => x.MapNavigation)
                                            .Where(x=>x.Loaiphong=="Kho").Distinct().ToList();
            return khoList;
        }
    }
}
