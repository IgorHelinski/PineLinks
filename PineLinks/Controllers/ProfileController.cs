using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PineLinks.Models;
using System.Data;
using System.Data.SqlClient;

namespace PineLinks.Controllers
{
    public class ProfileController : Controller
    {
        public static IList<LinkModel> Links = new List<LinkModel>();
        public static IList<UserModel> Users = new List<UserModel>();

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
        public async Task<byte[]> GenerateBytes(UserModel usr)
        {
            var bytes = await usr.Image.GetBytes();
            var bruh = Convert.ToBase64String(bytes);
            return bytes;
        }

        public IActionResult View(string id)
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

        [Authorize]
        public IActionResult EditProfile(string id, UserModel usr)
        {
            if (User.Identity.Name == id)
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
                    usr.ImageInBytes = (byte[])dr["UserPfp"];
                    con.Close();

                    return View(usr);
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

        public IActionResult Change(UserModel usr)
        {
            byte[] bity = GenerateBytes(usr).Result;
            usr.ImageInBytes = bity;

            connectionString();
            SqlCommand com = new SqlCommand("usr_ChangePfp", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserPfp", usr.ImageInBytes);
            com.Parameters.AddWithValue("@UserName", User.Identity.Name);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("Index", "Home");
        }


        public IActionResult EditNotes()
        {
            return View();
        }
    }
}
    

