using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThietBiDienTu_2.Migrations;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.Authentication;
using ThietBiDienTu_2.Models.ViewModels;
using ThietBiDienTu_2.Repository;

namespace ThietBiDienTu_2.Controllers
{
    [AuthenticationCustomer]
    public class CartController : Controller
    {
        private readonly ToolDbContext _dataContext;
        public CartController(ToolDbContext _context)
        {
            _dataContext = _context;
        }

        public IActionResult Index()
        {

            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>(); // neu co du lieu thi hien thi con khong se tao moi 1 list 
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Soluong)//Tinh tong


            };

            return View(cartVM);
        }


        public IActionResult Add(int id)
        {
            Dongthietbi dongthietbi = _dataContext.Dongthietbis.FirstOrDefault(x => x.Madongtb == id);

            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            CartItemModel cartItem = cart.FirstOrDefault(c => c.Madongtb == id);
            //sử dụng phương thức LINQ FirstOrDefault. Phương thức này sẽ trả về phần tử đầu tiên trong danh sách thỏa mãn điều kiện, hoặc null nếu không có phần tử nào thỏa mãn.
            if (cartItem == null) // trong trường hợp == nulll thì tạo 1 cart mới còn ngược lại thì nếu có tồn tại thì tăng nó +1
            {
                cart.Add(new CartItemModel(dongthietbi));
            }
            else
            {
                cartItem.Soluong += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            return Redirect(Request.Headers["Referer"].ToString()); // tra ve trang hien tai
        }
        public IActionResult Decrease(int id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            CartItemModel cartItem = cart.Where(c => c.Madongtb == id).FirstOrDefault();
            if (cartItem != null)
            {
                if (cartItem.Soluong > 1)
                {
                    --cartItem.Soluong;
                }
                else
                {
                    cart.RemoveAll(p => p.Madongtb == id);
                }
                if (cart.Count == 0)
                {
                    HttpContext.Session.Remove("Cart");
                }
                else
                {
                    HttpContext.Session.SetJson("Cart", cart);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Increase(int id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            CartItemModel cartItem = cart.Where(c => c.Madongtb == id).FirstOrDefault();
            if (cartItem.Soluong >= 1)
            {
                ++cartItem.Soluong;
            }
            else
            {
                cart.RemoveAll(p => p.Madongtb == id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int Madongtb)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            cart.RemoveAll(x => x.Madongtb == Madongtb);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Clear(string Matb)
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
        public IActionResult Details()
        {
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>(); // neu co du lieu thi hien thi con khong se tao moi 1 list 
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                Sv = _dataContext.Sinhviens.Find(HttpContext.Session.GetInt32("UserName")),
                Phieumuon = new Phieumuon()
                {
                    Ngaylap = DateTime.Now,
                    Ngaymuon = DateTime.Parse(HttpContext.Session.GetString("NgayDat")),

                }

            };

            return View(cartVM);
        }

        [HttpPost]
        public IActionResult Details(CartItemViewModel cartVM)
        {
            if (cartVM.Phieumuon.Lydomuon != null) // Kiểm tra xem dữ liệu gửi lên có hợp lệ không
            {
                // Lấy dữ liệu từ form
                string lyDoMuon = cartVM.Phieumuon.Lydomuon;
                DateTime ngayMuon = DateTime.Parse(HttpContext.Session.GetString("NgayDat"));
                int maSv = HttpContext.Session.GetInt32("UserName") ?? 0;

                // Bắt đầu giao dịch
                using (var transaction = _dataContext.Database.BeginTransaction())
                {
                    try
                    {

                        // Tạo đối tượng Phiieumuon và gán dữ liệu từ form
                        Phieumuon phieuMuon = new Phieumuon
                        {
                            Lydomuon = lyDoMuon,
                            Ngaymuon = ngayMuon,
                            Ngaylap = DateTime.Now,
                            Masv = maSv,
                        };

                        // Lưu đối tượng Phieumuon vào cơ sở dữ liệu
                        _dataContext.Phieumuons.Add(phieuMuon);
                        _dataContext.SaveChanges();

                        // Lấy Mã phiếu mượn mới tạo
                        int maPhieuMuon = phieuMuon.Mapm;

                        // Lấy danh sách các thiết bị trong giỏ hàng của sinh viên
                        var cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

                        List<Chitietphieumuon> ctpmThatDay = _dataContext.Chitietphieumuons.Include(x=>x.MapmNavigation)
                                                                .Where(a=>a.MapmNavigation.Ngaymuon == ngayMuon).ToList();
                        List<string> maThietBiDaMuon = ctpmThatDay.Select(ct => ct.Matb.ToString()).ToList();
                        // Tạo danh sách tạm thời để lưu các phần tử cần loại bỏ
                        var itemsToRemove = new List<CartItemModel>();

                        foreach (var cartItem in cartItems)
                        {
                            // Tìm mã thiết bị có trạng thái "Sẵn sàng" và chưa được đặt trong cùng một ngày
                            List<Thietbi> tbss = _dataContext.Thietbis.Include(a=> a.MapNavigation)
                                .Where(x => x.Madongtb == cartItem.Madongtb && x.Trangthai == "Sẵn sàng" 
                                && !maThietBiDaMuon.Contains(x.Matb.ToString()))
                                .OrderByDescending(z=>z.MapNavigation.Douutien )
                                .OrderByDescending(u=>u.Seri).ToList();
                            
                            
                            

                            if (tbss != null && tbss.Count >=cartItem.Soluong)
                            {
                                // Tạo ChiTietPhieuMuon chỉ với số lượng thiết bị đặt tương ứng
                                for (int i = 0; i < cartItem.Soluong; i++)
                                {
                                    Chitietphieumuon chiTietPhieuMuon = new Chitietphieumuon
                                    {
                                        Mapm = maPhieuMuon,
                                        Matb = tbss.ElementAt(i).Matb,
                                    };
                                    // Lưu đối tượng ChiTietPhieuMuon vào cơ sở dữ liệu
                                    _dataContext.Chitietphieumuons.Add(chiTietPhieuMuon);
                                }

                                // Thêm phần tử vào danh sách cần loại bỏ
                                itemsToRemove.Add(cartItem);
                            }
                        }

                        // Loại bỏ các phần tử đã được xử lý khỏi danh sách giỏ hàng
                        foreach (var item in itemsToRemove)
                        {
                            cartItems.Remove(item);
                        }

                        // Lưu thay đổi vào cơ sở dữ liệu
                        _dataContext.SaveChanges();

                        HttpContext.Session.SetJson("Cart", cartItems);
                        HttpContext.Session.Remove("Cart");

                        // Hoàn thành giao dịch
                        transaction.Commit();

                        // Sau khi lưu thành công, bạn có thể thực hiện các hành động khác ở đây, ví dụ: redirect đến trang thành công, hiển thị thông báo, v.v.
                        return RedirectToAction("Index", "Histroy");
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi xảy ra, rollback giao dịch
                        transaction.Rollback();
                        // Xử lý lỗi (ví dụ: log, hiển thị thông báo)
                        Debug.WriteLine("Error: " + ex.Message);
                        // Quay trở lại trang trước đó hoặc hiển thị thông báo lỗi
                        return RedirectToAction("Index", "Histroy");
                    }
                }
            }
            else
            {
                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>(); // neu co du lieu thi hien thi con khong se tao moi 1 list 
                cartVM = new()
                {
                    CartItems = cartItems,
                    Sv = _dataContext.Sinhviens.Find(HttpContext.Session.GetInt32("UserName")),
                };

                return View(cartVM);
            }
        }

    }
}
