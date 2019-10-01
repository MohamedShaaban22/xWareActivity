using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xWAREActivity.ViewModel
{
    public class EventDescriptionView
    {
        public Guid eventid { get; set; }
        public string title { get; set; }
        public string team { get; set; }
        public string description { get; set; }
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }
        public DateTime date { get; set; }
        public Guid userid { get; set; }
    }
}