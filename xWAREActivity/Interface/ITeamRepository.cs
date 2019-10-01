using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWAREActivity.Models;

namespace xWAREActivity.Interface
{
    public interface ITeamRepository : IDisposable
    {
        IEnumerable<Team> GetTeams();
        Team GetTeamByID(Guid TeamId);
        void InsertTeam(Team Team);
        bool DeleteTeam(Guid TeamID);
        void UpdateTeam(Team Team);
        void Save();
    }
}
