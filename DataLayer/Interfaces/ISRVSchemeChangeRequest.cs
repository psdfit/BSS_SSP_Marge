using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{

    public interface ISRVSchemeChangeRequest
    {
        public SchemeChangeRequestModel GetBySchemeChangeRequestID(int SchemeChangeRequestID, SqlTransaction transaction = null);
        public List<SchemeChangeRequestModel> SaveSchemeChangeRequest(SchemeChangeRequestModel SchemeChangeRequest);
        public List<SchemeChangeRequestModel> FetchSchemeChangeRequest(SchemeChangeRequestModel mod);
        public List<SchemeChangeRequestModel> FetchSchemeChangeRequest();
        public List<SchemeChangeRequestModel> FetchSchemeChangeRequest(bool InActive);
        public void Update_CR_SchemeApprovalHistory(SchemeChangeRequestModel model);
        public bool CrSchemeApproveReject(SchemeChangeRequestModel model, SqlTransaction transaction = null);

        public void ActiveInActive(int SchemeChangeRequestID, bool? InActive, int CurUserID);
    }
}
