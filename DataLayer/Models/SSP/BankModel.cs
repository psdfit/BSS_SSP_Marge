using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class BankModel: ModelBase
    {

            public BankModel() { }
            public int UserID { get; set; }
            public int BankDetailID { get; set; }
            public string BankName { get; set; }
            public string OtherBank { get; set; }
            public string AccountTitle { get; set; }
            public string AccountNumber { get; set; }
            public string BranchAddress { get; set; }
            public string BranchCode { get; set; }

        
    }
}
