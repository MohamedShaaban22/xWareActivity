using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWAREActivity.Models;

namespace xWAREActivity.Interface
{
    public interface ICommentRepository : IDisposable
    {
        IEnumerable<Comment> GetComments();
        IEnumerable<Comment> GetPostComments(Guid id);
        Comment GetCommentByID(Guid CommentId);
        int PostTotalComments(Guid postid);
        void InsertComment(Comment Comment);
        bool DeleteComment(Guid CommentID);
        void Save();
    }
}
