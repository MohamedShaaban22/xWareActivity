using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWAREActivity.Models;

namespace xWAREActivity.Interface
{
    public interface ILikeRepository :IDisposable
    {
        IEnumerable<Like> PostLikes(Guid postid);
        Like GetLikeByID(Guid LikeId);
        int UserTotalLikes(Guid userid);
        int PostTotalLikes(Guid postid);
        void InsertLike(Like Like);
        bool DeleteLike(Guid LikeID);
        void Save();
    }
}
