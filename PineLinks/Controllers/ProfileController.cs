using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PineLinks.Models;
using System.Data.SqlClient;

namespace PineLinks.Controllers
{
    public class ProfileController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public ProfileController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        public IActionResult Index(string id, ProfileModel prof)
        {
            //check if there is a user (id is the name of user)
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Users where UserName='" + id + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                //found user
                prof.ProfileName = dr["UserName"].ToString();
                prof.ProfileEmail = dr["UserEmail"].ToString();

                con.Close();
                return View(prof);
            }
            else
            {
                //user not found
                return View("UserNotFound");
            }

                
        }
    }
}
