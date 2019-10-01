using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xWAREActivity.ViewModel
{
    public class PostView
    {
        
        public Guid id { get; set; }
        public string image { get; set; }
        public string text { get; set; }
        public string filepath { get; set; }
        public int totallikes { get; set; }
        public DateTime insertiondate { get; set; }
        public UserView User { get; set; }
    }
}