using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xWAREActivity.ViewModel;

namespace xWAREActivity.Models
{
    public class rank
    {
        public List<TotalRankView> ranks { get; set; }
    }

    public class Eventview
    {
        public EventView _event { get; set; }
    }

    public class Events
    {
        public List<EventView> events { get; set; }
    }

    public class EventDiscview
    {
        public EventDescriptionView _event { get; set; }
    }

    public class EventsDisc
    {
        public List<EventDescriptionView> events { get; set; }
    }

    public class EventDBView
    {
        public Event _event { get; set; }
    }

    public class EventsDBView
    {
        public List<Event> events { get; set; }
    }

    public class PostJsonView
    {
        public List<PostView> Posts { get; set; }
    }

    public class CommentJsonView
    {
        public List<CommentView> comments { get; set; }
    }

    public class LikeJsonView
    {
        public List<LikeView> likes { get; set; }
    }

    public class TeamJsonView
    {
        public List<AdminTeamView> Teams { get; set; }
    }

    public class LoginJsonView
    {
        public JsonTokenLogin token { get; set; }
        public UserView user { get; set; }
    }
}