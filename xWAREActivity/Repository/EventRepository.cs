using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xWAREActivity.Interface;
using xWAREActivity.Models;
using System.Data.Entity;

namespace xWAREActivity.Repository
{
    public class EventRepository : IEventRepository, IDisposable
    {
        private ActivityDBEntities context;

        public EventRepository(ActivityDBEntities context)
        {
            this.context = context;
        }
        public IEnumerable<Event> GetEvents()
        {
            return context.Events.OrderByDescending(ev=>ev.date).ThenByDescending(ev=>ev.start).ToList();
        }

        public Event GetEventByID(Guid EventId)
        {
            return context.Events.Find(EventId);
        }

        public void InsertEvent(Event Event)
        {
            context.Events.Add(Event);
        }

        public bool DeleteEvent(Guid EventID)
        {
            Event Event = context.Events.Find(EventID);
            if (Event != null)
            {
                context.Events.Remove(Event);
                return true;
            }
            else
                return false;
        }

        public void UpdateEvent(Event Event)
        {
            context.Entry(Event).State = EntityState.Modified;
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
        // ~EventRepository() {
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