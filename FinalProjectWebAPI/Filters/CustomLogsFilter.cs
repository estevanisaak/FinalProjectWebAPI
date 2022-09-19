using FinalProjectWebAPI.Interfaces;
using FinalProjectWebAPI.Logs;
using FinalProjectWebAPI.Models;
using FinalProjectWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinalProjectWebAPI.Filters
{
    public class CustomLogsFilter : Attribute, IActionFilter
    {
        const string PUT = "put";
        const string PATCH = "patch";
        const string DELETE = "delete";

        private readonly List<int> _successStatusCodes;
        private readonly IRepository _repository;
        private readonly Dictionary<int, NbaTeamModel> _contextDict;

        public CustomLogsFilter()
        {
            _repository = new Repository();
            _contextDict = new Dictionary<int, NbaTeamModel>();
            _successStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status204NoContent };
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_successStatusCodes.Contains(context.HttpContext.Response.StatusCode))
            {
                int id = 0;
                if (int.TryParse(context.HttpContext.Request.Path.ToString().Split("/").Last(), out id))
                {
                    NbaTeamModel teamBefore;
                    if (_contextDict.TryGetValue(id, out teamBefore))
                    {
                        string method = context.HttpContext.Request.Method;
                        if (method.Equals(PUT, StringComparison.InvariantCultureIgnoreCase) || method.Equals(PATCH, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var teamAfter = _repository.GetDataBaseContent().Where(x => x.Id == id).FirstOrDefault();
                            if (teamAfter != null)
                            {
                                CustomLogs.SaveLog(method, teamBefore, teamAfter);
                            }
                        }
                        else if (method.Equals(DELETE, StringComparison.InvariantCultureIgnoreCase))
                        {
                            CustomLogs.SaveLog(method, teamBefore);
                        }
                        _contextDict.Remove(id);
                    }
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int id = 0;
            if (int.TryParse(context.ActionArguments["id"].ToString(), out id))
            {
                var teamBefore = _repository.GetDataBaseContent().Where(x => x.Id == id).FirstOrDefault();
                if (teamBefore != null)
                {
                    if (_contextDict.ContainsKey(id))
                        _contextDict[id] = teamBefore;
                    else
                        _contextDict.Add(id, teamBefore);
                }

            }
        }
    }
}
