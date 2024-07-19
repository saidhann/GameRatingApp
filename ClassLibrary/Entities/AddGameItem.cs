using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class AddGameItem
    {
        public AddGameItem() { }
        public AddGameItem(string title,DateTime date,string developer,string genre,string pegi,string description)
        {
            Title = title;
            Date = date;
            Developer = developer;
            Genre = genre;
            Pegi = pegi;
            Description = description;
        }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Developer { get; set; }
        public string Genre { get; set; }
        public string Pegi { get; set; }
        public string Description { get; set; }
    }
}
