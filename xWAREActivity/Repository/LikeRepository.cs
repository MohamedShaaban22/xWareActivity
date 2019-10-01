using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xWAREActivity.Interface;
using xWAREActivity.Models;
using System.Data.Entity;
using System.Data.SqlClient;

namespace xWAREActivity.Repository
{
    public class LikeRepository : ILikeRepository, IDisposable
    {
        private ActivityDBEntities context;

        public LikeRepository(ActivityDBEntities context)
        {
            
            this.context = context;
        }
        
        public int UserTotalLikes(Guid userid)
        {
            var result = (context.Database.SqlQuery<int>("Select dbo.TotalLikesCount(@userId)", new SqlParameter("@userId",userid.ToString())).FirstOrDefault()).ToString();            
            return Convert.ToInt32(result);
        }

        public IEnumerable<Like> PostLikes(Guid postid)
        {
            return context.Likes.Where(likes => likes.postid == postid).ToList();
        }

        public int PostTotalLikes(Guid postid)
        {
            return context.Likes.Count(likes => likes.postid == postid);
        }


        public Like GetLikeByID(Guid LikeId)
        {
            return context.Likes.Find(LikeId);
        }

        public void InsertLike(Like Like)
        {
            context.Likes.Add(Like);
        }

        public bool DeleteLike(Guid LikeID)
        {
            Like Like = context.Likes.Find(LikeID);
            if (Like != null)
            {
                context.Likes.Remove(Like);
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
        // ~LikeRepository() {
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