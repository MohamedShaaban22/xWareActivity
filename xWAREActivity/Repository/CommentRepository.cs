using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xWAREActivity.Interface;
using xWAREActivity.Models;
using System.Data.Entity;

namespace xWAREActivity.Repository
{
    public class CommentRepository:ICommentRepository,IDisposable
    {
        private ActivityDBEntities context;

        public CommentRepository(ActivityDBEntities context)
        {
            this.context = context;
        }
        public IEnumerable<Comment> GetComments()
        {
            return context.Comments.OrderByDescending(comment=>comment.insertiondate).ToList();
        }
        public IEnumerable<Comment> GetPostComments(Guid postid)
        {
            return context.Comments.Where(comment => comment.postid == postid).OrderByDescending(comment => comment.insertiondate).ToList();
        }

        public int PostTotalComments(Guid postid)
        {
            return context.Comments.Count(comment => comment.postid == postid);
        }

        public Comment GetCommentByID(Guid CommentId)
        {
            return context.Comments.Find(CommentId);
        }

        public void InsertComment(Comment Comment)
        {
            context.Comments.Add(Comment);
        }

        public bool DeleteComment(Guid CommentID)
        {
            Comment Comment = context.Comments.Find(CommentID);
            if (Comment != null)
            {
                context.Comments.Remove(Comment);
                return true;
            }
            else
                return false;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CommentRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
        }

        
        #endregion
    }
}