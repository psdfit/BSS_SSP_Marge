using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.IP
{
    public class PrometricCostUploadModel
    {
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public int TspID { get; set; }
        public string ClassCode { get; set; }
        public List<FileModelPC> Files { get; set; }
    }

    public class FileModelPC
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileContent { get; set; } // Ensure this is mapped correctly
    }


    public class PrometricCostDocumentModel
    {
        public int PrometricTraineeDocumentsID { get; set; }
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public int TspID { get; set; }
        public string ClassCode { get; set; }
        public string VisaStampDoc { get; set; }
    }

    public class PrometricCostResponseModel : ModelBase
    {
        public int PrometricTraineeDocumentsID { get; set; }
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
