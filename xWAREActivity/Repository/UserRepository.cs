using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using xWAREActivity.Models;
using xWAREActivity.Interface;
using xWAREActivity.ViewModel;

namespace xWAREActivity.Repository
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private ActivityDBEntities context;

        public UserRepository(ActivityDBEntities context)
        {
            this.context = context;
        }
        public IEnumerable<User> GetUsers()
        {
            var entity= context.Users.ToList();
            
            return entity;
        }

        public User GetUserById(Guid userid)
        {
            var entity= context.Users.Find(userid);
           
            return entity;
        }

        public void InsertUser(User User)
        {
            context.Users.Add(User);
        }

        public bool DeleteUser(Guid userid)
        {
            User user = context.Users.Find(userid);
            if (user != null)
            {
                context.Users.Remove(user);
                return true;
            }
            else
                return false;
        }

        public void UpdateUser(User User)
        {
            context.Entry(User).State = EntityState.Modified;
        }
        
     
        public IEnumerable<TotalRankView> TotalRank()
        {
            var result = (context.Database.SqlQuery<TotalRankView>("select * from  dbo.TotalRank()")).OrderByDescending(x => x.totallikes).ThenBy(x => x.username).ToList();
            
            return result;
        }

        public int UserRank(Guid userid)
        {
            
            var result = (context.Database.SqlQuery<TotalRankView>("select * from  dbo.TotalRank()")).OrderByDescending(x => x.totallikes).ThenBy(x => x.username).ToList();
            int rank = result.IndexOf(result.Single(user => user.userid == userid));
            return rank+1;
        }

        public User FindUser(string email,string password)
        {
            var entity = context.Users.First(user =>
             user.email.Equals(email, StringComparison.OrdinalIgnoreCase)
             && user.password == password);
            return entity;
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
        // ~UserRepository() {
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
