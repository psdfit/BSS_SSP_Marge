using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class BusinessProfileModel : ModelBase
    {
        public BusinessProfileModel() { }


        public int TspID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FilePath { get; set; }
        public string DocPath { get; set; }
        public string InstituteName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string InstituteNTN { get; set; }
        public string NTNAttachment { get; set; }
        public int[] TaxType { get; set; }
        public string TaxTypeID { get; set; }
        public int ApprovalLevel { get; set; }

        public string GSTNumber { get; set; }
        public string GSTAttachment { get; set; }
        public string PRANumber { get; set; }
        public string PRAAttachment { get; set; }
        public string LegalStatus { get; set; }
        public string LegalStatusAttachment { get; set; }
        public string Province { get; set; }
        public string Cluster { get; set; }
        public string District { get; set; }
        public string Tehsil { get; set; }
        public string LatitudeAndLongitude { get; set; }
        public string HeadOfficeAddress { get; set; }
        public string BusinessType { get; set; }
        public string Website { get; set; }
        public string HeadofOrgName { get; set; }
        public string HeadofOrgDesi { get; set; }
        public string CNICofHeadofOrg { get; set; }
        public string HeadofOrgCNICFrontPhoto { get; set; }
        public string HeadofOrgCNICBackPhoto { get; set; }
        public string HeadofOrgEmail { get; set; }
        public string HeadofOrgMobile { get; set; }
        public string ORGLandline { get; set; }
        public string POCName { get; set; }
        public string POCDesignation { get; set; }
        public string POCEmail { get; set; }
        public string POCMobile { get; set; }

    }
}


