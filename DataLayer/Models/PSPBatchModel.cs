
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]

    public class PSPBatchModel : ModelBase
    {
        public PSPBatchModel() : base() { }
        public PSPBatchModel(bool InActive) : base(InActive) { }

        public int PSPBatchID { get; set; }
        public string BatchName { get; set; }
        public int TradeID { get; set; }
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string TraineeCNIC { get; set; }
        public string ContactNumber1 { get; set; }
        public bool? IsInterested { get; set; }
        public List<RD_CompletedTraineeByClassModel> CompletedTrainees { get; set; }
        public string PSPAssignedTraineeIDs { get; set; }
        public int PSPUserID { get; set; }
        public string PSPUserIDs { get; set; }


    }}
