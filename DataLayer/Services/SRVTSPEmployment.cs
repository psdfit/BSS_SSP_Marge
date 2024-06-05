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

namespace DataLayer.Services
{
    public class SRVTSPEmployment : ISRVTSPEmployment
    {
        private readonly IDapperConfig _dapper;
        private readonly ISRVSendEmail srvSendEmail;
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVEmploymentVerification srvVEmploymentVerification;
        public SRVTSPEmployment(ISRVEmploymentVerification srvVEmploymentVerification, IDapperConfig dapper, ISRVSendEmail srvSendEmail, ISRVTSPMaster srvTSPMaster)
        {
            this.srvSendEmail = srvSendEmail;
            _dapper = dapper;
            this.srvTSPMaster = srvTSPMaster;
            this.srvVEmploymentVerification = srvVEmploymentVerification;
        }


        public DeoDashboardModel GetDeoDashboardStats()
        {
            try
            {
                var deoDashboard = _dapper.QuerySingle<DeoDashboardModel>("dbo.RD_DeoDashboardStats", CommandType.StoredProcedure);
                //SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@UserID", userID);
                //param[1] = new SqlParameter("@ClassID", 0);

                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassForTSP", param).Tables[0];
                return deoDashboard;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public TSPEmploymentModel GetTraineeData(int classID, int traineeID)
        {
            var peQuery = $"Select PE.*,C.ClassCode,C.EndDate,tp.TraineeCode,tp.ContactNumber1," +
                $"CASE when tp.ContactNumber1 is not null then tp.ContactNumber1 when tp.ContactNumber2 is not null then tp.ContactNumber2 Else 'N/A' End as ContactNumber" +
                $" From PlacementFormE pe INNER JOIN TraineeProfile tp ON tp.TraineeID = PE.TraineeID INNER JOIN Class C ON pe.ClassID=C.ClassID Where pe.ClassID = {classID} AND pe.TraineeID = {traineeID}";
            var trainee = _dapper.QueryFirstOrDefault<TSPEmploymentModel>(peQuery);
            if (trainee == null)
            {
                var tQuery = $"Select p.TraineeName,p.TraineeCode,C.EndDate,p.ContactNumber1," +
                    $"CASE when p.ContactNumber1 is not null then p.ContactNumber1 when p.ContactNumber2 is not null then p.ContactNumber2 Else 'N/A' End as ContactNumber," +
                    $"p.ClassID, p.TraineeID,C.ClassCode From TraineeProfile p INNER JOIN Class C ON p.ClassID=C.ClassID Where p.TraineeID = {traineeID}";
                trainee = _dapper.QueryFirstOrDefault<TSPEmploymentModel>(tQuery);
            }
            return trainee;
        }

        public List<RD_ClassForTSPModel> GetClasses(int userID)
        {
            try
            {
                var list = _dapper.Query<RD_ClassForTSPModel>("dbo.RD_ClassForDEOVerification", new { @ClassID = 0, @UserID = userID }, CommandType.StoredProcedure);
                //SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@UserID", userID);
                //param[1] = new SqlParameter("@ClassID", 0);

                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassForTSP", param).Tables[0];
                return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<RD_TSPForEmploymentVerificationModel> GetTPSDetailForEmploymentVerification(int placementTypeId, int veificationMethodId)
        {
            try
            {
                var list = _dapper.Query<RD_TSPForEmploymentVerificationModel>("dbo.RD_TSPDetailForEmploymentVerification", new { @PlacementTypeID = placementTypeId, @VerificationMethodID = veificationMethodId }, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RD_ClassForTSPModel> GetTelephonicEmploymentVerificationClasses()
        {
            try
            {
                var list = _dapper.Query<RD_ClassForTSPModel>("dbo.RD_ClassForTelephonicVerification", CommandType.StoredProcedure);
                //SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@UserID", userID);
                //param[1] = new SqlParameter("@ClassID", 0);

                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassForTSP", param).Tables[0];
                return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<RD_ClassForTSPModel> GetClassesForEmploymentVerification(int PlacementID, int VerificationMethodID, int TspID, int ClassID)
        {
            try
            {
                var list = _dapper.Query<RD_ClassForTSPModel>("dbo.RD_ClassForEmploymentVerification", new { @PlacementTypeID = PlacementID, @VerificationMethodID = VerificationMethodID, @TSPID = TspID, @ClassID = ClassID }, CommandType.StoredProcedure);
                //SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@UserID", userID);
                //param[1] = new SqlParameter("@ClassID", 0);

                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassForTSP", param).Tables[0];
                return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RD_ClassForTSPModel> GetTelephonicEmploymentVerificationClasses(int PlacementID, int VerificationMethodID, int TspID, int ClassID)
        {
            try
            {
                var list = _dapper.Query<RD_ClassForTSPModel>("dbo.RD_TelephonicEmploymentVerificationClasses", new { @PlacementTypeID = PlacementID, @VerificationMethodID = VerificationMethodID, @TSPID = TspID, @ClassID = ClassID }, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RD_ClassForTSPModel> FetchClassFilters(int[] filters)
        {
            //List<RD_ClassForTSPModel> list = new List<RD_ClassForTSPModel>();

            int schemeId = filters[0];
            int tspId = filters[1];
            int classId = filters[2];
            int userId = filters[3];
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@SchemeID", schemeId);
                param[1] = new SqlParameter("@TSPID", tspId);
                param[2] = new SqlParameter("@ClassID", classId);
                param[3] = new SqlParameter("@UserID", userId);
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MasterSheets", param).Tables[0];
                //list = LoopinData(dt);
                //var data = SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_ClassForTSP", param);

                var list = _dapper.Query<RD_ClassForTSPModel>("dbo.RD_ClassForTSP", new { @SchemeID = schemeId, @TSPID = tspId, @ClassID = classId, @UserID = userId }, CommandType.StoredProcedure);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public List<RD_CompletedTraineeByClassModel> GetCompletedTraineeByClass(int ClassID)
        {
            try
            {
                var list = _dapper.Query<RD_CompletedTraineeByClassModel>("dbo.RD_CompletedTraineeByClass", new { @ClassID = ClassID }, CommandType.StoredProcedure);
                return list;
                //var result = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CompletedTraineeByClass", new SqlParameter("@ClassID", ClassID));
                ////DataTable dt = result.Tables[0];
                ////DataTable dt2 = result.Tables[1];
                //return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool SavePlacementFormE(TSPEmploymentModel PlacementFormE)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[38];
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
                param[24] = new SqlParameter("@TraineeCode", PlacementFormE.TraineeCode);
                param[25] = new SqlParameter("@ContactNumber", PlacementFormE.ContactNumber);

                param[26] = new SqlParameter("@CurUserID", PlacementFormE.CurUserID);

                param[27] = new SqlParameter("@PlatformName", PlacementFormE.PlatformName); 
                param[28] = new SqlParameter("@NameofTraineeStorePage", PlacementFormE.NameofTraineeStorePage); 
                param[29] = new SqlParameter("@LinkofTraineeStorePage", PlacementFormE.LinkofTraineeStorePage);


                param[30] = new SqlParameter("@PlacementTypeID", PlacementFormE.PlacementTypeID);
                param[31] = new SqlParameter("@EOBI", PlacementFormE.EOBI);
                param[32] = new SqlParameter("@VerificationMethodId", PlacementFormE.VerificationMethodId);
                param[33] = new SqlParameter("@FileType", PlacementFormE.FileType);
                param[34] = new SqlParameter("@FilePath", PlacementFormE.FilePath);
                param[35] = new SqlParameter("@FileName", PlacementFormE.FileName);
                param[36] = new SqlParameter("@PlacementReturnID", SqlDbType.Int);

                param[37] = new SqlParameter("@EmployerNTN", PlacementFormE.EmployerNTN);

                param[36].Direction = ParameterDirection.Output;
                var saved = SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PlacementFormE", param);
                long PlacementReturnID = Convert.ToInt32(param[36].Value);
                //return FetchPlacementFormE();
                // Notification send to KAM
                ApprovalModel approvalsModelForNotification = new ApprovalModel();
                ApprovalHistoryModel model = new ApprovalHistoryModel();
                TSPMasterModel KAMmodel = srvTSPMaster.GetKAMUserByClassID(PlacementFormE.ClassID ?? 0);
                approvalsModelForNotification.UserIDs = KAMmodel.UserID.ToString();
                approvalsModelForNotification.ProcessKey = EnumApprovalProcess.Employment_Report;
                approvalsModelForNotification.CustomComments = "Employment data submitted by TSP is ready for verification.";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                return saved > 0;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<TSPEmploymentModel> FetchPlacementFormE(TSPEmploymentModel mod)
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentModel>("dbo.RD_PlacementFormE", new
                {
                    @ClassID = mod.ClassID,
                    @TSPID = mod.TSPID ?? 0,
                    @TraineeID = mod.TraineeID ?? 0,
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

        public List<TSPEmploymentModel> FetchPlacementFormEForVerification(TSPEmploymentModel mod)
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentModel>("dbo.RD_PlacementFormE", new
                {
                    @ClassID = mod.ClassID,
                    @TSPID = mod.TSPID ?? 0,
                    @TraineeID = mod.TraineeID ?? 0,
                    @VerificationMethodID = mod.VerificationMethodId ?? 0,
                    @PlacementTypeID = mod.PlacementTypeID ?? 0
                }, CommandType.StoredProcedure);
                if (mod.TraineeID > 0)
                {
                    list.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                }
                //list.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE", Common.GetParams(mod)).Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPEmploymentModel> FetchTraineeForEmploymentVerification(TSPEmploymentModel mod)
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentModel>("dbo.RD_PlacementFormE", new
                {
                    @TraineeID = mod.TraineeID,
                }, CommandType.StoredProcedure);
                list.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                //list.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE", Common.GetParams(mod)).Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TSPEmploymentModel> FetchReportedPlacementFormE(TSPEmploymentModel mod)
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentModel>("dbo.RD_PlacementFormEReported", new
                {
                    @ClassID = mod.ClassID,
                    @TSPID = mod.TSPID ?? 0,
                    @TraineeID = mod.TraineeID ?? 0,
                    @VerificationMethodID = mod.VerificationMethodId ?? 0,
                    @PlacementTypeID = mod.PlacementTypeID ?? 0
                }, CommandType.StoredProcedure);
                //list.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandTyp4
                //e.StoredProcedure, "RD_PlacementFormE", Common.GetParams(mod)).Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TSPEmploymentModel> FetchEmployedTraineesForTSP(TSPEmploymentModel mod)
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentModel>("dbo.RD_PlacementFormE", new
                {
                    @ClassID = mod.ClassID,
                    @TSPID = mod.TSPID ?? 0,
                    @TraineeID = mod.TraineeID ?? 0,
                    @VerificationMethodID = mod.VerificationMethodId ?? 0,
                    @PlacementTypeID = mod.PlacementTypeID ?? 0
                }, CommandType.StoredProcedure);
                //list.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE", Common.GetParams(mod)).Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPEmploymentModel> FetchPlacementFormEByTraineeID(TSPEmploymentModel mod)
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentModel>("dbo.RD_PlacementFormE", new
                {
                    @ClassID = mod.ClassID,
                    @TSPID = mod.TSPID ?? 0,
                    @TraineeID = mod.TraineeID ?? 0,
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
        public List<RD_ClassForTSPModel> FetchDEOEmploymentClassesByTSP(int pId, int vmId, int tspId)
        {
            try
            {
                var list = _dapper.Query<RD_ClassForTSPModel>("dbo.RD_PlacementFormE", new
                {
                    @TSPID = tspId,
                    @VerificationMethodID = vmId,
                    @PlacementTypeID = pId
                }, CommandType.StoredProcedure);
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE", Common.GetParams(mod)).Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPEmploymentModel> FetchPlacementFormE()
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentModel>("dbo.RD_PlacementFormE", new { }, CommandType.StoredProcedure);
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE").Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPEmploymentExcelModel> FetchPlacementsForDEOToExport(TSPEmploymentExcelModel mod)
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentExcelModel>("RD_PlacementFormE_ExportExcelForDEO", new
                {
                    @ClassID = mod.ClassID,
                    @TSPID = mod.TSPID,
                    @TraineeID = mod.TraineeID ?? 0,
                    @VerificationMethodID = mod.VerificationMethodId ?? 0,
                    @PlacementTypeID = mod.PlacementTypeID ?? 0
                }, CommandType.StoredProcedure);
                //list.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return list;
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE", Common.GetParams(mod)).Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPEmploymentExcelModel> FetchReportedPlacementToExport(QueryFilters filters)
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentExcelModel>("RD_PlacementFormE_ExportExcel_Reported", new
                {
                    @SchemeID = filters.SchemeID,
                    @TSPID = filters.TSPID,
                    @ClassID = filters.ClassID,
                }, CommandType.StoredProcedure);
                return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<RD_ClassForTSPModelExportExcelVerifedEmploymentReport> FetchVerifiedPlacementToExport(QueryFilters filters)
        {
            try
            {
                var list = _dapper.Query<RD_ClassForTSPModelExportExcelVerifedEmploymentReport>("RD_PlacementFormE_ExportExcel_Verified", new
                {
                    @SchemeID = filters.SchemeID,
                    @TSPID = filters.TSPID,
                    @ClassID = filters.ClassID,
                }, CommandType.StoredProcedure);
                return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPEmploymentModel> FetchPlacementFormE(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementFormE", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int SubmitClassEmployment(int ClassID, int CurUserID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@ClassID", ClassID);
                param[1] = new SqlParameter("@CurUserID", CurUserID);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "SubmitClassEmployment", new SqlParameter("@ClassID", ClassID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "A_PlacementVerification", new SqlParameter("@ClassID", ClassID));        
                return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "CalculateROSIOnEmploymentSubmission", param);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataSet GetSelfTSPList(int ClassID)
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

        public DataSet GetFormalTSPList(int ClassID)
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

        private List<TSPEmploymentModel> LoopinData(DataTable dt)
        {
            List<TSPEmploymentModel> PlacementFormEL = new List<TSPEmploymentModel>();

            foreach (DataRow r in dt.Rows)
            {
                PlacementFormEL.Add(RowOfPlacementFormE(r));
            }
            return PlacementFormEL;
        }

        private TSPEmploymentModel RowOfPlacementFormE(DataRow r)
        {
            try
            {
                bool IsTradeName = r.Table.Columns.Contains("TradeName");

                TSPEmploymentModel PlacementFormE = new TSPEmploymentModel();
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

        public bool ForwardedToTelephonic(ForwardToTelephonicVerification Model)
        {
            try
            {
                var ids = string.Join(",", Model.ClassIDslist);
                var query = "";
                if (string.IsNullOrEmpty(ids))
                {
                    return false;
                }
                if (Model.VerificationMethodID == 0)
                {
                    query = @"Update PlacementFormE
                              SET ForwardedToTelephonic = 1
                              Where PlacementTypeID=" + Model.PlacementTypeID + " AND  ClassID IN ( " + ids + " ) ";

                }
                else
                {
                    query = @"Update PlacementFormE
                              SET ForwardedToTelephonic = 1
                              Where PlacementTypeID=" + Model.PlacementTypeID + " AND VerificationMethodId=" + Model.VerificationMethodID + " AND  ClassID IN ( " + ids + " ) ";

                }
                var success = _dapper.ExecuteQuery(query);
                return success > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<TSPEmploymentModel> FetchTelephonicPlacementFormE(TSPEmploymentModel mod)
        {
            try
            {
                var list = _dapper.Query<TSPEmploymentModel>("dbo.RD_PlacementFormE_Telephonic", new
                {
                    @ClassID = mod.ClassID,
                    @TSPID = mod.TSPID ?? 0,
                    @TraineeID = mod.TraineeID ?? 0,
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
    }
}