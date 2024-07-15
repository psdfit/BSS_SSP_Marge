using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DataLayer.Services
{
    public class SRVAttendancePerception : SRVBase, DataLayer.Interfaces.ISRVAttendancePerception
    {
        public DataTable FetchAttendancePerceptionList(AMSReportsParamModel model)
        {
            String sDate = model.Month.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String Month = datevalue.Day.ToString(); //Converting Day to Month
            String Day = datevalue.Month.ToString();  // Converting Month to Day
            String yy = datevalue.Year.ToString();
            string strMonth = yy + "-" + Month + "-" + Day;

            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@TSPID", model.TSPID);
            param[2] = new SqlParameter("@ClassID", model.ClassID);
            param[3] = new SqlParameter("@Month", model.Month);
            param[4] = new SqlParameter("@UserID", model.UserID);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AttendancePerceptionData", param).Tables[0];
           return dt;
        }
        public List<AttendancePerceptionModel> GetAttendancePerceptionList(AMSReportsParamModel model)
        {
            try
            {
                String sDate = model.Month.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

                String Month = datevalue.Day.ToString(); //Converting Day to Month
                String Day = datevalue.Month.ToString();  // Converting Month to Day
                String yy = datevalue.Year.ToString();
                string strMonth = yy + "-" + Month + "-" + Day;

                SqlParameter[] param = new SqlParameter[5];
               
                param[0] = new SqlParameter("@SchemeID", model.SchemeID);
                param[1] = new SqlParameter("@TSPID", model.TSPID);
                param[2] = new SqlParameter("@ClassID", model.ClassID);
                param[3] = new SqlParameter("@Month", model.Month);
                param[4] = new SqlParameter("@UserID", model.UserID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AttendancePerceptionData", param).Tables[0];
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

        private List<AttendancePerceptionModel> LoopinData(DataTable dt)
        {
            List<AttendancePerceptionModel> List = new List<AttendancePerceptionModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfAttendancePerception(r));
            }
            return List;
        }

        private AttendancePerceptionModel RowOfAttendancePerception(DataRow r)
        {
            AttendancePerceptionModel attendancePerception = new AttendancePerceptionModel();
            attendancePerception.SchemeName = r["SchemeName"] != "" ? r["SchemeName"].ToString():"-";
            attendancePerception.TSPName = r["TSPName"] != "" ? r["TSPName"].ToString():"-";
            attendancePerception.ClassCode = r["ClassCode"] != "" ? r["ClassCode"].ToString():"-";
            attendancePerception.TradeName = r["TradeName"] != "" ? r["TradeName"].ToString():"-";
            attendancePerception.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);

            attendancePerception.VisitDate = r["VisitDate"].ToString().GetDate();

            attendancePerception.VisitDateAttendance1 = r["VisitDateAttendance1"] != "" ? r["VisitDateAttendance1"].ToString():"-";
            attendancePerception.OneDayBefore1 = r["OneDayBefore1"] != "" ? r["OneDayBefore1"].ToString():"-";
            attendancePerception.TwoDayBefore1 = r["TwoDayBefore1"] != "" ? r["TwoDayBefore1"].ToString():"-";
            attendancePerception.ThreeDayBefore1 = r["ThreeDayBefore1"] != "" ? r["ThreeDayBefore1"].ToString():"-";
            attendancePerception.SufficientConsumablesVisit1 = r["SufficientConsumablesVisit1"] != "" ? r["SufficientConsumablesVisit1"].ToString():"-";
            attendancePerception.SufficientEquipmentTools1 = r["SufficientEquipmentToolsVisit1"] != "" ? r["SufficientEquipmentToolsVisit1"].ToString():"-";
            attendancePerception.QualityOfPracticalTrainingVisit1 = r["QualityOfPracticalTrainingVisit1"] != "" ? r["QualityOfPracticalTrainingVisit1"].ToString():"-";
            attendancePerception.QualityOfMealsVisit1 = r["QualityOfMealsVisit1"] != "" ? r["QualityOfMealsVisit1"].ToString():"-";
            attendancePerception.QualityOfBoardingFacilityVisit1 = r["QualityOfBoardingFacilityVisit1"] != "" ? r["QualityOfBoardingFacilityVisit1"].ToString():"-";
            attendancePerception.TrainingUsefulnessVisit1 = r["TrainingUsefulnessVisit1"] != "" ? r["TrainingUsefulnessVisit1"].ToString():"-";
            attendancePerception.AverageDailyHoursVisit1 = r["AverageDailyHoursVisit1"] != "" ? r["AverageDailyHoursVisit1"].ToString():"-";

            attendancePerception.VisitDateAttendance2 = r["VisitDateAttendance2"] != "" ? r["VisitDateAttendance2"].ToString() : "-";
            attendancePerception.OneDayBefore2 = r["OneDayBefore2"] != "" ? r["OneDayBefore2"].ToString(): "-" ;
            attendancePerception.TwoDayBefore2 = r["TwoDayBefore2"] != "" ? r["TwoDayBefore2"].ToString():"-";
            attendancePerception.ThreeDayBefore2 = r["ThreeDayBefore2"] != "" ? r["ThreeDayBefore2"].ToString():"-";
            attendancePerception.SufficientConsumablesVisit2 = r["SufficientConsumablesVisit2"] != "" ? r["SufficientConsumablesVisit2"].ToString():"-";
            attendancePerception.SufficientEquipmentTools2 = r["SufficientEquipmentToolsVisit2"] != "" ? r["SufficientEquipmentToolsVisit2"].ToString():"-";
            attendancePerception.QualityOfPracticalTrainingVisit2 = r["QualityOfPracticalTrainingVisit2"] != "" ? r["QualityOfPracticalTrainingVisit2"].ToString():"-";
            attendancePerception.QualityOfMealsVisit2 = r["QualityOfMealsVisit2"] != "" ? r["QualityOfMealsVisit2"].ToString():"-";
            attendancePerception.QualityOfBoardingFacilityVisit2 = r["QualityOfBoardingFacilityVisit2"] != "" ? r["QualityOfBoardingFacilityVisit2"].ToString():"-";
            attendancePerception.TrainingUsefulnessVisit2 = r["TrainingUsefulnessVisit2"] != "" ? r["TrainingUsefulnessVisit2"].ToString():"-";
            attendancePerception.AverageDailyHoursVisit2 = r["AverageDailyHoursVisit2"] != "" ? r["AverageDailyHoursVisit2"].ToString():"-";

            attendancePerception.VisitDateAttendance3 = r["VisitDateAttendance3"] != "" ? r["VisitDateAttendance3"].ToString() : "-";
            attendancePerception.OneDayBefore3 = r["OneDayBefore3"] != "" ? r["OneDayBefore3"].ToString():"-";
            attendancePerception.TwoDayBefore3 = r["TwoDayBefore3"] != "" ? r["TwoDayBefore3"].ToString():"-";
            attendancePerception.ThreeDayBefore3 = r["ThreeDayBefore3"] != "" ? r["ThreeDayBefore3"].ToString():"-";
            attendancePerception.SufficientConsumablesVisit3 = r["SufficientConsumablesVisit3"] != "" ? r["SufficientConsumablesVisit3"].ToString():"-";
            attendancePerception.SufficientEquipmentTools3 = r["SufficientEquipmentToolsVisit3"] != "" ? r["SufficientEquipmentToolsVisit3"].ToString():"-";
            attendancePerception.QualityOfPracticalTrainingVisit3 = r["QualityOfPracticalTrainingVisit3"] != "" ? r["QualityOfPracticalTrainingVisit3"].ToString():"-";
            attendancePerception.QualityOfMealsVisit3 = r["QualityOfMealsVisit3"] != "" ? r["QualityOfMealsVisit3"].ToString():"-";
            attendancePerception.QualityOfBoardingFacilityVisit3 = r["QualityOfBoardingFacilityVisit3"] != "" ? r["QualityOfBoardingFacilityVisit3"].ToString():"-";
            attendancePerception.TrainingUsefulnessVisit3 = r["TrainingUsefulnessVisit3"] != "" ? r["TrainingUsefulnessVisit3"].ToString():"-";
            attendancePerception.AverageDailyHoursVisit3 = r["AverageDailyHoursVisit3"] != "" ? r["AverageDailyHoursVisit3"].ToString():"-";

            attendancePerception.VisitDateAttendance4 = r["VisitDateAttendance4"] != "" ? r["VisitDateAttendance4"].ToString() : "-";
            attendancePerception.OneDayBefore4 = r["OneDayBefore4"] != "" ? r["OneDayBefore4"].ToString():"-";
            attendancePerception.TwoDayBefore4 = r["TwoDayBefore4"] != "" ? r["TwoDayBefore4"].ToString():"-";
            attendancePerception.ThreeDayBefore4 = r["ThreeDayBefore4"] != "" ? r["ThreeDayBefore4"].ToString():"-";
            attendancePerception.SufficientConsumablesVisit4 = r["SufficientConsumablesVisit4"] != "" ? r["SufficientConsumablesVisit4"].ToString():"-";
            attendancePerception.SufficientEquipmentTools4 = r["SufficientEquipmentToolsVisit4"] != "" ? r["SufficientEquipmentToolsVisit4"].ToString():"-";
            attendancePerception.QualityOfPracticalTrainingVisit4 = r["QualityOfPracticalTrainingVisit4"] != "" ? r["QualityOfPracticalTrainingVisit4"].ToString():"-";
            attendancePerception.QualityOfMealsVisit4 = r["QualityOfMealsVisit4"] != "" ? r["QualityOfMealsVisit4"].ToString():"-";
            attendancePerception.QualityOfBoardingFacilityVisit4 = r["QualityOfBoardingFacilityVisit4"] != "" ? r["QualityOfBoardingFacilityVisit4"].ToString():"-";
            attendancePerception.TrainingUsefulnessVisit4 = r["TrainingUsefulnessVisit4"] != "" ? r["TrainingUsefulnessVisit4"].ToString():"-";
            attendancePerception.AverageDailyHoursVisit4 = r["AverageDailyHoursVisit4"] != "" ? r["AverageDailyHoursVisit4"].ToString():"-";

            attendancePerception.Remarks = "";

            return attendancePerception;

        }
    }
}
