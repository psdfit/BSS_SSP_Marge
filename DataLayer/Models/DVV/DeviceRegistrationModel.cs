using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.DVV
{
    public class DeviceRegistrationModel : ModelBase
    {
        public int RegistrationID { get; set; }
        public int ActivationLogID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string RoleTitle { get; set; }
        public string DeviceStatusRequest { get; set; }
        public string Remarks { get; set; }
        public int UserID { get; set; }
      


    }
}
