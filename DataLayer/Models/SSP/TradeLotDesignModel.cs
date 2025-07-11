using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class TradeLotDesignModel : ModelBase
    {

        public TradeLotDesignModel() { }
        public int UserID { get; set; }
        public int GenderID { get; set; }
        public int ProgramDesignID { get; set; }
        public string ProgramDesignOn { get; set; }
        public int TradeDesignID { get; set; } = 0;
        public int ContraTargetThreshold { get; set; }
        public int Scheme { get; set; }
        public int[] Province { get; set; } = {0};
        public int[] Cluster { get; set; } = { 0 };
        public int[] District { get; set; } = { 0 };
        public int SelectedCount { get; set; }
        public string SelectedShortList { get; set; }
        public int ProgramFocus { get; set; }
        public int Trade { get; set; }
        public int TradeLayer { get; set; }
        public int CTM { get; set; }
        public int OJTPayment { get; set; }
        public int GuruPayment { get; set; }
        public int TransportationCost { get; set; }
        public int MedicalCost { get; set; }
        public int PrometricCost { get; set; }
        public int ProtectorateCost { get; set; }
        public int OtherTrainingCost { get; set; }
        public int ExamCost { get; set; }
        public int TraineeContraTarget { get; set; }
        public int TraineeCompTarget { get; set; }
        public int PerSelectedContraTarget { get; set; }
        public int PerSelectedCompTarget { get; set; }
        public List<TradeLot> TradeLot { get; set; } = new List<TradeLot>();


    }



    public class TradeLot
    {
        public int UserID { get; set; }
        public int TradeLotID { get; set; } = 0;
        public int TradeDesignID { get; set; }
        public int TradeID { get; set; }
        public int TradeDetailMapID { get; set; }
        public string TradeLayer { get; set; }
        public string AnnouncedDistrict { get; set; }
        public float Duration { get; set; }
        public string TraineeContTarget { get; set; }
        public int CTM { get; set; }
        public int TrainingCost { get; set; }
        public int Stipend { get; set; }
        public int BagAndBadge { get; set; }
        public int ExamCost { get; set; }
        public int TotalCost { get; set; }

    }
}
