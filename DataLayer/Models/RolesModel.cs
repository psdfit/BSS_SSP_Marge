using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]
    public class RolesModel : ModelBase
    {
        public RolesModel() : base()
        {
        }

        public RolesModel(bool InActive) : base(InActive)
        {
        }

        public int RoleID { get; set; }
        public string RoleTitle { get; set; }
        public List<RolesRightsModel> RoleRights { get; set; }
        public List<RolesRightsModel> DiffRoleRights { get; set; }
    }}