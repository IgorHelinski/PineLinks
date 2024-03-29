﻿using Microsoft.AspNetCore.Mvc;
using PineLinks.Models;
using System.Data;
using System.Data.SqlClient;


namespace PineLinks.Controllers
{

    public class RegisterController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        bool canRegister;

        private IConfiguration Configuration;
        public RegisterController(IConfiguration _configuration)
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

        //sprawdza czy mozemy zapisac uzytkownika w bazie danych (czy nie ma tos takiego loginu itp)
        void CheckRegister(RegisterModel reg)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Users where UserLogin='" + reg.Login + "' or UserEmail ='" + reg.Email + "'or UserName ='" + reg.Name + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                //jest taki urzytkownik (nie mozna zarejestrowac)
                canRegister = false;
            }
            else
            {
                con.Close();
                //nie ma takiego urzttkownika (mozna zarejestrowac)
                canRegister = true;
            }
        }

        //Tutaj zapisujemy dane na bazie danych
        [HttpPost]
        public ActionResult CreateAccount(RegisterModel reg) //Rejestracja
        {
            //sprawdzamy czy mozemy zarejestrowac nowego klienta
            CheckRegister(reg);

            if (canRegister)
            {
                if(reg.Image != null)
                {
                    byte[] bity = GenerateBytes(reg).Result;
                    reg.ImageInBytes = bity;
                }
                else
                {
                    reg.ImageInBytes = null;
                }
                
                connectionString();
                SqlCommand com = new SqlCommand("usr_AddUser", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserLogin", reg.Login);
                com.Parameters.AddWithValue("@UserPassword", reg.Password);
                com.Parameters.AddWithValue("@UserName", reg.Name);
                com.Parameters.AddWithValue("@UserEmail", reg.Email);
                com.Parameters.AddWithValue("@UserRole", "User");
                if(reg.Image != null)
                {
                    com.Parameters.AddWithValue("@UserPfp", reg.ImageInBytes);
                }
                else
                {
                    byte[] ba = new byte[1] { 1 };
                    com.Parameters.AddWithValue("@UserPfp", ba);
                }
                
                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {
                    //Success
                    ViewData["Success"] = "success";
                    return View("Index");
                }
                else
                {
                    //Failed
                    ViewData["Failed"] = "failed";
                    return View("Index");
                }
            }
            else
            {
                //nie moze zarejestrowac
                ViewData["LoginDouble"] = "double";
                return View("Index");
            }

        }

        public async Task<byte[]> GenerateBytes(RegisterModel usr)
        {
            var bytes = await usr.Image.GetBytes();
            var bruh = Convert.ToBase64String(bytes);
            return bytes;
        }

        public async Task<byte[]> GenerateBytesToDefaulPfp(RegisterModel usr)
        {
            var bytes = await usr.Image.GetBytes();

            return bytes;
        }
    }
}
