namespace FinalProjectWebAPI.Models
{
    public class NbaTeamModel
    {
        public int Id { get; set; }
        public string City { get; set; }

        public string Name { get; set; }

        public string Conference { get; set; }

        public string Division { get; set; }

        public int GamesWon { get; set; }
        public int GamesLost => 82 - (int)GamesWon;

        public NbaTeamModel(int id, string city, string name, string conference, string division, int gamesWon)
        {
            Id = id;
            City = city;
            Name = name;
            Conference = conference;
            Division = division;
            GamesWon = gamesWon;
        }
    }
}
