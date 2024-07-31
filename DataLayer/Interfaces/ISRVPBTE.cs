using DataLayer.Models;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{

    public interface ISRVPBTE
    {
        List<PBTEClassModel> FetchPBTEClasses(PBTEQueryFilters filters);
        List<PBTEClassModel> FetchNAVTTCClasses(PBTEQueryFilters filters);

        List<PBTETspModel> FetchPBTETSPs(PBTEQueryFilters filters);
        List<PBTETspModel> FetchNAVTTCTSPs(PBTEQueryFilters filters);

        List<PBTETraineeModel> FetchPBTETrainees(PBTEQueryFilters filters);
        List<PBTETraineeModel> FetchNAVTTCTrainees(PBTEQueryFilters filters);

        List<PBTETraineeModel> FetchPBTEDropoutTrainees(PBTEQueryFilters filters);
        public List<PBTEModel> FetchPBTEStatsData(PBTEQueryFilters filters);
        public List<PBTEModel> FetchNAVTTCStatsData(PBTEQueryFilters filters);

        List<PBTETraineeModel> FetchNAVTTCDropoutTrainees(PBTEQueryFilters filters);
        void UpdatePBTEClasses(List<PBTEModel> ls);

        List<SchemeModel> FetchPBTESchemes();

        void UpdatePBTETSPs(List<PBTEModel> ls);

        void UpdatePBTETrainees(List<PBTEModel> ls);

        void UpdatePBTETraineesResult(List<PBTEModel> ls, int CurUserID);

        public List<PBTETradeModel> FetchTradePBTE(PBTEQueryFilters filters);

        //public void UpdatePBTEDropoutTraineesLock();

        void UpdatePBTETrades(List<PBTEModel> ls);

        public List<PBTETraineeModel> FetchPBTETraineesExamResult(PBTEQueryFilters filters);

        public List<PBTETraineeModel> FetchNAVTTCTraineesExamResult(PBTEQueryFilters filters);
        public List<PBTETraineeExamScriptModel> FetchPBTETraineesExamScriptData(PBTEQueryFilters filters);
        public void UpdateNAVTTCClasses(List<PBTEModel> ls);
        public void UpdateNAVTTCTrainees(List<PBTEModel> ls);
        public List<PBTETraineeExamScriptModel> FetchNAVTTCTraineesRegisterSqlScript(PBTEQueryFilters filters);

        public List<PBTETraineeModel> FetchNAVTTCTraineesSqlScript(PBTEQueryFilters filters);

       bool savePBTESchemeMapping(List<SchemeMappingModel> data,int CurUser);
       bool savePBTEDBFile(string data,int CurUser);
        DataTable FetchReportBySPName(string spName);


    }
}
