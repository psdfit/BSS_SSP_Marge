using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class TSPAssignmentModel:ModelBase
    {
        public int TSPAssignmentID { get; set; }
        public int ProvinceID { get; set; }
        public int ClusterID { get; set; }
        public int DistrictID { get; set; }
        public int ProgramID { get; set; }
        public List<int> TspTrainingLocationID { get; set; }
        public List<int> TspAssociationEvaluationID { get; set; }
        public List<int> TSP { get; set; }
        //public int[]? TSP { get; set; }
        public int TradeLotID { get; set; }

        public int UserID { get; set; }
    }
}
