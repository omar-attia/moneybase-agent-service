using Agent.Models;
using Agent.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agent.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentController(IAgentService agentService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAgent([FromBody] AgentModel agentModel)
        {
            try
            {
                await agentService.CreateAgentAsync(agentModel);
                return Ok("Agent Created Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAgent(int id)
        {
            try
            {
                var agent = await agentService.GetAgentAsync(id);
                return Ok(agent);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAgent(int id, [FromBody] AgentModel agent)
        {
            try
            {
                var result = await agentService.UpdateAgentAsync(id, agent);
                if (!result)
                {
                    return NotFound();
                }

                return Ok("Agent Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAgent(int id)
        {
            try
            {
                var result = await agentService.DeleteAgentAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                return Ok("Agent Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateAgentStatus(int id, [FromBody] bool isActive)
        {
            try
            {
                var result = await agentService.UpdateAgentStatusAsync(id, isActive);
                if (!result)
                {
                    return NotFound();
                }

                return Ok("Agent Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
