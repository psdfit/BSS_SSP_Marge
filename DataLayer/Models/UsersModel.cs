using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]
    public class UsersModel : ModelBase
    {
        public UsersModel() : base()
        {
        }

        public UsersModel(bool InActive) : base(InActive)
        {
        }

        public int UserID { get; set; }
        public string UserName { get; set; }        public string UserPassword { get; set; }
        public string Fname { get; set; }
        public string lname { get; set; }
        public string Email { get; set; }
        public string logFilePath { get; set; }
        public string FullName { get; set; }
        public string UserImage { get; set; }
        public int RoleID { get; set; }
        public int UserLevel { get; set; }
        public string RoleTitle { get; set; }
        public DateTime? LoginDT { get; set; }
        public List<UsersRightsModel> usersRights { get; set; }
        public List<UserOrganizationsModel> userOrgs { get; set; }
        public object Token { get; internal set; }
        public string IMEI { get; set; }
        public int CurrentState { get; set; }
        public string SessionID { get; set; }
        public int AllowedSessions { get; set; }
        //public bool IsAuthenticated { get; set; }
        //public int LoginAttempt { get; set; }
        //public string UserPassAge { get; set; }
    }}