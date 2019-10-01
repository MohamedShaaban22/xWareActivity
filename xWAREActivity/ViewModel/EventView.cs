using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xWAREActivity.ViewModel
{
    public class EventView
    {
        public Guid eventid { get; set; }
        public string title { get; set; }
        public string teamname { get; set; }
        public DateTime date { get; set; }
        public TimeSpan start { get; set; }
    }
}