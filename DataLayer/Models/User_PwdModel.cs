using System;

namespace DataLayer.Models
{
    [Serializable]
    public class User_PwdModel : ModelBase
    {
        public User_PwdModel() : base()
        {
        }

        public User_PwdModel(bool InActive) : base(InActive)
        {
        }

        public int pwdID { get; set; }
        public string UserPassword { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }}