﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class AssociationEvaluationModel : ModelBase
    {

        public AssociationEvaluationModel() { }
        public int ID { get; set; }
        public int UserID { get; set; }
        public int TSPID { get; set; }
        public string TspName { get; set; }
        public string TradeLot { get; set; }
        public int ProgramID { get; set; }
        public int TspAssociationMasterID { get; set; }
        public int TspAssociationEvaluationID { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string VerifiedCapacityMorning { get; set; }
        public string Remarks { get; set; }
        public string VerifiedCapacityEvening { get; set; }
        public string MarksBasedOnEvaluation { get; set; }
        public string CategoryBasedOnEvaluation { get; set; }
        public string Attachment { get; set; }


    }
}
