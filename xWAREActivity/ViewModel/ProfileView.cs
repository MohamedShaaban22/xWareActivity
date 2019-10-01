using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xWAREActivity.ViewModel
{
    public class ProfileView
    {
        public Guid userid { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string team { get; set; }
        public string email { get; set; }
        public int totallikes { get; set; }
        public int rank { get; set; }
    }
}