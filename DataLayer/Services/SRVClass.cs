using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVClass : SRVBase, DataLayer.Interfaces.ISRVClass
    {
        public SRVClass()
        {
        }

        public ClassModel GetByClassID(int ClassID)

        {
            try
            {
                SqlParameter param = new SqlParameter("@ClassID", ClassID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfClass(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CheckRegistrationCriteriaModel> CheckRegistrationCriteria(int ClassID)

        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@ClassID", ClassID);
                param[1] = new SqlParameter("@Channel", "Form");
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CheckRegistrationCriteria", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinCheckRegistrationCriteria(dt);
                }
                else
                    return new List<CheckRegistrationCriteriaModel>();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<CheckRegistrationCriteriaModel> LoopinCheckRegistrationCriteria(DataTable dt)
        {
            List<CheckRegistrationCriteriaModel> list = new List<CheckRegistrationCriteriaModel>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(RowOfheckRegistrationCriteria(r));
            }
            return list;
        }

        private CheckRegistrationCriteriaModel RowOfheckRegistrationCriteria(DataRow r)
        {
            CheckRegistrationCriteriaModel model = new CheckRegistrationCriteriaModel();
            model.ErrorMessage = r.Field<string>("ErrorMessage");
            model.ErrorTypeID = r.Field<int>("ErrorTypeID");
            model.ErrorTypeName = r.Field<string>("ErrorTypeName");
            return model;
        }

        public List<ClassModel> GetBySchemeID(int SchemeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SchemeID", SchemeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_Class]", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinData(dt);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ClassModel SaveClass(ClassModel Class)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[57];
                param[0] = new SqlParameter("@ClassID", Class.ClassID);
                param[1] = new SqlParameter("@ClassCode", Class.ClassCode);
                param[2] = new SqlParameter("@ClassStatusID", 1);
                param[3] = new SqlParameter("@TSPID", Class.TSPID);
                param[4] = new SqlParameter("@SectorID", Class.SectorID);
                param[5] = new SqlParameter("@TradeID", Class.TradeID);
                param[6] = new SqlParameter("@Duration", Class.Duration);
                param[7] = new SqlParameter("@TraineesPerClass", Class.TraineesPerClass);
                param[8] = new SqlParameter("@Batch", Class.Batch);
                param[9] = new SqlParameter("@GenderID", Class.GenderID);
                param[10] = new SqlParameter("@TrainingAddressLocation", Class.TrainingAddressLocation);
                param[11] = new SqlParameter("@AttendanceStandardPercentage", Class.AttendanceStandardPercentage);
                param[12] = new SqlParameter("@Latitude", Class.Latitude);
                param[13] = new SqlParameter("@Longitude", Class.Longitude);
                param[14] = new SqlParameter("@DistrictID", Class.DistrictID);
                param[15] = new SqlParameter("@TehsilID", Class.TehsilID);
                param[16] = new SqlParameter("@ClusterID", Class.ClusterID);
                param[17] = new SqlParameter("@MinHoursPerMonth", Class.MinHoursPerMonth);
                param[18] = new SqlParameter("@StartDate", Class.StartDate);
                param[19] = new SqlParameter("@EndDate", Class.EndDate);
                param[20] = new SqlParameter("@SourceOfCurriculum", Class.SourceOfCurriculum);
                param[21] = new SqlParameter("@EntryQualification", Class.EntryQualification);
                param[22] = new SqlParameter("@CertAuthID", Class.CertAuthID);
                param[23] = new SqlParameter("@EmploymentCommitmentSelf", Class.EmploymentCommitmentSelf);
                param[24] = new SqlParameter("@TrainingCostPerTraineePerMonthExTax", Class.TrainingCostPerTraineePerMonthExTax);
                param[25] = new SqlParameter("@SalesTax", Class.SalesTax);
                param[26] = new SqlParameter("@TrainingCostPerTraineePerMonthInTax", Class.TrainingCostPerTraineePerMonthInTax);
                param[27] = new SqlParameter("@UniformBagCost", Class.UniformBagCost);
                param[28] = new SqlParameter("@TotalPerTraineeCostInTax", Class.TotalPerTraineeCostInTax);
                param[29] = new SqlParameter("@TotalCostPerClassInTax", Class.TotalCostPerClassInTax);
                param[30] = new SqlParameter("@PerTraineeTestCertCost", Class.PerTraineeTestCertCost);
                param[31] = new SqlParameter("@TotalCostPerClass", Class.TotalCostPerClass);
                param[32] = new SqlParameter("@EmploymentCommitmentFormal", Class.EmploymentCommitmentFormal);
                param[33] = new SqlParameter("@OverallEmploymentCommitment", Class.OverallEmploymentCommitment);
                param[34] = new SqlParameter("@Stipend", Class.Stipend);
                param[35] = new SqlParameter("@StipendMode", Class.StipendMode);
                param[36] = new SqlParameter("@BoardingAllowancePerTrainee", Class.BoardingAllowancePerTrainee);
                param[37] = new SqlParameter("@BidPrice", Class.BidPrice);
                param[38] = new SqlParameter("@BMPrice", Class.BMPrice);
                param[39] = new SqlParameter("@OfferedPrice", Class.OfferedPrice);
                param[40] = new SqlParameter("@BidOfferPriceSavings", Class.BidOfferPriceSavings);
                param[41] = new SqlParameter("@BMOfferPriceSaving", Class.BMOfferPriceSaving);
                param[42] = new SqlParameter("@TotalTestingCertificationOfClass", Class.TotalTestingCertificationOfClass);

                param[43] = new SqlParameter("@CurUserID", Class.CurUserID);
                param[44] = new SqlParameter("@OrganizationID", Class.OrganizationID);
                param[45] = new SqlParameter("@SchemeID", Class.SchemeID);
                param[46] = new SqlParameter("@MinAge", Class.MinAge);
                param[47] = new SqlParameter("@MaxAge", Class.MaxAge);
                param[48] = new SqlParameter("@RTP", Class.RTP);
                param[49] = new SqlParameter("@NTP", Class.NTP);

                param[50] = new SqlParameter("@Ident", SqlDbType.Int);
                param[50].Direction = ParameterDirection.Output;

                Class.UID = Guid.NewGuid().ToString();
                param[51] = new SqlParameter("@UID", Class.UID);

                param[52] = new SqlParameter("@IsMigrated", Class.IsMigrated);
                param[53] = new SqlParameter("@TaxRate", Class.SalesTaxRate);
                param[54] = new SqlParameter("@TradeDetailMapID", Class.TradeDetailMapID);
                param[55] = new SqlParameter("@RegistrationAuthorityID", Class.RegistrationAuthorityID);
                param[56] = new SqlParameter("@ProgramFocusID", Class.ProgramFocusID);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Class]", param);
                int k = Convert.ToInt32(param[50].Value);
                return GetByClassID(k);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ClassModel> LoopinData(DataTable dt)
        {
            List<ClassModel> ClassL = new List<ClassModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassL.Add(RowOfClass(r));
            }
            return ClassL;
        }

        private List<ClassModel> LoopinPBTEFilterClassData(DataTable dt)
        {
            List<ClassModel> ClassL = new List<ClassModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassL.Add(RowOfPBTEFilterClass(r));
            }
            return ClassL;
        }

        private List<ClassModel> LoopinDashboardData(DataTable dt)
        {
            List<ClassModel> ClassL = new List<ClassModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassL.Add(RowOfDashboardClass(r));
            }
            return ClassL;
        }

        public List<ClassModel> FetchClass(ClassModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchApprovadClassesByModel(ClassModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Approved_Class", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void UpdateClassStatus(int ClassID, int ClassStatusID)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@ClassID", ClassID);
            param[1] = new SqlParameter("@ClassStatusID", ClassStatusID);

            try
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_ClassStatus", param);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchClassByFilters(int[] filters)
        {
            List<ClassModel> list = new List<ClassModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                int traineeId = filters[3];
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TSPID", tspId);
                    param[2] = new SqlParameter("@ClassID", classId);
                    param[3] = new SqlParameter("@TraineeID", traineeId);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FilteredClasses", param).Tables[0];
                    list = LoopinData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        public List<ClassModel> FetchClassByTsp(int tspId)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassByTsp", new SqlParameter("@TSPId", tspId)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchClassByScheme(int SchemeID, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    //SqlDataReader sReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "RD_Class", new SqlParameter("@SchemeID", SchemeID));
                    //Create a new DataTable.
                    //dt.Load(sReader);
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_Class", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchClass()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchApprovedClass()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Approved_Class").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchClassForPBTEFilter()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTE_Filter_Class").Tables[0];
                return LoopinPBTEFilterClassData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchClass(bool InActive)
        {
            try
            {

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<ClassModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Class]", param);
        }

        public List<ClassModel> GetBySectorID(int SectorID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", new SqlParameter("@SectorID", SectorID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> GetByTradeID(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> GetByGenderID(int GenderID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", new SqlParameter("@GenderID", GenderID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> GetByDistrictID(int DistrictID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", new SqlParameter("@DistrictID", DistrictID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> GetByTehsilID(int TehsilID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", new SqlParameter("@TehsilID", TehsilID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> GetByClusterID(int ClusterID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", new SqlParameter("@ClusterID", ClusterID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> GetByCertAuthID(int CertAuthID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Class", new SqlParameter("@CertAuthID", CertAuthID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int GetClassSequence()
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetClassSequence"));
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        public List<ClassModel> FetchClassesDataByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassesByUser", param.ToArray()).Tables[0];
                return LoopinDashboardData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int ClassID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ClassID", ClassID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Class]", PLead);
        }

        public void RTP(int ClassID, bool? RTP, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ClassID", ClassID);
            PLead[1] = new SqlParameter("@RTP", RTP);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RTP_Class]", PLead);
        }

        private ClassModel RowOfClass(DataRow r)
        {
            ClassModel Class = new ClassModel();
            Class.ClassID = Convert.ToInt32(r["ClassID"]);
            Class.ClassCode = r["ClassCode"].ToString();
            Class.ClassStatusID = Convert.ToInt32(r["ClassStatusID"]);
            Class.ClassStatusName = r["ClassStatusName"].ToString();
            Class.TSPID = Convert.ToInt32(r["TSPID"]);
            Class.SectorID = Convert.ToInt32(r["SectorID"]);
            Class.SectorName = r["SectorName"].ToString();
            Class.TradeID = Convert.ToInt32(r["TradeID"]);
            Class.TradeName = r["TradeName"].ToString();
            Class.Duration = Convert.ToDecimal(r["Duration"]);
            Class.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);
            Class.Batch = Convert.ToInt32(r["Batch"]);
            Class.GenderID = Convert.ToInt32(r["GenderID"]);
            Class.GenderName = r["GenderName"].ToString();
            Class.TrainingAddressLocation = r["TrainingAddressLocation"].ToString();
            Class.AttendanceStandardPercentage = r["AttendanceStandardPercentage"].ToString();
            Class.Latitude = r["Latitude"].ToString();
            Class.Longitude = r["Longitude"].ToString();
            Class.DistrictID = Convert.ToInt32(r["DistrictID"]);
            Class.DistrictName = r["DistrictName"].ToString();
            Class.DistrictNameUrdu = r["DistrictNameUrdu"].ToString();
            Class.TehsilID = Convert.ToInt32(r["TehsilID"]);
            Class.TehsilName = r["TehsilName"].ToString();
            Class.ClusterID = Convert.ToInt32(r["ClusterID"]);
            Class.ClusterName = r["ClusterName"].ToString();
            Class.MinHoursPerMonth = Convert.ToInt32(r["MinHoursPerMonth"]);
            Class.StartDate = r["StartDate"].ToString().GetDate();
            Class.EndDate = r["EndDate"].ToString().GetDate();
            Class.SourceOfCurriculum = r["SourceOfCurriculum"].ToString();
            Class.EntryQualification = r["EntryQualification"].ToString();
            Class.CertAuthID = Convert.ToInt32(r["CertAuthID"]);
            Class.CertAuthName = r["CertAuthName"].ToString();
            Class.EmploymentCommitmentSelf = Convert.ToInt32(r["EmploymentCommitmentSelf"]);
            Class.TrainingCostPerTraineePerMonthExTax = Convert.ToDecimal(r["TrainingCostPerTraineePerMonthExTax"]);
            Class.SalesTax = Convert.ToDecimal(r["SalesTax"]);
            Class.TrainingCostPerTraineePerMonthInTax = Convert.ToDecimal(r["TrainingCostPerTraineePerMonthInTax"]);
            Class.UniformBagCost = Convert.ToDecimal(r["UniformBagCost"]);
            Class.TotalPerTraineeCostInTax = Convert.ToDecimal(r["TotalPerTraineeCostInTax"]);
            Class.TotalCostPerClassInTax = Convert.ToDecimal(r["TotalCostPerClassInTax"]);
            Class.PerTraineeTestCertCost = Convert.ToDecimal(r["PerTraineeTestCertCost"]);
            Class.TotalCostPerClass = Convert.ToDecimal(r["TotalCostPerClass"]);
            Class.EmploymentCommitmentFormal = Convert.ToInt32(r["EmploymentCommitmentFormal"]);
            Class.OverallEmploymentCommitment = Convert.ToInt32(r["OverallEmploymentCommitment"]);
            Class.Stipend = Convert.ToInt32(r["Stipend"]);
            Class.StipendMode = r["StipendMode"].ToString();
            Class.BoardingAllowancePerTrainee = Convert.ToInt32(r["BoardingAllowancePerTrainee"]);
            Class.BidPrice = Convert.ToDecimal(r["BidPrice"]);
            Class.BMPrice = Convert.ToDecimal(r["BMPrice"]);
            Class.OfferedPrice = Convert.ToDecimal(r["OfferedPrice"]);
            Class.BidOfferPriceSavings = Convert.ToDecimal(r["BidOfferPriceSavings"]);
            Class.BMOfferPriceSaving = Convert.ToDecimal(r["BMOfferPriceSaving"]);
            Class.TotalTestingCertificationOfClass = Convert.ToDecimal(r["TotalTestingCertificationOfClass"]);
            Class.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Class.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            Class.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Class.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Class.InActive = Convert.ToBoolean(r["InActive"]);
            Class.RTP = Convert.ToBoolean(r["RTP"]);
            Class.NTP = Convert.ToBoolean(r["NTP"]);
            Class.SchemeID = Convert.ToInt32(r["SchemeID"]);
            Class.IsDual = Convert.ToBoolean(r["IsDual"]);
            Class.TSPCode = r["TSPCode"].ToString();
            Class.OrganizationID = Convert.ToInt32(r["OrganizationID"]);
            Class.MinAge = Convert.ToInt32(r["MinAge"]);
            Class.MaxAge = Convert.ToInt32(r["MaxAge"]);
            Class.UID = r["UID"].ToString();
            Class.IsMigrated = Convert.ToBoolean(r["IsMigrated"]);
            Class.SalesTaxRate = Convert.ToDecimal(r["TaxRate"]);
            Class.TSPName = r["TSPName"].ToString();
            Class.EntryQualificationName = r["EntryQualificationName"].ToString();
            if (r.Table.Columns.Contains("SchemeName"))
            {
                Class.SchemeName = r["SchemeName"].ToString();
            }
            if (r.Table.Columns.Contains("EmploymentSubmited"))
            {
                Class.EmploymentSubmited = Convert.ToBoolean(r["EmploymentSubmited"]);
            }
            if (r.Table.Columns.Contains("TradeDetailMapID"))
            {
                Class.TradeDetailMapID = Convert.ToInt32(r["TradeDetailMapID"]);
            }
            if (r.Table.Columns.Contains("ProvinceID"))
            {
                Class.ProvinceID = Convert.ToInt32(r["ProvinceID"]);
            }
            if (r.Table.Columns.Contains("ProvinceName"))
            {
                Class.ProvinceName = r["ProvinceName"].ToString();
            }
            if (r.Table.Columns.Contains("ProgramFocusName"))
            {
                Class.ProgramFocusName = r["ProgramFocusName"].ToString();
            }
            if (r.Table.Columns.Contains("RegistrationAuthorityName"))
            {
                Class.RegistrationAuthorityName = r["RegistrationAuthorityName"].ToString();
            }
            //if (r.Table.Columns.Contains("ProgramFocusID"))
            //{
            //    Class.ProgramFocusID = Convert.ToInt32(r["ProgramFocusID"]);
            //}
            //if (r.Table.Columns.Contains("RegistrationAuthorityID"))
            //{
            //    Class.RegistrationAuthorityID = Convert.ToInt32(r["RegistrationAuthorityID"]);
            //}

            return Class;
        }

        private ClassModel RowOfPBTEFilterClass(DataRow r)
        {
            ClassModel Class = new ClassModel();
            Class.ClassID = Convert.ToInt32(r["ClassID"]);
            Class.ClassCode = r["ClassCode"].ToString();

            return Class;
        }

        private ClassModel RowOfDashboardClass(DataRow r)
        {
            ClassModel Class = new ClassModel();
            Class.ClassID = Convert.ToInt32(r["ClassID"]);
            Class.ClassCode = r["ClassCode"].ToString();
            Class.ClassStatusID = Convert.ToInt32(r["ClassStatusID"]);
            Class.ClassStatusName = r["ClassStatusName"].ToString();
            Class.TSPID = Convert.ToInt32(r["TSPID"]);
            Class.SectorID = Convert.ToInt32(r["SectorID"]);
            Class.SectorName = r["SectorName"].ToString();
            Class.TradeID = Convert.ToInt32(r["TradeID"]);
            Class.TradeName = r["TradeName"].ToString();
            Class.Duration = Convert.ToDecimal(r["Duration"]);
            Class.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);
            Class.Batch = Convert.ToInt32(r["Batch"]);
            Class.GenderID = Convert.ToInt32(r["GenderID"]);
            Class.GenderName = r["GenderName"].ToString();
            Class.TrainingAddressLocation = r["TrainingAddressLocation"].ToString();
            Class.AttendanceStandardPercentage = r["AttendanceStandardPercentage"].ToString();
            Class.Latitude = r["Latitude"].ToString();
            Class.Longitude = r["Longitude"].ToString();
            Class.DistrictID = Convert.ToInt32(r["DistrictID"]);
            Class.DistrictName = r["DistrictName"].ToString();
            Class.DistrictNameUrdu = r["DistrictNameUrdu"].ToString();
            Class.TehsilID = Convert.ToInt32(r["TehsilID"]);
            Class.TehsilName = r["TehsilName"].ToString();
            Class.ClusterID = Convert.ToInt32(r["ClusterID"]);
            Class.ClusterName = r["ClusterName"].ToString();
            Class.MinHoursPerMonth = Convert.ToInt32(r["MinHoursPerMonth"]);
            Class.StartDate = r["StartDate"].ToString().GetDate();
            Class.EndDate = r["EndDate"].ToString().GetDate();
            Class.SourceOfCurriculum = r["SourceOfCurriculum"].ToString();
            Class.EntryQualification = r["EntryQualification"].ToString();
            Class.CertAuthID = Convert.ToInt32(r["CertAuthID"]);
            Class.CertAuthName = r["CertAuthName"].ToString();
            Class.EmploymentCommitmentSelf = Convert.ToInt32(r["EmploymentCommitmentSelf"]);
            Class.TrainingCostPerTraineePerMonthExTax = Convert.ToDecimal(r["TrainingCostPerTraineePerMonthExTax"]);
            Class.SalesTax = Convert.ToDecimal(r["SalesTax"]);
            Class.SalesTaxRate = Convert.ToDecimal(r["TaxRate"] ?? 0);
            Class.TrainingCostPerTraineePerMonthInTax = Convert.ToDecimal(r["TrainingCostPerTraineePerMonthInTax"]);
            Class.UniformBagCost = Convert.ToDecimal(r["UniformBagCost"]);
            Class.TotalPerTraineeCostInTax = Convert.ToDecimal(r["TotalPerTraineeCostInTax"]);
            Class.TotalCostPerClassInTax = Convert.ToDecimal(r["TotalCostPerClassInTax"]);
            Class.PerTraineeTestCertCost = Convert.ToDecimal(r["PerTraineeTestCertCost"]);
            Class.TotalCostPerClass = Convert.ToDecimal(r["TotalCostPerClass"]);
            Class.EmploymentCommitmentFormal = Convert.ToInt32(r["EmploymentCommitmentFormal"]);
            Class.OverallEmploymentCommitment = Convert.ToInt32(r["OverallEmploymentCommitment"]);
            Class.Stipend = Convert.ToInt32(r["Stipend"]);
            Class.StipendMode = r["StipendMode"].ToString();
            Class.BoardingAllowancePerTrainee = Convert.ToInt32(r["BoardingAllowancePerTrainee"]);
            Class.BidPrice = Convert.ToDecimal(r["BidPrice"]);
            Class.BMPrice = Convert.ToDecimal(r["BMPrice"]);
            Class.OfferedPrice = Convert.ToDecimal(r["OfferedPrice"]);
            Class.BidOfferPriceSavings = Convert.ToDecimal(r["BidOfferPriceSavings"]);
            Class.BMOfferPriceSaving = Convert.ToDecimal(r["BMOfferPriceSaving"]);
            Class.TotalTestingCertificationOfClass = Convert.ToDecimal(r["TotalTestingCertificationOfClass"]);
            Class.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Class.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            Class.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Class.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Class.InActive = Convert.ToBoolean(r["InActive"]);
            Class.RTP = Convert.ToBoolean(r["RTP"]);
            Class.NTP = Convert.ToBoolean(r["NTP"]);
            Class.SchemeID = Convert.ToInt32(r["SchemeID"]);
            Class.IsDual = Convert.ToBoolean(r["IsDual"]);
            Class.TSPCode = r["TSPCode"].ToString();
            Class.OrganizationID = Convert.ToInt32(r["OrganizationID"]);
            if (r.Table.Columns.Contains("KamID"))
            {
                Class.KamID = Convert.ToInt32(r["KamID"]);
            }
            Class.MinAge = Convert.ToInt32(r["MinAge"]);
            Class.MaxAge = Convert.ToInt32(r["MaxAge"]);
            Class.UID = r["UID"].ToString();
            Class.IsMigrated = Convert.ToBoolean(r["IsMigrated"]);
            Class.EnrolledTrainees = Convert.ToInt32(r["EnrolledTrainees"]);
            Class.InstructorName = r["InstructorName"].ToString();
            Class.PTypeName = r["PTypeName"].ToString();
            Class.NTPStatus = r["NTPStatus"].ToString();
            Class.InceptionReportDueOn = r["InceptionReportDueOn"].ToString().GetDate();
            Class.TraineeRegistrationDueOn = r["TraineeRegistrationDueOn"].ToString().GetDate();
            if (r.Table.Columns.Contains("SchemeName"))
            {
                Class.SchemeName = r["SchemeName"].ToString();
            }
            if (r.Table.Columns.Contains("Batch"))
            {
                Class.Batch = Convert.ToInt32(r["Batch"]);
            }
            if (r.Table.Columns.Contains("ShowInceptionToInternal"))
            {
                Class.ShowInceptionToInternal = r["ShowInceptionToInternal"].ToString();
            }
            if (r.Table.Columns.Contains("TSPName"))
            {
                Class.TSPName = r["TSPName"].ToString();
            }
            return Class;
        }

        public List<ClassModel> FetchClassesByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassesByUser", param.ToArray()).Tables[0];
                return LoopinDashboardData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchPendingInceptionReportClassesByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Pending_IR_ClassesByUser", param.ToArray()).Tables[0];
                return LoopinDashboardData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchPendingRegisterationClassesByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Pending_Classes_For_Registeration_ByUser", param.ToArray()).Tables[0];
                return LoopinDashboardData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchPendingRTPClassesByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Pending_Classes_For_RTP_ByUser", param.ToArray()).Tables[0];
                return LoopinDashboardData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchPendingInceptionReportClassesByKAMUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Pending_IR_ClassesByKAMUser", param.ToArray()).Tables[0];
                return LoopinDashboardData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchPendingRegisterationClassesByKAMUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Pending_Classes_For_Registeration_ByKAMUser", param.ToArray()).Tables[0];
                return LoopinDashboardData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchPendingRTPClassesByKAMUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Pending_Classes_For_RTP_ByKAMUser", param.ToArray()).Tables[0];
                return LoopinDashboardData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchClassesByUser_DVV(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassesByUser_DVV", param.ToArray()).Tables[0];
                return LoopinClassesDataDVV(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private ClassModel RowOfClassesDVV(DataRow r)
        {
            ClassModel Class = new ClassModel();
            Class.ClassID = Convert.ToInt32(r["ClassID"]);
            Class.ClassCode = r["ClassCode"].ToString();
            Class.ClassStatusID = Convert.ToInt32(r["ClassStatusID"]);
            Class.ClassStatusName = r["ClassStatusName"].ToString();
            Class.TSPID = Convert.ToInt32(r["TSPID"]);
            Class.SectorID = Convert.ToInt32(r["SectorID"]);
            Class.SectorName = r["SectorName"].ToString();
            Class.TradeID = Convert.ToInt32(r["TradeID"]);
            Class.TradeName = r["TradeName"].ToString();
            Class.Duration = Convert.ToInt32(r["Duration"]);
            Class.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);
            Class.Batch = Convert.ToInt32(r["Batch"]);
            Class.GenderID = Convert.ToInt32(r["GenderID"]);
            Class.GenderName = r["GenderName"].ToString();
            Class.TrainingAddressLocation = r["TrainingAddressLocation"].ToString();
            Class.AttendanceStandardPercentage = r["AttendanceStandardPercentage"].ToString();
            Class.Latitude = r["Latitude"].ToString();
            Class.Longitude = r["Longitude"].ToString();
            Class.DistrictID = Convert.ToInt32(r["DistrictID"]);
            Class.DistrictName = r["DistrictName"].ToString();
            Class.DistrictNameUrdu = r["DistrictNameUrdu"].ToString();
            Class.TehsilID = Convert.ToInt32(r["TehsilID"]);
            Class.TehsilName = r["TehsilName"].ToString();
            Class.ClusterID = Convert.ToInt32(r["ClusterID"]);
            Class.ClusterName = r["ClusterName"].ToString();
            Class.MinHoursPerMonth = Convert.ToInt32(r["MinHoursPerMonth"]);
            Class.StartDate = r["StartDate"].ToString().GetDate();
            Class.EndDate = r["EndDate"].ToString().GetDate();
            Class.SourceOfCurriculum = r["SourceOfCurriculum"].ToString();
            Class.EntryQualification = r["EntryQualification"].ToString();
            Class.CertAuthID = Convert.ToInt32(r["CertAuthID"]);
            Class.CertAuthName = r["CertAuthName"].ToString();
            Class.EmploymentCommitmentSelf = Convert.ToInt32(r["EmploymentCommitmentSelf"]);
            Class.TrainingCostPerTraineePerMonthExTax = Convert.ToDecimal(r["TrainingCostPerTraineePerMonthExTax"]);
            Class.SalesTax = Convert.ToDecimal(r["SalesTax"]);
            Class.TrainingCostPerTraineePerMonthInTax = Convert.ToDecimal(r["TrainingCostPerTraineePerMonthInTax"]);
            Class.UniformBagCost = Convert.ToDecimal(r["UniformBagCost"]);
            Class.TotalPerTraineeCostInTax = Convert.ToDecimal(r["TotalPerTraineeCostInTax"]);
            Class.TotalCostPerClassInTax = Convert.ToDecimal(r["TotalCostPerClassInTax"]);
            Class.PerTraineeTestCertCost = Convert.ToDecimal(r["PerTraineeTestCertCost"]);
            Class.TotalCostPerClass = Convert.ToDecimal(r["TotalCostPerClass"]);
            Class.EmploymentCommitmentFormal = Convert.ToInt32(r["EmploymentCommitmentFormal"]);
            Class.OverallEmploymentCommitment = Convert.ToInt32(r["OverallEmploymentCommitment"]);
            Class.Stipend = Convert.ToInt32(r["Stipend"]);
            Class.StipendMode = r["StipendMode"].ToString();
            Class.BoardingAllowancePerTrainee = Convert.ToInt32(r["BoardingAllowancePerTrainee"]);
            Class.BidPrice = Convert.ToDecimal(r["BidPrice"]);
            Class.BMPrice = Convert.ToDecimal(r["BMPrice"]);
            Class.OfferedPrice = Convert.ToDecimal(r["OfferedPrice"]);
            Class.BidOfferPriceSavings = Convert.ToDecimal(r["BidOfferPriceSavings"]);
            Class.BMOfferPriceSaving = Convert.ToDecimal(r["BMOfferPriceSaving"]);
            Class.TotalTestingCertificationOfClass = Convert.ToDecimal(r["TotalTestingCertificationOfClass"]);
            Class.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Class.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            Class.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Class.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Class.InActive = Convert.ToBoolean(r["InActive"]);
            Class.RTP = Convert.ToBoolean(r["RTP"]);
            Class.NTP = Convert.ToBoolean(r["NTP"]);
            Class.SchemeID = Convert.ToInt32(r["SchemeID"]);
            Class.IsDual = Convert.ToBoolean(r["IsDual"]);
            Class.TSPCode = r["TSPCode"].ToString();
            Class.OrganizationID = Convert.ToInt32(r["OrganizationID"]);
            if (r.Table.Columns.Contains("KamID"))
            {
                Class.KamID = Convert.ToInt32(r["KamID"]);
            }
            Class.MinAge = Convert.ToInt32(r["MinAge"]);
            Class.MaxAge = Convert.ToInt32(r["MaxAge"]);
            Class.UID = r["UID"].ToString();
            Class.IsMigrated = Convert.ToBoolean(r["IsMigrated"]);
            Class.EnrolledTrainees = Convert.ToInt32(r["EnrolledTrainees"]);
            //Class.InstructorName = r["InstructorName"].ToString();
            Class.Instructors = JsonConvert.DeserializeObject<List<InstructorModel>>(r["Instructors"].ToString());

            return Class;
        }

        private List<ClassModel> LoopinClassesDataDVV(DataTable dt)
        {
            List<ClassModel> ClassL = new List<ClassModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassL.Add(RowOfClassesDVV(r));
            }
            return ClassL;
        }

        public List<ClassProceeedingStatusData> FetchClassProceeedingStatusDataByFilters(int[] filters)
        {
            List<ClassProceeedingStatusData> list = new List<ClassProceeedingStatusData>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                //int traineeID = filters[3];
                //int userID = filters[4];
                try
                {
                    SqlParameter[] param = new SqlParameter[5];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TSPID", tspId);
                    param[2] = new SqlParameter("@ClassID", classId);
                    //param[3] = new SqlParameter("@TraineeID", traineeID);
                    //param[4] = new SqlParameter("@UserID", userID);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassProceeedingStatusData", param).Tables[0];
                    list = LoopinClassProceeedingStatusData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        private List<ClassProceeedingStatusData> LoopinClassProceeedingStatusData(DataTable dt)
        {
            List<ClassProceeedingStatusData> list = new List<ClassProceeedingStatusData>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(RowOfClassProceeedingStatusData(r));
            }
            return list;
        }

        private ClassProceeedingStatusData RowOfClassProceeedingStatusData(DataRow row)
        {
            ClassProceeedingStatusData obj = new ClassProceeedingStatusData();
            obj.ClassCode = row.Field<string>("ClassCode");
            obj.MonthOfMPR = row.Field<DateTime?>("MonthOfMPR");
            obj.IsGeneratedMPR = row.Field<bool>("IsGeneratedMPR");
            obj.IsDataInsertedInAMS = row.Field<bool>("IsDataInsertedInAMS");
            obj.IsGeneratedPRNRegular = row.Field<bool>("IsGeneratedPRNRegular");
            obj.IsGeneratedPRNRegularPO = row.Field<bool>("IsGeneratedPRNRegularPO");
            obj.IsGeneratedPRNRegularInvoice = row.Field<bool>("IsGeneratedPRNRegularInvoice");
            obj.IsGeneratedSRN = row.Field<bool>("IsGeneratedSRN");
            obj.IsGeneratedSRNPO = row.Field<bool>("IsGeneratedSRNPO");
            obj.IsGeneratedSRNInvoice = row.Field<bool>("IsGeneratedSRNInvoice");
            return obj;
        }

        public List<ClassModel> FetchClassesByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
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
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassesByUserPaged", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinDashboardData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchClassesForPRNCompletion(QueryFilters filters, out string TotalCompletedClasses, out string CompletedClassesWithResult, out string IsGenerated)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@Month", filters.Month));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_ClassesForPRNCompletion", param.ToArray());
                TotalCompletedClasses = ds.Tables[0].Rows[0]["TotalCompletedClasses"].ToString();
                CompletedClassesWithResult = ds.Tables[0].Rows[0]["CompletedClassesWithResult"].ToString();
                IsGenerated = ds.Tables[0].Rows[0]["IsGenerated"].ToString();
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    return LoopinData(ds.Tables[1]);
                return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchClassesForPRNFinal(QueryFilters filters, out string TotalCompletedClasses, out string CompletedClassesWithResult, out string IsGenerated)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@Month", filters.Month));
                TotalCompletedClasses = null;
                IsGenerated = null;
                CompletedClassesWithResult = null;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_ClassesForPRNFinal", param.ToArray());
                TotalCompletedClasses = ds.Tables[0].Rows[0]["TotalCompletedClasses"].ToString();
                CompletedClassesWithResult = ds.Tables[0].Rows[0]["CompletedClassesWithResult"].ToString();
                IsGenerated = ds.Tables[0].Rows[0]["IsGenerated"].ToString();
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    return LoopinData(ds.Tables[1]);
                return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassModel> FetchClassesForTRN(QueryFilters filters, out string TotalCompletedClasses, out string CompletedClassesWithResult, out string IsGenerated)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@CertAuthID", filters.CertAuthID));
                param.Add(new SqlParameter("@Month", filters.Month));
                TotalCompletedClasses = null;
                IsGenerated = null;
                CompletedClassesWithResult = null;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_ClassesForTRN", param.ToArray());
                TotalCompletedClasses = ds.Tables[0].Rows[0]["TotalCompletedClasses"].ToString();
                CompletedClassesWithResult = ds.Tables[0].Rows[0]["CompletedClassesWithResult"].ToString();
                IsGenerated = ds.Tables[0].Rows[0]["IsGenerated"].ToString();
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    return LoopinData(ds.Tables[1]);
                return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}