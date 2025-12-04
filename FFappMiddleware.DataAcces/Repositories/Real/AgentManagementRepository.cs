using FFappMiddleware.DataAcces.DataBaseConnection;
using FFappMiddleware.DataBase.Logger;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.Agents;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FFappMiddleware.DataBase.Repositories.Real
{
    public class AgentManagementRepository : IAgentManagementRepository
    {
        private readonly DataBaseAccesConfig _db;

        public AgentManagementRepository()
        {
            _db = new DataBaseAccesConfig();
        }


        public async Task<List<AgentModel>> GetAllAgents()
        {
            try
            {
                List<AgentModel> agents = new List<AgentModel>();

                string query = @"SELECT
                                       a.agent_id branch_id,
                                       a.agent_latitude coord1,
                                       a.agent_longitude coord2,
                                       a.agent_physical_address address,
                                       a.agent_phone phones,
                                       a.agent_email emails,
                                       CASE WHEN a.agent_is_closed = 1 THEN 0 ELSE 1 END status,
                                       a.agent_name name,
									   workingHour.start_hour,
									   workingHour.end_hour
      
                                       FROM agents a 
									OUTER APPLY(
									   SELECT 
												TOP 1 
													awh.start_hour,
													awh.end_hour
												FROM agents_working_hours awh
													INNER JOIN dbo.days d
														ON d.day_id = awh.day_id
										
												WHERE
													awh.agent_id = a.agent_id
													ORDER BY awh.day_id
									   ) workingHour
        							WHERE a.agent_fiscal_code='1005600017723' AND a.agent_is_company=1";

                using (SqlCommand command = new SqlCommand(query, await _db.PharmaFFConnectionAsync))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            AgentModel agentModel = new AgentModel();

                            if (reader["branch_id"] != DBNull.Value)
                                agentModel.AegntId = Convert.ToInt64(reader["branch_id"]);

                            if(reader["coord1"] != DBNull.Value)    
                                agentModel.AgentLatitude = Convert.ToDecimal(reader["coord1"]);

                            if (reader["coord2"] != DBNull.Value)   
                                agentModel.AgentLongitude = Convert.ToDecimal(reader["coord2"]);

                            if (reader["address"] != DBNull.Value)
                                agentModel.PhysicalAddress = reader["address"].ToString();
                            
                            if (reader["phones"] != DBNull.Value)
                                agentModel.Phones = reader["phones"].ToString();

                            if (reader["emails"] != DBNull.Value)
                                agentModel.Emails = reader["emails"].ToString();

                            if (reader["status"] != DBNull.Value)
                                agentModel.IsClosedAgent = Convert.ToBoolean(reader["status"]);

                            if (reader["name"] != DBNull.Value)
                                agentModel.AgentName = reader["name"].ToString();

                            if(reader["start_hour"] != DBNull.Value)
                                agentModel.StartHour = reader.GetTimeSpan(reader.GetOrdinal("start_hour"));

                            if (reader["end_hour"] != DBNull.Value)
                                agentModel.EndHour = reader.GetTimeSpan(reader.GetOrdinal("end_hour"));

                            agents.Add(agentModel);
                        }
                        reader.Close();
                    }
                  
                }

                return agents;
            }
            catch (SqlException sqlEx)
            {
                WriteLog.DB.Error("SqlException GetAllAgents: ", sqlEx);
                return null;
            }
            catch (Exception ex)
            {
                WriteLog.DB.Error("Exception GetAllAgents: ", ex);
                return null;
            }
        }
    }
}
