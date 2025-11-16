using FFAppMiddleware.Model.Models.Agents;

namespace FFappMiddleware.DataBase.Repositories.Abstract
{
    public interface IAgentManagementRepository
    {
        Task<List<AgentModel>> GetAllAgents();
    }
}
