using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    
    public class BusinessPartnerModel
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string HouseBankAccount { get; set; }
        public string Currency { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string VatIdUnCmp { get; set; }
        public string AddID { get; set; }
        public string TaxPayerName { get; set; }
        public string PNTN { get; set; }
        public string e_mail { get; set; }
        public string Cellular { get; set; }
        public string Phone2 { get; set; }
        public string IntrntSite { get; set; }
        public string U_Account { get; set; }
        public string U_AT { get; set; }
        public List<dynamic> AddressList { get; set; }
        public List<dynamic> ContactPersons { get; set; }
        public string AddUpdate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }

    }

}
