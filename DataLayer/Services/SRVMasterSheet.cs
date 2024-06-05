using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using DataLayer.Interfaces;
using Newtonsoft.Json;

namespace DataLayer.Services
{
    public class SRVMasterSheet : SRVBase, ISRVMasterSheet
    {
        public SRVMasterSheet()
        { }

        public MasterSheetModel GetByID(int ID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfMasterSheet(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> SaveMasterSheet(MasterSheetModel MasterSheet)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[33];
                //param[0] = new SqlParameter("@ID", MasterSheet.ID);
                param[1] = new SqlParameter("@DistrictID", MasterSheet.DistrictID);
                param[2] = new SqlParameter("@SchemeID", MasterSheet.SchemeID);
                param[3] = new SqlParameter("@ClassID", MasterSheet.ClassID);
                param[4] = new SqlParameter("@TSPID", MasterSheet.TSPID);
                param[5] = new SqlParameter("@TradeID", MasterSheet.TradeID);
                param[6] = new SqlParameter("@Batch", MasterSheet.Batch);
                param[7] = new SqlParameter("@TrainingAddressLocation", MasterSheet.TrainingAddressLocation);
                param[8] = new SqlParameter("@TehsilID", MasterSheet.TehsilID);
                //param[9] = new SqlParameter("@DeliveringTrainer", MasterSheet.DeliveringTrainer);
                //param[10] = new SqlParameter("@TestingCertifyAuthority", MasterSheet.TestingCertifyAuthority);
                //param[11] = new SqlParameter("@ContractualTrainees", MasterSheet.ContractualTrainees);
                param[12] = new SqlParameter("@GenderID", MasterSheet.GenderID);
                //param[13] = new SqlParameter("@TrainingDuration", MasterSheet.TrainingDuration);
                //param[14] = new SqlParameter("@TotalTrainingHours", MasterSheet.TotalTrainingHours);
                param[15] = new SqlParameter("@StartDate", MasterSheet.StartDate);
                param[16] = new SqlParameter("@EndDate", MasterSheet.EndDate);
                //param[17] = new SqlParameter("@TotalTraineeProfilesReceived", MasterSheet.TotalTraineeProfilesReceived);
                param[18] = new SqlParameter("@RTP", MasterSheet.RTP);
                //param[19] = new SqlParameter("@CompletionReportStatus", MasterSheet.CompletionReportStatus);
                //param[20] = new SqlParameter("@Remarks", MasterSheet.Remarks);
                //param[21] = new SqlParameter("@SchemeType", MasterSheet.SchemeType);
                //param[22] = new SqlParameter("@ContractualClassHours", MasterSheet.ContractualClassHours);
                param[23] = new SqlParameter("@EmploymentInvoiceStatus", MasterSheet.EmploymentInvoiceStatus);
                param[24] = new SqlParameter("@SectorID", MasterSheet.SectorID);
                param[25] = new SqlParameter("@OverallEmploymentCommitment", MasterSheet.OverallEmploymentCommitment);
                param[26] = new SqlParameter("@MinimumEducation", MasterSheet.MinimumEducation);
                //param[27] = new SqlParameter("@Donor", MasterSheet.Donor);
                param[28] = new SqlParameter("@ClusterID", MasterSheet.ClusterID);
                //param[29] = new SqlParameter("@KAMID", MasterSheet.KAMID);
                //param[30] = new SqlParameter("@TrainerName", MasterSheet.TrainerName);
                //param[31] = new SqlParameter("@TrainerCNIC", MasterSheet.TrainerCNIC);

                param[32] = new SqlParameter("@CurUserID", MasterSheet.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_MasterSheet]", param);
                return FetchMasterSheet();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> FetchMasterSheetByPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));
                param.Add(new SqlParameter("@OID", filterModel.OID));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheets_Paged", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<MasterSheetModel> LoopinData(DataTable dt)
        {
            List<MasterSheetModel> MasterSheetL = new List<MasterSheetModel>();

            foreach (DataRow r in dt.Rows)
            {
                MasterSheetL.Add(RowOfMasterSheet(r));
            }
            return MasterSheetL;
        }

        public List<MasterSheetModel> FetchMasterSheet(MasterSheetModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> FetchMasterSheet()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheets").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> FetchMasterSheet(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> GetByDistrictID(int DistrictID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", new SqlParameter("@DistrictID", DistrictID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> GetBySchemeID(int SchemeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> GetByTSPID(int TSPID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", new SqlParameter("@TSPID", TSPID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> GetByTradeID(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> GetByTehsilID(int TehsilID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", new SqlParameter("@TehsilID", TehsilID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> GetByGenderID(int GenderID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", new SqlParameter("@GenderID", GenderID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> GetBySectorID(int SectorID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", new SqlParameter("@SectorID", SectorID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MasterSheetModel> GetByClusterID(int ClusterID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheet", new SqlParameter("@ClusterID", ClusterID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int ID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ID", ID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_MasterSheet]", PLead);
        }

        public List<MasterSheetModel> FetchMasterSheetByFilters(int[] filters)
        {
            List<MasterSheetModel> list = new List<MasterSheetModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                int userId = filters[3];
                int oID = filters[4];
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TSPID", tspId);
                    param[2] = new SqlParameter("@ClassID", classId);
                    param[3] = new SqlParameter("@UserID", userId);
                    param[4] = new SqlParameter("@OID", oID);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheets", param).Tables[0];
                    list = LoopinData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        private MasterSheetModel RowOfMasterSheet(DataRow r)
        {
            MasterSheetModel MasterSheet = new MasterSheetModel();

            MasterSheet.DistrictID = Convert.ToInt32(r["DistrictID"]);
            MasterSheet.District = r["District"].ToString();
            MasterSheet.SchemeID = Convert.ToInt32(r["SchemeID"]);
            MasterSheet.Scheme = r["Scheme"].ToString();
            MasterSheet.SchemeCode = r["SchemeCode"].ToString();
            MasterSheet.InstructorName = r["InstructorName"].ToString();
            MasterSheet.InstructorCNIC = r["InstructorCNIC"].ToString();
            MasterSheet.SchemeCode = r["SchemeCode"].ToString();
            MasterSheet.FundingSourceName = r["FundingSourceName"].ToString();
            MasterSheet.Class = r["Class"].ToString();
            MasterSheet.ClassID = Convert.ToInt32(r["ClassID"]);
            MasterSheet.ClassStatusName = r["ClassStatusName"].ToString();
            MasterSheet.Shift = r["Shift"].ToString();
            MasterSheet.Section = r["Section"].ToString();
            //MasterSheet.ClassStatusID = Convert.ToInt32(r["ClassStatusID"]);
            MasterSheet.Duration = Convert.ToDecimal(r["Duration"]);
            MasterSheet.MinHoursPerMonth = Convert.ToInt32(r["MinHoursPerMonth"]);
            MasterSheet.TSPID = Convert.ToInt32(r["TSPID"]);
            MasterSheet.TSP = r["TSP"].ToString();
            MasterSheet.TSPCode = r["TSPCode"].ToString();
            MasterSheet.TradeID = Convert.ToInt32(r["TradeID"]);
            MasterSheet.Trade = r["Trade"].ToString();
            MasterSheet.TradeCode = r["TradeCode"].ToString();
            MasterSheet.Batch = r["Batch"].ToString();
            MasterSheet.TrainingAddressLocation = r["TrainingAddressLocation"].ToString();
            MasterSheet.TehsilID = Convert.ToInt32(r["TehsilID"]);
            MasterSheet.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);
            MasterSheet.Tehsil = r["Tehsil"].ToString();
            //MasterSheet.DeliveringTrainer = r["DeliveringTrainer"].ToString();
            MasterSheet.SchemeCode = r["SchemeCode"].ToString();
            MasterSheet.CertAuthID = Convert.ToInt32(r["CertAuthID"]);
            MasterSheet.Certification_Authority = r["Certification_Authority"].ToString();
            //MasterSheet.ContractualTrainees = Convert.ToInt32(r["ContractualTrainees"]);
            MasterSheet.GenderID = Convert.ToInt32(r["GenderID"]);
            MasterSheet.Gender = r["Gender"].ToString();
            MasterSheet.WhoIsDeliveringTraining = r["WhoIsDeliveringTraining"].ToString();
            MasterSheet.InceptionReportReceived = r["InceptionReportReceived"].ToString();
            MasterSheet.InceptionReportDeliveredToTPM = r["InceptionReportDeliveredToTPM"].ToString();
            MasterSheet.TradeGroup = r["TradeGroup"].ToString();
            MasterSheet.TraineeProfilesReceived = r["TraineeProfilesReceived"].ToString();
            MasterSheet.ClassID_U = r["ClassID_U"].ToString();
            MasterSheet.SchemeID_U = r["SchemeID_U"].ToString();
            MasterSheet.TSPID_U = r["TSPID_U"].ToString();
            MasterSheet.SchemeType = r["SchemeType"].ToString();
            //MasterSheet.TrainingDuration = Convert.ToInt32(r["TrainingDuration"]);
            //MasterSheet.TotalTrainingHours = Convert.ToInt32(r["TotalTrainingHours"]);
            MasterSheet.StartDate = r["StartDate"].ToString().GetDate();
            MasterSheet.EndDate = r["EndDate"].ToString().GetDate();
            MasterSheet.ClassStartTime = r.Field<DateTime?>("ClassStartTime");
            MasterSheet.ClassEndTime = r.Field<DateTime?>("ClassEndTime");
            //MasterSheet.ClassEndTime = r["ClassEndTime"].ToString().GetDate();
            MasterSheet.InceptionReportDueOn = r.Field<DateTime?>("InceptionReportDueOn");
            //MasterSheet.StudentProfileOverDueOn = r["StudentProfileOverDueOn"].ToString().GetDate();
            MasterSheet.StudentProfileOverDueOn = r.Field<DateTime?>("StudentProfileOverDueOn");
            MasterSheet.CompletionReportDue = r.Field<DateTime?>("CompletionReportDue");
            MasterSheet.TraineeProfileReceivedDate = r["TraineeProfileReceivedDate"].ToString();
            MasterSheet.TotalTrainingHours = Convert.ToInt32(r["TotalTrainingHours"]);
            MasterSheet.TotalTraineeProfilesReceived = Convert.ToInt32(r["TotalTraineeProfilesReceived"]);
            MasterSheet.EnrolledTrainees = Convert.ToInt32(r["EnrolledTrainees"]);
            MasterSheet.GenderID = Convert.ToInt32(r["GenderID"]);
            //MasterSheet.TotalTraineeProfilesReceived = Convert.ToInt32(r["TotalTraineeProfilesReceived"]);
            MasterSheet.RTP = r["RTP"].ToString();
            MasterSheet.Remarks = r["Remarks"].ToString();
            MasterSheet.EmploymentInvoiceStatus = r["EmploymentInvoiceStatus"].ToString();
            //MasterSheet.CompletionReportStatus = Convert.ToBoolean(r["CompletionReportStatus"]);
            //MasterSheet.Remarks = r["Remarks"].ToString();
            //MasterSheet.SchemeType = Convert.ToInt32(r["SchemeType"]);
            //MasterSheet.ContractualClassHours = Convert.ToInt32(r["ContractualClassHours"]);
            //MasterSheet.EmploymentInvoiceStatus = r["EmploymentInvoiceStatus"].ToString();
            MasterSheet.SectorID = Convert.ToInt32(r["SectorID"]);
            MasterSheet.Sector = r["Sector"].ToString();
            MasterSheet.OverallEmploymentCommitment = Convert.ToInt32(r["OverallEmploymentCommitment"]);
            MasterSheet.MinimumEducation = r["MinimumEducation"].ToString();
            MasterSheet.OID = Convert.ToInt32(r["OID"]);
            MasterSheet.Organization = r["Organization"].ToString();
            MasterSheet.ClusterID = Convert.ToInt32(r["ClusterID"]);
            //MasterSheet.KAMID = Convert.ToInt32(r["KAMID"]);
            MasterSheet.Cluster = r["Cluster"].ToString();
            MasterSheet.CompletionReportStatus = r["CompletionReportStatus"].ToString();
            //MasterSheet.KAM = r["KAM"].ToString();
            //MasterSheet.TrainerName = r["TrainerName"].ToString();
            //MasterSheet.TrainerCNIC = r["TrainerCNIC"].ToString();
            //MasterSheet.InActive = Convert.ToBoolean(r["InActive"]);
            MasterSheet.UserID = Convert.ToInt32(r["UserID"]);
            //MasterSheet.Role = Convert.ToInt32(r["Role"]);
            //MasterSheet.UserLevel = Convert.ToInt32(r["UserLevel"]);
            MasterSheet.UserName = r["UserName"].ToString();

            MasterSheet.IsDVV = r["IsDVV"].ToString();
            MasterSheet.DayNames = r["DayNames"].ToString();
            MasterSheet.TotalClassDays = r["TotalClassDays"].ToString();
            MasterSheet.SourceOfCurriculum = r["SourceOfCurriculum"].ToString();
            MasterSheet.PaymentSchedule = r["PaymentSchedule"].ToString();
            MasterSheet.FundingCategoryName = r["FundingCategoryName"].ToString();
            MasterSheet.ProgramFocusName = r["ProgramFocusName"].ToString();
            MasterSheet.TSPNTN = r["TSPNTN"].ToString();
            MasterSheet.PTypeName = r["PTypeName"].ToString();
            //MasterSheet.SAPID = r["SAPID"].ToString();

            if (r.Table.Columns.Contains("ProvinceID"))
            {
                //MasterSheet.ProvinceID = Convert.ToInt32(r.Field<int?>("ProvinceID") ?? 0);
                MasterSheet.ProvinceID = (r["ProvinceID"] == DBNull.Value) ? 0 : (int?)Convert.ToInt32(r["ProvinceID"]);
            }
            if (r.Table.Columns.Contains("ProvinceName"))
            {
                MasterSheet.ProvinceName = r["ProvinceName"].ToString();
            }
            if (r.Table.Columns.Contains("SAPID"))
            {
                MasterSheet.SAPID = r["SAPID"].ToString();
            }
            if (r.Table.Columns.Contains("RegistrationAuthorityName"))
            {
                MasterSheet.RegistrationAuthorityName = r["RegistrationAuthorityName"].ToString();
            }
            //MasterSheet.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            //MasterSheet.CreatedDate = r["CreatedDate"].ToString().GetDate();
            //MasterSheet.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            //if (r["KamID"] != null)
            //{
            //    MasterSheet.KamID = Convert.ToInt32(r["KamID"]);
            //}
            //else
            //{
            //    MasterSheet.KamID = 0;
            //}

            return MasterSheet;
        }
    }
}