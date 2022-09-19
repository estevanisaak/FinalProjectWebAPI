using FinalProjectWebAPI.Dtos;
using FinalProjectWebAPI.Filters;
using FinalProjectWebAPI.Interfaces;
using FinalProjectWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NbaTeamController : ControllerBase
    {
        private readonly ILogger<NbaTeamController> _logger;
        private readonly IRepository _repository;

        public NbaTeamController(ILogger<NbaTeamController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<NbaTeamModel>), StatusCodes.Status200OK)]
        public IActionResult Get([FromQuery] int page, int maxResults)
        {
            if (maxResults <= 0)
                maxResults = _repository.GetDataBaseContent().Count;
            var response = _repository.GetDataBaseContent().AsQueryable().Skip((page - 1) * maxResults).Take(maxResults);
            return Ok(response);
        }

        [HttpPost("query")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(List<NbaTeamModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<NbaTeamModel>), StatusCodes.Status204NoContent)]
        public IActionResult Get([FromQuery] int page, int maxResults, [FromBody] NbaTeamDto searchFilter)
        {
            if (maxResults <= 0)
                maxResults = _repository.GetDataBaseContent().Count;

            var response = _repository.GetDataBaseContent().AsQueryable();
            if (String.IsNullOrEmpty(searchFilter.Name) == false && searchFilter.Name.ToLower() != "string")
                response = response.Where(x => x.Name.ToLower().Contains(searchFilter.Name.ToLower()));
            if (String.IsNullOrEmpty(searchFilter.City) == false && searchFilter.City.ToLower() != "string")
                response = response.Where(x => x.City.ToLower().Contains(searchFilter.City.ToLower()));
            if (String.IsNullOrEmpty(searchFilter.Conference) == false && searchFilter.Conference.ToLower() != "string")
                response = response.Where(x => x.Conference.ToLower().Contains(searchFilter.Conference.ToLower()));
            if (String.IsNullOrEmpty(searchFilter.Division) == false && searchFilter.Division.ToLower() != "string")
                response = response.Where(x => x.Division.ToLower().Contains(searchFilter.Division.ToLower()));

            response = response.Skip((page - 1) * maxResults).Take(maxResults);
            if (response.Count() == 0)
                return NoContent();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NbaTeamModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult Get([FromRoute] int id)
        {
            if (_repository.Exists(id) == false)
                return NotFound("Time nao encontrado.");
            var team = _repository.GetDataBaseContent().FirstOrDefault(x => x.Id == id);
            return Ok(team);
        }

        [HttpPut("{id}")]
        [Consumes(typeof(NbaTeamModel), "application/json")]
        [ProducesResponseType(typeof(NbaTeamModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [CustomLogsFilter]
        public IActionResult Put([FromRoute] int id, [FromBody] NbaTeamDto updatedTeam)
        {
            if (_repository.Exists(id))
            {
                return Ok(_repository.Update(id, updatedTeam));
            }
            else
            {
                var newTeam = new NbaTeamModel(id, updatedTeam.City, updatedTeam.Name, updatedTeam.Conference, updatedTeam.Division, updatedTeam.GamesWon);
                _repository.Insert(newTeam);
                return Created(String.Empty, updatedTeam);
            }
        }

        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [CustomLogsFilter]
        public IActionResult Patch([FromRoute] int id,[FromBody] NbaTeamDto updatedTeam)
        {
            if (_repository.Exists(id) == false)
                return NotFound("Time nao encontrado.");
            _repository.Update(id, updatedTeam);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [CustomLogsFilter]
        public IActionResult Delete([FromRoute] int id)
        {
            if (_repository.Exists(id) == false)
                return NotFound("Time nao encontrado.");
            _repository.Delete(id);
            return Ok("Time removido.");
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public IActionResult Post([FromBody] NbaTeamModel newTeam)
        {
            int lastId = _repository.GetLastId();
            newTeam.Id = lastId + 1;
            return Created(String.Empty, newTeam);
        }
    }
}