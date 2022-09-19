using FinalProjectWebAPI.Dtos;
using FinalProjectWebAPI.Interfaces;
using FinalProjectWebAPI.Models;
using System.Text.Json;

namespace FinalProjectWebAPI.Repositories
{
    public class Repository : IRepository
    {
        private readonly string _databaseFile;

        public Repository()
        {
            _databaseFile = Path.Combine(Directory.GetCurrentDirectory(), "dataBase.json");
        }

        public void RestartDataBase()
        {
            string NbaTeamsFileContent = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "NbaTeams.json"));
            File.WriteAllText(_databaseFile, NbaTeamsFileContent);
        }

        public List<NbaTeamModel> GetDataBaseContent()
        {
            return JsonSerializer.Deserialize<List<NbaTeamModel>>(File.ReadAllText(_databaseFile));
        }

        public void UpdateDataBaseContent(List<NbaTeamModel> nbaTeams)
        {
            File.WriteAllText(_databaseFile, JsonSerializer.Serialize<List<NbaTeamModel>>(nbaTeams));
        }

        public void Insert(NbaTeamModel newTeam)
        {
            var nbaTeams = GetDataBaseContent();
            nbaTeams.Add(newTeam);
            UpdateDataBaseContent(nbaTeams);
        }

        public void Delete(int id)
        {
            var nbaTeams = GetDataBaseContent();
            var team = nbaTeams.Where(x => x.Id == id).FirstOrDefault();
            nbaTeams.Remove(team);
            UpdateDataBaseContent(nbaTeams);
        }

        public NbaTeamModel Update(int id, NbaTeamDto teamUpdated)
        {
            var nbaTeams = GetDataBaseContent();
            int teamIndex = nbaTeams.IndexOf(nbaTeams.Where(x => x.Id == id).FirstOrDefault());
            nbaTeams[teamIndex].City = teamUpdated.City;
            nbaTeams[teamIndex].Name = teamUpdated.Name;
            nbaTeams[teamIndex].Conference = teamUpdated.Conference;
            nbaTeams[teamIndex].Division = teamUpdated.Division;
            nbaTeams[teamIndex].GamesWon = teamUpdated.GamesWon;
            UpdateDataBaseContent(nbaTeams);
            return nbaTeams[teamIndex];
        }

        public bool Exists(int id)
        {
            var nbaTeams = GetDataBaseContent();
            var team = nbaTeams.Where(x => x.Id == id).FirstOrDefault();
            if (team == null)
                return false;
            return true;
        }

        public int GetLastId()
        {
            var nbaTeams = GetDataBaseContent();
            if (nbaTeams.Count == 0)
                return 0;
            int lastId = nbaTeams.Max(x => x.Id);
            return lastId;
        }
    }
}