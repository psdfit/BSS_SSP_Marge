using DataLayer.Models;
using DataLayer.Models.SSP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVAssociation
    {
      
        public DataTable SaveAssociationSubmission(AssociationSubmissionModel data);
        public DataTable SaveAssociationEvaluation(AssociationEvaluationModel data);
        public void SaveTSPAssignment(TSPAssignmentModel data);
        public DataTable FetchDataListBySPName(string spName);
        public DataTable LoopinData(DataTable dt, string[] attachmentColumns);

    }
}
