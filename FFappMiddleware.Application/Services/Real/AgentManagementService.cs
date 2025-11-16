using FFappMiddleware.Application.Services.Abstract;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.Agents;
using System.Threading.Tasks;

namespace FFappMiddleware.Application.Services.Real
{
    public class AgentManagementService : IAgentManagementService
    {
        private readonly IAgentManagementRepository _agentManagementRepository;

        public AgentManagementService(IAgentManagementRepository agentManagementRepository)
        {
            _agentManagementRepository = agentManagementRepository;
        }

        public  async Task<List<AgentModel>> GetAgents()
        {
            List<AgentModel> agents = await _agentManagementRepository.GetAllAgents();

            if (agents == null || agents.Count == 0)
                throw new Exception("Eroare la incarcarea agentilor");

            return agents;
        }
    }
}
