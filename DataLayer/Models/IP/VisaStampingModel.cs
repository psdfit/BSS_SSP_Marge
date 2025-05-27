using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.IP
{
    public class VisaStampingUploadModel
    {
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public int TspID { get; set; }
        public string ClassCode { get; set; }
        public List<FileModel> Files { get; set; }
    }

    public class FileModel
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileContent { get; set; } // Ensure this is mapped correctly
    }


    public class VisaStampingDocumentModel
    {
        public int VisaStampingTraineeDocumentsID { get; set; }
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public int TspID { get; set; }
        public string ClassCode { get; set; }
        public string VisaStampDoc { get; set; }
    }

    public class VisaStampingResponseModel : ModelBase
    {
        public int VisaStampingTraineeDocumentsID { get; set; }
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
