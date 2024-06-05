using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class EmailUsersModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsAlreadyExist { get; set; }
        public string FullName { get; set; }
        public string TSPContectPersonEmail { get; set; }
    }
}
