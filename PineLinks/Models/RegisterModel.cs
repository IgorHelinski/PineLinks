using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace PineLinks.Models
{
    public class RegisterModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IFormFile Image { get; set; }
        public byte[] ImageInBytes { get; set; }
        public bool ImageAdded { get; set; }
        public string ImageInString { get; set; }
    }
}
