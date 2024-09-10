using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace DataLayer.Services
{
    public class SRVPBTE : SRVBase, DataLayer.Interfaces.ISRVPBTE
    {
        private readonly ISRVSendEmail srvSendEmail;
        public SRVPBTE(ISRVSendEmail srvSendEmail)
        {
            this.srvSendEmail = srvSendEmail;
        }

        //public PBTEModel GetByPBTEID(int PBTEID)
        //{
        //    try
        //    {
        //        SqlParameter param = new SqlParameter("@PBTEID", PBTEID);
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTE", param).Tables[0];
        //        if (dt.Rows.Count > 0)
        //        {
        //            return RowOfPBTE(dt.Rows[0]);
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        public void UpdatePBTEClasses(List<PBTEModel> ls)
        {
            try
            {
                if (ls[0].PBTECollegeID != null)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Classes_PBTE]", param);
                }
                if (ls[0].NAVTTCCollegeID != null)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Classes_NAVTTC]", param);
                }
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public void UpdateNAVTTCClasses(List<PBTEModel> ls)
        {
            try
            {
                if (ls[0].PBTECollegeID != null)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Classes_NAVTTC]", param);
                }
                //if(ls[0].NAVTTCCollegeID != null)
                //{
                //    SqlParameter[] param = new SqlParameter[1];
                //    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                //    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Classes_NAVTTC]", param);
                //}
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public void UpdatePBTETSPs(List<PBTEModel> ls)
        {
            try
            {
                if (ls[0].PBTEID != 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_TSPs_PBTE]", param);
                }
                if (ls[0].NAVTTCID != 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_TSPs_NAVTTC]", param);
                }

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public void UpdatePBTETrainees(List<PBTEModel> ls)
        {
            try
            {
                if (ls[0].PBTEStudentID != 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Trainees_PBTE]", param);
                }
                if (ls[0].PBTEDistrictID != 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Trainees_NAVTTC]", param);
                }

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public void UpdateNAVTTCTrainees(List<PBTEModel> ls)
        {
            try
            {
                if (ls[0].PBTEDistrictID != 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Trainees_NAVTTC]", param);
                }


            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public void UpdatePBTETrades(List<PBTEModel> ls)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Trades_PBTE]", param);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public void UpdatePBTETraineesResult(List<PBTEModel> ls, int CurUserID)
        {
            try
            {
                ApprovalModel approvalModel = new ApprovalModel();
                ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
                param[1] = new SqlParameter("@CurUserID", CurUserID);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Trainees_Result_PBTE]", param);

                if (ls.Count > 0)
                {
                    foreach (var item in ls)
                    {
                        approvalModel.CustomComments += ",(" + "TraineeCode :" + item.TraineeCode + "," + "ResultStatusName :" + item.ResultStatusName + ")";
                    }

                }


                approvalModel.ProcessKey = EnumApprovalProcess.EXAM_STATUS;
                approvalModel.UserIDs = CurUserID.ToString();
                approvalModel.isUserMapping = true;
                srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        //private void UpdatePBTEClasses(List<PBTEClass> pBTEClass)
        //{
        //    throw new NotImplementedException();
        //}

        private List<SchemeModel> LoopinSchemeData(DataTable dt)
        {
            List<SchemeModel> PBTESchemeL = new List<SchemeModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTESchemeL.Add(RowOfPBTESchemes(r));
            }
            return PBTESchemeL;
        }
        private List<PBTEModel> LoopinPBTEStatsData(DataTable dt)
        {
            List<PBTEModel> PBTEStatsL = new List<PBTEModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTEStatsL.Add(RowOfPBTEStats(r));
            }
            return PBTEStatsL;
        }

        private List<PBTEClassModel> LoopinClassData(DataTable dt)
        {
            List<PBTEClassModel> PBTEClassesL = new List<PBTEClassModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTEClassesL.Add(RowOfPBTEClasses(r));
            }
            return PBTEClassesL;
        }

        private List<PBTETspModel> LoopinTSPData(DataTable dt)
        {
            List<PBTETspModel> PBTETSPsL = new List<PBTETspModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTETSPsL.Add(RowOfPBTETSPs(r));
            }
            return PBTETSPsL;
        }

        private List<PBTETraineeModel> LoopinTraineeData(DataTable dt)
        {
            List<PBTETraineeModel> PBTETraineesL = new List<PBTETraineeModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTETraineesL.Add(RowOfPBTETrainees(r));
            }
            return PBTETraineesL;
        }
        private List<PBTETraineeExamScriptModel> LoopinTraineeExamScriptData(DataTable dt)
        {
            List<PBTETraineeExamScriptModel> PBTETraineesL = new List<PBTETraineeExamScriptModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTETraineesL.Add(RowOfPBTEExamScriptTrainees(r));
            }
            return PBTETraineesL;
        }
        private List<PBTETraineeModel> LoopinNAVTTCTraineeSqlScriptData(DataTable dt)
        {
            List<PBTETraineeModel> PBTETraineesL = new List<PBTETraineeModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTETraineesL.Add(RowOfNAVTTCScriptTrainees(r));
            }
            return PBTETraineesL;
        }
        private List<PBTETraineeExamScriptModel> LoopinNAVTTCTraineeRegisterSqlScriptData(DataTable dt)
        {
            List<PBTETraineeExamScriptModel> PBTETraineesL = new List<PBTETraineeExamScriptModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTETraineesL.Add(RowOfNAVTTCRegisterScriptTrainees(r));
            }
            return PBTETraineesL;
        }

        private List<PBTETraineeModel> LoopinTraineeDropoutData(DataTable dt)
        {
            List<PBTETraineeModel> PBTETraineesL = new List<PBTETraineeModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTETraineesL.Add(RowOfPBTEDropoutTrainees(r));
            }
            return PBTETraineesL;
        }
        private List<PBTETradeModel> LoopinTradeData(DataTable dt)
        {
            List<PBTETradeModel> PBTETradesL = new List<PBTETradeModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTETradesL.Add(RowOfPBTETrades(r));
            }
            return PBTETradesL;
        }


        //public List<PBTEModel> FetchPBTE(PBTEModel mod)
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTE", Common.GetParams(mod)).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        public List<SchemeModel> FetchPBTESchemes()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeForFilter").Tables[0];
                return LoopinSchemeData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PBTEClassModel> FetchPBTEClasses(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_PBTE_Classes", param.ToArray()).Tables[0];
                return LoopinClassData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PBTETradeModel> FetchTradePBTE(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade_PBTE", param.ToArray()).Tables[0];
                return LoopinTradeData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PBTEClassModel> FetchNAVTTCClasses(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_NAVTTC_Classes", param.ToArray()).Tables[0];
                return LoopinClassData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PBTETspModel> FetchPBTETSPs(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_PBTE_TSPs", param.ToArray()).Tables[0];
                return LoopinTSPData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTETspModel> FetchNAVTTCTSPs(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_NAVTTC_TSPs", param.ToArray()).Tables[0];
                return LoopinTSPData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PBTETraineeModel> FetchPBTETrainees(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_PBTE_Trainees", param.ToArray()).Tables[0];
                return LoopinTraineeData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTETraineeExamScriptModel> FetchPBTETraineesExamScriptData(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_PBTE_Trainees_Exam_Script_Data", param.ToArray()).Tables[0];
                return LoopinTraineeExamScriptData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTETraineeModel> FetchPBTETraineesExamResult(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_PBTE_Trainees_ExamResult", param.ToArray()).Tables[0];
                return LoopinTraineeData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTETraineeModel> FetchNAVTTCTraineesExamResult(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_NAVTTC_Trainees_ExamResult", param.ToArray()).Tables[0];
                return LoopinTraineeData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTETraineeModel> FetchNAVTTCTrainees(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_NAVTTC_Trainees", param.ToArray()).Tables[0];
                return LoopinTraineeData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTETraineeModel> FetchNAVTTCTraineesSqlScript(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_NAVTTC_Trainees_Sql_Script", param.ToArray()).Tables[0];
                return LoopinNAVTTCTraineeSqlScriptData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTETraineeExamScriptModel> FetchNAVTTCTraineesRegisterSqlScript(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_NAVTTC_Trainees_Examination_Sql_Script", param.ToArray()).Tables[0];
                return LoopinNAVTTCTraineeRegisterSqlScriptData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PBTETraineeModel> FetchPBTEDropoutTrainees(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_PBTE_Dropped_Trainees_CurrentQuarter", param.ToArray()).Tables[0];
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_PBTE_Dropped_Trainees_Lock]");
                return LoopinTraineeDropoutData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTEModel> FetchPBTEStatsData(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_PBTE_Stats", param.ToArray()).Tables[0];
                return LoopinPBTEStatsData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTEModel> FetchNAVTTCStatsData(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_NAVTTC_Stats", param.ToArray()).Tables[0];
                return LoopinPBTEStatsData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        //public void UpdatePBTEDropoutTraineesLock()

        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}
        public List<PBTETraineeModel> FetchNAVTTCDropoutTrainees(PBTEQueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@DistrictID", filters.DistrictID));
                param.Add(new SqlParameter("@TradeID", filters.TradeID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_NAVTTC_DropOut_Trainees", param.ToArray()).Tables[0];
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_NAVTTC_Dropped_Trainees_Lock]");
                return LoopinTraineeDropoutData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }




        //public List<PBTEModel> FetchPBTE(bool InActive)
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTE", new SqlParameter("@InActive", InActive)).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        //public int BatchInsert(List<PBTEModel> ls, int @BatchFkey, int CurUserID)
        //{
        //    SqlParameter[] param = new SqlParameter[3];

        //    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
        //    param[1] = new SqlParameter("@", @BatchFkey);
        //    param[2] = new SqlParameter("@CurUserID", CurUserID);
        //    return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_PBTE]", param);
        //}

        //public List<PBTEModel> GetByClusterID(int ClusterID)
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTE", new SqlParameter("@ClusterID", ClusterID)).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        //public List<PBTEModel> GetByRegionID(int RegionID)
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTE", new SqlParameter("@RegionID", RegionID)).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        //public void ActiveInActive(int PBTEID, bool? InActive, int CurUserID)
        //{
        //    SqlParameter[] PLead = new SqlParameter[3];
        //    PLead[0] = new SqlParameter("@PBTEID", PBTEID);
        //    PLead[1] = new SqlParameter("@InActive", InActive);
        //    PLead[2] = new SqlParameter("@CurUserID", CurUserID);
        //    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_PBTE]", PLead);
        //}

        private PBTEClassModel RowOfPBTEClasses(DataRow r)
        {
            PBTEClassModel PBTE = new PBTEClassModel();
            PBTE.SchemeName = r["SchemeName"].ToString();
            PBTE.PBTESchemeName = r["PBTESchemeName"].ToString();
            PBTE.Batch = Convert.ToInt32(r["Batch"]);
            PBTE.TSPID = Convert.ToInt32(r["TSPID"]);
            PBTE.SchemeID = Convert.ToInt32(r["SchemeID"]);
            PBTE.TSPName = r["TSPName"].ToString();
            PBTE.ClassID = Convert.ToInt32(r["ClassID"]);
            PBTE.TradeID = Convert.ToInt32(r["TradeID"]);
            PBTE.DistrictID = Convert.ToInt32(r["DistrictID"]);
            PBTE.ClassCode = r["ClassCode"].ToString();
            PBTE.TradeName = r["TradeName"].ToString();
            PBTE.TrainingAddressLocation = r["TrainingAddressLocation"].ToString();
            PBTE.PBTEAddress = r["PBTEAddress"].ToString();
            PBTE.TehsilName = r["TehsilName"].ToString();
            PBTE.DistrictName = r["DistrictName"].ToString();
            PBTE.CertAuthName = r["CertAuthName"].ToString();
            PBTE.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);
            PBTE.GenderName = r["GenderName"].ToString();
            PBTE.Duration = Convert.ToInt32(r["Duration"]);
            PBTE.StartDate = r["StartDate"].ToString().GetDate();
            PBTE.EndDate = r["EndDate"].ToString().GetDate();
            PBTE.ClassStatusName = r["ClassStatusName"].ToString();


            return PBTE;
        }
        private SchemeModel RowOfPBTESchemes(DataRow r)
        {
            SchemeModel sch = new SchemeModel();
            sch.SchemeName = r["SchemeName"].ToString();
            sch.SchemeID = Convert.ToInt32(r["SchemeID"]);
            return sch;
        }
        private PBTETradeModel RowOfPBTETrades(DataRow r)
        {
            PBTETradeModel tr = new PBTETradeModel();
            tr.TradeName = r["TradeName"].ToString();
            tr.TradeCode = r["TradeCode"].ToString();
            tr.Duration = Convert.ToInt32(r["Duration"]);
            tr.TradeID = Convert.ToInt32(r["TradeID"]);
            return tr;
        }

        private PBTETspModel RowOfPBTETSPs(DataRow r)
        {
            PBTETspModel PBTE = new PBTETspModel();
            PBTE.TSPID = Convert.ToInt32(r["TSPID"]);
            PBTE.TSPMasterID = Convert.ToInt32(r["TSPMasterID"]);
            PBTE.SchemeID = Convert.ToInt32(r["SchemeID"]);
            PBTE.TSPName = r["TSPName"].ToString();
            PBTE.TSPCode = r["TSPCode"].ToString();
            PBTE.ClassID = Convert.ToInt32(r["ClassID"]);
            PBTE.TradeID = Convert.ToInt32(r["TradeID"]);
            PBTE.DistrictID = Convert.ToInt32(r["DistrictID"]);
            PBTE.Address = r["Address"].ToString();
            PBTE.HeadName = r["HeadName"].ToString();
            PBTE.HeadDesignation = r["HeadDesignation"].ToString();
            PBTE.HeadEmail = r["HeadEmail"].ToString();
            PBTE.HeadLandline = r["HeadLandline"].ToString();
            PBTE.OrgLandline = r["OrgLandline"].ToString();
            PBTE.Website = r["Website"].ToString();
            PBTE.CPName = r["CPName"].ToString();
            PBTE.CPDesignation = r["CPDesignation"].ToString();
            PBTE.CPEmail = r["CPEmail"].ToString();
            PBTE.CPLandline = r["CPLandline"].ToString();


            return PBTE;
        }

        private PBTETraineeModel RowOfPBTETrainees(DataRow r)
        {
            PBTETraineeModel PBTE = new PBTETraineeModel();

            PBTE.TraineeID = Convert.ToInt32(r["TraineeID"]);
            PBTE.PBTEID = Convert.ToInt32(r["PBTEID"]);
            PBTE.SchemeID = Convert.ToInt32(r["SchemeID"]);
            PBTE.CollegeID = Convert.ToInt32(r["CollegeID"]);
            PBTE.ClassID = Convert.ToInt32(r["ClassID"]);
            PBTE.TSPID = Convert.ToInt32(r["TSPID"]);
            PBTE.TraineeName = r["TraineeName"].ToString();
            PBTE.TraineeCode = r["TraineeCode"].ToString();
            PBTE.DistrictID = Convert.ToInt32(r["DistrictID"]);
            PBTE.TradeID = Convert.ToInt32(r["TradeID"]);
            PBTE.TradeName = r["TradeName"].ToString();
            if (r.Table.Columns.Contains("CourseID"))
            {
                PBTE.CourseID = Convert.ToInt32(r["CourseID"]);
            }
            if (r.Table.Columns.Contains("CourseCategoryID"))
            {
                PBTE.CourseCategoryID = Convert.ToInt32(r["CourseCategoryID"]);
            }

            PBTE.ClassCode = r["ClassCode"].ToString();
            PBTE.Batch = Convert.ToInt32(r["Batch"]);
            PBTE.ExamID = Convert.ToInt32(r["ExamID"]);

            PBTE.FatherName = r["FatherName"].ToString();
            PBTE.DateOfBirth = r["DateOfBirth"].ToString().GetDate();
            PBTE.TraineeCNIC = r["TraineeCNIC"].ToString();
            PBTE.Education = r["Education"].ToString();
            if (r.Table.Columns.Contains("TraineeImg"))
            {
                PBTE.TraineeImg = string.IsNullOrEmpty(r.Field<string>("TraineeImg")) ? string.Empty : Common.GetFileBase64(r["TraineeImg"].ToString());
            }
            if (r.Table.Columns.Contains("TraineeShift"))
            {
                PBTE.TraineeShift = r["TraineeShift"].ToString();
            }
            if (r.Table.Columns.Contains("ShiftFrom"))
            {
                PBTE.ShiftFrom = r["ShiftFrom"].ToString().GetDate();
            }
            if (r.Table.Columns.Contains("ShiftTo"))
            {
                PBTE.ShiftTo = r["ShiftTo"].ToString().GetDate();
            }
            if (r.Table.Columns.Contains("GenderID"))
            {
                PBTE.GenderID = Convert.ToInt32(r["GenderID"]);
            }
            if (r.Table.Columns.Contains("StatusName"))
            {
                PBTE.StatusName = r["StatusName"].ToString();
                PBTE.ResultStatusName = r["ResultStatusName"].ToString();
            }


            return PBTE;
        }
        private PBTETraineeExamScriptModel RowOfPBTEExamScriptTrainees(DataRow r)
        {
            PBTETraineeExamScriptModel PBTE = new PBTETraineeExamScriptModel();


            PBTE.ExamID = Convert.ToInt32(r["ExamID"]);
            PBTE.ExamSessionUrdu = r["ExamSessionUrdu"].ToString();
            PBTE.ExamSessionenglish = r["ExamSessionenglish"].ToString();
            PBTE.maincategoryid = Convert.ToInt32(r["maincategoryid"]);
            PBTE.ExamYear = Convert.ToInt32(r["ExamYear"]);
            PBTE.StartDate = r["StartDate"].ToString();
            PBTE.EndDate = r["EndDate"].ToString();
            PBTE.TraineeDescription = r["TraineeDescription"].ToString();
            PBTE.Active = Convert.ToInt32(r["Active"]);
            PBTE.ExamSession = r["ExamSession"].ToString();
            PBTE.Batch = Convert.ToInt32(r["Batch"]);

            return PBTE;
        }

        private PBTETraineeModel RowOfNAVTTCScriptTrainees(DataRow r)
        {
            PBTETraineeModel PBTE = new PBTETraineeModel();

            PBTE.TraineeName = r["TraineeName"].ToString();
            PBTE.FatherName = r["FatherName"].ToString();
            //PBTE.TraineeCode = r["TraineeCode"].ToString();
            PBTE.DistrictID = Convert.ToInt32(r["DistrictID"]);
            PBTE.instituteId = Convert.ToInt32(r["instituteId"]);
            PBTE.GenderID = Convert.ToInt32(r["GenderID"]);
            PBTE.disableId = Convert.ToInt32(r["disableId"]);
            PBTE.DistrictID = Convert.ToInt32(r["DistrictID"]);
            PBTE.instituteId = Convert.ToInt32(r["instituteId"]);
            PBTE.pathWayId = Convert.ToInt32(r["pathWayId"]);
            PBTE.address = r["address"].ToString();
            PBTE.email = r["email"].ToString();
            PBTE.mobile = r["mobile"].ToString();
            PBTE.Education = r["Education"].ToString();
            PBTE.TraineeCNIC = r["TraineeCNIC"].ToString();
            PBTE.psdf_traineeId = r["psdf_traineeId"].ToString();
            PBTE.DOB = r["DOB"].ToString().GetDate(); ;
            PBTE.Education = r["Education"].ToString();
            PBTE.psdf_ClassCode = r["psdf_ClassCode"].ToString();

            return PBTE;
        }
        private PBTETraineeExamScriptModel RowOfNAVTTCRegisterScriptTrainees(DataRow r)
        {
            PBTETraineeExamScriptModel PBTE = new PBTETraineeExamScriptModel();


            PBTE.instituteId = Convert.ToInt32(r["instituteId"]);
            PBTE.qabId = Convert.ToInt32(r["qabId"]);
            PBTE.termId = Convert.ToInt32(r["termId"]);
            PBTE.sessionId = Convert.ToInt32(r["sessionId"]);
            PBTE.shiftId = Convert.ToInt32(r["shiftId"]);
            PBTE.Education = r["Education"].ToString();
            PBTE.psdf_ClassCode = r["psdf_ClassCode"].ToString();

            return PBTE;
        }

        private PBTETraineeModel RowOfPBTEDropoutTrainees(DataRow r)
        {
            PBTETraineeModel PBTE = new PBTETraineeModel();

            PBTE.TraineeID = Convert.ToInt32(r["TraineeID"]);
            PBTE.SchemeID = Convert.ToInt32(r["SchemeID"]);
            PBTE.PBTEID = Convert.ToInt32(r["PBTEID"]);
            PBTE.CollegeID = Convert.ToInt32(r["CollegeID"]);
            PBTE.TraineeName = r["TraineeName"].ToString();
            PBTE.TraineeCode = r["TraineeCode"].ToString();
            PBTE.ClassID = Convert.ToInt32(r["ClassID"]);
            PBTE.TSPID = Convert.ToInt32(r["TSPID"]);
            PBTE.TradeID = Convert.ToInt32(r["TradeID"]);
            PBTE.TradeName = r["TradeName"].ToString();

            PBTE.ClassCode = r["ClassCode"].ToString();
            PBTE.Batch = Convert.ToInt32(r["Batch"]);

            PBTE.FatherName = r["FatherName"].ToString();
            PBTE.DateOfBirth = r["DateOfBirth"].ToString().GetDate();
            PBTE.TraineeCNIC = r["TraineeCNIC"].ToString();
            PBTE.Education = r["Education"].ToString();
            PBTE.StatusName = r["StatusName"].ToString();

            return PBTE;
        }
        private PBTEModel RowOfPBTEStats(DataRow r)
        {
            PBTEModel PBTE = new PBTEModel();

            PBTE.ClassesCount = Convert.ToInt32(r["ClassesCount"]);
            PBTE.TSPsCount = Convert.ToInt32(r["TSPsCount"]);
            PBTE.TraineesCount = Convert.ToInt32(r["TraineesCount"]);
            PBTE.DropOutTraineesCount = Convert.ToInt32(r["DropOutTraineesCount"]);


            return PBTE;
        }

        public DataTable FetchReportBySPName(string spName)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), spName);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable PbteData(string SpName, string paramValue)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@EndMonth", paramValue));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure,SpName, param.ToArray()).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool savePBTESchemeMapping(List<SchemeMappingModel> data,int CurUser)
        {
            foreach (var model in data)
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PBTESchemeName", model.PBTESchemeName));
                param.Add(new SqlParameter("@SchemeName", model.SchemeName));
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@CurUserID", CurUser));
                 SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PBTESchemeMapping", param.ToArray());
            }
            return true;
        }
        public bool savePBTETradeMapping(TradeMappingModel data,int CurUser)
        {
            
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PBTETradeID", data.PBTETradeID));
                param.Add(new SqlParameter("@TradeName", data.TradeName));
                param.Add(new SqlParameter("@TradeID", data.TradeID));
                param.Add(new SqlParameter("@CurUserID", CurUser));
                param.Add(new SqlParameter("@Duration", data.Duration));
                 SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PBTETradeMapping", param.ToArray());
            
            return true;
        }

        public bool savePBTEDBFile(string attachment, int CurUser)
        {

            string _dbFile = SaveAttachment("PBTE-DB-BAK-File", attachment);
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@DBFile", _dbFile));
            param.Add(new SqlParameter("@CreatedUserID", CurUser));
            SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PBTEDBRestorationLog", param.ToArray());
            return true;
        }


        private static string SaveAttachment(string FolderName, string attachment)
        {
            if (!string.IsNullOrEmpty(attachment))
            {
                string path = FilePaths.DOCUMENTS_FILE_DIR + FolderName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string paths = path + "\\";
                return Common.AddFile(attachment, paths,"bak");
            }
            return "";
        }

        public DataTable SavePBTECenterMapping(PBTECenterLocationMappingModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TSPName", data.TSPName));
            param.Add(new SqlParameter("@TSPCenterLocation", data.TSPCenterLocation));
            param.Add(new SqlParameter("@TSPCenterDistrict", data.TSPCenterDistrict));
            param.Add(new SqlParameter("@PBTECollegeID", data.PBTECollegeID));
            param.Add(new SqlParameter("@CreatedUserID", data.CurUserID));
            
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PBTECenterLocationMapping", param.ToArray()).Tables[0];
            return dt;
        } 
        
        public bool SavePBTEExam(List<PbteExamDataModel> data)
        {
            if (data == null || data.Count == 0)
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(data));
            }

            foreach (var item in data)
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@BatchNo", item.Batch));
                param.Add(new SqlParameter("@ClassStartDate", item.ClassStartDate));
                param.Add(new SqlParameter("@ClassEndDate", item.ClassEndDate));
                param.Add(new SqlParameter("@SchemeForPBTE", item.SchemeForPBTE));
                param.Add(new SqlParameter("@Duration", item.Duration));
                param.Add(new SqlParameter("@ExamYear", item.ExamYear));
                //param.Add(new SqlParameter("@CreatedUserID", item.CurUserID));
           
            
              SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PBTEExamination", param.ToArray());
            }

            return true;
        } 
        
        public bool SavePBTETrainee(List<PbteTraineeDataModel> data)
        {
            if (data == null || data.Count == 0)
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(data));
            }

            //pbte student data truncate 
              SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "PBTETruncate_StudentData");


            foreach (var item in data)
            {
                string jsonString = JsonConvert.SerializeObject(item,Formatting.Indented);
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Batch", item.Batch));
                param.Add(new SqlParameter("@CNIC", item.CNIC));
                param.Add(new SqlParameter("@CNICVerified", item.CNICVerified));
                param.Add(new SqlParameter("@CertificationAuthority", item.CertificationAuthority));
                param.Add(new SqlParameter("@ClassCode", item.ClassCode));
                param.Add(new SqlParameter("@ClassEndDate", item.ClassEndDate));
                param.Add(new SqlParameter("@ClassStartDate", item.ClassStartDate));
                param.Add(new SqlParameter("@ClassStatus", item.ClassStatus));
                param.Add(new SqlParameter("@ContactNumber", item.ContactNumber));
                param.Add(new SqlParameter("@Duration", item.Duration));
                param.Add(new SqlParameter("@Education", item.Education));
                param.Add(new SqlParameter("@FatherName", item.FatherName));
                param.Add(new SqlParameter("@Gender", item.Gender));
                param.Add(new SqlParameter("@ResidenceDistrict", item.ResidenceDistrict));
                param.Add(new SqlParameter("@ResidenceTehsil", item.ResidenceTehsil));
                param.Add(new SqlParameter("@Scheme", item.Scheme));
                param.Add(new SqlParameter("@SchemeForPBTE", item.SchemeForPBTE));
                param.Add(new SqlParameter("@TSP", item.TSP));
                param.Add(new SqlParameter("@Trade", item.Trade));
                param.Add(new SqlParameter("@TraineeID", item.TraineeID));
                param.Add(new SqlParameter("@TraineeAddress", item.TraineeAddress));
                param.Add(new SqlParameter("@TraineeName", item.TraineeName));
                param.Add(new SqlParameter("@TraineeStatusName", item.TraineeStatusName));
                param.Add(new SqlParameter("@TrainingLocation", item.TrainingLocation));
                param.Add(new SqlParameter("@TrainingDistrict", item.TrainingDistrict));
                //param.Add(new SqlParameter("@TraineeYear", item.TraineeYear));
                //param.Add(new SqlParameter("@CreatedUserID", item.CurUserID));


                SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PBTEStudent", param.ToArray());
            }

            return true;
        }
    }
}