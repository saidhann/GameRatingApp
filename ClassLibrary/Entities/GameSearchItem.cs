using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class GameSearchItem
    {
        public GameSearchItem() { }
        public GameSearchItem(string title,string genre,string pegi,HowToSortEnum howToSort )
        {
            Title = title;
            Genre = genre;
            Pegi = pegi;
            HowToSort = howToSort;
        }
        //Filters
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Pegi { get; set; }
        //Sorts
        public HowToSortEnum HowToSort { get; set; }

        
    }
}
