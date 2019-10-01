using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWAREActivity.Models;
using xWAREActivity.ViewModel;

namespace xWAREActivity.Interface
{
    public interface IPostRepository : IDisposable
    {
        IEnumerable<Post> GetPosts();
        IEnumerable<Post> GetUserPosts(Guid userid);
        Post GetPostByID(Guid PostId);
        void InsertPost(Post Post);
        bool DeletePost(Guid PostID);
        void UpdatePost(Post Post);
        void Save();
    }
}
