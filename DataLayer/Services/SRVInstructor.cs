using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataLayer.Dapper;

namespace DataLayer.Services
{
    public class SRVInstructor : SRVBase, DataLayer.Interfaces.ISRVInstructor
    {
        //private readonly IDapperConfig _dapper;

        //public SRVInstructor(IDapperConfig dapper)
        //{
        //    _dapper = dapper;
        //}

        public SRVInstructor() { }

        public InstructorModel GetByInstrID(int InstrID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@InstrID", InstrID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfInstructor(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassCodeByInstrID> GetClassCodeByInstrID(int InstrID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@InstrID", InstrID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GetClassCodesByInstrID", param).Tables[0];

                List<ClassCodeByInstrID> classCodes = new List<ClassCodeByInstrID>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        classCodes.Add(RowOfClassCode(row));
                    }
                }

                return classCodes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<InstructorModel> GetByInstructorID(int InstrID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@InstrID", InstrID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor", param).Tables[0];
                return LoopinData(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }         
        public List<InstructorModel> GetInstructorByTradeID(int ClassID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ClassID", ClassID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor_ByTradeID", param).Tables[0];
                return LoopinData(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
public List<InstructorInceptionReportCRModel> GetIRInstructorByClassID(int ClassID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ClassID", ClassID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor_ByClassID_InceptionReport", param).Tables[0];
                return LoopinInstructorDataInceptionReport(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<InstructorModel> GetBySchemeID(int SchemeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SchemeID", SchemeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_Instructor]", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinData(dt);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public InstructorModel SaveInstructor(InstructorModel Instructor)
        {
            try
            {
                int i = 0;
                SqlParameter[] param = new SqlParameter[16];
                param[i++] = new SqlParameter("@InstrID", Instructor.InstrID);
                param[i++] = new SqlParameter("@InstructorName", Instructor.InstructorName);
                param[i++] = new SqlParameter("@CNICofInstructor", Instructor.CNICofInstructor);
                //param[i++] = new SqlParameter("@InstrClassID", Instructor.InstrClassID);
                param[i++] = new SqlParameter("@ClassCode", Instructor.ClassCode);
                param[i++] = new SqlParameter("@QualificationHighest", Instructor.QualificationHighest);
                param[i++] = new SqlParameter("@TotalExperience", Instructor.TotalExperience);
                param[i++] = new SqlParameter("@GenderID", Instructor.GenderID);
                //param[i++] = new SqlParameter("@District_OLD", Instructor.District_OLD);
                //param[i++] = new SqlParameter("@Tehsil_OLD", Instructor.Tehsil_OLD);
                Instructor.PicturePath = String.IsNullOrWhiteSpace(Instructor.PicturePath) ? "" : Common.AddFile(Instructor.PicturePath, FilePaths.INSTRUCTOR_FILE_DIR);
                param[i++] = new SqlParameter("@PicturePath", Instructor.PicturePath);
                param[i++] = new SqlParameter("@TSPID", Instructor.TSPID);
                param[i++] = new SqlParameter("@CurUserID", Instructor.CurUserID);
                param[i++] = new SqlParameter("@NameOfOrganization", Instructor.NameOfOrganization);
                param[i++] = new SqlParameter("@TradeID", Instructor.TradeID);
                param[i++] = new SqlParameter("@LocationAddress", Instructor.LocationAddress);
                //param[i++] = new SqlParameter("@ClassID", Instructor.ClassID);
                param[i++] = new SqlParameter("@InstrMasterID", Instructor.InstrMasterID);
                param[i++] = new SqlParameter("@SchemeID", Instructor.SchemeID);

                param[i] = new SqlParameter("@Ident", SqlDbType.Int);
                param[i].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Instructor]", param);

                int InstrID = Convert.ToInt32(param[i].Value);
                return GetByInstrID(InstrID);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<InstructorModel> LoopinData(DataTable dt)
        {
            List<InstructorModel> InstructorL = new List<InstructorModel>();

            foreach (DataRow r in dt.Rows)
            {
                InstructorL.Add(RowOfInstructor(r));
            }
            return InstructorL;
        }

        private List<InstructorInceptionReportCRModel> LoopinInstructorDataInceptionReport(DataTable dt)
        {
            List<InstructorInceptionReportCRModel> InstructorL = new List<InstructorInceptionReportCRModel>();

            foreach (DataRow r in dt.Rows)
            {
                InstructorL.Add(RowOfInstructorInceptionReport(r));
            }
            return InstructorL;
        }
        private List<CheckInstructorCNICModel> LoopinInstructorCNICData(DataTable dt)
        {
            List<CheckInstructorCNICModel> InstructorL = new List<CheckInstructorCNICModel>();

            foreach (DataRow r in dt.Rows)
            {
                InstructorL.Add(RowOfInstructorCNIC(r));
            }
            return InstructorL;
        }

        public List<InstructorModel> FetchInstructor(InstructorModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InstructorModel> FetchInstructor_DVV(int classID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor_DVV", new SqlParameter("@ClassID", classID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<InstructorModel> FetchInstructorByScheme(int SchemeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<InstructorModel> FetchInstructor()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CheckInstructorCNICModel> FetchOldInstructorCNICs()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_Instructors_CNICs").Tables[0];
                return LoopinInstructorCNICData(dt);
                //var list = _dapper.Query<CheckInstructorCNICModel>("dbo.Get_Instructors_CNICs", new { }, CommandType.StoredProcedure);
                //return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<InstructorModel> FetchInstructor(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public List<InstructorModel> FetchInstructorDataByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorByTSPUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InstructorModel> FetchCRInstructorDataByUser(InstructorCRFiltersModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", mod.UserID));
                param.Add(new SqlParameter("@TradeID", mod.TradeID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CRInstructorByTSPUser", param.ToArray()).Tables[0];
                return LoopinData(dt);

                //var list = _dapper.Query<InstructorModel>("dbo.RD_CRInstructorByTSPUser", new { @UserID = mod.UserID,@TradeID = mod.TradeID }, CommandType.StoredProcedure);
                //return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<InstructorModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Instructor]", param);
        }

        public List<InstructorModel> GetByGenderID(int GenderID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor", new SqlParameter("@GenderID", GenderID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InstructorModel> GetByClassID(int ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor_ByClassID", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InstructorModel> GetByTSPUserID(int userid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Instructor_ByTSPUserID", new SqlParameter("@UserID", userid)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int InstrID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@InstrID", InstrID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Instructor]", PLead);
        }

        private InstructorModel RowOfInstructor(DataRow r)
        {
            InstructorModel Instructor = new InstructorModel();

            Instructor.InstrID = Convert.ToInt32(r["InstrID"]);
            Instructor.InstructorName = r["InstructorName"].ToString();
            Instructor.CNICofInstructor = r["CNICofInstructor"].ToString();
            //Instructor.InstrClassID = Convert.ToInt32(r["InstrClassID"]);
            Instructor.ClassCode = r["ClassCode"].ToString();
            Instructor.QualificationHighest = r["QualificationHighest"].ToString();
            Instructor.TotalExperience = r["TotalExperience"].ToString();
            Instructor.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Instructor.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            Instructor.GenderID = Convert.ToInt32(r["GenderID"]);

            if (r.Table.Columns.Contains("GenderName"))
            {
                Instructor.GenderName = r["GenderName"].ToString();
            }
            //Instructor.District_OLD = Convert.ToInt32(r["District_OLD"]);
            //Instructor.Tehsil_OLD = Convert.ToInt32(r["Tehsil_OLD"]);

            if (r["PicturePath"].ToString() == "" || r["PicturePath"] == null)
                Instructor.PicturePath = "";
            else
                Instructor.PicturePath = Common.GetFileBase64(r["PicturePath"].ToString());

            Instructor.TSPID = Convert.ToInt32(r["TSPID"]);
            Instructor.TradeID = Convert.ToInt32(r["TradeID"]);
            if (r.Table.Columns.Contains("TradeName"))
            {
                Instructor.TradeName = r["TradeName"].ToString();

            }
            if (r.Table.Columns.Contains("SchemeName"))
            {
                Instructor.SchemeName = r["SchemeName"].ToString();

            }
            if (r.Table.Columns.Contains("TSPName"))
            {
                Instructor.TSPName = r["TSPName"].ToString();

            }
            Instructor.LocationAddress = r["LocationAddress"].ToString();
            Instructor.NameOfOrganization = r["NameOfOrganization"].ToString();
            Instructor.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Instructor.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Instructor.InActive = Convert.ToBoolean(r["InActive"]);
            Instructor.InstrMasterID = Convert.ToInt32(r["InstrMasterID"]);
            Instructor.SchemeID = Convert.ToInt32(r["SchemeID"]);
            //Instructor.ClassID = Convert.ToInt32(r["ClassID"]);
            Instructor.BiometricData1 = r.Field<string>("BiometricData1");
            Instructor.BiometricData2 = r.Field<string>("BiometricData2");
            Instructor.BiometricData3 = r.Field<string>("BiometricData3");
            Instructor.BiometricData4 = r.Field<string>("BiometricData4");
            Instructor.Latitude = r.Field<decimal?>("Latitude") ?? 0;
            Instructor.Longitude = r.Field<decimal?>("Longitude") ?? 0;
            Instructor.TimeStampOfVerification = r.Field<DateTime?>("TimeStampOfVerification");
            if (r.Table.Columns.Contains("IsVerifiedByDVV"))
            {
                Instructor.IsVerifiedByDVV = Convert.ToBoolean(r["IsVerifiedByDVV"]);
            }
            if (r.Table.Columns.Contains("InstrCrID"))
            {
                Instructor.InstrCrID = Convert.ToInt32(r["InstrCrID"]);
            }
            if (r.Table.Columns.Contains("CrIsApproved"))
            {
                Instructor.CrIsApproved = Convert.ToBoolean(r["CrIsApproved"]);
            }
            if (r.Table.Columns.Contains("CrIsRejected"))
            {
                Instructor.CrIsRejected = Convert.ToBoolean(r["CrIsRejected"]);
            }
            if (r.Table.Columns.Contains("InstructorCRComments"))
            {
                Instructor.InstructorCRComments = r.Field<string>("InstructorCRComments");
            }

            return Instructor;
        }

        private InstructorInceptionReportCRModel RowOfInstructorInceptionReport(DataRow r)
        {
            InstructorInceptionReportCRModel instructor = new InstructorInceptionReportCRModel();

            // Map InstrID
            instructor.InstrID = Convert.ToInt32(r["InstrID"]);

            // Map InstructorName
            instructor.InstructorName = r["InstructorName"].ToString();

            // Map GenderID
            instructor.GenderID = Convert.ToInt32(r["GenderID"]);

            // Map GenderName, checking if the column exists
            if (r.Table.Columns.Contains("GenderName"))
            {
                instructor.GenderName = r["GenderName"].ToString();
            }

            // Map TradeID
            instructor.TradeID = Convert.ToInt32(r["TradeID"]);

            instructor.IncepReportID = Convert.ToInt32(r["IncepReportID"]);

            // Map TradeName, checking if the column exists
            if (r.Table.Columns.Contains("TradeName"))
            {
                instructor.TradeName = r["TradeName"].ToString();
            }

            return instructor;
        }

        private ClassCodeByInstrID RowOfClassCode(DataRow r)
        {
            ClassCodeByInstrID classCode = new ClassCodeByInstrID();

            if (r.Table.Columns.Contains("ClassCode"))
            {
                classCode.ClassCode = r["ClassCode"].ToString();
            }

            return classCode;
        }


        private CheckInstructorCNICModel RowOfInstructorCNIC(DataRow r)
        {
            CheckInstructorCNICModel Instructor = new CheckInstructorCNICModel();

            Instructor.CNICofInstructor = r["CNICofInstructor"].ToString();

            return Instructor;
        }
        public bool SaveInstructorAttendance(InstructorAttendanceDVV model, out string errMsg)
        {
            errMsg = string.Empty;
            bool result = false;
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@InstructorID", model.InstructorID));
                param.Add(new SqlParameter("@CheckIn", model.CheckIn));
                param.Add(new SqlParameter("@CheckOut", model.CheckOut));
                param.Add(new SqlParameter("@Latitude", model.Latitude));
                param.Add(new SqlParameter("@Longitude", model.Longitude));
                param.Add(new SqlParameter("@TimeStamp", model.TimeStamp));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                param.Add(new SqlParameter("@BiometricData", model.BiometricData1));
                param.Add(new SqlParameter("@TSPId", model.TSPId));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "A_InstructorAttendance", param.ToArray());
                result = true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return result;
        }
        public bool SaveInstructorDVV(InstructorDVV model, out string errMsg)
        {
            errMsg = string.Empty;
            bool result = false;
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@InstructorID", model.InstructorID));
                param.Add(new SqlParameter("@InstructorName", model.Name));
                //param.Add(new SqlParameter("@CNICofInstructor", model.CNIC));
                param.Add(new SqlParameter("@Latitude", model.Latitude));
                param.Add(new SqlParameter("@Longitude", model.Longitude));
                param.Add(new SqlParameter("@BiometricData1", model.BiometricData1));
                param.Add(new SqlParameter("@BiometricData2", model.BiometricData2));
                param.Add(new SqlParameter("@BiometricData3", model.BiometricData3));
                param.Add(new SqlParameter("@BiometricData4", model.BiometricData4));
                param.Add(new SqlParameter("@ClassID", model.ClassID));
                param.Add(new SqlParameter("@GenderID", model.GenderID));
                param.Add(new SqlParameter("@TimeStampOfVerification", model.TimeStampOfVerification));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                param.Add(new SqlParameter("@IsVerifiedByDVV", model.IsVerifiedByDVV));
                param.Add(new SqlParameter("@LocationAddress", model.LocationAddress));
                //param.Add(new SqlParameter("@QualificationHighest", model.QualificationHighest));
                //param.Add(new SqlParameter("@TotalExperience", model.TotalExperience));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_InstructorDVV1", param.ToArray());
                result = true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return result;
        }
    }
}