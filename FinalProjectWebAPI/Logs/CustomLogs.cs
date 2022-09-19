using FinalProjectWebAPI.Models;
using System.Text.Json;

namespace FinalProjectWebAPI.Logs
{
    public class CustomLogs
    {
        const string PUT = "put";
        const string PATCH = "patch";
        const string DELETE = "delete";

        public static void SaveLog(string method, NbaTeamModel teamBefore, NbaTeamModel? teamAfter = null)
        {
            var now = DateTime.Now.ToString("G");

            if (method.Equals(PUT, StringComparison.InvariantCultureIgnoreCase) || method.Equals(PATCH, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"{now} - Time {teamAfter.Id}: {teamAfter.City} {teamAfter.Name} - Alterado de {JsonSerializer.Serialize(teamBefore)} para {JsonSerializer.Serialize(teamAfter)}");
            }
            else if (method.Equals(DELETE, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"{now} - Time {teamBefore.Id}: {teamBefore.City} {teamBefore.Name} - Removido");
            }
        }
    }
}
