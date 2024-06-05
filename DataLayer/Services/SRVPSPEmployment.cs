/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Dapper;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace DataLayer.Services
{
    public class SRVPSPEmployment : ISRVPSPEmployment
    {
        private readonly IDapperConfig _dapper;

        public SRVPSPEmployment(IDapperConfig dapper)
        {
            _dapper = dapper;
        }

        public PSPEmploymentModel GetTraineeData(int classID, int traineeID)
        {
            var peQuery = $"Select PE.*,C.ClassCode From PlacementFormE pe INNER JOIN Class C ON pe.ClassID=C.ClassID Where pe.ClassID = {classID} AND pe.TraineeID = {traineeID}";
            var trainee = _dapper.QueryFirstOrDefault<PSPEmploymentModel>(peQuery);
            if (trainee == null)
            {
                var tQuery = $"Select p.TraineeName, p.ClassID, p.TraineeID,C.ClassCode From TraineeProfile p INNER JOIN Class C ON p.ClassID=C.ClassID Where p.TraineeID = {traineeID}";
                trainee = _dapper.QueryFirstOrDefault<PSPEmploymentModel>(tQuery);
            }
            return trainee;
        }

        public List<RD_CompletedTraineeByClassModel> GetCompletedTraineeByClass(QueryFilters filters)
        {
            try
            {
                var list = _dapper.Query<RD_CompletedTraineeByClassModel>("dbo.RD_CompletedTraineeForPSP", new {@TradeID = filters.TradeID, @ClassID = filters.ClassID }, CommandType.StoredProcedure);
                return list;
                //var result = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CompletedTraineeByClass", new SqlParameter("@ClassID", ClassID));
                ////DataTable dt = result.Tables[0];
                ////DataTable dt2 = result.Tables[1];
                //return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public List<RD_CompletedTraineeByClassModel> GetPSPBatchTraineeByFilters(QueryFilters filters)
        {
            try
            {
                var list = _dapper.Query<RD_CompletedTraineeByClassModel>("dbo.RD_PSPBatchTraineeByFilters", new {@TradeID = filters.TradeID, @ClassID = filters.ClassID }, CommandType.StoredProcedure);
                return list;
                //var result = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CompletedTraineeByClass", new SqlParameter("@ClassID", ClassID));
                ////DataTable dt = result.Tables[0];
                ////DataTable dt2 = result.Tables[1];
                //return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public List<PSPBatchModel> GetPSPTraineeForDEOVerification(QueryFilters filters)
        {
            try
            {
                var list = _dapper.Query<PSPBatchModel>("dbo.RD_PSPBatchTraineeByFilters", new {@TradeID = filters.TradeID, @ClassID = filters.ClassID }, CommandType.StoredProcedure);
                return list;
                //var result = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CompletedTraineeByClass", new SqlParameter("@ClassID", ClassID));
                ////DataTable dt = result.Tables[0];
                ////DataTable dt2 = result.Tables[1];
                //return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool SavePlacementFormE(PSPEmploymentModel PlacementFormE)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[31];
                param[0] = new SqlParameter("@PlacementID", PlacementFormE.PlacementID);
                param[1] = new SqlParameter("@TraineeID", PlacementFormE.TraineeID);
                param[2] = new SqlParameter("@ClassID", PlacementFormE.ClassID);
                param[3] = new SqlParameter("@Designation", PlacementFormE.Designation);
                param[4] = new SqlParameter("@Department", PlacementFormE.Department);
                param[5] = new SqlParameter("@EmploymentDuration", PlacementFormE.EmploymentDuration);
                param[6] = new SqlParameter("@Salary", PlacementFormE.Salary);
                param[7] = new SqlParameter("@SupervisorName", PlacementFormE.SupervisorName);
                param[8] = new SqlParameter("@SupervisorContact", PlacementFormE.SupervisorContact);
                param[9] = new SqlParameter("@EmploymentStartDate", PlacementFormE.EmploymentStartDate);
                param[10] = new SqlParameter("@EmploymentStatus", PlacementFormE.EmploymentStatus);
                param[11] = new SqlParameter("@EmploymentType", PlacementFormE.EmploymentType);
                param[12] = new SqlParameter("@EmployerName", PlacementFormE.EmployerName);
                param[13] = new SqlParameter("@EmployerBusinessType", PlacementFormE.EmployerBusinessType);
                param[14] = new SqlParameter("@EmploymentAddress", PlacementFormE.EmploymentAddress);
                param[15] = new SqlParameter("@District", PlacementFormE.District);
                param[16] = new SqlParameter("@EmploymentTehsil", PlacementFormE.EmploymentTehsil);
                param[17] = new SqlParameter("@EmploymentTiming", PlacementFormE.EmploymentTiming);
                param[18] = new SqlParameter("@IsTSP", PlacementFormE.IsTSP);
                param[19] = new SqlParameter("@OldID", PlacementFormE.OldID);
                param[20] = new SqlParameter("@EmploymentTehsilOld", PlacementFormE.EmploymentTehsilOld);
                param[21] = new SqlParameter("@OfficeContactNo", PlacementFormE.OfficeContactNo);
                param[22] = new SqlParameter("@IsMigrated", false);
                param[23] = new SqlParameter("@TraineeName", PlacementFormE.TraineeName);

                param[24] = new SqlParameter("@CurUserID", PlacementFormE.CurUserID);

                param[25] = new SqlParameter("@PlacementTypeID", PlacementFormE.PlacementTypeID);
                param[26] = new SqlParameter("@EOBI", PlacementFormE.EOBI);
                param[27] = new SqlParameter("@VerificationMethodId", PlacementFormE.VerificationMethodId);
                param[28] = new SqlParameter("@FileType", PlacementFormE.FileType);
                param[29] = new SqlParameter("@FilePath", PlacementFormE.FilePath);
                param[30] = new SqlParameter("@FileName", PlacementFormE.FileName);

                var saved = SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PSP_PlacementFormE", param);
                //return FetchPlacementFormE();
                return saved > 0;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }



        public void SavePSPBatch(PSPBatchModel mod)
        {
            try
            {
                    SqlParameter[] param = new SqlParameter[6];
                    param[0] = new SqlParameter("@PSPBatchID",mod.PSPBatchID);
                    param[1] = new SqlParameter("@BatchName",mod.BatchName);
                    param[2] = new SqlParameter("@Json", JsonConvert.SerializeObject(mod.CompletedTrainees));
                    param[3] = new SqlParameter("@CurUserID", mod.CurUserID);
                    param[4] = new SqlParameter("@TradeID",mod.TradeID);

                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Save_PSP_Batch]", param);               

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public void SavePSPBatchTrainees(List<PSPBatchModel> ls)
        {
            try
            {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));

                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Save_PSP_Batch_Trainees]", param);               
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<RD_ClassForTSPModel> FetchClassFilters(QueryFilters filters)
        {
            try
            {
              
                var list = _dapper.Query<RD_ClassForTSPModel>("dbo.RD_ClassForPSP", new { @TradeID = filters.TradeID, ClassID = filters.ClassID, UserID = filters.UserID }, CommandType.StoredProcedure);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<PSPBatchModel> FetchPSPBatchTraineeByID(PSPBatchModel mod)
        {
            try
            {
                var list = _dapper.Query<PSPBatchModel>("dbo.RD_PSPBatchTraineeByID", new { @PSPBatchID = mod.PSPBatchID }, CommandType.StoredProcedure);
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE").Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PSPBatchModel> FetchPSPBatches()
        {
            try
            {
              
                var list = _dapper.Query<PSPBatchModel>("dbo.RD_PSPBatches", CommandType.StoredProcedure);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<PSPBatchModel> FetchPSPInterestedTraineesForAssignment(int pspbatchid)
        {
            try
            {
              
                var list = _dapper.Query<PSPBatchModel>("dbo.RD_PSPBatchInterestedTraineeByID", new { @PSPBatchID = pspbatchid }, CommandType.StoredProcedure);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        public List<PSPBatchModel> FetchPSPAssignedTrainees(PSPBatchModel mod)
        {
            try
            {
              
                var list = _dapper.Query<PSPBatchModel>("dbo.RD_PSPAssignedTraineesByID", new { @PSPUserID = mod.PSPUserID, @PSPBatchID = mod.PSPBatchID }, CommandType.StoredProcedure);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public List<PSPEmploymentModel> FetchPSPAssignedTrainees_ForEmployment_ByDEO(PSPEmploymentModel mod)
        //{
        //    try
        //    {

        //        var list = _dapper.Query<PSPEmploymentModel>("dbo.RD_PSPAssignedTraineesByID", CommandType.StoredProcedure);
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public List<PSPEmploymentModel> FetchPlacementFormE_PSP(PSPEmploymentModel mod)
        {
            try
            {
                var list = _dapper.Query<PSPEmploymentModel>("dbo.RD_PlacementFormE_PSP", new
                {
                    @PSPBatchID = mod.PSPBatchID,
                    @TraineeID = mod.TraineeID,
                    @VerificationMethodID = mod.VerificationMethodId ?? 0,
                    @PlacementTypeID = mod.PlacementTypeID ?? 0
                }, CommandType.StoredProcedure);
                list.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE", Common.GetParams(mod)).Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PSPEmploymentModel> FetchPlacementFormE(PSPEmploymentModel mod)
        {
            try
            {
                var list = _dapper.Query<PSPEmploymentModel>("dbo.RD_PlacementFormE_PSP", new
                {
                    @PSPBatchID = mod.PSPBatchID,
                    @TraineeID = mod.TraineeID,
                    @VerificationMethodID = mod.VerificationMethodId ?? 0,
                    @PlacementTypeID = mod.PlacementTypeID ?? 0
                }, CommandType.StoredProcedure);
                list.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE", Common.GetParams(mod)).Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PSPEmploymentModel> FetchPlacementFormE()
        {
            try
            {
                var list = _dapper.Query<PSPEmploymentModel>("dbo.RD_PlacementFormE", new { }, CommandType.StoredProcedure);
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE").Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RD_CompletedTraineeByTraineeIDsModel> GetCompletedTraineeByTraineeIDs(string TraineeIDs)
        {
            try
            {
                var list = _dapper.Query<RD_CompletedTraineeByTraineeIDsModel>("dbo.RD_CompletedTraineeByTraineeIDs", new { @TraineeIDs = TraineeIDs }, CommandType.StoredProcedure);
                return list;
                //var result = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CompletedTraineeByClass", new SqlParameter("@ClassID", ClassID));
                ////DataTable dt = result.Tables[0];
                ////DataTable dt2 = result.Tables[1];
                //return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PSPEmploymentModel> FetchPlacementFormE(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int SubmitClassEmployment(int ClassID)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "SubmitClassEmployment", new SqlParameter("@ClassID", ClassID));
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void UpdateTraineesPSP(PSPBatchModel mod)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@PSPUserIDs", mod.PSPUserIDs);
                param[1] = new SqlParameter("@PSPTraineeIDs", mod.PSPAssignedTraineeIDs);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_Trainees_PSPID]", param);

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public DataSet GetSelfPSPList(int ClassID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Filter", "Self");
                param[1] = new SqlParameter("@ClassID", ClassID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GetFilteredVerificationList", param);
                return ds;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataSet GetFormalPSPList(int ClassID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Filter", "Formal");
                param[1] = new SqlParameter("@ClassID", ClassID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GetFilteredVerificationList", param);
                return ds;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<PSPEmploymentModel> LoopinData(DataTable dt)
        {
            List<PSPEmploymentModel> PlacementFormEL = new List<PSPEmploymentModel>();

            foreach (DataRow r in dt.Rows)
            {
                PlacementFormEL.Add(RowOfPlacementFormE(r));
            }
            return PlacementFormEL;
        }

        private PSPEmploymentModel RowOfPlacementFormE(DataRow r)
        {
            try
            {
                bool IsTradeName = r.Table.Columns.Contains("TradeName");

                PSPEmploymentModel PlacementFormE = new PSPEmploymentModel();
                PlacementFormE.PlacementID = Convert.ToInt32(r["PlacementID"]);
                PlacementFormE.TraineeID = Convert.ToInt32(r["TraineeID"]);
                PlacementFormE.TraineeName = r["TraineeName"].ToString();
                if (!string.IsNullOrEmpty(r["ClassID"].ToString()))
                    PlacementFormE.ClassID = Convert.ToInt32(r["ClassID"]);
                if (r.Table.Columns.Contains("ClassCode"))
                {
                    if (!string.IsNullOrEmpty(r["ClassCode"].ToString()))
                        PlacementFormE.ClassCode = r["ClassCode"].ToString();
                }
                if (IsTradeName)
                    PlacementFormE.TradeName = r["TradeName"].ToString();
                PlacementFormE.Designation = r["Designation"].ToString();
                PlacementFormE.Department = r["Department"].ToString();
                if (!string.IsNullOrEmpty(r["EmploymentDuration"].ToString()))
                    PlacementFormE.EmploymentDuration = Convert.ToInt32(r["EmploymentDuration"]);
                if (!string.IsNullOrEmpty(r["Salary"].ToString()))
                    PlacementFormE.Salary = Convert.ToDouble(r["Salary"]);
                PlacementFormE.SupervisorName = r["SupervisorName"].ToString();
                PlacementFormE.SupervisorContact = r["SupervisorContact"].ToString();
                PlacementFormE.EmploymentStartDate = r["EmploymentStartDate"].ToString().GetDate();
                if (!string.IsNullOrEmpty(r["EmploymentStatus"].ToString()))
                    PlacementFormE.EmploymentStatus = r["EmploymentStatus"].ToString();
                if (!string.IsNullOrEmpty(r["EmploymentType"].ToString()))
                    PlacementFormE.EmploymentType = Convert.ToInt32(r["EmploymentType"]);
                PlacementFormE.EmployerName = r["EmployerName"].ToString();
                PlacementFormE.EmployerBusinessType = r["EmployerBusinessType"].ToString();
                PlacementFormE.EmploymentAddress = r["EmploymentAddress"].ToString();
                if (!string.IsNullOrEmpty(r["District"].ToString()))
                    PlacementFormE.District = Convert.ToInt32(r["District"]);
                if (!string.IsNullOrEmpty(r["EmploymentTehsil"].ToString()))
                    PlacementFormE.EmploymentTehsil = Convert.ToInt32(r["EmploymentTehsil"]);
                PlacementFormE.EmploymentTiming = r["EmploymentTiming"].ToString();
                PlacementFormE.IsTSP = Convert.ToBoolean(r["IsTSP"]);
                PlacementFormE.OldID = r["OldID"].ToString();
                PlacementFormE.EmploymentTehsilOld = r["EmploymentTehsilOld"].ToString();
                PlacementFormE.OfficeContactNo = r["OfficeContactNo"].ToString();
                PlacementFormE.IsMigrated = Convert.ToBoolean(r["IsMigrated"]);
                PlacementFormE.CreatedDate = r["CreatedDate"].ToString().GetDate();
                PlacementFormE.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
                if (!string.IsNullOrEmpty(r["CreatedUserID"].ToString()))
                    PlacementFormE.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
                if (!string.IsNullOrEmpty(r["ModifiedUserID"].ToString()))
                    PlacementFormE.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
                PlacementFormE.TraineeName = r["TraineeName"].ToString();

                return PlacementFormE;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActiveInActive(int? PlacementID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@PlacementID", PlacementID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AI_PlacementFormE", PLead);
        }

        public bool ForwardedToTelephonic(int[] list)
        {
            try
            {
                var ids = string.Join(",", list);
                if (string.IsNullOrEmpty(ids))
                {
                    return false;
                }
                var query = @"Update PlacementFormE
                              SET ForwardedToTelephonic = 1
                              Where PlacementID IN ( " + ids + " ) ";

                var success = _dapper.ExecuteQuery(query);
                return success > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}