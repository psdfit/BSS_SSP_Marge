using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVDashboard
    {
        public DataTable FetchTrades();
        public DataTable FetchClusters();
        public DataTable FetchDistricts();
        public DataTable FetchSchemes();
        public DataTable FetchSchemesGSR();
        public DataTable FetchTSPs();
        public DataTable FetchTSPDetail();
        public DataTable FetchTSPsByScheme(int SchemeID);
        public DataTable FetchTSPsByMultipleSchemes(string SchemeID);
        public DataTable FetchClassesBySchemeTSP(int SchemeID, int TspID);
        public DataTable FetchClassesByTSP(int TspID);
        public DataTable FetchMultipleClassesByTSP(string TspID);
        public DataTable FetchPrograms();
        public DataTable FetchSchemesByKam(int KamID);
        public DataTable FetchTSPsByKamScheme(int KamID, int SchemeID);
        public DataTable FetchSchemesByUsers(int UserID);
        public DataTable FetchSchemesByGSRUsers(int UserID);
        public DataTable FetchClassesBySchemeUser(int SchemeID, int UserID);
        public DataTable FetchClassesByMultipleSchemeUser(string SchemeID, int UserID);
        public DataTable FetchSchemesByProgramCategory(int PCategoryID);
        public DataTable FetchClasses();
        public DataTable FetchClassesByUser(int UserID);
        public List<TimeLineChart> FetchTraineeJourney(string type, int ID, string startDate, string endDate);
        public void UpdateTraineeJourneyParam(string type, int ID, string startDate, string endDate, ref SqlParameter[] param);
        public List<TimeLineChart> SetTraineeJourneyData(DataSet data);
        public List<TimeLineChart> FetchTraineeJourneySingle(string traineeCode, string traineeCNIC);
        public List<TimeLineChart> SetTraineeJourneySingleData(DataSet data);
        public ClassJourneyModel FetchClassJourney(int ClassID);
        public ClassJourneyModel SetClassJourneyData(DataSet data);
        public ManagementDashboard FetchManagementDashboard(string type, int ID, string startDate, string endDate);
    }
}
