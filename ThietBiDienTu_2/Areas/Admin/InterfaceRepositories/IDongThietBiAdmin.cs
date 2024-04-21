using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.InterfaceRepositories
{
    public interface IDongThietBiAdmin
    {
        List<Dongthietbi> GetAllDongThietBi();
        void Updatedtb(Dongthietbi dongthietbi);
        bool CountSlFullOrNot(int MaDongTb);
        Dongthietbi GetDtbById(int id);
    }
}
