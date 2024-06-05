/* **** Aamer Rehman Malik *****/

using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVPSPEmployment
    {
        //public DataTable GetClasses(int userID);
        //List<RD_ClassForPSPModel> GetClasses(int userID);

        //public DataSet GetCompletedTraineeByClass(int ClassID);

        List<PSPEmploymentModel> FetchPlacementFormE();

        List<PSPEmploymentModel> FetchPlacementFormE(bool InActive);

        void ActiveInActive(int? PlacementID, bool? InActive, int CurUserID);

        int SubmitClassEmployment(int ClassID);

        List<PSPEmploymentModel> FetchPlacementFormE(PSPEmploymentModel model);

        DataSet GetFormalPSPList(int ClassID);

        DataSet GetSelfPSPList(int ClassID);

        PSPEmploymentModel GetTraineeData(int classID, int traineeID);

        bool ForwardedToTelephonic(int[] list);

        public List<RD_ClassForTSPModel> FetchClassFilters(QueryFilters filters);

        public void SavePSPBatch(PSPBatchModel mod);

        public List<PSPBatchModel> FetchPSPBatches();
        public List<PSPBatchModel> FetchPSPBatchTraineeByID(PSPBatchModel mod);
        public void SavePSPBatchTrainees(List<PSPBatchModel> ls);
        public List<RD_CompletedTraineeByClassModel> GetCompletedTraineeByClass(QueryFilters filters);
        public List<PSPBatchModel> FetchPSPInterestedTraineesForAssignment(int pspbatchid);
        public void UpdateTraineesPSP(PSPBatchModel mod);
        public List<PSPBatchModel> FetchPSPAssignedTrainees(PSPBatchModel mod);
        public List<PSPBatchModel> GetPSPTraineeForDEOVerification(QueryFilters filters);
        public List<PSPEmploymentModel> FetchPlacementFormE_PSP(PSPEmploymentModel mod);
        public List<RD_CompletedTraineeByTraineeIDsModel> GetCompletedTraineeByTraineeIDs(string TraineeIDs);
        public bool SavePlacementFormE(PSPEmploymentModel PlacementFormE);


    }
}