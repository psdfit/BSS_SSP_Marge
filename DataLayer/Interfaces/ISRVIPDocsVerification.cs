using DataLayer.JobScheduler.Scheduler;
using DataLayer.Models;
using DataLayer.Models.DVV;
using DataLayer.Models.IP;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVIPDocsVerification
    {      
       DataTable GetTSPDetailsByClassID(int classID);
        // New method to fetch IPTrainees
       DataTable GetIPTrainees(int? traineeID, int? schemeID, int? tspID, int? classID, int? userID, int? oid);

       void SaveVisaStampingDocs(int traineeID, string traineeCode, string traineeName, int tspID, string classCode, List<string> filePaths);
       DataTable GetVisaStampingDocs(int traineeID);
       public bool VisaStampingApproveReject(VisaStampingResponseModel model, SqlTransaction transaction = null);

       void SaveMedicalCostDocs(int traineeID, string traineeCode, string traineeName, int tspID, string classCode, List<string> filePaths);
       DataTable GetMedicalCostDocs(int traineeID);
       public bool MedicalCostApproveReject(MedicalCostResponseModel model, SqlTransaction transaction = null);

       void SavePrometricCostDocs(int traineeID, string traineeCode, string traineeName, int tspID, string classCode, List<string> filePaths);
       DataTable GetPrometricCostDocs(int traineeID);
       public bool PrometricCostApproveReject(PrometricCostResponseModel model, SqlTransaction transaction = null);

       void SaveOtherTrainingCostDocs(int traineeID, string traineeCode, string traineeName, int tspID, string classCode, List<string> filePaths);
       DataTable GetOtherTrainingCostDocs(int traineeID);
       public bool OtherTrainingCostApproveReject(OtherTrainingCostResponseModel model, SqlTransaction transaction = null);

    }
}
