using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using xWAREActivity.Interface;
using xWAREActivity.Models;
using xWAREActivity.ViewModel;

namespace xWAREActivity.Repository
{
    public class PostRepository:IPostRepository,IDisposable
    {
        private ActivityDBEntities context;


        public PostRepository(ActivityDBEntities context)
        {
            this.context = context;

        }
        public IEnumerable<Post> GetPosts()
        {
            var entity= context.Posts.OrderByDescending(post => post.insertiondate).ToList();
            return entity;
             
        }

        public IEnumerable<Post> GetUserPosts(Guid userid)
        {
            return context.Posts.Where(posts => posts.userid == userid).OrderByDescending(post => post.insertiondate).ToList();
        }

        public Post GetPostByID(Guid PostId)
        {
            return context.Posts.Find(PostId);
        }

        public void InsertPost(Post Post)
        {
            context.Posts.Add(Post);
        }

        public bool DeletePost(Guid PostID)
        {
            Post Post = context.Posts.Find(PostID);
            if (Post != null)
            {
                context.Posts.Remove(Post);
                return true;
            }
            else
                return false;
        }

        public void UpdatePost(Post Post)
        {
            context.Entry(Post).State = EntityState.Modified;
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
        // ~PostRepository() {
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