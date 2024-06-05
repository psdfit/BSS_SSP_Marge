/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace DataLayer.Services
{
    public class SRVTradeDetailMap : SRVBase, ISRVTradeDetailMap
    {
        public SRVTradeDetailMap()
        {
        }

        public TradeDetailMapModel GetByTradeDetailMapID(int TradeDetailMapID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TradeDurationMapID", TradeDetailMapID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeDetailMap", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTradeDetailMap(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeDetailMapModel> SaveTradeDetailMap(TradeDetailMapModel TradeDetailMap)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[17];
                param[0] = new SqlParameter("@TradeDetailMapID", TradeDetailMap.TradeDetailMapID);
                param[1] = new SqlParameter("@TradeID", TradeDetailMap.TradeID);
                param[2] = new SqlParameter("@DurationID", TradeDetailMap.DurationID);
                param[3] = new SqlParameter("@TotalTrainingHours", TradeDetailMap.TotalTrainingHours);
                param[4] = new SqlParameter("@DailyTrainingHours", TradeDetailMap.DailyTrainingHours);
                param[5] = new SqlParameter("@WeeklyTrainingHours", TradeDetailMap.WeeklyTrainingHours);
                param[6] = new SqlParameter("@CertificationCategoryID", TradeDetailMap.CertificationCategoryID);
                param[7] = new SqlParameter("@CertAuthID", TradeDetailMap.CertAuthID);
                param[8] = new SqlParameter("@SourceOfCurriculumID", TradeDetailMap.SourceOfCurriculumID);
                param[9] = new SqlParameter("@TraineeEducationTypeID", TradeDetailMap.TraineeEducationTypeID);
                param[10] = new SqlParameter("@TraineeAcademicDisciplineID", TradeDetailMap.TraineeAcademicDisciplineID);
                //param[6] = new SqlParameter("@Duration", Trade.Duration);

                param[11] = new SqlParameter("@PracticalPercentage", TradeDetailMap.PracticalPercentage);
                param[12] = new SqlParameter("@TheoryPercentage", TradeDetailMap.TheoryPercentage);
                param[13] = new SqlParameter("@TrainerEducationTypeID", TradeDetailMap.TrainerEducationTypeID);
                param[14] = new SqlParameter("@TrainerAcademicDisciplineID", TradeDetailMap.TrainerAcademicDisciplineID);
                //param[6] = new SqlParameter("@EquipmentToolID", TradeDetailMap.EquipmentToolID);
                //param[7] = new SqlParameter("@ConsumableMaterialID", TradeDetailMap.ConsumableMaterialID);

                param[15] = new SqlParameter("@CurUserID", TradeDetailMap.CurUserID);
                param[16] = new SqlParameter("@Ident", SqlDbType.Int);
                param[16].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TradeDetailMap]", param);
                //new SRVTradeEquipmentToolsMap().BatchInsert(TradeDetailMap.EquipmentToolID, Convert.ToInt32(param[7].Value), TradeDetailMap.CurUserID);
                //new SRVTradeConsumableMaterialMap().BatchInsert(TradeDetailMap.ConsumableMaterialID, Convert.ToInt32(param[7].Value), TradeDetailMap.CurUserID);
                return FetchTradeDetailMap();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TradeDetailMapModel> LoopinData(DataTable dt)
        {
            List<TradeDetailMapModel> TradeDetailMapL = new List<TradeDetailMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                TradeDetailMapL.Add(RowOfTradeDetailMap(r));
            }
            return TradeDetailMapL;
        }

        public List<TradeDetailMapModel> FetchTradeDetailMap(TradeDetailMapModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeDetailMap", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public FileResult GetDocument(string filePath)
        {
            string PdfFilePath = filePath;

            return new PhysicalFileResult(PdfFilePath, "application/pdf");

        }
            public List<TradeDetailMapModel> FetchTradeDetailMap()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeDetailMap").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeDetailMapModel> FetchTradeDetailMap(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeDetailMap", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeDetailMapModel> FetchTradeDetailMapAll(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeDetailMap", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeDetailMapModel> FetchTradeDetailMap(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeDetailMap", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeDetailMapModel> GetByTradeID(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeDetailMap", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TradeDetailMapModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@TradeID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TradeDetailMap]", param);
        }

        public int BatchInsertTradeDetail(List<TradeDetailMapModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@TradeID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TradeDetailMapAfterSubmit]", param);
        }

        public void ActiveInActive(int TradeDetailMapID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TradeDetailMapID", TradeDetailMapID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TradeDurationMap]", PLead);
        }

        private TradeDetailMapModel RowOfTradeDetailMap(DataRow r)
        {
            TradeDetailMapModel TradeDetailMap = new TradeDetailMapModel();
            TradeDetailMap.TradeDetailMapID = Convert.ToInt32(r["TradeDetailMapID"]);
            TradeDetailMap.TradeID = Convert.ToInt32(r["TradeID"]);
            //TradeDetailMap.TradeName = r["TradeName"].ToString();
            TradeDetailMap.SourceOfCurriculumName = r["Name"].ToString();
            TradeDetailMap.DurationID = Convert.ToInt32(r["DurationID"]);
            TradeDetailMap.TotalTrainingHours = Convert.ToInt32(r["TotalTrainingHours"]);
            TradeDetailMap.DailyTrainingHours = Convert.ToInt32(r["DailyTrainingHours"]);
            TradeDetailMap.WeeklyTrainingHours = Convert.ToInt32(r["WeeklyTrainingHours"]);
            TradeDetailMap.CertificationCategoryID = Convert.ToInt32(r["CertificationCategoryID"] ?? 0);
            TradeDetailMap.CertAuthID = Convert.ToInt32(r["CertAuthID"] ?? 0);
            TradeDetailMap.SourceOfCurriculumID = Convert.ToInt32(r["SourceOfCurriculumID"] ?? 0);
            TradeDetailMap.TraineeEducationTypeID = Convert.ToInt32(r["TraineeEducationTypeID"] ?? 0);
            TradeDetailMap.TraineeAcademicDisciplineID = Convert.ToInt32(r["TraineeAcademicDisciplineID"] ?? 0);
            //Trade.Duration = Convert.ToInt32(r["Duration"]);
            TradeDetailMap.PracticalPercentage = Convert.ToDouble(r["PracticalPercentage"] ?? 0);
            TradeDetailMap.TheoryPercentage = Convert.ToDouble(r["TheoryPercentage"] ?? 0);
            TradeDetailMap.TrainerEducationTypeID = Convert.ToInt32(r["TrainerEducationTypeID"] ?? 0);
            TradeDetailMap.TrainerAcademicDisciplineID = Convert.ToInt32(r["TrainerAcademicDisciplineID"] ?? 0);
            TradeDetailMap.EquipmentToolID = r["EquipmentToolID"].ToString();
            TradeDetailMap.ConsumableMaterialID = r["ConsumableMaterialID"].ToString();
            TradeDetailMap.InActive = Convert.ToBoolean(r["InActive"]);
            TradeDetailMap.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TradeDetailMap.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TradeDetailMap.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TradeDetailMap.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            TradeDetailMap.MappedWithClass = Convert.ToBoolean(r["MappedWithClass"]);

            if (r.Table.Columns.Contains("CurriculaAttachment"))
            {
                TradeDetailMap.CurriculaAttachment = string.IsNullOrEmpty(r.Field<string>("CurriculaAttachment")) ? string.Empty : Common.GetFileBase64(r["CurriculaAttachment"].ToString());
                TradeDetailMap.CurriculaAttachmentPre = r["CurriculaAttachment"].ToString();
            }
            return TradeDetailMap;
        }
    }
}