using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class AttendancePerceptionModel : ModelBase
    {

        public AttendancePerceptionModel() : base()
        {
        }

        public int RowNumber { get; set; }
        public int ClassID { get; set; }
        public int spID { get; set; }
        public int ClassInspectionRequestID { get; set; }
        public string ClassCode { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string TradeName { get; set; }
        public int TraineesPerClass { get; set; }
        public DateTime VisitDate { get; set; }

        public string OneDayBefore1 { get; set; }
        public string TwoDayBefore1 { get; set; }
        public string ThreeDayBefore1 { get; set; }
        public string VisitDateAttendance1 { get; set; }
        public string SufficientConsumablesVisit1 { get; set; }
        public string SufficientEquipmentTools1 { get; set; }
        public string QualityOfPracticalTrainingVisit1 { get; set; }
        public string QualityOfMealsVisit1 { get; set; }
        public string QualityOfBoardingFacilityVisit1 { get; set; }
        public string TrainingUsefulnessVisit1 { get; set; }
        public string AverageDailyHoursVisit1 { get; set; }
        public string OneDayBefore2 { get; set; }
        public string TwoDayBefore2 { get; set; }
        public string ThreeDayBefore2 { get; set; }
        public string VisitDateAttendance2 { get; set; }
        public string SufficientConsumablesVisit2 { get; set; }
        public string SufficientEquipmentTools2 { get; set; }
        public string QualityOfPracticalTrainingVisit2 { get; set; }
        public string QualityOfMealsVisit2 { get; set; }
        public string QualityOfBoardingFacilityVisit2 { get; set; }
        public string TrainingUsefulnessVisit2 { get; set; }
        public string AverageDailyHoursVisit2 { get; set; }
        public string OneDayBefore3 { get; set; }
        public string TwoDayBefore3 { get; set; }
        public string ThreeDayBefore3 { get; set; }
        public string VisitDateAttendance3 { get; set; }
        public string SufficientConsumablesVisit3 { get; set; }
        public string SufficientEquipmentTools3 { get; set; }
        public string QualityOfPracticalTrainingVisit3 { get; set; }
        public string QualityOfMealsVisit3 { get; set; }
        public string QualityOfBoardingFacilityVisit3 { get; set; }
        public string TrainingUsefulnessVisit3 { get; set; }
        public string AverageDailyHoursVisit3 { get; set; }
        public string OneDayBefore4 { get; set; }
        public string TwoDayBefore4 { get; set; }
        public string ThreeDayBefore4 { get; set; }
        public string VisitDateAttendance4 { get; set; }
        public string SufficientConsumablesVisit4 { get; set; }
        public string SufficientEquipmentTools4 { get; set; }
        public string QualityOfPracticalTrainingVisit4 { get; set; }
        public string QualityOfMealsVisit4 { get; set; }
        public string QualityOfBoardingFacilityVisit4 { get; set; }
        public string TrainingUsefulnessVisit4 { get; set; }
        public string AverageDailyHoursVisit4 { get; set; }
        public string Remarks { get; set; }
    }
}
