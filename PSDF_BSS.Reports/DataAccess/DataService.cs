using DataLayer.Classes;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace PSDF_BSS.Reports.DataAccess
{
    public class DataService
    {
        #region TraineeProfile
        public List<TraineeProfileModel> TraineeProfileRegistration(int schemeId = 0, int tspId = 0, int classId = 0, string traineeIDs = "")
        {
            List<TraineeProfileModel> list = new List<TraineeProfileModel>();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@SchemeId", schemeId);
                param[1] = new SqlParameter("@TspId", tspId);
                param[2] = new SqlParameter("@ClassId", classId);
                param[3] = new SqlParameter("@TraineeIDs", traineeIDs);
                var data = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RPT_TraineeProfile_Registration", param).Tables[0];
                list = LoopinDataTraineeProfile(data);
            }
            catch (Exception)
            {

                throw;
            }
            return list;
        }
        private List<TraineeProfileModel> LoopinDataTraineeProfile(DataTable dt)
        {
            List<TraineeProfileModel> TraineeProfileL = new List<TraineeProfileModel>();
            foreach (DataRow r in dt.Rows)
            {
                TraineeProfileL.Add(RowOfTraineeProfile(r));
            }
            return TraineeProfileL;
        }
        private TraineeProfileModel RowOfTraineeProfile(DataRow row)
        {
            string traineeImageRootPath = WebConfigurationManager.AppSettings["traineeImageRootPath"];
            //GetFiles by Path
            string traineeImage = traineeImageRootPath + row.Field<string>("TraineeImg") ?? string.Empty;
            string traineeCNICImage = traineeImageRootPath + row.Field<string>("CNICImg") ?? string.Empty;

            TraineeProfileModel TraineeProfile = new TraineeProfileModel();
            TraineeProfile.TraineeID = row.Field<int>("TraineeID");
            TraineeProfile.TraineeCode = row.Field<string>("TraineeCode");
            TraineeProfile.TraineeName = row.Field<string>("TraineeName");
            TraineeProfile.TraineeCNIC = row.Field<string>("TraineeCNIC");
            TraineeProfile.FatherName = row.Field<string>("FatherName");
            TraineeProfile.GenderID = row.Field<int>("GenderID");
            TraineeProfile.GenderName = row.Field<string>("GenderName");
            TraineeProfile.TradeID = row.Field<int>("TradeID");
            TraineeProfile.TradeName = row.Field<string>("TradeName");
            TraineeProfile.SectionID = row.Field<int>("SectionID");
            TraineeProfile.SectionName = row.Field<string>("SectionName");
            TraineeProfile.DateOfBirth = row.Field<DateTime?>("DateOfBirth");
            TraineeProfile.CNICIssueDate = row.Field<DateTime?>("CNICIssueDate");
            //TraineeProfile.IsDual = row.Field<bool>("IsDual");
            TraineeProfile.UpdatedDate = row.Field<DateTime?>("UpdatedDate");
            TraineeProfile.SchemeID = row.Field<int>("SchemeID");
            TraineeProfile.SchemeName = row.Field<string>("SchemeName");
            TraineeProfile.TspID = row.Field<int>("TspID");
            TraineeProfile.TSPName = row.Field<string>("TSPName");
            TraineeProfile.ClassID = row.Field<int>("ClassID");
            TraineeProfile.ClassCode = row.Field<string>("ClassCode");
            TraineeProfile.EducationID = row.Field<int>("EducationID");
            TraineeProfile.ContactNumber1 = row.Field<string>("ContactNumber1");
            TraineeProfile.CNICVerified = row.Field<bool>("CNICVerified");
            TraineeProfile.TraineeImg = new Uri(traineeImage).AbsoluteUri;
            TraineeProfile.CNICImg = new Uri(traineeCNICImage).AbsoluteUri;
            TraineeProfile.TraineeAge = row.Field<int>("TraineeAge");
            TraineeProfile.ReligionID = row.Field<int>("ReligionID");
            TraineeProfile.VoucherHolder = row.Field<bool>("VoucherHolder");
            TraineeProfile.VoucherNumber = row.Field<string>("VoucherNumber");
            TraineeProfile.VoucherOrganization = row.Field<string>("VoucherOrganization");
            TraineeProfile.WheredidYouHearAboutUs = row.Field<string>("WheredidYouHearAboutUs");
            TraineeProfile.TraineeIndividualIncomeID = row.Field<int?>("TraineeIndividualIncomeID") ?? 0;
            TraineeProfile.HouseHoldIncomeID = row.Field<int>("HouseHoldIncomeID");
            TraineeProfile.EmploymentStatusBeforeTrainingID = row.Field<int>("EmploymentStatusBeforeTrainingID");
            TraineeProfile.Undertaking = row.Field<bool>("Undertaking");
            TraineeProfile.GuardianNextToKinName = row.Field<string>("GuardianNextToKinName");
            TraineeProfile.GuardianNextToKinContactNo = row.Field<string>("GuardianNextToKinContactNo");
            TraineeProfile.TraineeHouseNumber = row.Field<string>("TraineeHouseNumber");
            TraineeProfile.TraineeStreetMohalla = row.Field<string>("TraineeStreetMohalla");
            TraineeProfile.TraineeMauzaTown = row.Field<string>("TraineeMauzaTown");
            TraineeProfile.TraineeDistrictID = row.Field<int>("TraineeDistrictID");
            TraineeProfile.TraineeTehsilID = row.Field<int>("TraineeTehsilID");
            TraineeProfile.AgeVerified = row.Field<bool>("AgeVerified");
            TraineeProfile.DistrictVerified = row.Field<bool>("DistrictVerified");
            TraineeProfile.TraineeVerified = row.Field<int>("TraineeVerified") == 1 ? true : false;
            TraineeProfile.CreatedDate = row.Field<DateTime?>("CreatedDate");
            TraineeProfile.CreatedUserID = row.Field<int>("CreatedUserID");
            TraineeProfile.IsSubmitted = row.Field<bool>("IsSubmitted");
            TraineeProfile.CNICImgNADRA = row.Field<string>("CNICImgNADRA");
            TraineeProfile.CNICUnVerifiedReason = row.Field<string>("CNICUnVerifiedReason");
            TraineeProfile.AgeUnVerifiedReason = row.Field<string>("AgeUnVerifiedReason");
            TraineeProfile.ResidenceUnVerifiedReason = row.Field<string>("ResidenceUnVerifiedReason");
            //TraineeProfile.CNICVerificationDate = row.Field<string>("CNICVerificationDate");
            TraineeProfile.CNICVerificationDate = row.Field<DateTime?>("CNICVerificationDate");
            TraineeProfile.ResultStatusID = row.Field<int?>("ResultStatusID") ?? 0;
            TraineeProfile.UID = row.Field<string>("UID");
            TraineeProfile.IsMigrated = row.Field<bool>("IsMigrated");
            //TraineeProfile.TraineeStatusChangeDate = row.Field<DateTime?>("TraineeStatusChangeDate");
            //TraineeProfile.TraineeStatusTypeID = row.Field<int?>("TraineeStatusTypeID") ?? 0;
            //TraineeProfile.TraineeStatusChangeReason = row["TraineeStatusChangeReason"].ToString();
            TraineeProfile.ResultStatusChangeDate = row.Field<DateTime?>("ResultStatusChangeDate");
            //TraineeProfile.ResultStatusChangeReason = row["ResultStatusChangeReason"].ToString();
            TraineeProfile.ResultStatusChangeReason = row.Field<string>("ResultStatusChangeReason");

            TraineeProfile.ModifiedUserID = row.Field<int?>("ModifiedUserID") ?? 0;
            TraineeProfile.ModifiedDate = row.Field<DateTime?>("ModifiedDate");
            TraineeProfile.InActive = row.Field<bool>("InActive");
            TraineeProfile.IsManual = row.Field<bool>("IsManual");

            TraineeProfile.TSP_CPName = row.Field<string>("TSP_CPName");
            TraineeProfile.TraineeMaxEducation = row.Field<string>("TraineeMaxEducation");
            TraineeProfile.SchemeCode = row.Field<string>("SchemeCode");
            TraineeProfile.ReligionName = row.Field<string>("ReligionName");
            //TraineeProfile.ClassCode = row["ClassCode"].ToString();
            TraineeProfile.ClassCenterLocation = row.Field<string>("ClassCenterLocation");
            TraineeProfile.DistrictName = row.Field<string>("DistrictName");
            TraineeProfile.DistrictNameUrdu = row.Field<string>("DistrictNameUrdu");
            TraineeProfile.TehsilName = row.Field<string>("TehsilName");
            TraineeProfile.EmploymentStatusBeforeTraining = row.Field<string>("EmploymentStatusBeforeTraining");
            TraineeProfile.TraineeCurrentStatus = row.Field<string>("TraineeCurrentStatus");
            TraineeProfile.CertAuthName = row.Field<string>("CertAuthName");
            TraineeProfile.TraineeRollNumber = row.Field<string>("TraineeRollNumber");
            TraineeProfile.Disability = row.Field<string>("Disability");
            TraineeProfile.TraineeIndividualIncomeRange = row.Field<string>("TraineeIndividualIncomeRange");
            TraineeProfile.HouseHoldIncomeRange = row.Field<string>("HouseHoldIncomeRange");
            TraineeProfile.PermanentDistrictName= row.Field<string>("PermanentDistrictName");
            TraineeProfile.PermanentTehsilName= row.Field<string>("PermanentTehsilName");
            TraineeProfile.PermanentResidence= row.Field<string>("PermanentResidence"); 
            TraineeProfile.TemporaryDistrictName= row.Field<string>("TemporaryDistrictName");
            TraineeProfile.TemporaryTehsilName= row.Field<string>("TemporaryTehsilName");
            TraineeProfile.TemporaryResidence= row.Field<string>("TemporaryResidence");

            return TraineeProfile;
        }

        #endregion TraineeProfile
    }
}