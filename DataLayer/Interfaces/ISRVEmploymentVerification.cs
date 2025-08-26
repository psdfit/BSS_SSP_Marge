using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static DataLayer.Models.CallCenterVerificationModels;

namespace DataLayer.Interfaces
{
    public interface ISRVEmploymentVerification
    {
        EmploymentVerificationModel GetByID(int ID);
        List<EmploymentVerificationModel> SaveEmploymentVerification(EmploymentVerificationModel EmploymentVerification);
        List<EmploymentVerificationModel> SaveEmploymentVerificationOJT(EmploymentVerificationModel EmploymentVerification);
        List<EmploymentVerificationModel> FetchEmploymentVerification(EmploymentVerificationModel mod);
        List<EmploymentVerificationModel> FetchEmploymentVerification();
        List<EmploymentVerificationModel> FetchEmploymentVerification(bool InActive);
        void ActiveInActive(int ID, bool? InActive, int CurUserID);
        int SubmitVerification(int ClassID, int CurUserID);
        int SubmitVerificationOJT(int ClassID, int CurUserID);
        void UpdateVerifyStatus(int PlacementID, bool? Status, int CurUserID);
        DataTable GetVerificationMethod(int PlacementID);
        public List<EmploymentVerificationModel> UpdateTelephonicEmploymentVerification(EmploymentVerificationModel EmploymentVerification);
        public List<EmploymentVerificationModel> UpdateTelephonicEmploymentVerificationOJT(EmploymentVerificationModel EmploymentVerification);
        public List<CallCenterVerificationTraineeModel> GetCallCenterVerificationTrainee();
        public List<CallCenterVerificationSupervisorModel> GetCallCenterVerificationSupervisor();
        public List<CallCenterVerificationCommentsModel> GetCallCenterVerificationComments();
        public int SubmitVerificationByCallCenter(int ClassID, int CurUserID);
        public int SubmitVerificationByCallCenterOJT(int ClassID, int CurUserID);

    }
}

