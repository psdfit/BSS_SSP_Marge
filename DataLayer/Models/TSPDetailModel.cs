/* **** Aamer Rehman Malik *****/

using System;

namespace DataLayer.Models
{
    [Serializable]
    public class TSPDetailModel : ModelBase
    {
        public TSPDetailModel() : base()
        {
        }

        public TSPDetailModel(bool InActive) : base(InActive)
        {
        }

        public int TSPID { get; set; }
        public string TSPName { get; set; }
        public string Address { get; set; }
        public string TSPCode { get; set; }
        public int OrganizationID { get; set; }
        public string TSPColor { get; set; }
        public int TierID { get; set; }
        public int? KAMID { get; set; }
        public string NTN { get; set; }
        public string PNTN { get; set; }
        public string GST { get; set; }
        public string FTN { get; set; }

        //public int TspStatusID_OLD	{ get; set; }
        public int? DistrictID { get; set; }

        public string DistrictName { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        //public int? TehsilID { get; set; }
        //public string TehsilName { get; set; }
        public string HeadName { get; set; }
        public string HeadDesignation { get; set; }
        public string HeadEmail { get; set; }
        public string HeadLandline { get; set; }
        public string OrgLandline { get; set; }
        public string CPName { get; set; }
        public string CPDesignation { get; set; }
        //public int CPMobile	{ get; set; }
        public string CPLandline { get; set; }
        public string CPEmail { get; set; }
        public string Website { get; set; }
        public string CPAdmissionsName { get; set; }
        public string CPAdmissionsDesignation { get; set; }

        //public int CPAdmissionsMobile	{ get; set; }
        public string CPAdmissionsLandline { get; set; }
        public string CPAdmissionsEmail { get; set; }
        public string CPAccountsName { get; set; }
        public string CPAccountsDesignation { get; set; }

        //public int CPAccountsMobile	{ get; set; }
        public string CPAccountsLandline { get; set; }
        public string CPAccountsEmail { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string AccountTitle { get; set; }
        public string BankBranch { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public int TSPMasterID { get; set; }
        public string UID { get; set; }
        public bool IsMigrated { get; set; }
        public int AssignedUser { get; set; }
        public int UserID { get; set; }
        public string OrganizationName { get; set; }
        public string SAPID { get; set; }
        public string TSPColorName { get; set; }
    }}