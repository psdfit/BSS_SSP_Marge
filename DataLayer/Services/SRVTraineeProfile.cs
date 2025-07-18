using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using DataLayer.Models.DVV;
using System.Xml.Linq;

namespace DataLayer.Services
{
    public class SRVTraineeProfile : SRVBase, ISRVTraineeProfile
    {
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVClass srvClass;
        private readonly ISRVReligion srvReligion;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTehsil srvTehsil;
        private readonly ISRVGender srvGender;
        private readonly ISRVTraineeStatus srvTraineeStatus;
        private readonly ISRVSendEmail srvSendEmail;

        public SRVTraineeProfile(ISRVSendEmail srvSendEmail, ISRVOrgConfig srvOrgConfig, ISRVClass srvClass, ISRVReligion srvReligion,
            ISRVDistrict srvDistrict, ISRVTehsil srvTehsil, ISRVGender srvGender, ISRVTraineeStatus srvTraineeStatus)
        {
            this.srvSendEmail = srvSendEmail;
            this.srvOrgConfig = srvOrgConfig;
            this.srvClass = srvClass;
            this.srvReligion = srvReligion;
            this.srvDistrict = srvDistrict;
            this.srvTehsil = srvTehsil;
            this.srvGender = srvGender;
            this.srvTraineeStatus = srvTraineeStatus;
        }

        public TraineeProfileModel GetByTraineeID(int traineeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TraineeID", traineeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTraineeProfile(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw; }
        }

        public TSPMasterModel GetTSPUserAndKAMUserByTraineeID(int traineeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TraineeID", traineeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GET_TSPUserAndKAMUserByTraineeID", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<TSPMasterModel> KAMUser = Helper.ConvertDataTableToModel<TSPMasterModel>(dt);
                    return (KAMUser[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw; }
        }

        public TraineeProfileModel GetTraineeProfileByCNIC(string cnic)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeCNIC", cnic));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTraineeProfile(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TraineeProfileModel> SaveTraineeProfile(TraineeProfileModel traineeProfile)
        {
            string errMsg = string.Empty;
            try
            {
                if (!IsValidCNICFormat(traineeProfile.TraineeCNIC, out errMsg))
                {
                    throw new Exception(errMsg);
                }
                if (!IsValidMobileNoFormat(traineeProfile.ContactNumber1, out errMsg))
                {
                    throw new Exception(errMsg);
                }
                CalculateAgeEligibility(traineeProfile.DateOfBirth.Value, traineeProfile.ClassID, out errMsg);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    throw new Exception(errMsg);
                }
                if (!isEligibleTrainee(traineeProfile, out errMsg))
                {
                    throw new Exception(errMsg);
                }
                string traineeDocPath = null;

                if (traineeProfile.TraineeDoc != null || traineeProfile.TraineeDoc != string.Empty)
                {
                    string path = FilePaths.TRAINEE_DOCUMENTS + traineeProfile.ClassCode + "\\" + traineeProfile.TraineeCode;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string paths = path + "\\";
                    traineeDocPath = Common.AddFile(traineeProfile.TraineeDoc, paths);
                }

                string traineeImagePath = Common.AddFile(traineeProfile.TraineeImg, FilePaths.TRAINEE_PROFILE_DIR);
                string traineeCNICImagePath = !string.IsNullOrEmpty(traineeProfile.CNICImg) ? Common.AddFile(traineeProfile.CNICImg, FilePaths.TRAINEE_PROFILE_DIR) : traineeProfile.CNICImg;
                //save trainee object
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeID", traineeProfile.TraineeID));
                param.Add(new SqlParameter("@TraineeCode", traineeProfile.TraineeCode));
                param.Add(new SqlParameter("@TraineeName", traineeProfile.TraineeName));
                param.Add(new SqlParameter("@TraineeCNIC", traineeProfile.TraineeCNIC));
                param.Add(new SqlParameter("@FatherName", traineeProfile.FatherName));
                param.Add(new SqlParameter("@GenderID", traineeProfile.GenderID));
                param.Add(new SqlParameter("@TradeID", traineeProfile.TradeID));
                param.Add(new SqlParameter("@SectionID", traineeProfile.SectionID));
                param.Add(new SqlParameter("@DateOfBirth", traineeProfile.DateOfBirth));
                param.Add(new SqlParameter("@CNICIssueDate", traineeProfile.CNICIssueDate));
                param.Add(new SqlParameter("@UpdatedDate", traineeProfile.UpdatedDate));
                param.Add(new SqlParameter("@SchemeID", traineeProfile.SchemeID));
                param.Add(new SqlParameter("@TspID", traineeProfile.TSPID));
                param.Add(new SqlParameter("@ClassID", traineeProfile.ClassID));
                param.Add(new SqlParameter("@ClassCode", traineeProfile.ClassCode));
                //param.Add(new SqlParameter("@TemporaryResidence", traineeProfile.TemporaryResidence));
                //param.Add(new SqlParameter("@PermanentResidence", traineeProfile.PermanentResidence));
                param.Add(new SqlParameter("@EducationID", traineeProfile.EducationID));
                param.Add(new SqlParameter("@ContactNumber1", traineeProfile.ContactNumber1));
                //param.Add(new SqlParameter("@ContactNumber2", traineeProfile.ContactNumber2));
                param.Add(new SqlParameter("@CNICVerified", traineeProfile.CNICVerified));
                param.Add(new SqlParameter("@TraineeImg", traineeImagePath));
                param.Add(new SqlParameter("@CNICImg", traineeCNICImagePath));
                param.Add(new SqlParameter("@TraineeDoc", traineeDocPath));
                param.Add(new SqlParameter("@TraineeAge", traineeProfile.TraineeAge));
                param.Add(new SqlParameter("@ReligionID", traineeProfile.ReligionID));
                param.Add(new SqlParameter("@VoucherHolder", traineeProfile.VoucherHolder));
                param.Add(new SqlParameter("@VoucherNumber", traineeProfile.VoucherNumber));
                param.Add(new SqlParameter("@VoucherOrganization", traineeProfile.VoucherOrganization));
                param.Add(new SqlParameter("@WheredidYouHearAboutUs", traineeProfile.WheredidYouHearAboutUs));
                param.Add(new SqlParameter("@TraineeIndividualIncomeID", traineeProfile.TraineeIndividualIncomeID));
                param.Add(new SqlParameter("@HouseHoldIncomeID", traineeProfile.HouseHoldIncomeID));
                param.Add(new SqlParameter("@EmploymentStatusBeforeTrainingID", traineeProfile.EmploymentStatusBeforeTrainingID));
                param.Add(new SqlParameter("@Undertaking", traineeProfile.Undertaking));
                param.Add(new SqlParameter("@GuardianNextToKinName", traineeProfile.GuardianNextToKinName));
                param.Add(new SqlParameter("@GuardianNextToKinContactNo", traineeProfile.GuardianNextToKinContactNo));
                param.Add(new SqlParameter("@TraineeHouseNumber", traineeProfile.TraineeHouseNumber));
                param.Add(new SqlParameter("@TraineeStreetMohalla", traineeProfile.TraineeStreetMohalla));
                param.Add(new SqlParameter("@TraineeMauzaTown", traineeProfile.TraineeMauzaTown));
                param.Add(new SqlParameter("@TraineeDistrictID", traineeProfile.TraineeDistrictID));
                param.Add(new SqlParameter("@TraineeTehsilID", traineeProfile.TraineeTehsilID));
                param.Add(new SqlParameter("@AgeVerified", traineeProfile.AgeVerified));
                param.Add(new SqlParameter("@DistrictVerified", traineeProfile.DistrictVerified));
                //param[51] = new SqlParameter("@TraineeVerified", traineeProfile.TraineeVerified);
                //param.Add(new SqlParameter("@NextToKinName", traineeProfile.NextToKinName));
                //param.Add(new SqlParameter("@NextToKinContactNo", traineeProfile.NextToKinContactNo));
                param.Add(new SqlParameter("@CurUserID", traineeProfile.CurUserID));
                param.Add(new SqlParameter("@IsManual", traineeProfile.IsManual));
                param.Add(new SqlParameter("@IsExtra", traineeProfile.IsExtra));
                param.Add(new SqlParameter("@IsSubmitted", traineeProfile.IsSubmitted));
                //param.Add(new SqlParameter("@CNICImgNADRA", traineeProfile.CNICImgNADRA));
                param.Add(new SqlParameter("@CNICUnVerifiedReason", traineeProfile.CNICUnVerifiedReason));
                param.Add(new SqlParameter("@AgeUnVerifiedReason", traineeProfile.AgeUnVerifiedReason));
                param.Add(new SqlParameter("@ResidenceUnVerifiedReason", traineeProfile.ResidenceUnVerifiedReason));
                param.Add(new SqlParameter("@CNICVerificationDate", traineeProfile.CNICVerificationDate));
                param.Add(new SqlParameter("@ResultStatusID", traineeProfile.ResultStatusID));
                param.Add(new SqlParameter("@IsMigrated", traineeProfile.IsMigrated));
                param.Add(new SqlParameter("@ResultStatusChangeDate", traineeProfile.ResultStatusChangeDate));
                param.Add(new SqlParameter("@ResultStatusChangeReason", traineeProfile.ResultStatusChangeReason));
                param.Add(new SqlParameter("@TraineeDisabilityID", traineeProfile.TraineeDisabilityID));
                param.Add(new SqlParameter("@ReferralSourceID", traineeProfile.ReferralSourceID));
                param.Add(new SqlParameter("@TraineeEmail", traineeProfile.TraineeEmail));
                param.Add(new SqlParameter("@IBANNumber", traineeProfile.IBANNumber));
                param.Add(new SqlParameter("@BankName", traineeProfile.BankName));
                param.Add(new SqlParameter("@Accounttitle", traineeProfile.Accounttitle));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeProfile]", param.ToArray());



            }
            catch (Exception ex)
            {
                throw;
            }


            if (traineeProfile.IsReferredByGuru)
            {
                var trainees = FetchTraineeProfileByClass(traineeProfile.ClassID).FirstOrDefault(t => t.TraineeCNIC == traineeProfile.TraineeCNIC && t.TraineeName == traineeProfile.TraineeName);
                SaveTraineeGuru(traineeProfile, trainees.TraineeID);
            }
            return FetchTraineeProfileByClass(traineeProfile.ClassID);
        }

        public void SaveTraineeGuru(TraineeProfileModel traineeData, int traineeID)
        {

            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@GuruProfileID", traineeData.GuruProfileID));
            param.Add(new SqlParameter("@TraineeID", traineeID));
            param.Add(new SqlParameter("@UserID", traineeData.CurUserID));
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_TraineeGuruProfile", param.ToArray());
        }

        public List<TraineeProfileModel> FetchTraineeProfile(TraineeProfileModel model)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", Common.GetParams(model)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TraineeProfileModel> FetchTraineeProfile()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TraineeProfileModel> FetchTraineeProfileByClass(int classId, bool isSubmitted = false)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@ClassId", classId);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TraineeProfileModel> FetchTraineeDraftDataByTsp(int? TspId)
        {
            try
            {
                List<TraineeProfileModel> trainees = new List<TraineeProfileModel>();

                using (SqlConnection connection = new SqlConnection(SqlHelper.GetCon()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("RD_TraineeProfileByUserId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TspId", TspId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TraineeProfileModel trainee = new TraineeProfileModel
                                {
                                    TraineeID = (int)reader["TraineeID"],
                                    TraineeCode = reader["TraineeCode"].ToString(),
                                    TSPName = reader["TSPName"].ToString(),
                                    EnrollmentEndDate = reader["EnrollmentEndDate"].ToString(),
                                    TraineeName = reader["TraineeName"].ToString(),
                                    TraineeCNIC = reader["TraineeCNIC"].ToString(),
                                    ClassID = (int)reader["ClassID"],
                                    TraineeStatusTypeID = (int)reader["TraineeStatusTypeID"],
                                    ClassCode = reader["ClassCode"].ToString(),
                                    // Add other properties as needed
                                };

                                trainees.Add(trainee);
                            }
                        }
                    }
                }

                return trainees;
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw;
            }
        }

        public List<TraineeProfileModel> FetchTraineeDraftDataByKam(int? KamID)
        {
            try
            {
                List<TraineeProfileModel> trainees = new List<TraineeProfileModel>();

                using (SqlConnection connection = new SqlConnection(SqlHelper.GetCon()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("RD_TraineeProfileByUserId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@KamID", KamID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TraineeProfileModel trainee = new TraineeProfileModel
                                {
                                    TraineeID = (int)reader["TraineeID"],
                                    TraineeCode = reader["TraineeCode"].ToString(),
                                    TSPName = reader["TSPName"].ToString(),
                                    TraineeName = reader["TraineeName"].ToString(),
                                    EnrollmentEndDate = reader["EnrollmentEndDate"].ToString(),
                                    TraineeCNIC = reader["TraineeCNIC"].ToString(),
                                    ClassID = (int)reader["ClassID"],
                                    TraineeStatusTypeID = (int)reader["TraineeStatusTypeID"],
                                    ClassCode = reader["ClassCode"].ToString(),
                                };

                                trainees.Add(trainee);
                            }
                        }
                    }
                }

                return trainees;
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw;
            }
        }

        public List<TraineeProfileDVV> FetchTraineeProfileByClass_DVV(int classId, bool? isSubmitted = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@ClassId", classId);
                param[1] = new SqlParameter("@isSubmitted", isSubmitted);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile_DVV", param).Tables[0];
                return LoopinData_DVV(dt);
            }
            catch (Exception ex) { throw; }
        }

        private List<TraineeProfileDVV> LoopinData_DVV(DataTable dt)
        {
            List<TraineeProfileDVV> TraineeProfileL = new List<TraineeProfileDVV>();
            foreach (DataRow r in dt.Rows)
            {
                TraineeProfileL.Add(RowOfTraineeProfile_DVV(r));
            }
            return TraineeProfileL;
        }

        private TraineeProfileDVV RowOfTraineeProfile_DVV(DataRow row)
        {
            TraineeProfileDVV TraineeProfile = new TraineeProfileDVV();

            TraineeProfile.TraineeID = row.Field<int>("TraineeID");
            TraineeProfile.TraineeCode = row.Field<string>("TraineeCode");
            TraineeProfile.TraineeName = row.Field<string>("TraineeName");
            TraineeProfile.TraineeCNIC = row.Field<string>("TraineeCNIC");
            TraineeProfile.FatherName = row.Field<string>("FatherName");
            TraineeProfile.GenderID = row.Field<int>("GenderID");
            TraineeProfile.GenderName = row.Field<string>("GenderName");
            TraineeProfile.DateOfBirth = row.Field<DateTime>("DateOfBirth");
            TraineeProfile.CNICIssueDate = row.Field<DateTime>("CNICIssueDate");
            TraineeProfile.ClassID = row.Field<int>("ClassID");
            TraineeProfile.TSPID = row.Field<int>("TSPID");
            TraineeProfile.EducationID = row.Field<int>("EducationID");
            TraineeProfile.ReligionID = row.Field<int>("ReligionID");
            TraineeProfile.TraineeStatusTypeID = row.Field<int?>("TraineeStatusTypeID") ?? 0;
            TraineeProfile.BiometricData1 = row.Field<string>("BiometricData1");
            TraineeProfile.BiometricData2 = row.Field<string>("BiometricData2");
            TraineeProfile.BiometricData3 = row.Field<string>("BiometricData3");
            TraineeProfile.BiometricData4 = row.Field<string>("BiometricData4");
            TraineeProfile.Latitude = row.Field<decimal?>("Latitude") ?? 0;
            TraineeProfile.Longitude = row.Field<decimal?>("Longitude") ?? 0;
            TraineeProfile.TimeStampOfVerification = row.Field<DateTime?>("TimeStampOfVerification");
            TraineeProfile.PermanentDistrict = row.Field<int>("PermanentDistrict");
            TraineeProfile.PermanentTehsil = row.Field<int>("PermanentTehsil");
            TraineeProfile.PermanentResidence = row.Field<string>("PermanentResidence");
            TraineeProfile.TemporaryDistrict = row.Field<int>("TemporaryDistrict");
            TraineeProfile.TemporaryTehsil = row.Field<int>("TemporaryTehsil");
            TraineeProfile.TemporaryResidence = row.Field<string>("TemporaryResidence");

            return TraineeProfile;
        }

        public List<TraineeProfileModel> FetchTraineeProfile(bool inActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", new SqlParameter("@InActive", inActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw; }
        }

        public int FetchTraineeProfileNextCodeVal(int classID)
        {
            try
            {
                int TraineeProfile_seqNextVal = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "TraineeProfile_seqNextVal", new SqlParameter("@ClassID", classID)));
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile").Tables[0];
                return TraineeProfile_seqNextVal;
            }
            catch (Exception ex) { throw; }
        }

        public int CalculateAgeEligibility1(DateTime dateOfBirth, int classID, out string errMsg)
        {
            errMsg = string.Empty;
            int years, months, days;
            int age = Age(dateOfBirth, DateTime.Now, out years, out months, out days);
            var classModel = srvClass.GetByClassID(classID);
            var orgConfigModel = srvOrgConfig.GetByClassID(classID);
            if (orgConfigModel == null)
            {
                errMsg = "Class configurations are not found for checking age eligibility.";
                return age;
            }
            //pending
            //validate age on ClassStart and ClassEnd class date
            int ageOnClassStart = Age(dateOfBirth, classModel.StartDate.Value, out years, out months, out days);
            int ageOnClassEnd = Age(dateOfBirth, classModel.EndDate.Value, out years, out months, out days);

            if (!(ageOnClassStart >= orgConfigModel.MinAge && ageOnClassStart <= orgConfigModel.MaxAge)
                && !(ageOnClassEnd >= orgConfigModel.MinAge && ageOnClassEnd <= orgConfigModel.MaxAge))
            {
                if (ageOnClassStart > orgConfigModel.MaxAge)
                {
                    errMsg = $"Unsatisfied upper age limit, Age must be in ({orgConfigModel.MinAge} to {orgConfigModel.MaxAge})";
                    return ageOnClassStart;
                }
                if (ageOnClassEnd < orgConfigModel.MaxAge)
                {
                    errMsg = $"Unsatisfied lower age limit, Age must be in ({orgConfigModel.MinAge} to {orgConfigModel.MaxAge})";
                    return ageOnClassEnd;
                }
            }

            return age;
        }

        public bool IsValidClassByUser(int classID, int userID, out string errMsg)
        {
            bool isValid = false;
            errMsg = "Invalid Class";
            var classes = srvClass.FetchClassesByUser_DVV(new QueryFilters() { ClassID = classID, UserID = userID });
            if (classes.Count > 0)
            {
                isValid = true;
                errMsg = String.Empty;
            }
            return isValid;
        }

        public int CalculateAgeEligibility(DateTime dateOfBirth, int classID, out string errMsg)
        {
            errMsg = string.Empty;

            var classModel = srvClass.GetByClassID(classID);
            var orgConfigModel = srvOrgConfig.GetByClassID(classID);

            int maxAge = orgConfigModel.MaxAge;
            int minAge = orgConfigModel.MinAge;

            string startDateAge = CalculateTraineeAge(dateOfBirth.AddDays(1), classModel.StartDate.Value);
            string[] startDateArray = startDateAge.Split(new string[] { "," }, StringSplitOptions.None);
            int startYears = Convert.ToInt32(startDateArray[0]);
            int startMonths = Convert.ToInt32(startDateArray[1]);
            int startDays = Convert.ToInt32(startDateArray[2]);
            string endDateAge = CalculateTraineeAge(dateOfBirth.AddDays(1), classModel.EndDate.Value);
            string[] endDateArray = endDateAge.Split(new string[] { "," }, StringSplitOptions.None);
            int endYears = Convert.ToInt32(endDateArray[0]);
            int endMonths = Convert.ToInt32(endDateArray[1]);
            int endDays = Convert.ToInt32(endDateArray[2]);
            bool isAgeVerified = true;
            string UnderOverAge = string.Empty;
            int age = startYears;
            if (startYears > maxAge || (startYears == maxAge && (startMonths + startDays) > 0))
            {
                //if(Convert.ToInt32(startDateAge) > maxAge)
                isAgeVerified = false;
                //UnderOverAge = "Overage";
                UnderOverAge = $"Unsatisfied upper age limit, Age must be in ({orgConfigModel.MinAge} to {orgConfigModel.MaxAge})";
                age = startYears;
            }
            else if (startYears < minAge)
            {
                if (endYears < minAge)
                {
                    isAgeVerified = false;
                    //UnderOverAge = "Underage";
                    UnderOverAge = $"Unsatisfied lower age limit, Age must be in ({orgConfigModel.MinAge} to {orgConfigModel.MaxAge})";
                    age = startYears;
                }
                else if (endYears >= minAge)
                {
                    isAgeVerified = true;
                    UnderOverAge = string.Empty;
                    age = endYears;
                }
            }
            //else
            //{
            //    UnderOverAge = "";
            //    age = startYears;
            //}
            errMsg = UnderOverAge;
            return age;
        }

        public bool isEligibleTrainee(TraineeProfileModel model, out string errMsg)
        {
            errMsg = string.Empty;
            bool result = false;
            var org = srvOrgConfig.GetByClassID(model.ClassID);
            if (org == null)
            {
                errMsg = "Not found class's settings in OrgConfig.";
                return result;
            }
            result = !IsAlreadyExistsByCNIC(model.TraineeID, model.TraineeCNIC, model.ClassID, org.DualRegistration.Value, out errMsg);
            if (!result) goto NotEligible;

            if (!org.DualRegistration.Value)
            {
                result = !IsAlreadyExistsByCNICAMS(model.TraineeCNIC, out errMsg);
                if (!result) goto NotEligible;
            }
            result = org.IsCheckBISP ? IsAcceptableByBISP(model.TraineeCNIC, model.ClassID, out errMsg) : true;
            if (!result) goto NotEligible;
            result = org.IsCheckPBTE ? IsAcceptableByPBTE(model.TraineeCNIC, out errMsg) : true;
            if (!result) goto NotEligible;
            NotEligible:
            return result;
        }

        public bool isEligibleTraineeEmail(TraineeProfileModel model, out string errMsg)
        {
            errMsg = string.Empty;
            bool result = false;
            result = !IsAlreadyExistsByEmail(model.TraineeID, model.TraineeEmail, model.ClassID, out errMsg);

            return result;
        }

        public bool IsAlreadyExistsByEmail(int TraineeID, string traineeEmail, int classid, out string errMsg)
        {
            bool isExist = false;
            errMsg = string.Empty;
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeEmail", traineeEmail));
                param.Add(new SqlParameter("@ClassID", classid));

                // DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[dbo].[V_CheckDualEmail]", param.ToArray()).Tables[0];
                DataSet dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "SELECT DBO.CheckDualEmail('" + traineeEmail + "', '"+ classid +"') as Count");
                if (dt.Tables[0].Rows.Count > 0 && Convert.ToInt32(dt.Tables[0].Rows[0]["Count"]) > 0)
                {
                    isExist = true;
                    errMsg = "(Email) is already exist in BSS";
                }
                else
                {
                    isExist = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return isExist;
        }

        public bool IsAlreadyExistsByCNIC(int traineeID, string traineeCNIC, int classId, bool isDual, out string errMsg)
        {
            bool isExist = false;
            errMsg = string.Empty;
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeCNIC", traineeCNIC));
                //// IsDual='false' means that Dual enrolment not allowed in class
                if (isDual)
                {
                    param.Add(new SqlParameter("@ClassID", classId));
                }
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[CheckDualEnrollmentBSS]", param.ToArray()).Tables[0];

                List<TraineeProfileModel> list = LoopinDataDuall(dt);

                ///null when trainee only saved not submitted
                if (!list.Any(x => x.TraineeStatusTypeID == null))
                {
                    ///Excluded Expelled Trainee

                    list = list.Where(x => x.TraineeStatusTypeID != (int)EnumTraineeStatusType.Expelled).ToList();
                }

                if (list.Count > 0)
                {
                    // Found atleast 1 recored in 'Edit' UseCase
                    if (!list.Any(x => x.TraineeID == traineeID))
                    {
                        errMsg = "(CNIC) is already exist" + (isDual ? " in this class !" : " !");
                        isExist = true;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                isExist = true;
            }
            return isExist;
        }

        public bool IsAlreadyExistsByCNICAMS(string traineeCNIC, out string errMsg)
        {
            bool isExist = false;
            errMsg = string.Empty;
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeCNIC", traineeCNIC));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CheckDualEnrollmentAMS", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    int count = dt.Rows[0].Field<int>("Count");
                    isExist = count > 0 ? true : false;
                    errMsg = "(CNIC) is already exist in MIS";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return isExist;
        }

        public bool IsAcceptableByBISP(string traineeCNIC, int classId, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                // Call BISP API with parameter TraineeCNIC
                // API will return an index score that need to be stored in bispIndex variable
                // Accept trainee if index score is between 20 & 40 inclusive, otherwise reject.

                //List<OrgConfigModel> orgConfig = new SRVOrgConfig().FetchOrgConfig(new OrgConfigModel() { ClassID=classId});
                List<OrgConfigModel> orgConfig = srvOrgConfig.FetchOrgConfig(new OrgConfigModel() { ClassID = classId });
                if (orgConfig.Count == 0)
                {
                    errMsg = "OrgConfig not found with classid=" + classId;
                    return false;
                }
                int bispIndex = 30;

                if (bispIndex >= orgConfig[0].BISPIndexFrom && bispIndex <= orgConfig[0].BISPIndexTo)
                {
                    return true; // Accept Trainee
                }
                else
                {
                    errMsg = traineeCNIC + " (CNIC) Trainee rejected because of BISP score!";
                    return false; // Reject Trainee
                }

                //return TraineeProfile_CNICExists;
            }
            catch (Exception ex) { throw; }
        }

        public bool IsAcceptableByPBTE(string traineeCNIC, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                // Call PBTE API with parameter TraineeCNIC
                // API will return a Boolean value that need to be stored in variable TraineeProfile_CNICExists
                // Accept trainee if false is returned, otherwise accept.
                // True means trainee is found in pbte database and reject trainee
                // False means trainee is not found in pbte database so accept trainee

                bool TraineeProfile_CNICExists = false;

                if (!TraineeProfile_CNICExists)
                {
                    return true; //  Accept Trainee
                }
                else
                {
                    errMsg = traineeCNIC + " (CNIC) Trainee rejected because of PBTE!";
                    return false; // Reject Trainee
                }

                //return TraineeProfile_CNICExists;
            }
            catch (Exception ex) { throw; }
        }

        public int BatchInsert(List<TraineeProfileModel> ls, int @BatchFkey, int currentUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", currentUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TraineeProfile]", param);
        }

        public List<TraineeProfileModel> GetByTraineeCNIC(string TraineeCNIC)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", new SqlParameter("@TraineeCNIC", TraineeCNIC)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<TraineeProfileModel> trainee = LoopinData(dt);

                    return trainee;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeProfileModel> GetByGenderID(int genderID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", new SqlParameter("@GenderID", genderID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw; }
        }

        public List<TraineeProfileModel> GetByTradeID(int tradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", new SqlParameter("@TradeID", tradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw; }
        }

        public List<TraineeProfileModel> GetBySectionID(int sectionID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", new SqlParameter("@SectionID", sectionID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw; }
        }

        public List<TraineeProfileModel> GetBySchemeID(int schemeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", new SqlParameter("@SchemeID", schemeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw; }
        }

        public List<TraineeProfileModel> GetByTspID(int tspID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", new SqlParameter("@TspID", tspID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw; }
        }

        public void ActiveInActive(int traineeID, bool? inActive, int curUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TraineeID", traineeID);
            PLead[1] = new SqlParameter("@InActive", inActive);
            PLead[2] = new SqlParameter("@CurUserID", curUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TraineeProfile]", PLead);
        }

        public List<TraineeProfileModel> FetchTraineeProfileByFilters(int[] filters)
        {
            List<TraineeProfileModel> list = new List<TraineeProfileModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                int oID = filters?[3] ?? 0;
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TspId", tspId);
                    param[2] = new SqlParameter("@ClassId", classId);
                    param[3] = new SqlParameter("@IsManual", true);
                    param[4] = new SqlParameter("@OID", oID);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", param).Tables[0];
                    list = LoopinData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        public List<TraineeProfileModel> FetchTraineeProfileByFiltersPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
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
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfileByFiltersPaged", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeProfileModel> FetchTraineeProfileVerificationByFiltersPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
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
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfileVerificationByFiltersPaged", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeProfileModel> FetchTraineesByFilters(int[] filters)
        {
            List<TraineeProfileModel> list = new List<TraineeProfileModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TspId", tspId);
                    param[2] = new SqlParameter("@ClassId", classId);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeProfile", param).Tables[0];
                    list = LoopinData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        public bool UpdateNadraImage(int traineeId, string imagePath)
        {
            bool result = false;
            try
            {
                if (traineeId > 0)
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@TraineeID", traineeId);
                    param[1] = new SqlParameter("@CNICImgNADRA", imagePath);
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[TraineeProfile_UpdateNadraImage]", param);
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public TraineeProfileModel TraineeManualVerification(TraineeProfileModel traineeProfile)
        {
            TraineeProfileModel updatedObj = new TraineeProfileModel();
            try
            {
                if (traineeProfile.TraineeID > 0)
                {
                    SqlParameter[] param = new SqlParameter[11];
                    param[0] = new SqlParameter("@TraineeID", traineeProfile.TraineeID);
                    param[1] = new SqlParameter("@CNICVerified", traineeProfile.CNICVerified);
                    param[2] = new SqlParameter("@AgeVerified", traineeProfile.AgeVerified);
                    param[3] = new SqlParameter("@DistrictVerified", traineeProfile.DistrictVerified);
                    param[4] = new SqlParameter("@TemporaryResidence", traineeProfile.TemporaryResidence);
                    param[5] = new SqlParameter("@PermanentResidence", traineeProfile.PermanentResidence);
                    param[6] = new SqlParameter("@CNICUnVerifiedReason", traineeProfile.CNICUnVerifiedReason);
                    param[7] = new SqlParameter("@AgeUnVerifiedReason", traineeProfile.AgeUnVerifiedReason);
                    param[8] = new SqlParameter("@ResidenceUnVerifiedReason", traineeProfile.ResidenceUnVerifiedReason);
                    param[10] = new SqlParameter("@CurUserID", traineeProfile.CurUserID);
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_TraineeManualVerification]", param);
                    updatedObj = GetByTraineeID(traineeProfile.TraineeID);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return updatedObj;
        }

        public bool UpdateTraineeStatus(int traineeID, int traineeStatusTypeID, int CurUserID, string TraineeReason)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TraineeID", traineeID);
                param[1] = new SqlParameter("@TraineeStatusTypeID", traineeStatusTypeID);
                param[2] = new SqlParameter("@CurUserID", CurUserID);
                param[3] = new SqlParameter("@Reason", TraineeReason);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[UpdateTraineeStatus]", param);

                ApprovalModel approvalModel = new ApprovalModel();
                ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();

                int value = traineeStatusTypeID;
                var enumDisplayStatus = (EnumTraineeStatusType)value;
                string ChangeEnumDisplay = enumDisplayStatus.ToString();
                TraineeProfileModel trainee = this.GetByTraineeID(traineeID);
                TSPMasterModel GET_KamIDAndTspID = this.GetTSPUserAndKAMUserByTraineeID(traineeID);
                string ChangeStatusComments = ",(From OnRoll to " + ChangeEnumDisplay + " " + "of this Trainee Code : " + trainee.TraineeCode + ")";
                approvalModel.CustomComments = ChangeStatusComments;
                approvalModel.ProcessKey = EnumApprovalProcess.TR_STATS_CNGE;
                approvalModel.UserIDs = GET_KamIDAndTspID.UserID.ToString();
                approvalModel.UserIDs += "," + GET_KamIDAndTspID.KAMID.ToString();
                approvalModel.isUserMapping = true;
                srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TraineeProfileModel UpdateTraineeResultStatus(TraineeProfileModel model)
        {
            TraineeProfileModel result = new TraineeProfileModel();
            try
            {
                if (model.TraineeID > 0)
                {
                    model.ResultDocument = !string.IsNullOrEmpty(model.ResultDocument) ? Common.AddFile(model.ResultDocument, FilePaths.TRAINEE_PROFILE_RESULT_DOCUMENT_DIR) : string.Empty;

                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@TraineeID", model.TraineeID);
                    param[1] = new SqlParameter("@ResultStatusID", model.ResultStatusID);
                    param[2] = new SqlParameter("@ResultStatusChangeReason", model.ResultStatusChangeReason);
                    param[3] = new SqlParameter("@ResultDocument", model.ResultDocument);
                    param[4] = new SqlParameter("@CurUserID", model.CurUserID);
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_TraineeProfileResultStatus]", param);
                    result = GetByTraineeID(model.TraineeID);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public List<TraineeProfileModel> FetchTraineesByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineesByUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw; }
        }

        public List<TraineeProfileModel> FetchTraineeHistoryByTraineeID(int TraineeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeHistoryByID", new SqlParameter("@TraineeProfileID", TraineeID)).Tables[0];
                return LoopinTraineeHistoryData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeProfileModel> FetchCRTraineesByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@TraineeID", filters.TraineeID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CR_TraineesByUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw; }
        }

        public List<TraineeProfileModel> FetchTraineesDataByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineesByUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool BatchUpdateExtraTrainees(string json, int currentUserID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Json", json);
                param[2] = new SqlParameter("@CurUserID", currentUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BU_ExtraTrainees]", param);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateTraineeCNICImg(TraineeProfileModel model)
        {
            bool result = false;
            try
            {
                if (model.TraineeID > 0)
                {
                    model.CNICImg = !string.IsNullOrEmpty(model.CNICImg) ? Common.AddFile(model.CNICImg, FilePaths.TRAINEE_PROFILE_CNIC_IMG_DIR) : string.Empty;

                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@TraineeID", model.TraineeID);
                    param[1] = new SqlParameter("@CNICImg", model.CNICImg);
                    param[4] = new SqlParameter("@CurUserID", model.CurUserID);
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_TraineeCNICImg]", param);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public void SaveTraineeProfileDVVResponse(TraineeProfileDVV model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Message", model));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "DVVLog", param.ToArray());
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        public int SaveTraineeProfileDVV(TraineeProfileDVV model, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                //if (!IsValidClassByUser(model.ClassID, model.CurUserID, out errMsg))
                //{
                //    return false;
                //}
                if (!IsValidCNICFormat(model.TraineeCNIC, out errMsg))
                {
                    return 0;
                }
                //Remove Mobile format by Ali Haider 01-07-22
                //if (!IsValidMobileNoFormat(model.MobileNumber, out errMsg))
                //{
                //    return false;
                //}
                int age = CalculateAgeEligibility(model.DateOfBirth, model.ClassID, out errMsg);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    return 0;
                }
                bool isEligible = isEligibleTrainee(new TraineeProfileModel()
                {
                    TraineeID = 0,
                    TraineeCNIC = model.TraineeCNIC,
                    ClassID = model.ClassID
                }, out errMsg);
                if (!isEligible)
                {
                    return 0;
                }

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeID", model.TraineeID));
                param.Add(new SqlParameter("@TraineeName", model.TraineeName));
                param.Add(new SqlParameter("@FatherName", model.FatherName));
                param.Add(new SqlParameter("@TraineeCNIC", model.TraineeCNIC));
                param.Add(new SqlParameter("@CNICIssueDate", model.CNICIssueDate));
                param.Add(new SqlParameter("@DistrictID", model.DistrictID));
                param.Add(new SqlParameter("@DateOfBirth", model.DateOfBirth));
                param.Add(new SqlParameter("@Age", age));
                param.Add(new SqlParameter("@MobileNumber", model.MobileNumber));
                param.Add(new SqlParameter("@Latitude", model.Latitude));
                param.Add(new SqlParameter("@Longitude", model.Longitude));
                param.Add(new SqlParameter("@BiometricData1", model.BiometricData1));
                param.Add(new SqlParameter("@BiometricData2", model.BiometricData2));
                param.Add(new SqlParameter("@BiometricData3", model.BiometricData3));
                param.Add(new SqlParameter("@BiometricData4", model.BiometricData4));
                //param.Add(new SqlParameter("@BiometricData1", model.BiometricData1));
                //param.Add(new SqlParameter("@BiometricData2", model.BiometricData2));
                //param.Add(new SqlParameter("@BiometricData3", model.BiometricData3));
                //param.Add(new SqlParameter("@BiometricData4", model.BiometricData4));
                param.Add(new SqlParameter("@ClassID", model.ClassID));
                param.Add(new SqlParameter("@GenderID", model.GenderID));
                param.Add(new SqlParameter("@EducationID", model.EducationID));
                param.Add(new SqlParameter("@ReligionID", model.ReligionID));
                param.Add(new SqlParameter("@TimeStampOfVerification", model.TimeStampOfVerification));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                param.Add(new SqlParameter("@IsSubmitted", false));
                param.Add(new SqlParameter("@PermanentDistrict", model.PermanentDistrict));
                param.Add(new SqlParameter("@PermanentTehsil", model.PermanentTehsil));
                param.Add(new SqlParameter("@PermanentResidence", model.PermanentResidence));
                param.Add(new SqlParameter("@TemporaryDistrict", model.TemporaryDistrict));
                param.Add(new SqlParameter("@TemporaryTehsil", model.TemporaryTehsil));
                param.Add(new SqlParameter("@TemporaryResidence", model.TemporaryResidence));
                DataSet dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_TraineeProfileDVV", param.ToArray());

                if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                {
                    int TraineeID = Convert.ToInt32(dt.Tables[0].Rows[0]["TraineeID"]);
                    return TraineeID;
                }

                return 0;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return 0;
            }
        }

        public bool SaveTraineeAttendance(TraineeAttendanceDVV model, out string errMsg)
        {
            errMsg = string.Empty;
            bool result = false;
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeID", model.TraineeID));
                param.Add(new SqlParameter("@CheckIn", model.CheckIn));
                param.Add(new SqlParameter("@CheckOut", model.CheckOut));
                param.Add(new SqlParameter("@Latitude", model.Latitude));
                param.Add(new SqlParameter("@Longitude", model.Longitude));
                param.Add(new SqlParameter("@TimeStamp", model.TimeStamp));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                param.Add(new SqlParameter("@BiometricData", model.BiometricData1));
                param.Add(new SqlParameter("@TSPId", model.TSPId));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "A_TraineeAttendance", param.ToArray());
                result = true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return result;
        }

        public bool SaveTraineeAcknowledgement(TraineeAcknowledgementDVV model, out string errMsg)
        {
            errMsg = string.Empty;
            bool result = false;
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@AknowledgementTypeID", model.AknowledgementTypeID));
                param.Add(new SqlParameter("@TraineeID", model.TraineeID));
                param.Add(new SqlParameter("@Latitude", model.Latitude));
                param.Add(new SqlParameter("@Longitude", model.Longitude));
                param.Add(new SqlParameter("@TimeStamp", model.TimeStamp));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                param.Add(new SqlParameter("@TSPId", model.TSPId));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "A_TraineeAcknowledgement", param.ToArray());
                result = true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return result;
        }

        public List<CNICStatusModel> GetTraineesFromFile(List<CNICStatusModel> cnic)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(cnic));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Get_Trainees_From_File]", param).Tables[0];
                return LoopinTraineeCNICData(dt);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public void DeleteDraftTrainee(int traineeID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeID", traineeID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "DeleteDraftTrainee", param.ToArray());
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        #region Private Methods

        private TraineeProfileModel RowOfTraineeProfile(DataRow row)
        {
            TraineeProfileModel TraineeProfile = new TraineeProfileModel();

            if (row.Table.Columns.Contains("PermanentResidence"))
            {
                TraineeProfile.TraineeImg = string.IsNullOrEmpty(row["TraineeImg"].ToString()) ? string.Empty : Common.GetFileBase64(row["TraineeImg"].ToString());
            }
            //GetFiles by Path

            //TraineeProfile.TraineeImg = string.IsNullOrEmpty(row.Field<string>("TraineeImg")) ? string.Empty : Common.GetFileBase64(row["TraineeImg"].ToString());
            //TraineeProfile.CNICImg = string.IsNullOrEmpty(row.Field<string>("CNICImg")) ? string.Empty : Common.GetFileBase64(row.Field<string>("CNICImg"));
            TraineeProfile.CNICImgNADRA = string.IsNullOrEmpty(row.Field<string>("CNICImgNADRA")) ? string.Empty : Common.GetFileBase64(row.Field<string>("CNICImgNADRA"));
            TraineeProfile.TraineeDoc = string.IsNullOrEmpty(row.Field<string>("TraineeDoc")) ? string.Empty : Common.GetFileBase64(row["TraineeDoc"].ToString());
            TraineeProfile.TraineeDocURL = string.IsNullOrEmpty(row.Field<string>("TraineeDoc")) ? string.Empty : row["TraineeDoc"].ToString();

            TraineeProfile.TraineeID = row.Field<int>("TraineeID");
            TraineeProfile.TraineeCode = row.Field<string>("TraineeCode");
            TraineeProfile.TraineeName = row.Field<string>("TraineeName");
            TraineeProfile.TraineeCNIC = row.Field<string>("TraineeCNIC");
            TraineeProfile.FatherName = row.Field<string>("FatherName");
            TraineeProfile.GenderID = row.Field<int>("GenderID");
            TraineeProfile.GenderName = row.Field<string>("GenderName");
            TraineeProfile.TradeID = row.Field<int>("TradeID");
            TraineeProfile.TradeName = row.Field<string>("TradeName");
            TraineeProfile.SectionID = row.Field<int?>("SectionID");
            TraineeProfile.SectionName = row.Field<string>("SectionName");
            TraineeProfile.SchemeCode = row.Field<string>("SchemeCode");
            TraineeProfile.DateOfBirth = row.Field<DateTime?>("DateOfBirth");
            TraineeProfile.CNICIssueDate = row.Field<DateTime?>("CNICIssueDate");
            TraineeProfile.UpdatedDate = row.Field<DateTime?>("UpdatedDate");
            TraineeProfile.SchemeID = row.Field<int>("SchemeID");
            TraineeProfile.SchemeName = row.Field<string>("SchemeName");
            TraineeProfile.TSPID = row.Field<int>("TspID");
            TraineeProfile.TSPName = row.Field<string>("TSPName");
            TraineeProfile.ClassID = row.Field<int>("ClassID");
            TraineeProfile.ClassCode = row.Field<string>("ClassCode");
            TraineeProfile.TraineeEmail = row.Field<string>("TraineeEmail");
            //TraineeProfile.TemporaryResidence = row.Field<string>("TemporaryResidence");
            if (row.Table.Columns.Contains("PermanentResidence"))
            {
                TraineeProfile.PermanentResidence = row.Field<string>("PermanentResidence");
            }
            if (row.Table.Columns.Contains("PermanentDistrictName"))
            {
                TraineeProfile.PermanentDistrictName = row.Field<string>("PermanentDistrictName");
            }
            if (row.Table.Columns.Contains("PermanentTehsilName"))
            {
                TraineeProfile.PermanentTehsilName = row.Field<string>("PermanentTehsilName");
            }
            if (row.Table.Columns.Contains("TemporaryResidence"))
            {
                TraineeProfile.TemporaryResidence = row.Field<string>("TemporaryResidence");
            }
            if (row.Table.Columns.Contains("TemporaryDistrict"))
            {
                TraineeProfile.TemporaryDistrict = row.Field<int?>("TemporaryDistrict") ?? 0; ;
            }
            if (row.Table.Columns.Contains("TemporaryTehsil"))
            {
                TraineeProfile.TemporaryTehsil = row.Field<int?>("TemporaryTehsil") ?? 0; ;
            }

            TraineeProfile.EducationID = row.Field<int?>("EducationID");
            TraineeProfile.ContactNumber1 = row.Field<string>("ContactNumber1");
            //TraineeProfile.ContactNumber2 = row.Field<string>("ContactNumber2");
            TraineeProfile.CNICVerified = row.Field<bool>("CNICVerified");
            TraineeProfile.TraineeAge = row.Field<int>("TraineeAge");
            TraineeProfile.ReligionID = row.Field<int?>("ReligionID");
            TraineeProfile.VoucherHolder = row.Field<bool>("VoucherHolder");
            TraineeProfile.VoucherNumber = row.Field<string>("VoucherNumber");
            TraineeProfile.VoucherOrganization = row.Field<string>("VoucherOrganization");
            TraineeProfile.WheredidYouHearAboutUs = row.Field<string>("WheredidYouHearAboutUs");
            TraineeProfile.TraineeIndividualIncomeID = row.Field<int?>("TraineeIndividualIncomeID");
            TraineeProfile.HouseHoldIncomeID = row.Field<int?>("HouseHoldIncomeID");
            TraineeProfile.EmploymentStatusBeforeTrainingID = row.Field<int?>("EmploymentStatusBeforeTrainingID");
            TraineeProfile.Undertaking = row.Field<bool>("Undertaking");
            TraineeProfile.GuardianNextToKinName = row.Field<string>("GuardianNextToKinName");
            TraineeProfile.GuardianNextToKinContactNo = row.Field<string>("GuardianNextToKinContactNo");
            TraineeProfile.TraineeHouseNumber = row.Field<string>("TraineeHouseNumber");
            TraineeProfile.TraineeStreetMohalla = row.Field<string>("TraineeStreetMohalla");
            TraineeProfile.TraineeMauzaTown = row.Field<string>("TraineeMauzaTown");
            TraineeProfile.AgeVerified = row.Field<bool>("AgeVerified");
            TraineeProfile.DistrictVerified = row.Field<bool>("DistrictVerified");
            TraineeProfile.TraineeVerified = row.Field<int>("TraineeVerified") == 1 ? true : false;
            //TraineeProfile.NextToKinName = row.Field<string>("NextToKinName");
            //TraineeProfile.NextToKinContactNo = row.Field<string>("NextToKinContactNo");
            TraineeProfile.CreatedDate = row.Field<DateTime?>("CreatedDate");
            TraineeProfile.CreatedUserID = row.Field<int>("CreatedUserID");
            TraineeProfile.IsSubmitted = row.Field<bool>("IsSubmitted");
            TraineeProfile.CNICUnVerifiedReason = row.Field<string>("CNICUnVerifiedReason");
            TraineeProfile.AgeUnVerifiedReason = row.Field<string>("AgeUnVerifiedReason");
            TraineeProfile.ResidenceUnVerifiedReason = row.Field<string>("ResidenceUnVerifiedReason");
            TraineeProfile.CNICVerificationDate = row.Field<DateTime?>("CNICVerificationDate");
            TraineeProfile.ResultStatusID = row.Field<int?>("ResultStatusID") ?? 0;
            TraineeProfile.UID = row.Field<string>("UID");
            TraineeProfile.IsMigrated = row.Field<bool>("IsMigrated");
            //TraineeProfile.TraineeStatusChangeDate = row.Field<DateTime?>("TraineeStatusChangeDate");
            if (row.Table.Columns.Contains("TraineeStatusTypeID"))
            {
                TraineeProfile.TraineeStatusTypeID = row.Field<int?>("TraineeStatusTypeID") ?? 0;
            }

            //TraineeProfile.TraineeStatusTypeID = row.Field<int?>("TraineeStatusTypeID") ?? 0;
            //TraineeProfile.TraineeStatusChangeReason = row["TraineeStatusChangeReason"].ToString();
            TraineeProfile.StatusName = row.Field<string>("StatusName");
            TraineeProfile.ResultStatusChangeDate = row.Field<DateTime?>("ResultStatusChangeDate");
            TraineeProfile.ResultStatusChangeReason = row.Field<string>("ResultStatusChangeReason");
            TraineeProfile.ResultDocument = row.Field<string>("ResultDocument");
            TraineeProfile.ResultDocument = !string.IsNullOrEmpty(TraineeProfile.ResultDocument) ? Common.GetFileBase64(TraineeProfile.ResultDocument) : string.Empty;

            TraineeProfile.ModifiedUserID = row.Field<int?>("ModifiedUserID") ?? 0;
            TraineeProfile.ModifiedDate = row.Field<DateTime?>("ModifiedDate");
            TraineeProfile.InActive = row.Field<bool>("InActive");
            TraineeProfile.IsManual = row.Field<bool>("IsManual");

            if (TraineeProfile.IsManual == false)
            {
                TraineeProfile.TraineeDistrictID = row.Field<int?>("TraineeDistrictID") ?? 0;
                TraineeProfile.TraineeTehsilID = row.Field<int?>("TraineeTehsilID") ?? 0;
            }
            else
            {
                TraineeProfile.TraineeDistrictID = row.Field<int?>("TraineeDistrictID");
                TraineeProfile.TraineeTehsilID = row.Field<int?>("TraineeTehsilID");
            }

            TraineeProfile.IsExtra = row.Field<bool>("IsExtra");
            TraineeProfile.TraineeDisabilityID = row.Field<int?>("TraineeDisabilityID");
            TraineeProfile.ReferralSourceID = row.Field<int?>("ReferralSourceID");
            TraineeProfile.DistrictName = row.Field<string>("DistrictName");
            TraineeProfile.DistrictNameUrdu = row.Field<string>("DistrictNameUrdu");
            if (row.Table.Columns.Contains("ProvinceID"))
            {
                TraineeProfile.ProvinceID = row.Field<int?>("ProvinceID") ?? 0;
            }
            if (row.Table.Columns.Contains("ProvinceName"))
            {
                TraineeProfile.ProvinceName = row.Field<string>("ProvinceName");
            }
            //TraineeProfile.ProvinceNameUrdu = row.Field<string>("ProvinceNameUrdu");
            TraineeProfile.TehsilName = row.Field<string>("TehsilName");
            TraineeProfile.TrainingAddressLocation = row.Field<string>("TrainingAddressLocation");
            if (row.Table.Columns.Contains("ClassBatch"))
            {
                TraineeProfile.ClassBatch = row.Field<int>("ClassBatch");
            }
            if (row.Table.Columns.Contains("VerificationStatus"))
            {
                TraineeProfile.VerificationStatus = row.Field<string>("VerificationStatus");
            }
            if (row.Table.Columns.Contains("TraineeCrID"))
            {
                TraineeProfile.TraineeCrID = row.Field<int>("TraineeCrID");
            }
            if (row.Table.Columns.Contains("TraineeCrIsApproved"))
            {
                TraineeProfile.TraineeCrIsApproved = row.Field<bool>("TraineeCrIsApproved");
            }
            if (row.Table.Columns.Contains("TraineeCrIsRejected"))
            {
                TraineeProfile.TraineeCrIsRejected = row.Field<bool>("TraineeCrIsRejected");
            }
            if (row.Table.Columns.Contains("Accounttitle"))
            {
                TraineeProfile.Accounttitle = row.Field<string>("Accounttitle");
            }
            if (row.Table.Columns.Contains("BankName"))
            {
                TraineeProfile.BankName = row.Field<string>("BankName");
            }
            if (row.Table.Columns.Contains("IBANNumber"))
            {
                TraineeProfile.IBANNumber = row.Field<string>("IBANNumber");
            }
            return TraineeProfile;
        }

        private TraineeProfileModel RowOfTraineeProfileHistory(DataRow row)
        {
            TraineeProfileModel TraineeProfile = new TraineeProfileModel();

            //GetFiles by Path

            TraineeProfile.TraineeID = row.Field<int>("TraineeID");
            TraineeProfile.TraineeCode = row.Field<string>("TraineeCode");
            TraineeProfile.TraineeName = row.Field<string>("TraineeName");
            TraineeProfile.TraineeCNIC = row.Field<string>("TraineeCNIC");
            TraineeProfile.FatherName = row.Field<string>("FatherName");
            TraineeProfile.GenderID = row.Field<int>("GenderID");
            TraineeProfile.TradeID = row.Field<int>("TradeID");
            TraineeProfile.SectionID = row.Field<int?>("SectionID");
            TraineeProfile.DateOfBirth = row.Field<DateTime?>("DateOfBirth");
            TraineeProfile.CNICIssueDate = row.Field<DateTime?>("CNICIssueDate");
            TraineeProfile.UpdatedDate = row.Field<DateTime?>("UpdatedDate");
            TraineeProfile.SchemeID = row.Field<int>("SchemeID");
            TraineeProfile.TSPID = row.Field<int>("TspID");
            TraineeProfile.ClassID = row.Field<int>("ClassID");

            TraineeProfile.ContactNumber1 = row.Field<string>("ContactNumber1");
            //TraineeProfile.ContactNumber2 = row.Field<string>("ContactNumber2");
            TraineeProfile.CNICVerified = row.Field<bool>("CNICVerified");
            TraineeProfile.TraineeAge = row.Field<int>("TraineeAge");
            TraineeProfile.ReligionID = row.Field<int>("ReligionID");
            TraineeProfile.VoucherHolder = row.Field<bool>("VoucherHolder");
            TraineeProfile.VoucherNumber = row.Field<string>("VoucherNumber");
            TraineeProfile.VoucherOrganization = row.Field<string>("VoucherOrganization");
            TraineeProfile.WheredidYouHearAboutUs = row.Field<string>("WheredidYouHearAboutUs");
            TraineeProfile.TraineeIndividualIncomeID = row.Field<int?>("TraineeIndividualIncomeID") ?? 0;
            TraineeProfile.HouseHoldIncomeID = row.Field<int?>("HouseHoldIncomeID") ?? 0;
            TraineeProfile.EmploymentStatusBeforeTrainingID = row.Field<int>("EmploymentStatusBeforeTrainingID");
            TraineeProfile.Undertaking = row.Field<bool>("Undertaking");
            TraineeProfile.GuardianNextToKinName = row.Field<string>("GuardianNextToKinName");
            TraineeProfile.GuardianNextToKinContactNo = row.Field<string>("GuardianNextToKinContactNo");
            TraineeProfile.TraineeHouseNumber = row.Field<string>("TraineeHouseNumber");
            TraineeProfile.TraineeStreetMohalla = row.Field<string>("TraineeStreetMohalla");
            TraineeProfile.TraineeMauzaTown = row.Field<string>("TraineeMauzaTown");
            TraineeProfile.TraineeDistrictID = row.Field<int>("TraineeDistrictID");
            TraineeProfile.TraineeTehsilID = row.Field<int?>("TraineeTehsilID");
            TraineeProfile.AgeVerified = row.Field<bool>("AgeVerified");
            TraineeProfile.DistrictVerified = row.Field<bool>("DistrictVerified");
            TraineeProfile.TraineeVerified = row.Field<int>("TraineeVerified") == 1 ? true : false;

            TraineeProfile.CreatedDate = row.Field<DateTime?>("CreatedDate");
            TraineeProfile.CreatedUserID = row.Field<int>("CreatedUserID");
            TraineeProfile.IsSubmitted = row.Field<bool>("IsSubmitted");
            TraineeProfile.CNICUnVerifiedReason = row.Field<string>("CNICUnVerifiedReason");
            TraineeProfile.AgeUnVerifiedReason = row.Field<string>("AgeUnVerifiedReason");
            TraineeProfile.ResidenceUnVerifiedReason = row.Field<string>("ResidenceUnVerifiedReason");
            TraineeProfile.CNICVerificationDate = row.Field<DateTime?>("CNICVerificationDate");
            TraineeProfile.ResultStatusID = row.Field<int?>("ResultStatusID") ?? 0;
            TraineeProfile.UID = row.Field<string>("UID");
            TraineeProfile.IsMigrated = row.Field<bool>("IsMigrated");

            TraineeProfile.ModifiedUserID = row.Field<int?>("ModifiedUserID") ?? 0;
            TraineeProfile.ModifiedDate = row.Field<DateTime?>("ModifiedDate");
            TraineeProfile.InActive = row.Field<bool>("InActive");

            return TraineeProfile;
        }

        private TraineeProfileModel RowOfTraineeProfileDuall(DataRow row)
        {
            TraineeProfileModel TraineeProfile = new TraineeProfileModel();
            TraineeProfile.TraineeCNIC = row.Field<string>("TraineeCNIC");
            TraineeProfile.ClassID = row.Field<int>("ClassID");
            TraineeProfile.TraineeID = row.Field<int>("TraineeID");
            TraineeProfile.TraineeStatusTypeID = row.Field<int?>("TraineeStatusTypeID");
            return TraineeProfile;
        }

        private CNICStatusModel RowOfTraineeProfileCNIC(DataRow row)
        {
            CNICStatusModel TraineeProfile = new CNICStatusModel();

            TraineeProfile.TraineeCNIC = row.Field<string>("TraineeCNIC");
            TraineeProfile.CNICStatus = row.Field<string>("CNICStatus");

            return TraineeProfile;
        }

        private List<TraineeProfileModel> LoopinDataDuall(DataTable dt)
        {
            List<TraineeProfileModel> TraineeProfileL = new List<TraineeProfileModel>();
            foreach (DataRow r in dt.Rows)
            {
                TraineeProfileL.Add(RowOfTraineeProfileDuall(r));
            }
            return TraineeProfileL;
        }

        private List<TraineeProfileModel> LoopinData(DataTable dt)
        {
            List<TraineeProfileModel> TraineeProfileL = new List<TraineeProfileModel>();
            foreach (DataRow r in dt.Rows)
            {
                TraineeProfileL.Add(RowOfTraineeProfile(r));
            }
            return TraineeProfileL;
        }

        private List<CNICStatusModel> LoopinTraineeCNICData(DataTable dt)
        {
            List<CNICStatusModel> TraineeProfileL = new List<CNICStatusModel>();
            foreach (DataRow r in dt.Rows)
            {
                TraineeProfileL.Add(RowOfTraineeProfileCNIC(r));
            }
            return TraineeProfileL;
        }

        private List<TraineeProfileModel> LoopinTraineeHistoryData(DataTable dt)
        {
            List<TraineeProfileModel> TraineeProfileL = new List<TraineeProfileModel>();
            foreach (DataRow r in dt.Rows)
            {
                TraineeProfileL.Add(RowOfTraineeProfileHistory(r));
            }
            return TraineeProfileL;
        }

        private int YearDifference(DateTime minDate, DateTime maxDate)
        {
            int year = 0;
            //TimeSpan span = maxDate.Subtract(minDate);
            //year = (int)Math.Ceiling(Convert.ToDouble(span.TotalDays / 365));

            //year = span.TotalDays % 365 > 0 ? year + 1 : year;

            //DateTime Now = DateTime.Now;
            //int Years = new DateTime(maxDate.Subtract(minDate).Ticks).Year - 1;
            //DateTime PastYearDate = minDate.AddYears(Years);
            //int Months = 0;
            //for (int i = 1; i <= 12; i++)
            //{
            //    if (PastYearDate.AddMonths(i) == Now)
            //    {
            //        Months = i;
            //        break;
            //    }
            //    else if (PastYearDate.AddMonths(i) >= Now)
            //    {
            //        Months = i - 1;
            //        break;
            //    }
            //}
            //int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            //year = Months + Days > 0 ? Years + 1 : Years;

            //DateTime newDate =new DateTime( maxDate.Subtract(minDate).Ticks);
            // year = newDate.Year;

            year = maxDate.Year - minDate.Year;
            //if (minDate.Date > maxDate.AddYears(-year)) year--;

            if (maxDate.DayOfYear < minDate.DayOfYear)
            {
                year = year - 1;
            }
            if (maxDate.DayOfYear > minDate.DayOfYear)
            {
                year = year + 1;
            }

            return year;
        }

        private int Age(DateTime Bday, DateTime Cday, out int years, out int months, out int days)
        {
            if ((Cday.Year - Bday.Year) > 0 ||
        (((Cday.Year - Bday.Year) == 0) && ((Bday.Month < Cday.Month) ||
          ((Bday.Month == Cday.Month) && (Bday.Day <= Cday.Day)))))
            {
                int DaysInBdayMonth = DateTime.DaysInMonth(Bday.Year, Bday.Month);
                int DaysRemain = Cday.Day + (DaysInBdayMonth - Bday.Day);

                if (Cday.Month > Bday.Month)
                {
                    years = Cday.Year - Bday.Year;
                    months = Cday.Month - (Bday.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
                else if (Cday.Month == Bday.Month)
                {
                    if (Cday.Day >= Bday.Day)
                    {
                        years = Cday.Year - Bday.Year;
                        months = 0;
                        days = Cday.Day - Bday.Day;
                    }
                    else
                    {
                        years = (Cday.Year - 1) - Bday.Year;
                        months = 11;
                        days = DateTime.DaysInMonth(Bday.Year, Bday.Month) - (Bday.Day - Cday.Day);
                    }
                }
                else
                {
                    years = (Cday.Year - 1) - Bday.Year;
                    months = Cday.Month + (11 - Bday.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
            }
            else
            {
                throw new ArgumentException("Birthday date must be earlier than current month.");
            }

            return months + days > 0 ? years + 1 : years;
        }

        private string CalculateTraineeAge(DateTime Dob, DateTime paramDate)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(paramDate.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == paramDate)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= paramDate)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = paramDate.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = paramDate.Subtract(PastYearDate).Hours;
            int Minutes = paramDate.Subtract(PastYearDate).Minutes;
            int Seconds = paramDate.Subtract(PastYearDate).Seconds;
            return String.Format("{0},{1},{2}",
            Years, Months, Days);
            // return String.Format("{0}",
            //Years);
        }

        public bool NadraVerysisExcel(int TraineeID, string CNICNUmber, string traineeName, string fatherNAME, DateTime dOB,
            string gender, bool cNICVerified, string cNICUnVerifiedReason, int intDistrictVerified, bool isAgeVerified,
            string underOverAge, string disrictUnverifiedReason,
            string ResidenceDistrict, string ResidenceTehsil, string TraineeReligion,
            string permanentAddress, string permanentDistrict, string permanentTehsil)
        {
            try
            {
                TraineeProfileModel trainee = this.GetByTraineeID(TraineeID);
                List<GenderModel> genderModel = srvGender.FetchGender();
                List<DistrictModel> districtModel = srvDistrict.FetchDistrict();
                List<ReligionModel> religionModel = srvReligion.FetchReligion();
                int districtID = 0; int tehsilID = 0; int religionID = 0; int genderID = 0;
                int permanentDistrictID = 0; int permanentTehsilID = 0;
                foreach (GenderModel gen in genderModel)
                {
                    if (gen.GenderName.Trim().ToLower() == gender.Trim().ToLower())
                    {
                        genderID = gen.GenderID;
                        TraineeProfileModel traineeProfileModel = this.GetByTraineeID(TraineeID);
                        ClassModel classModel = srvClass.GetByClassID(traineeProfileModel.ClassID);
                        int classGenderID = classModel.GenderID;
                        bool isValidGender = true;
                        if (classGenderID == 5 && genderID != 5) //female
                        {
                            isValidGender = false;
                        }
                        else if (classGenderID == 3 && genderID != 3)//Male
                        {
                            isValidGender = false;
                        }
                        else if (classGenderID == 6 && genderID != 6)//Transgender
                        {
                            isValidGender = false;
                        }
                        else if (classGenderID == 7 && (genderID != 5 && genderID != 3))//Both
                        {
                            isValidGender = false;
                        }
                        else if (classGenderID == 8 && (genderID != 5 && genderID != 3 && genderID != 6))//All Genders
                        {
                            isValidGender = false;
                        }
                        if (!isValidGender)
                        {
                            srvTraineeStatus.SaveTraineeStatus(new TraineeStatusModel()
                            {
                                TraineeStatusID = 0,
                                TraineeProfileID = TraineeID,
                                TraineeStatusTypeID = (int)EnumTraineeStatusType.Expelled,
                                CreatedUserID = (int)EnumUsers.System,
                                Comments = "Incorrect /Opposite Gender"
                            });
                        }
                    }
                }
                if (genderID == 0) { return false; }

                foreach (ReligionModel religion in religionModel)
                {
                    if (religion.ReligionName.Trim().ToLower() == TraineeReligion.Trim().ToLower())
                    {
                        religionID = religion.ReligionID;
                    }
                }
                // if (religionID == 0) { return false; }

                foreach (DistrictModel model in districtModel)
                {
                    if (model.DistrictName.Trim().ToLower() == ResidenceDistrict.Trim().ToLower())
                    {
                        districtID = model.DistrictID;
                        permanentDistrictID = model.DistrictID;
                        List<TehsilModel> tehsilModel = srvTehsil.GetByDistrictID(model.DistrictID);
                        foreach (TehsilModel model1 in tehsilModel)
                        {
                            if (model1.TehsilName.Trim().ToLower() == ResidenceTehsil.Trim().ToLower())
                            {
                                tehsilID = model1.TehsilID;
                                permanentTehsilID = model1.TehsilID;
                            }
                        }
                    }
                }
                //if (districtID == 0 || tehsilID == 0) { return false; }

                SqlParameter[] param = new SqlParameter[20];
                param[0] = new SqlParameter("@TraineeID", TraineeID);
                param[1] = new SqlParameter("@CNICVerified", cNICVerified);
                param[2] = new SqlParameter("@AgeVerified", isAgeVerified);
                param[3] = new SqlParameter("@DistrictVerified", intDistrictVerified);
                param[4] = new SqlParameter("@CNICUnVerifiedReason", cNICUnVerifiedReason);
                param[5] = new SqlParameter("@TraineeName", traineeName);
                param[6] = new SqlParameter("@FatherName", fatherNAME);
                param[7] = new SqlParameter("@AgeUnverifiedReason", underOverAge);
                param[8] = new SqlParameter("@DisrictUnverifiedReason", disrictUnverifiedReason);
                param[9] = new SqlParameter("@DOB", dOB);
                param[10] = new SqlParameter("@GenderID", genderID);
                //param[11] = new SqlParameter("@DistrictID", districtID);
                //param[12] = new SqlParameter("@TehsilID", tehsilID);
                param[11] = new SqlParameter("@ReligionID", religionID);
                param[12] = new SqlParameter("@PermanentAddress", permanentAddress);
                param[13] = new SqlParameter("@permanentDistrictID", permanentDistrictID);
                param[14] = new SqlParameter("@permanentTehsilID", permanentTehsilID);
                param[15] = new SqlParameter("@CurUserID", (int)EnumUsers.System);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_TraineeBulkExcelVerification]", param);
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion Private Methods

        public bool IsValidCNICFormat(string cnic, out string errMsg)
        {
            Regex regex = new Regex(@"^[0-9]{5}-[0-9]{7}-[0-9]{1}$");
            bool isValid = regex.IsMatch(cnic);
            errMsg = isValid ? string.Empty : $"Invalid CNIC Format, it should be xxxxx-xxxxxxx-x";
            return isValid;
        }

        public bool IsValidMobileNoFormat(string mobileNo, out string errMsg)
        {
            Regex regex = new Regex(@"^[0-9]{4}-[0-9]{7}$");
            bool isValid = regex.IsMatch(mobileNo);
            errMsg = isValid ? string.Empty : $"Invalid Phone number Format, it should be xxxx-xxxxxxx";
            return isValid;
        }

        public List<TraineeProfileModel> FetchTraineesByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
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
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineesByUserPaged", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;

                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeProfileModel> FetchCRTraineesByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));
                param.Add(new SqlParameter("@OID", filterModel.OID));
                param.AddRange(Common.GetPagingParams(pagingModel));
                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CR_TraineesByUserPaged", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinData(dt);
            }
            catch (Exception ex) { throw; }
        }
        public List<TraineeProfileModel> FetchSubmittedTraineeDataByTsp(int? TraineeFilter, int? programid, int? districtid, int? tradeid, int UserID, int? traininglocationid)
        {

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeFilter", TraineeFilter));
                param.Add(new SqlParameter("@ProgramID", programid));
                param.Add(new SqlParameter("@Districtid", districtid));
                param.Add(new SqlParameter("@TradeID", tradeid));
                param.Add(new SqlParameter("@TrainingLocationID", traininglocationid));
                param.Add(new SqlParameter("@TspId", UserID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPSubmittedTrainee", param.ToArray()).Tables[0];
                if (dt != null && dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return LoopinSubmittedTrainees(dt);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private List<TraineeProfileModel> LoopinSubmittedTrainees(DataTable dt)
        {
            List<TraineeProfileModel> TraineeProfileL = new List<TraineeProfileModel>();
            foreach (DataRow r in dt.Rows)
            {
                TraineeProfileL.Add(RowOfTraineeSubmitted(r));
            }
            return TraineeProfileL;
        }


        private TraineeProfileModel RowOfTraineeSubmitted(DataRow row)
        {
            TraineeProfileModel TraineeProfile = new TraineeProfileModel();

            TraineeProfile.TraineeID = row.Field<int>("TraineeID");
            TraineeProfile.TraineeName = row.Field<string>("FullName");
            TraineeProfile.TraineeCNIC = row.Field<string>("CNIC");
            TraineeProfile.FatherName = row.Field<string>("FatherName");
            TraineeProfile.GenderName = row.Field<string>("GenderName");
            TraineeProfile.ReligionName = row.Field<string>("ReligionName");
            TraineeProfile.DateOfBirth = row.Field<DateTime?>("DateOfBirth");
            TraineeProfile.TraineeEmail = row.Field<string>("Email");
            TraineeProfile.ContactNumber1 = row.Field<string>("MobileNumber");
            //TraineeProfile.IsSubmitted = row.Field<bool>("IsSubmitted");
            TraineeProfile.DistrictName = row.Field<string>("TraineeDistrict");
            TraineeProfile.TrainingAddressLocation = row.Field<string>("CurrentAddress");
            TraineeProfile.Disability = row.Field<string>("Disability");

            return TraineeProfile;
        }

        public List<CheckRegistrationCriteriaModel> checkTSPTradeCriteria(int programid, int tradeid, int userid)

        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ProgramID", programid);
                param[1] = new SqlParameter("@TradeID", tradeid);
                param[2] = new SqlParameter("@TSPID", userid);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTradeTSPTargetcheck", param).Tables[0];
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
            model.TSPCapacity = r.Field<int>("TSPCapacity");
            model.TradeCapicity = r.Field<int>("TradeCapacity");
            model.EnrolledTraineesTSP = r.Field<int>("RemainingCapacity");
            return model;
        }
        public List<TraineeProfileModel> SaveTraineeIntrestProfile(TraineeProfileModel traineeProfile)
        {
            string errMsg = string.Empty;
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeID", traineeProfile.TraineeID));
                param.Add(new SqlParameter("@UserID", traineeProfile.CurUserID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_SSPTraineeInterstProfile]", param.ToArray());
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public List<TraineeProfileModel> SaveTraineeInterviewSubmission(TraineeProfileModel traineeProfile)
        {
            string errMsg = string.Empty;
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeID", traineeProfile.TraineeID));
                param.Add(new SqlParameter("@ApprovalStatus", traineeProfile.TraineeIntrestStatus));
                param.Add(new SqlParameter("@StatusReason", traineeProfile.TraineeStatusChangeReason));
                param.Add(new SqlParameter("@UserID", traineeProfile.CurUserID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_SSPTraineeInterestInterview]", param.ToArray());
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public DataTable FetchReport(int UserID, string SpName)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@CreatedUserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, SpName, param.ToArray()).Tables[0];
            return dt;
        }

        public DataTable SaveTraineeBiometricData(BiometricTraineeDataModel model)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@TraineeID", model.TraineeID));
            param.Add(new SqlParameter("@RightIndexFinger", model.RightIndexFinger));
            param.Add(new SqlParameter("@RightMiddleFinger", model.RightMiddleFinger));
            param.Add(new SqlParameter("@LeftIndexFinger", model.LeftIndexFinger));
            param.Add(new SqlParameter("@LeftMiddleFinger", model.LeftMiddleFinger));
            param.Add(new SqlParameter("@CurUserID", model.CurUserID));

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_TraineeBiometricData", param.ToArray()).Tables[0];
            return dt;
        }

        public DataTable SaveBiometricAttendance(BiometricTraineeDataModel model)
        {
            List<SqlParameter> param = new List<SqlParameter>
        {
            new SqlParameter("@TraineeID", model.TraineeID),
            new SqlParameter("@FingerImpression", model.FingerImpression),
            new SqlParameter("@CheckIn", model.AttendanceType == "CheckIn" ? 1 : 0),
            new SqlParameter("@CheckOut", model.AttendanceType == "CheckOut" ? 1 : 0),
            new SqlParameter("@TimeStamp", DateTime.Now),
            new SqlParameter("@CurUserID", model.CurUserID)
        };

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_TraineeBiometricAttendance", param.ToArray()).Tables[0];
            return dt;
        }
        public void DeleteTraineeandAttandance(string cnic)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@CNIC", cnic));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "DeleteTraineeandAttandance", param.ToArray());
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        public int SavebiomatricTraineeProfileDVV(TraineeProfileDVV model, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeID", model.TraineeID));
                param.Add(new SqlParameter("@BiometricData1", model.BiometricData1));
                param.Add(new SqlParameter("@BiometricData2", model.BiometricData2));
                param.Add(new SqlParameter("@BiometricData3", model.BiometricData3));
                param.Add(new SqlParameter("@BiometricData4", model.BiometricData4));
                // Add an output parameter to capture the return value from the stored procedure
                SqlParameter returnParameter = new SqlParameter
                {
                    ParameterName = "@ReturnVal",
                    Direction = ParameterDirection.ReturnValue
                };
                param.Add(returnParameter);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_BioMatricTraineeProfileDVV", param.ToArray());

                // Get the return value from the stored procedure
                int returnValue = (int)returnParameter.Value;

                // Handle success or failure based on the return value
                if (returnValue == 0)
                {
                    return 0; // Success
                }
                else if (returnValue == -1)
                {
                    errMsg = "TraineeID not found.";
                    return -1; // Failure due to invalid TraineeID
                }
                else
                {
                    errMsg = $"An error occurred. Error Code: {returnValue}";
                    return returnValue; // Other error codes
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected exceptions
                errMsg = ex.Message;
                return -99; // Indicate unexpected error
            }
        }
    }
}