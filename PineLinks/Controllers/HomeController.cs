using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PineLinks.Models;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.IO;
using System;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace PineLinks.Controllers
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static IList<UserModel> Users = new List<UserModel>();

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public HomeController(IConfiguration _configuration, ILogger<HomeController> logger)
        {
            Configuration = _configuration;
            _logger = logger;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        public IActionResult Index()
        {
            Users.Clear();
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Users";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                Users.Add(new UserModel
                {
                    Name = dr["UserName"].ToString(),
                    ImageInString = dr["UserPfp"].ToString()
                    //ImageInBytes = Encoding.ASCII.GetBytes(dr["UserPfp"].ToString())
                }) ;

                while (dr.Read())
                {
                    Users.Add(new UserModel
                    {
                        Name = dr["UserName"].ToString(),
                        //ImageInBytes = Encoding.ASCII.GetBytes(dr["UserPfp"].ToString())
                        ImageInString = dr["UserPfp"].ToString()
                    });
                }

                
                return View(Users);
            }
            else
            {
                return View(Users);
            }
        }

        public byte[] ObjectToByteArray(object _Object)
        {
            try
            {
                // create new memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream();

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // Serializes an object, or graph of connected objects, to the given stream.
                _BinaryFormatter.Serialize(_MemoryStream, _Object);

                // convert stream to byte array and return
                return _MemoryStream.ToArray();
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            // Error occured, return null
            return null;
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Test(string cos, UserModel usr)
        {
            
            return View(usr);

        }
        
        public async Task<byte[]> Createe(UserModel usr)
        {
            var img = usr.Image;
            var fileName = Path.GetFileName(usr.Image.FileName);
            var contentType = usr.Image.ContentType;
            string FileName = usr.Image.FileName;
            GetByteArrayFromImage(usr.Image);

            var bytes = await usr.Image.GetBytes();
            //var bytes = GetBytes(usr.Image);
            var hexString = Convert.ToBase64String(bytes);

            return bytes;
        }

        public IActionResult nwm(UserModel usrr)
        {
            
            byte[] bity = Createe(usrr).Result;
            
            usrr.ImageInBytes = bity;
            return View(usrr);
        }

        

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}