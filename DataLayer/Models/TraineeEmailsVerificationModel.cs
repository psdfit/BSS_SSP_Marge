using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TraineeEmailsVerificationModel
    {
        public Guid TraineeEmailVerificationID { get; set; }
        public int TraineeProfileID { get; set; }
        public string EmailAddress { get; set; }
        public string TraineeName { get; set; }
        public int EmailSentCount { get; set; }
        public string ProcessKey { get; set; }
        public bool IsVerified { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsInactive { get; set; }
        public int CreatedByUserID { get; set; }
        public int ModifiedByUserID { get; set; }
    }
}