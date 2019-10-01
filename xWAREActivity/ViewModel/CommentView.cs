using System;

namespace xWAREActivity.ViewModel
{
    public class CommentView
    {
        public Guid id { get; set; }
        public Guid postid { get; set; }
        public string text { get; set; }
        public DateTime insertiondate { get; set; }
        public UserView user { get; set; }
       
    }
}