using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class RatingItem
    {
        public RatingItem() { }
        public RatingItem(string login,int rating,DateTime date) 
        {
            Login = login;
            Rating = rating;
            Date = date;
        }

        public string Login { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
