using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xWAREActivity.ViewModel
{
    public class AdminUserView
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public AdminTeamView team { get; set; }
        public string role { get; set; }
    }
}