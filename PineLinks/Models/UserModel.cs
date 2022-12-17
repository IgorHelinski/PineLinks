
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

using System.Web;
namespace PineLinks.Models
{
    public class UserModel
    {

        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public IFormFile Image { get; set; }

    }
}
