
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]

    public class TradeModel : ModelBase
    {
        public TradeModel() : base() { }
        public TradeModel(bool InActive) : base(InActive) { }

        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public string TradeCode { get; set; }
        public int SectorID { get; set; }
        public string SectorName { get; set; }
        public int SubSectorID { get; set; }
        public string SubSectorName { get; set; }
        public bool FinalSubmitted { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }

        public string ProcessKey { get; set; }
        public string SAPID { get; set; }

        public int TrainingLocationID { get; set; }

        //public int TraineeEducationTypeID { get; set; }
        ////public int Duration	{ get; set; }
        //public int TotalTrainingHours { get; set; }
        //public int DailyTrainingHours { get; set; }
        //public int WeeklyTrainingHours { get; set; }
        //public double PracticalPercentage { get; set; }
        //public double TheoryPercentage { get; set; }
        //public int CertificationCategoryID	{ get; set; }
        //public string CertificationCategoryName { get; set; }
        //public int CertAuthID { get; set; }
        //public string CertAuthName { get; set; }

        //public string EquipmentTools { get; set; }
        //public string ConsumableMaterial { get; set; }
        //public int TrainerEducationTypeID { get; set; }

        //public string SourceOfCurriculum { get; set; }


        public List<TradeDetailMapModel> TradeDetails { get; set; }
        //public List<TradeEquipmentToolsMapModel> TradeEquipmentTools { get; set; }
        //public List<TradeConsumableMaterialMapModel> TradeConsumableMaterials { get; set; }
        //public List<TradeSourceOfCurriculumMapModel> TradeSourceOfCurriculums { get; set; }


    }}
