
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class CallCenterAgentModel : ModelBase
    {
        public CallCenterAgentModel() : base() { }
        public CallCenterAgentModel(bool InActive) : base(InActive) { }
        
        public int CallCenterAgentID { get; set; }
        public string AgentName { get; set; }
        public string ContactNumber { get; set; }
        public int VisitPlanID { get; set; }

    }}
