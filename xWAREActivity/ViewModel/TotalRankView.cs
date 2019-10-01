using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xWAREActivity.ViewModel
{
    public class TotalRankView
    {
        public Guid userid { get; set; }
        public string username { get; set; }
        public string imagepath { get; set; }
        public int totallikes { get; set; }        
    }
}