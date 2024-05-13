using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.InterfaceRepositories
{
    public interface IPhongAdmin
    {
        List<Phong> GetPhongList();
        void Add(Phong phong);
        void Update(Phong phong);
        void Delete(Phong phong);
        Phong FindPhong(string Map);
        List<Phong> phongKhoListOfTbList(List<Thietbi> tbList);
        bool CheckIfPhongHasDevices(string id);
        object GetDevicesInPhong(string phongId);
    }
}
