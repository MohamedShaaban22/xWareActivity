using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWAREActivity.Models;

namespace xWAREActivity.Interface
{
    public interface IEventRepository :IDisposable
    {
        IEnumerable<Event> GetEvents();
        Event GetEventByID(Guid EventId);
        void InsertEvent(Event Event);
        bool DeleteEvent(Guid EventID);
        void UpdateEvent(Event Event);
        void Save();

    }
}
