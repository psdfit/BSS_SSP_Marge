﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.DVV
{
    public class DeviceRegistrationModel : ModelBase
    {
        public int RegistrationID { get; set; }
        public int ActivationLogID { get; set; }
        public int TSPID { get; set; }
        public string TSPLocation { get; set; }
        public int ClassID { get; set; }
        public int SchemeID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string RoleTitle { get; set; }
        public string StatusRequest { get; set; }
        public string Remarks { get; set; }
        public int UserID { get; set; }
      


    }
}
