using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class GameItem
    {
        public GameItem() { }
        public GameItem(string title, DateTime date, string description)
        {
            Title = title;
            Date = date;
            Description = description;
        }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
