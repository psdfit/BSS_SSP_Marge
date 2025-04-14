using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.IP
{
    public class MedicalCostUploadModel
    {
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public int TspID { get; set; }
        public string ClassCode { get; set; }
        public List<FileModelMC> Files { get; set; }
    }

    public class FileModelMC
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileContent { get; set; } // Ensure this is mapped correctly
    }


    public class MedicalCostDocumentModel
    {
        public int MedicalTraineeDocumentsID { get; set; }
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public int TspID { get; set; }
        public string ClassCode { get; set; }
        public string MedicalCostDoc { get; set; }
    }

    public class MedicalCostResponseModel : ModelBase
    {
        public int MedicalTraineeDocumentsID { get; set; }
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
