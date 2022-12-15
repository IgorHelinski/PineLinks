using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using PineLinks.Models;
using System.Data;

namespace PineLinks.Controllers
{
    public class LinkController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public LinkController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }
        public IActionResult DeleteLink(string id)
        {
            string[] idSplit;
            idSplit = id.Split("!");
            string ownerName = idSplit[0];
            string linkId = idSplit[1];

            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Delete from dbo.Links where OwnerName='" + ownerName + "' and LinkId='" + linkId + "'";
            dr = com.ExecuteReader();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult EditLink(string id)
        {
            return View();
        }

        public IActionResult AddLink()
        {
            return View();
        }

        public IActionResult Add(LinkModel link)
        {
            connectionString();
            SqlCommand com = new SqlCommand("usr_AddLink", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@OwnerName", User.Identity.Name.ToString());
            com.Parameters.AddWithValue("@LinkLabel", link.LinkLabel);
            com.Parameters.AddWithValue("@LinkUrl", link.LinkUrl);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index", "Home");
        }
    }
}
