using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xWAREActivity.Models;
using xWAREActivity.Interface;
using System.Data.Entity;

namespace xWAREActivity.Repository
{
    public class TeamRepository:ITeamRepository,IDisposable
    {
        private ActivityDBEntities context;

        public TeamRepository(ActivityDBEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Team> GetTeams()
        {
            return context.Teams.OrderBy(team=>team.name).ToList();
        }

        public Team GetTeamByID(Guid TeamId)
        {
            return context.Teams.Find(TeamId);
        }

        public void InsertTeam(Team Team)
        {
            context.Teams.Add(Team);
        }

        public bool DeleteTeam(Guid TeamID)
        {
            Team Team = context.Teams.Find(TeamID);
            var entity = context.Users.Where(user => user.teamid == TeamID);
                            
            if (Team != null)
            {
                foreach (var user in entity)
                {
                    user.teamid = null;
                    user.Team = null;
                    context.Entry(user).State = EntityState.Modified;
                }                
                context.SaveChanges();
                context.Teams.Remove(Team);
                return true;
            }
            else
                return false;
        }

        public void UpdateTeam(Team Team)
        {
            context.Entry(Team).State = EntityState.Modified;
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
        // ~TeamRepository() {
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