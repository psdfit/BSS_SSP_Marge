using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


namespace DataLayer.Interfaces
{

    public interface ISRVInstructorChangeRequest
    {
        public InstructorChangeRequestModel GetByInstructorChangeRequestID(int InstructorChangeRequestID, SqlTransaction transaction = null);
        public List<InstructorChangeRequestModel> SaveInstructorChangeRequest(InstructorChangeRequestModel InstructorChangeRequest);
        public List<InstructorChangeRequestModel> FetchInstructorChangeRequest(InstructorChangeRequestModel mod);
        public List<InstructorChangeRequestModel> FetchInstructorChangeRequest();
        public List<InstructorChangeRequestModel> FetchFilteredInstructorChangeRequest(QueryFilters filters);
        public List<InstructorChangeRequestModel> FetchInstructorChangeRequest(bool InActive);
        public void ActiveInActive(int InstructorChangeRequestID, bool? InActive, int CurUserID);
        public bool CrInstructorApproveReject(InstructorChangeRequestModel model, SqlTransaction transaction = null);
        public void SaveInstructor(InstructorModel Instructor);
        public bool CrNewInstructorApproveReject(InstructorChangeRequestModel model, SqlTransaction transaction = null);
        //public void SaveInstructorByCR(InstructorChangeRequestModel Instructor);
        public List<InstructorChangeRequestModel> SaveInstructorByCR(InstructorChangeRequestModel Instructor);
        public List<InstructorChangeRequestModel> FetchNewInstructorRequest();
        public List<InstructorChangeRequestModel> FetchFilteredNewInstructorRequest(QueryFilters filters);
        public List<InstructorChangeRequestModel> FetchNewInstructorRequestAttachments(int id);
        public List<InstructorChangeRequestModel> FetchNewInstructorChangeRequest(int userid);


        InstructorChangeRequestModel GetByInstructorChangeRequestID_Notification(int InstructorChangeRequestID, SqlTransaction transaction = null);
        public InstructorChangeRequestModel GetByNewInstructorChangeRequestID_Notification(int CRNewInstructorID, SqlTransaction transaction = null);
    }
}
