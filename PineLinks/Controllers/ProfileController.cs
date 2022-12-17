using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PineLinks.Models;
using System.Data.SqlClient;

namespace PineLinks.Controllers
{
    public class ProfileController : Controller
    {
        public static IList<LinkModel> Links = new List<LinkModel>();
        

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

        public IActionResult Index(string id)
        {
            Links.Clear();
            //check if there is a user (id is the name of user)
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Users where UserName='" + id + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                //found user
                ViewData["ProfileUserName"] = dr["UserName"].ToString();
                con.Close();

                connectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "select * from dbo.Links where OwnerName='" + id + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    Links.Add(new LinkModel
                    {
                        LinkId = Convert.ToInt32(dr["LinkId"]),
                        OwnerName = dr["OwnerName"].ToString(),
                        LinkLabel = dr["LinkLabel"].ToString(),
                        LinkUrl = dr["LinkUrl"].ToString()
                    });

                    while (dr.Read())
                    {
                        Links.Add(new LinkModel
                        {
                            LinkId = Convert.ToInt32(dr["LinkId"]),
                            OwnerName = dr["OwnerName"].ToString(),
                            LinkLabel = dr["LinkLabel"].ToString(),
                            LinkUrl = dr["LinkUrl"].ToString()
                        });
                    }

                    return View(Links);
                }
                else
                {
                    return View(Links);
                }
            }
            else
            {
                //user not found
                return View("UserNotFound");
            }
        }

        [Authorize]
        public IActionResult Edit(string id)
        {

            if(User.Identity.Name == id)
            {
                Links.Clear();
                //check if there is a user (id is the name of user)
                connectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "select * from dbo.Users where UserName='" + id + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    //found user
                    ViewData["ProfileUserName"] = dr["UserName"].ToString();
                    con.Close();

                    connectionString();
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "select * from dbo.Links where OwnerName='" + id + "'";
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        Links.Add(new LinkModel
                        {
                            LinkId = Convert.ToInt32(dr["LinkId"]),
                            OwnerName = dr["OwnerName"].ToString(),
                            LinkLabel = dr["LinkLabel"].ToString(),
                            LinkUrl = dr["LinkUrl"].ToString()
                        });

                        while (dr.Read())
                        {
                            Links.Add(new LinkModel
                            {
                                LinkId = Convert.ToInt32(dr["LinkId"]),
                                OwnerName = dr["OwnerName"].ToString(),
                                LinkLabel = dr["LinkLabel"].ToString(),
                                LinkUrl = dr["LinkUrl"].ToString()
                            });
                        }

                        return View(Links);
                    }
                    else
                    {
                        return View(Links);
                    }
                }
                else
                {
                    //user not found
                    return View("UserNotFound");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
