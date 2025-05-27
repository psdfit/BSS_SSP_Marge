/* **** Aamer Rehman Malik *****/

using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVTSPEmployment
    {
        //public DataTable GetClasses(int userID);
        List<RD_ClassForTSPModel> GetClasses(int userID);

        //public DataSet GetCompletedTraineeByClass(int ClassID);
        List<RD_CompletedTraineeByClassModel> GetCompletedTraineeByClass(int ClassID);
        List<RD_CompletedTraineeByClassModel> GetCompletedTraineeByClassOnJob(int ClassID);

        bool SavePlacementFormE(TSPEmploymentModel TSPEmploymentModel);

        List<TSPEmploymentModel> FetchPlacementFormE();

        List<TSPEmploymentModel> FetchPlacementFormE(bool InActive);

        void ActiveInActive(int? PlacementID, bool? InActive, int CurUserID);

        int SubmitClassEmployment(int ClassID, int CurUserID);

        List<TSPEmploymentModel> FetchPlacementFormE(TSPEmploymentModel model);

        DataSet GetFormalTSPList(int ClassID);

        DataSet GetSelfTSPList(int ClassID);

        TSPEmploymentModel GetTraineeData(int classID, int traineeID);

        bool ForwardedToTelephonic(ForwardToTelephonicVerification Model);
        public List<RD_ClassForTSPModel> FetchClassFilters(int[] filters);
        public List<RD_ClassForTSPModel> FetchClassFiltersOnJob(int[] filters);

        public DeoDashboardModel GetDeoDashboardStats();
        public List<RD_ClassForTSPModel> GetClassesForEmploymentVerification(int PlacementID, int VerificationMethodID, int TspID, int ClassID);
        public List<RD_ClassForTSPModel> GetTelephonicEmploymentVerificationClasses(int PlacementID, int VerificationMethodID, int TspID, int ClassID);

        public List<TSPEmploymentExcelModel> FetchPlacementsForDEOToExport(TSPEmploymentExcelModel mod);

        public List<RD_ClassForTSPModel> GetTelephonicEmploymentVerificationClasses();

        public List<TSPEmploymentExcelModel> FetchReportedPlacementToExport(QueryFilters filters);

        public List<RD_ClassForTSPModelExportExcelVerifedEmploymentReport> FetchVerifiedPlacementToExport(QueryFilters filters);

        public List<RD_TSPForEmploymentVerificationModel> GetTPSDetailForEmploymentVerification(int placementTypeId, int veificationMethodId);

        public List<RD_ClassForTSPModel> FetchDEOEmploymentClassesByTSP(int pId, int vmId, int tspId);

        public List<TSPEmploymentModel> FetchTelephonicPlacementFormE(TSPEmploymentModel mod);
        public List<TSPEmploymentModel> FetchPlacementFormEByTraineeID(TSPEmploymentModel mod);
        public List<TSPEmploymentModel> FetchEmployedTraineesForTSP(TSPEmploymentModel mod);
        public List<TSPEmploymentModel> FetchReportedPlacementFormE(TSPEmploymentModel mod);
        public List<TSPEmploymentModel> FetchPlacementFormEForVerification(TSPEmploymentModel mod);
        public List<TSPEmploymentModel> FetchTraineeForEmploymentVerification(TSPEmploymentModel mod);

    }
}