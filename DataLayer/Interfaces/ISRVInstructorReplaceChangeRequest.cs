using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
 public bool CrInstructorReplaceApproveReject(InstructorReplaceChangeRequestModel model, SqlTransaction transaction = null);
}