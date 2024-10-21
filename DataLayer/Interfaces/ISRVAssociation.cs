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
      
        DataTable SaveAssociationSubmission(AssociationSubmissionModel data);
        DataTable SaveAssociationEvaluation(AssociationEvaluationModel data);
        void SaveTSPAssignment(TSPAssignmentModel data);
        DataTable FetchDataListBySPName(string spName);
        
        DataTable FetchReportBySPNameAndParam(string spName, string param, int value);
        DataTable LoopingData(DataTable dt, string[] attachmentColumns);

    }
}
