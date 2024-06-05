using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class SignUpModel : ModelBase
    {
        public SignUpModel() { }




        public int TSPID { get; set; }
        public string UserName { get; set; }
        public string BusinessName { get; set; }
        public string BusinessNTN { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsEmailVerify { get; set; }
        public string OTPCode { get; set; }
        public string Password { get; set; }
        public string IsHeadOffice { get; set; }

    }
}


