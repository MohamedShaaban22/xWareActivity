using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWAREActivity.Models;
using xWAREActivity.ViewModel;
namespace xWAREActivity.Interface
{
    public interface IUserRepository:IDisposable
    {
        IEnumerable<User> GetUsers();
        User GetUserById(Guid userid);
        void InsertUser(User user);
        bool DeleteUser(Guid userid);
        void UpdateUser(User user);
        int UserRank(Guid userid);
        IEnumerable<TotalRankView>TotalRank();
        User FindUser(string email, string password);
        void Save();
    }
}
