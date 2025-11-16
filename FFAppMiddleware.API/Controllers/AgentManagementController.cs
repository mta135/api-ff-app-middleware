using FFappMiddleware.Application.Services.Abstract;
using FFappMiddleware.DataBase.Logger;
using FFAppMiddleware.Model.Models.Agents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Authorize]
    [Route("api/Agent")]
    [ApiController]
    public class AgentManagementController : ControllerBase
    {
        private readonly IAgentManagementService _agentManagementService;

        public AgentManagementController(IAgentManagementService agentManagementService)
        {
            _agentManagementService = agentManagementService;
        }


        [HttpGet("Agents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAgents()
        {
            try
            {
                List<AgentModel> users = await _agentManagementService.GetAgents();
                return Ok(users);
            }
            catch (Exception ex)
            {
                WriteLog.Web.Error("Error retrieving data from the database", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
    }
}
