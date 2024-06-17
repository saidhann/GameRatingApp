using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class LoginPasswordItem
    {
        public LoginPasswordItem(string login,string password) {
            Login = login;
            Password = password;
        }
        public LoginPasswordItem() { }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
