using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class CommentItem
    {
        public CommentItem() { }
        public CommentItem(string login,string content, DateTime date)
        {
            Login = login;
            Content = content;
            Date = date;
        }
        public string Login {  get; set; }
        public string Content {  get; set; }
        public DateTime Date { get; set; }
    }
}
