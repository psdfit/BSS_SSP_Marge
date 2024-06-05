using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using PSDF_BSS.Reports.Models;
using PSDF_BSS.Reports.Interfaces;

namespace PSDF_BSS.Reports.Services
{
    public class SRVProfileVerification : ISRVProfileVerification
    {
        private readonly Dictionary<string, string> statusValues = new Dictionary<string, string>()
        {
            {"deleted", "This trainee was deleted from attendance register"},
            {"dropout", "This trianee was mentioned as dropout"},
            {"blank_space", "The trainee was absent and there was blank space against this trainee"},
            {"short_leave", "This trainee was on short leave on the day of visit"},
            {"absent", "This trainee was absent on the day of visit"},
            {"marked_present_found_absent", "This trainee was marked present in attendance register but was not present on the day of visit"},
            {"dropout_but_present", "This trainee was dropout but marked present in attendance register"}
        };
        public List<ProfileVerificationModel> GetProfileVerificationReportList(int? schemeId, int? tspId, int? classId, string dateTime)
        {
            try
            {
                DateTime? date = null;
                if (!string.IsNullOrEmpty(dateTime))
                {
                    date = DateTime.ParseExact(dateTime, "dd/MM/yyyy", null);
                }
                schemeId = schemeId > 0 ? schemeId : 0;
                tspId = tspId > 0 ? tspId : 0;
                classId = classId > 0 ? classId : 0;
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@SchemeId", schemeId);
                param[1] = new SqlParameter("@TspId", tspId);
                param[2] = new SqlParameter("@ClassId", classId);
                param[3] = new SqlParameter("@Month", date);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_ProfileVerification", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinData(dt);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private List<ProfileVerificationModel> LoopinData(DataTable dt)
        {
            List<ProfileVerificationModel> List = new List<ProfileVerificationModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfProfileVerification(r));
            }
            return List;
        }
        private ProfileVerificationModel RowOfProfileVerification(DataRow r)
        {
            ProfileVerificationModel profileVerification = new ProfileVerificationModel();
            profileVerification.IsLock = r["IsLock"].ToString();
            profileVerification.SchemeName = r["SchemeName"].ToString();
            profileVerification.TSPName = r["TSPName"].ToString();
            profileVerification.ClassCode = r["ClassCode"].ToString();
            profileVerification.TradeName = r["TradeName"].ToString();
            profileVerification.TrainingCentreAddress = r["TrainingCentreAddress"].ToString();
            profileVerification.VisitDateTime = r["VisitDateTime"].ToString();
            profileVerification.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);
            profileVerification.TotalTraineesPresentPerClass = Convert.ToInt32(r["TotalTraineesPresentPerClass"]);
            profileVerification.Name = r["Name"].ToString();
            profileVerification.FatherName = r["FatherName"].ToString();
            profileVerification.Cnic = r["Cnic"].ToString();
            if (profileVerification.IsLock == "n")
            {
                profileVerification.AttendanceStatus = r["AttendanceStatus"].ToString();
                profileVerification.VerificationStatus = r["VerificationStatus"].ToString();
                profileVerification.TraineeCountRemarks = r["TraineeCountRemarks"].ToString();
                profileVerification.IsPresent = r["IsPresent"].ToString();
                if (profileVerification.AttendanceStatus == "present")
                {
                    if (profileVerification.VerificationStatus != "fake")
                    {
                        profileVerification.Status = "Verified";

                        if (profileVerification.VerificationStatus == "not_verified")
                        {
                            profileVerification.Remarks = "The trainee was present but reported as Not Verified by field monitor";
                        }
                        else
                        {
                            profileVerification.Remarks = "The trainee was present on the date of visit";
                        }
                    }
                    else
                    {
                        profileVerification.Status = "Not Verified";
                        if (profileVerification.VerificationStatus == "fake")
                        {
                            profileVerification.Remarks = "The trainee was present but was reported Fake by field monitor";
                        }
                    }
                }
                else
                {
                    if (profileVerification.AttendanceStatus == "blank_space" && profileVerification.IsPresent == "y")
                    {

                        if (profileVerification.VerificationStatus == "fake")
                        {
                            profileVerification.Status = "Not Verified";
                            profileVerification.Remarks = "The trainee was present but fake and there was blank space against this trainee";
                        }
                        else
                        {
                            profileVerification.Status = "Verified";
                            profileVerification.Remarks = "The trainee was present but there was blank space against this trainee";
                        }
                    }
                    else
                    {
                        profileVerification.Status = "Not Verified";

                        if (profileVerification.VerificationStatus == "fake")
                        {
                            profileVerification.Remarks = "The trainee was absent, fake and there was blank space against this trainee";
                        }
                        else
                        {
                            profileVerification.Remarks = statusValues[profileVerification.AttendanceStatus];
                        }
                    }
                }
            }
            else 
            {
                profileVerification.Status = r["Status"].ToString();
                profileVerification.Remarks = r["Remarks"].ToString();
            }
            return profileVerification;
        }
    }
}