using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.IP
{
    public class OtherTrainingCostUploadModel
    {
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public int TspID { get; set; }
        public string ClassCode { get; set; }
        public List<FileModelOT> Files { get; set; }
    }

    public class FileModelOT
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileContent { get; set; } // Ensure this is mapped correctly
    }


    public class OtherTrainingCostDocumentModel
    {
        public int OtherTraineeDocumentsID { get; set; }
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public int TspID { get; set; }
        public string ClassCode { get; set; }
        public string OtherTrainingDoc { get; set; }
    }

    public class OtherTrainingCostResponseModel : ModelBase
    {
        public int OtherTraineeDocumentsID { get; set; }
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public int TspID { get; set; }
        public string ClassCode { get; set; }
        public string FileName { get; set; }
        public string FileContentBase64 { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
    }

}
