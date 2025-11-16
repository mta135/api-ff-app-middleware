using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.Agents
{
    public class AgentModel
    {
        public long AegntId { get; set; }

        public decimal? AgentLatitude { get; set; }

        public decimal? AgentLongitude { get; set; }

        public string PhysicalAddress { get; set; }

        public string Phones { get; set; }

        public string Emails { get; set; }

        public bool IsClosedAgent { get; set; }

        public string AgentName { get; set; }

        public TimeSpan? StartHour { get; set; }

        public TimeSpan? EndHour { get; set; }
    }
}
