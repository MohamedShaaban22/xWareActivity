using AutoMapper;
using xWAREActivity.Models;
using xWAREActivity.ViewModel;

namespace xWAREActivity.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                config.CreateMap<Post, PostView>().ReverseMap();
                config.CreateMap<Event, EventDBView>().ReverseMap();
                config.CreateMap<User, UserView>().ReverseMap();
                config.CreateMap<Post, PostView>().ReverseMap();
                config.CreateMap<Comment, CommentView>().ReverseMap();
                config.CreateMap<Like, LikeView>().ReverseMap();
                config.CreateMap<Team, AdminTeamView>().ReverseMap();
                config.CreateMap<User,AdminUserView>().ReverseMap();

            });
          
        }
    }
}