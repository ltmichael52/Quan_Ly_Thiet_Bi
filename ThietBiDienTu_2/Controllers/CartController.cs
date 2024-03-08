using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThietBiDienTu_2.Models;
using ThietBiDienTu_2.Models.ViewModels;
using ThietBiDienTu_2.Repository;

namespace ThietBiDienTu_2.Controllers
{
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
        public ActionResult Checkout()
        {
            return View("~/View/Checkout/Index.cshtml");
        }

        public IActionResult Add(string id)
        {
            Thietbi thietbi = _dataContext.Thietbis.FirstOrDefault(x => x.Matb == id);

            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            CartItemModel cartItem = cart.FirstOrDefault(c => c.Matb == id);
            //sử dụng phương thức LINQ FirstOrDefault. Phương thức này sẽ trả về phần tử đầu tiên trong danh sách thỏa mãn điều kiện, hoặc null nếu không có phần tử nào thỏa mãn.
            if (cartItem == null) // trong trường hợp == nulll thì tạo 1 cart mới còn ngược lại thì nếu có tồn tại thì tăng nó +1
            {
                cart.Add(new CartItemModel(thietbi));
            }
            else
            {
                cartItem.Soluong += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            return Redirect(Request.Headers["Referer"].ToString()); // tra ve trang hien tai
        }
        public IActionResult Decrease(string id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            CartItemModel cartItem = cart.Where(c => c.Matb == id).FirstOrDefault();
            if(cartItem.Soluong >1){
                --cartItem.Soluong;
            }
            else
            {
                cart.RemoveAll(p => p.Matb == id);
            }
            if(cart.Count == 0) {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Increase(string id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            CartItemModel cartItem = cart.Where(c => c.Matb == id).FirstOrDefault();
            if (cartItem.Soluong >= 1)
            {
                ++cartItem.Soluong;
            }
            else
            {
                cart.RemoveAll(p => p.Matb == id);
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
        public IActionResult Delete(string Matb)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            cart.RemoveAll(x => x.Matb == Matb);
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
    }
}
