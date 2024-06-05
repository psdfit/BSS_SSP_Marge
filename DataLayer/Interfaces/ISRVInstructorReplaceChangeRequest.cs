using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{public interface ISRVInstructorReplaceChangeRequest { public InstructorReplaceChangeRequestModel GetByInstructorReplaceChangeRequestID(int InstructorReplaceChangeRequestID, SqlTransaction transaction = null); //public List<InstructorReplaceChangeRequestModel> SaveInstructorReplaceChangeRequest(InstructorReplaceChangeRequestModel InstructorReplaceChangeRequest); public List<InstructorReplaceChangeRequestModel> FetchInstructorReplaceChangeRequest(InstructorReplaceChangeRequestModel mod); public List<InstructorReplaceChangeRequestModel> FetchInstructorReplaceChangeRequest(); public List<InstructorReplaceChangeRequestModel> FetchInstructorReplaceChangeRequest(bool InActive);
 public bool CrInstructorReplaceApproveReject(InstructorReplaceChangeRequestModel model, SqlTransaction transaction = null); public void ActiveInActive(int InstructorReplaceChangeRequestID,bool? InActive,int CurUserID);}
}
