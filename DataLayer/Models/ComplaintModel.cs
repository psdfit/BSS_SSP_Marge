using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ComplaintModel : ModelBase
    {
        public ComplaintModel() : base() { }
        public ComplaintModel(bool InActive) : base(InActive) { }

        public class ComplaintUserModel: ComplaintModel
        {
            public string[]? UserID { get; set; }
        }

        public int? ComplainantID { get; set; }
        public int? ComplaintUserID { get; set; }
        public string ComplainantName { get; set; }
        public string ComplaintDescription { get; set; }
        public string ComplaintStatus { get; set; }
        public int? ComplaintTypeID { get; set; }
        public string ComplaintTypeName { get; set; }
        public int? ComplaintSubTypeID { get; set; }
        public string ComplaintSubTypeName { get; set; }
        public string ComplaintNo { get; set; }
        public int? ComplaintStatusTypeID { get; set; }
        public string ComplaintStatusType { get; set; }
        public string base64 { get; set; }
        public string ext { get; set; }
        public string FilePath { get; set; }
        public string FileGuid { get; set; }
        public string complaintStatusDetailComments { get; set; }
        public bool Submitted { get; set; }
        public int? UserID { get; set; }
        public string UserIDs { get; set; }
        public string FullName { get; set; }
        public string TraineeCNIC { get; set; }
        public string TSPCode { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }



        public int? ComplaintStatusDetailID { get; set; }
        public int? TraineeID { get; set; }
        public int? TSPMasterID { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string ContactNumber1 { get; set; }
        public string ClassCode { get; set; }
        public string TraineeHouseNumber { get; set; }
        public string TraineeStreetMohalla { get; set; }
        public string TraineeMauzaTown { get; set; }
        public string TrainingAddressLocation { get; set; }
        public string TSPName { get; set; }
        public string Address { get; set; }
        public string HeadLandline { get; set; }
        public int? ComplaintRegisterType { get; set; }
        


    }
}
