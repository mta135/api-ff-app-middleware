using FFAppMiddleware.Model.Models.Agents;

namespace FFappMiddleware.Application.Services.Abstract
{
    public interface IAgentManagementService
    {
        Task<List<AgentModel>> GetAgents();
    }
}
