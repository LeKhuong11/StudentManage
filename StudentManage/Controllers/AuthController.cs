using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManage.Data;
using StudentManage.Models;
using StudentManage.Models.Auth;
using StudentManage.Services;
using System.Security.Claims;


namespace StudentManage.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly StudentManageContext _context;

        public AuthController(UserService userService, StudentManageContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpGet]
        public IActionResult Login() => View();


        [HttpPost]
        public async Task<IActionResult> Register(S_User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.RegisterUser(user);
                return RedirectToAction("Login");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string passwordHashed = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Tìm người dùng dựa trên email và mật khẩu (hashed)
                var user = _context.S_Users.FirstOrDefault(u => u.Email == model.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    // Tạo danh sách các Claim (dữ liệu liên quan đến người dùng)
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                    };

                    // Tạo danh tính người dùng
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Tạo cookie đăng nhập
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // Ghi nhớ phiên đăng nhập
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Thời gian hết hạn
                    };

                    // Đăng nhập vào hệ thống
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    TempData["SuccessMessage"] = "Login successful!";

                    // Chuyển hướng về trang quản lý sinh viên
                    return RedirectToAction("Index", "Student");
                }

                TempData["ErrorMessage"] = "Invalid login credentials. Please try again.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["SuccessMessage"] = "You have been logged out successfully.";

            return RedirectToAction("Login", "Auth");
        }
    }
}
