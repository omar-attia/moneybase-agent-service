using Agent.Models;
using Agent.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agent.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentAssignmentController(IAgentAssignmentService agentAssignmentService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AssignChatSession([FromBody] AgentAssignmentModel agentAssignmentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await agentAssignmentService.AssignChatSessionToAgent(agentAssignmentModel.SessionId);
                return Ok("Agent Assignment Created Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] AgentAssignmentModel agentAssignmentModel)
        {
            try
            {
                var result = await agentAssignmentService.UpdateAssignmentAsync(id, agentAssignmentModel);
                if (!result)
                {
                    return NotFound();
                }

                return Ok("Agent Assignment Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
