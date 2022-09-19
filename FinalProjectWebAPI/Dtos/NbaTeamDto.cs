namespace FinalProjectWebAPI.Dtos
{
    public class NbaTeamDto
    {
        public string City { get; set; }

        public string Name { get; set; }

        public string Conference { get; set; }

        public string Division { get; set; }

        public int GamesWon { get; set; }
        public int GamesLost => 82 - (int)GamesWon;

        public NbaTeamDto(string city, string name, string conference, string division, int gamesWon)
        {
            City = city;
            Name = name;
            Conference = conference;
            Division = division;
            GamesWon = gamesWon;
        }
    }
}
