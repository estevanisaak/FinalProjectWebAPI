using FinalProjectWebAPI.Dtos;
using FinalProjectWebAPI.Models;

namespace FinalProjectWebAPI.Interfaces
{
    public interface IRepository
    {
        public void RestartDataBase();
        public List<NbaTeamModel> GetDataBaseContent();
        public void Insert(NbaTeamModel team);
        public void Delete(int id);
        public NbaTeamModel Update(int id, NbaTeamDto teamUpdated);
        public bool Exists(int id);
        public int GetLastId();
    }
}
