using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;
using PineLinks.Models;

namespace PineLinks.Controllers
{
    public class LoginController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public LoginController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginModel log, UserModel usr)
        {
            
            //sprawdza czy w bazie danych jest taki urzytkownik
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Users where UserLogin='" + log.Login + "' and UserPassword='" + log.Password + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                usr.Login = dr["UserLogin"].ToString();
                usr.Password = dr["UserPassword"].ToString();
                usr.Name = dr["UserName"].ToString();
                usr.Email = dr["UserEmail"].ToString();
                usr.Role = dr["UserRole"].ToString();

                //znalazl urzytkownika

                //log.loggedIn = true;
                //usr.loggedIn = true;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,usr.Email),
                    new Claim(ClaimTypes.Name,usr.Name),
                    new Claim(ClaimTypes.Role,usr.Role),
                    new Claim(ClaimTypes.SerialNumber, dr["UserId"].ToString())

                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new System.Security.Claims.ClaimsPrincipal(claimsIdentity));

               

                con.Close();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //nie znalazl urzytkownika
                con.Close();
                //log.loggedIn = false;
                ViewData["LoginFlag"] = "Nieprawidłowy login lub hasło!!!";
                return View("Index");
            }
        }
        public IActionResult Logout() //Wylogowanie
        {
            Response.Cookies.Delete("sesja");

            return RedirectToAction("Index", "Home");
        }
    }
}
