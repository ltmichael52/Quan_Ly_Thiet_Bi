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

       

    }
}
