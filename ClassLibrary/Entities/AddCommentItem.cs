using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class AddCommentItem
    {
        public AddCommentItem() { }
        public AddCommentItem(string login,string gameName,string comment) {
            Login = login;
            GameName = gameName;
            Comment = comment;
        }
        public string Login {  get; set; }
        public string GameName { get; set; }
        public string Comment { get; set; }
    }
}
