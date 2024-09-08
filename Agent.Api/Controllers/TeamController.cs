using Agent.Models;
using Agent.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agent.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController(ITeamService teamService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTeam(TeamModel teamModel)
        {
            try
            {
                await teamService.CreateTeam(teamModel);
                return Ok("Team Created successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTeam(int id)
        {
            try
            {
                var team = await teamService.GetTeamAsync(id);
                return Ok(team);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> CreateTeam(int id, TeamModel teamModel)
        {
            try
            {
                var response = await teamService.UpdateTeamAsync(id, teamModel);
                if (response)
                    return Ok("Team Created successfully");

                return BadRequest("Failed to update team");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
