using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Smart_Support2026.DataAccessLayer;
using Smart_Support2026.Models;
using System.Data;
using System.Data.Common;
using System.Security.Claims;

namespace Smart_Support2026.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        public UserController(ApplicationDbContext context, IToastNotification toastNotificat)
        {
            _context = context;
            _toastNotification = toastNotificat;
        }
        public IActionResult Index()
        {
            ViewData["Active"] = "active";
            return View();

        }
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetLogin(Users users)
        {
            try
            {
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@employeeid", users.Emp_Id);
                    parameters.Add("@password", users.Password);
                    var result = await connection.QueryFirstOrDefaultAsync<Users>("sp_getsslogin", parameters, commandType: CommandType.StoredProcedure);
                    if (result != null) 
                    {

                        var claims = new List<Claim>
                                    {
                                    new Claim(ClaimTypes.Name, users.Emp_Id),
                                    new Claim(ClaimTypes.Role, "User"),
                                    new Claim("CustomClaimType", "CustomValue"),
                                    new Claim(ClaimTypes.NameIdentifier, users.Emp_Id)
                                    };

                        // 2. Create an Identity. 
                        // IMPORTANT: You must provide the AuthenticationScheme name (e.g., "Cookies") 
                        // so that IsAuthenticated becomes true.
                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // 3. Create the Principal (This resolves the red line)
                        var principal = new ClaimsPrincipal(claimsIdentity);

                        // 4. Sign in
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        if (users.Emp_Id == "admin150")
                        {
                            TempData["Role"] = "Admin";
                           
                        }
                        else
                        {
                            TempData["Role"] = "Emp";
                            
                        }
                        return RedirectToAction("Index", "Home");

                    }
                    else {
                        _toastNotification.AddErrorToastMessage("Login Failed!");

                        return View("Login");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
                return View("Error");

            }
         

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string url)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
    }
    }

