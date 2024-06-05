
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]

    public class TradeDetailMapModel : ModelBase
    {
        public TradeDetailMapModel() : base() { }
        public TradeDetailMapModel(bool InActive) : base(InActive) { }

        public int TradeDetailMapID { get; set; }
        public int TradeID { get; set; }
        //public string TradeName { get; set; }
        public string SourceOfCurriculumName { get; set; }
        public int DurationID { get; set; }
        public int TotalTrainingHours { get; set; }
        public int DailyTrainingHours { get; set; }
        public int WeeklyTrainingHours { get; set; }
        public int CertificationCategoryID { get; set; }
        public int CertAuthID { get; set; }
        public int SourceOfCurriculumID { get; set; }
        public int TraineeEducationTypeID { get; set; }
        public int TraineeAcademicDisciplineID { get; set; }
        public double PracticalPercentage { get; set; }
        public double TheoryPercentage { get; set; }
        public int TrainerEducationTypeID { get; set; }
        public int TrainerAcademicDisciplineID { get; set; }
        //public List<TradeEquipmentToolsMapModel> TradeEquipmentTools { get; set; }
        //public List<TradeConsumableMaterialMapModel> TradeConsumableMaterials { get; set; }
        public string EquipmentToolID { get; set; }
        public string ConsumableMaterialID { get; set; }
        public bool MappedWithClass { get; set; }
        public string CurriculaAttachment { get; set; }
        public string CurriculaAttachmentPre { get; set; }

    }}
