using ThietBiDienTu_2.Areas.Admin.ViewModels;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.InterfaceRepositories
{
    public interface IPhieuMuonAdmin
    {
        List<Phieumuon> GetAllPhieuMuon();
        PhieuMuonViewModel GetPhieumuonViewById(int mapm);
        void DuyetPm(int trangthai, PhieuMuonViewModel pm);
        List<int> AllStatePhieuMuonToday();
        void CheckPmToday();
    }
}
