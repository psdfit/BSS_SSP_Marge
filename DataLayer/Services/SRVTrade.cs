using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace DataLayer.Services
{
    public class SRVTrade : SRVBase, DataLayer.Interfaces.ISRVTrade
    {
        public SRVTrade()
        {
        }

        public TradeModel GetByTradeID_Notification(int TradeID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TradeID", TradeID);
                DataTable dt = new DataTable();
                List<TradeModel> TradeModel = new List<TradeModel>();
                // dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade_Notification", param).Tables[0];


                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_Trade_Notification", param).Tables[0];

                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade_Notification", param).Tables[0];


                }

                if (dt.Rows.Count > 0)
                {
                    TradeModel = Helper.ConvertDataTableToModel<TradeModel>(dt);
                    return TradeModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public TradeModel GetByTradeID(int TradeID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TradeID", TradeID);
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_Trade", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade", param).Tables[0];
                }

                if (dt.Rows.Count > 0)
                {
                    return RowOfTrade(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public TradeCategoryModel GetTradeCertificationType(int TradeID, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter param = new SqlParameter("@TradeID", TradeID);
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_TradeCertificationCategory", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeCertificationCategory", param).Tables[0];
                }
                if (dt.Rows.Count > 0)
                {
                    return RowOfTradeCategory(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public List<TradeModel> SaveTrade(TradeModel Trade)
        {
            try
            {

                for (global::System.Int32 i = 0; i < Trade.TradeDetails.Count; i++)
                {
                    string CurriculaAttachment = null;
                    if (Trade.TradeDetails[i].CurriculaAttachment != null || Trade.TradeDetails[i].CurriculaAttachment != string.Empty)
                    {
                        string path = FilePaths.DOCUMENTS_FILE_DIR + "Trade\\CurriculumAttachments\\" + Trade.TradeCode + "\\";
                        //string path = FilePaths.DOCUMENTS_FILE_DIR + "Trade\\CurriculumAttachmnets\\" + Trade.TradeDetails[i].TradeID + "\\" + Guid.NewGuid();
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        CurriculaAttachment = Common.AddFile(Trade.TradeDetails[i].CurriculaAttachment, path);
                    }
                    Trade.TradeDetails[i].CurriculaAttachment = CurriculaAttachment;
                }



                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@TradeID", Trade.TradeID);
                param[1] = new SqlParameter("@TradeName", Trade.TradeName);
                param[2] = new SqlParameter("@TradeCode", Trade.TradeCode);
                param[3] = new SqlParameter("@SectorID", Trade.SectorID);
                param[4] = new SqlParameter("@SubSectorID", Trade.SubSectorID);
                param[5] = new SqlParameter("@FinalSubmitted", Trade.FinalSubmitted);
                param[6] = new SqlParameter("@IsApproved", Trade.IsApproved);
                param[7] = new SqlParameter("@IsRejected", Trade.IsRejected);
                //param[5] = new SqlParameter("@TraineeEducationTypeID", Trade.TraineeEducationTypeID);
                ////param[6] = new SqlParameter("@Duration", Trade.Duration);
                //param[6] = new SqlParameter("@TotalTrainingHours", Trade.TotalTrainingHours);
                //param[7] = new SqlParameter("@DailyTrainingHours", Trade.DailyTrainingHours);
                //param[8] = new SqlParameter("@WeeklyTrainingHours", Trade.WeeklyTrainingHours);
                //param[9] = new SqlParameter("@PracticalPercentage", Trade.PracticalPercentage);
                //param[10] = new SqlParameter("@TheoryPercentage", Trade.TheoryPercentage);
                //param[11] = new SqlParameter("@CertificationCategoryID", Trade.CertificationCategoryID);
                //param[12] = new SqlParameter("@CertAuthID", Trade.CertAuthID);
                //param[14] = new SqlParameter("@EquipmentTools", Trade.EquipmentTools);
                //param[15] = new SqlParameter("@ConsumableMaterial", Trade.ConsumableMaterial);
                //param[11] = new SqlParameter("@TrainerEducationTypeID", Trade.TrainerEducationTypeID);
                //param[17] = new SqlParameter("@SourceOfCurriculum", Trade.SourceOfCurriculum);
                //param[18] = new SqlParameter("@InActive", Trade.InActive);

                param[8] = new SqlParameter("@CurUserID", Trade.CurUserID);
                param[9] = new SqlParameter("@Ident", SqlDbType.Int);
                param[9].Direction = ParameterDirection.Output;
                if (Trade.FinalSubmitted)
                {
                    new SRVTradeDetailMap().BatchInsert(Trade.TradeDetails, Convert.ToInt32(param[9].Value), Trade.CurUserID);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Trade]", param);
                    new SRVTradeDetailMap().BatchInsert(Trade.TradeDetails, Convert.ToInt32(param[9].Value), Trade.CurUserID);

                }

                //TradeDurationMapModel tm = new TradeDurationMapModel();
                //new SRVTradeEquipmentToolsMap().BatchInsert(tm.EquipmentToolID, Convert.ToInt32(param[13].Value), tm.CurUserID);
                //new SRVTradeConsumableMaterialMap().BatchInsert(tm.ConsumableMaterialID, Convert.ToInt32(param[13].Value), tm.CurUserID);
                //new SRVTradeEquipmentToolsMap().BatchInsert(Trade.TradeEquipmentTools, Convert.ToInt32(param[15].Value), Trade.CurUserID);
                //new SRVTradeConsumableMaterialMap().BatchInsert(Trade.TradeConsumableMaterials, Convert.ToInt32(param[15].Value), Trade.CurUserID);
                //new SRVTradeSourceOfCurriculumMap().BatchInsert(Trade.TradeSourceOfCurriculums, Convert.ToInt32(param[15].Value), Trade.CurUserID);

                return FetchTradeForCRUD();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<TradeModel> SaveTradeDetail(TradeModel Trade)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@CurUserID", Trade.CurUserID);
                //param[1] = new SqlParameter("@Ident", SqlDbType.Int);
                //param[1].Direction = ParameterDirection.Output;


                for (global::System.Int32 i = 0; i < Trade.TradeDetails.Count; i++)
                {
                    string CurriculaAttachment = null;
                    if (Trade.TradeDetails[i].CurriculaAttachment != null || Trade.TradeDetails[i].CurriculaAttachment != string.Empty)
                    {
                        string path = FilePaths.DOCUMENTS_FILE_DIR + "Trade\\CurriculumAttachments\\" + Trade.TradeCode + "\\";
                        //string path = FilePaths.DOCUMENTS_FILE_DIR + "Trade\\CurriculumAttachmnets\\" + Trade.TradeDetails[i].TradeID + "\\" + Guid.NewGuid();
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        CurriculaAttachment = Common.AddFile(Trade.TradeDetails[i].CurriculaAttachment, path);
                    }
                    Trade.TradeDetails[i].CurriculaAttachment = CurriculaAttachment;
                }

                new SRVTradeDetailMap().BatchInsertTradeDetail(Trade.TradeDetails, Trade.TradeID, Trade.CurUserID);


                return FetchTradeForCRUD();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TradeModel> LoopinData(DataTable dt)
        {
            List<TradeModel> TradeL = new List<TradeModel>();

            foreach (DataRow r in dt.Rows)
            {
                TradeL.Add(RowOfTrade(r));
            }
            return TradeL;
        }

        public List<TradeModel> FetchTrade(TradeModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeModel> FetchTrade()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchTradeLayer()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Rd_TradeLayer").Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeModel> FetchTradeForCRUD()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeForCRUD").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }



        public List<TradeModel> FetchTrade(bool InActive)
        {

            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeModel> FetchTradeForROSIFilter(ROSIFiltersModel rosiFilters)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@SchemeIDs", rosiFilters.SchemeIDs);
                param[1] = new SqlParameter("@TSPIDs", rosiFilters.TSPIDs);
                param[2] = new SqlParameter("@PTypeIDs", rosiFilters.PTypeIDs);
                param[3] = new SqlParameter("@SectorIDs", rosiFilters.SectorIDs);
                param[4] = new SqlParameter("@ClusterIDs", rosiFilters.ClusterIDs);
                param[5] = new SqlParameter("@DistrictIDs", rosiFilters.DistrictIDs);
                param[6] = new SqlParameter("@OIDs", rosiFilters.OrganizationIDs);
                param[7] = new SqlParameter("@FundingSourceIDs", rosiFilters.FundingSourceIDs);
                param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
                param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
                //param[1] = new SqlParameter("@CurUserID", CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeForROSIFilter", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SubmittedTradesModel> GetSubmittedTrades(int OID = 0)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SubmittedTrades", new SqlParameter("@OID", OID)).Tables[0];
                return LoopinDataSubmitted(dt, true);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public bool TradeApproveReject(TradeModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TradeID", model.TradeID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_TradeApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_TradeApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        private List<SubmittedTradesModel> LoopinDataSubmitted(DataTable dt, bool user = false)
        {
            List<SubmittedTradesModel> TradeL = new List<SubmittedTradesModel>();

            foreach (DataRow r in dt.Rows)
            {
                TradeL.Add(RowOfSubmittedTrades(r, user));
            }
            return TradeL;
        }

        public List<TradeModel> FinalSubmitTrade(TradeModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TradeID", model.TradeID));
                param.Add(new SqlParameter("@FinalSubmitted", model.FinalSubmitted));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[FinalSubmitTrade]", param.ToArray());
                return FetchTradeForCRUD();

            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }



        public int BatchInsert(List<TradeModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Trade]", param);
        }

        public List<TradeModel> GetBySectorID(int SectorID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade", new SqlParameter("@SectorID", SectorID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeModel> GetBySubSectorID(int SubSectorID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade", new SqlParameter("@SubSectorID", SubSectorID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeModel> GetByCertificationCategoryID(int CertificationCategoryID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade", new SqlParameter("@CertificationCategoryID", CertificationCategoryID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeModel> GetByTestingAgencyID(int TestingAgencyID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trade", new SqlParameter("@TestingAgencyID", TestingAgencyID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeModel> GetByTradeName(string TradeName)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeForCRUD", new SqlParameter("@TradeName", TradeName)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<TradeModel> trade = LoopinData(dt);

                    return trade;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeModel> GetByTradeCode(string TradeCode)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeForCRUD", new SqlParameter("@TradeCode", TradeCode)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<TradeModel> trade = LoopinData(dt);

                    return trade;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public int GetTradeCodeSequence()
        {
            try
            {
                int seq = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetTradeCodeSequence"));
                return seq;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int TradeID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TradeID", TradeID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Trade]", PLead);
        }

        private SubmittedTradesModel RowOfSubmittedTrades(DataRow r, bool user = false)
        {
            SubmittedTradesModel Trade = new SubmittedTradesModel();
            Trade.TradeID = Convert.ToInt32(r["TradeID"]);
            Trade.TradeName = r["TradeName"].ToString();
            Trade.TradeCode = r["TradeCode"].ToString();
            //Trade.SectorID = Convert.ToInt32(r["SectorID"]);
            Trade.SectorName = r["SectorName"].ToString();
            //Trade.SubSectorID = Convert.ToInt32(r["SubSectorID"]);
            Trade.SubSectorName = r["SubSectorName"].ToString();
            Trade.Duration = Convert.ToInt32(r["Duration"]);
            Trade.CertificationCategoryName = r["CertificationCategoryName"].ToString();
            Trade.CertAuthName = r["CertAuthName"].ToString();
            Trade.Name = r["Name"].ToString();
            //Trade.CerificationCa = r["DurationName"].ToString();
            Trade.FinalSubmitted = Convert.ToBoolean(r["FinalSubmitted"]);
            Trade.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            Trade.IsRejected = Convert.ToBoolean(r["IsRejected"]);
            Trade.InActive = Convert.ToBoolean(r["InActive"]);
            Trade.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Trade.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Trade.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Trade.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Trade;
        }
        private TradeCategoryModel RowOfTradeCategory(DataRow r)
        {
            TradeCategoryModel Trade = new TradeCategoryModel();
            Trade.TradeID = r.Field<int>("TradeID");
            Trade.TradeName = r["TradeName"].ToString();
            Trade.CertificationCategoryID = r.Field<int>("CertificationCategoryID");
            Trade.CertificationCategoryName = r["CertificationCategoryName"].ToString();
            if (r.Table.Columns.Contains("SAPID"))
                Trade.SAPID = r.Field<string>("SAPID");
            return Trade;
        }



        private TradeModel RowOfTrade(DataRow r)
        {
            TradeModel Trade = new TradeModel();
            Trade.TradeID = Convert.ToInt32(r["TradeID"]);
            Trade.TradeName = r["TradeName"].ToString();
            Trade.TradeCode = r["TradeCode"].ToString();
            Trade.SectorID = Convert.ToInt32(r["SectorID"]);
            Trade.SectorName = r["SectorName"].ToString();
            Trade.SubSectorID = Convert.ToInt32(r["SubSectorID"]);
            Trade.SubSectorName = r["SubSectorName"].ToString();
            Trade.FinalSubmitted = Convert.ToBoolean(r["FinalSubmitted"]);
            Trade.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            Trade.IsRejected = Convert.ToBoolean(r["IsRejected"]);
            Trade.SAPID = r["SAPID"].ToString();
            Trade.InActive = Convert.ToBoolean(r["InActive"]);
            Trade.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Trade.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Trade.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Trade.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Trade;
        }

        public bool UpdateTradeSAPID(int TradeID, string sapObjId, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@TradeID", TradeID);
                param[1] = new SqlParameter("@SAPID", sapObjId);
                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[AU_TradeSAPID]", param);

                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TradeSAPID]", param);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public TradeCategoryModel GetTradeCertificationType(int TradeID, SqlTransaction transaction = null)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] param = new SqlParameter[1];
        //        param[0] = new SqlParameter("@TradeID", TradeID);
        //        if (transaction != null)
        //        {
        //            dt = SqlHelper.ExecuteDataset(transaction,SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeCertificationCategory", param).Tables[0];
        //        }
        //        else
        //        {
        //            dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeCertificationCategory", param).Tables[0];
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            return RowOfTradeCategory(dt.Rows[0]);
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        public List<TradeModel> FetchTradeTSP(int programid, int districtid, int UserID)
        {

            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@ProgramID", programid);
                param[1] = new SqlParameter("@DistrictID", districtid);
                param[2] = new SqlParameter("@UserID", UserID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPProgramTrade", param).Tables[0];
                return SSPLoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<TradeModel> SSPLoopinData(DataTable dt)
        {
            List<TradeModel> TradeL = new List<TradeModel>();

            foreach (DataRow r in dt.Rows)
            {
                TradeL.Add(SSPRowOfTrade(r));
            }
            return TradeL;
        }
        private TradeModel SSPRowOfTrade(DataRow r)
        {
            TradeModel Trade = new TradeModel();
            Trade.TradeID = Convert.ToInt32(r["TradeID"]);
            Trade.TradeName = r["TradeName"].ToString();
            Trade.TrainingLocationID = Convert.ToInt32(r["TrainingLocationID"]);

            return Trade;
        }
    }
}