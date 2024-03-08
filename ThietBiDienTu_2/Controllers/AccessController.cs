using Microsoft.AspNetCore.Mvc;
using ThietBiDienTu_2.Models;

namespace ThietBiDienTu_2.Controllers
{
    public class AccessController : Controller
    {
        ToolDbContext _context;

        public AccessController(ToolDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
                //If not login
            }
            else
            {
                if (HttpContext.Session.GetInt32("TypeAccount") == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                }
                //If has already login
            }
        }

        [HttpPost]
        public IActionResult Login(Account user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var u = _context.Accounts
                    .Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password))
                    .FirstOrDefault();
                //Check user and password

                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.Username.ToString());
                    HttpContext.Session.SetInt32("TypeAccount", (int)u.Loaiuser);

                    if (HttpContext.Session.GetInt32("TypeAccount") == 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                    }

                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Access");
        }
    }
}
