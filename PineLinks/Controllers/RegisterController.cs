using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;


namespace PineLinks.Controllers
{
    public class RegisterController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public RegisterController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }


        public IActionResult Register()
        {



            return View();
        }
    }
}
