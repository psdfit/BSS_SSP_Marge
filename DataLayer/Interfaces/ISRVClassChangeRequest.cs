using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{

    public interface ISRVClassChangeRequest
    {
        public ClassChangeRequestModel GetByClassChangeRequestID(int ClassChangeRequestID, SqlTransaction transaction = null);
        public List<ClassChangeRequestModel> SaveClassChangeRequest(ClassChangeRequestModel ClassChangeRequest);
        public List<ClassChangeRequestModel> FetchClassChangeRequest(ClassChangeRequestModel mod);
        public List<ClassChangeRequestModel> FetchClassChangeRequest();
        public List<ClassChangeRequestModel> FetchClassChangeRequest(bool InActive);
        public void ActiveInActive(int ClassChangeRequestID, bool? InActive, int CurUserID);
        public bool CrClassApproveReject(ClassChangeRequestModel model, SqlTransaction transaction = null);
        public List<ClassChangeRequestModel> FetchClassesForLocationChangeByUser(int UserID);
        public List<ClassChangeRequestModel> SaveClassDatesChangeRequest(ClassChangeRequestModel ClassChangeRequest);
        public bool CrClassDatesApproveReject(ClassChangeRequestModel model, SqlTransaction transaction = null);
        public List<ClassChangeRequestModel> FetchClassDatesChangeRequest();
        public List<ClassChangeRequestModel> FetchClassesForDatesChangeByUser(int UserID);
        public List<ClassChangeRequestModel> FetchClassChangeRequestByFilter(QueryFilters filters);
        public ClassChangeRequestModel GetByClassChangeRequestID_Notification(int ClassChangeRequestID, SqlTransaction transaction = null);
    }
}
