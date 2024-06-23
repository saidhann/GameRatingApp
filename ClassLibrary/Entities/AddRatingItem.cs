using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class AddRatingItem
    {
        public AddRatingItem() { }
        public AddRatingItem(string login, string gameName, int rating)
        {
            Login = login;
            GameName = gameName;
            Rating = rating;
        }
        public string Login { get; set; }
        public string GameName { get; set; }
        public int Rating { get; set; }
    }
}
