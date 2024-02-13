using ThietBiDienTu_2.Areas.Admin.Controllers;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Areas.Admin.ViewModels
{
    public class StuAcc
    {
        public StuAcc()
        {
            CourseName = new List<string>
            {
                "K16",
                "K15",
                "K14",
                "K13"
            };
            MajorName = new List<string>
                            {
                            "Truyền thông đa phương tiện",
                            "Quan hệ công chúng",
                            "Công nghệ thông tin",
                            "Công nghệ thông tin y tế",
                            "Thiết kế vi mạch",
                            "Thiết kế đồ họa",
                            "Công nghệ giáo dục",
                            "Luật kinh tế quốc tế",
                            "Trí tuệ nhân tạo",
                            "Hệ thống dữ liệu lớn",
                            "Kỹ thuật phần mềm",
                            "Mạng máy tính & An ninh thông tin",
                            "Quản trị kinh doanh",
                            "Thương mại quốc tế",
                            "Quản trị du lịch",
                            "Kinh tế đối ngoại",
                            "Marketing số",
                            "Kinh doanh số",
                            "Thương mại điện tử",
                            "Tiếng Anh giảng dạy",
                            "Tiếng Anh thương mại",
                            "Kế toán - Kiểm toán",
                            "Kế toán doanh nghiệp",
                            "Quản trị khách sạn",
                            "Logistics & Quản lý chuỗi cung ứng",
                            "Tâm lý học và tham vấn & trị liệu",
                            "Nhật Bản học",
                            "Hàn Quốc học",
                            "Trung Quốc học"
                            };
        }
        public Sinhvien sv {  get; set; }
        public Account acc {  get; set; }
        public readonly List<string> CourseName;
        public readonly List<string> MajorName;
        

    }
}
