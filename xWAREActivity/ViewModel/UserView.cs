using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xWAREActivity.ViewModel
{
    public class UserView
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string email { get; set; }
        public string Role { get; set; }
    }
}