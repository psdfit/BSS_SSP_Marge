using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{

public interface ISRVInceptionReportChangeRequest 
{
 public InceptionReportChangeRequestModel GetByInceptionReportChangeRequestID(int InceptionReportChangeRequestID, SqlTransaction transaction = null);
 public List<InceptionReportChangeRequestModel> SaveInceptionReportChangeRequest(InceptionReportChangeRequestModel InceptionReportChangeRequest);
 public List<InceptionReportChangeRequestModel> FetchInceptionReportChangeRequest(InceptionReportChangeRequestModel mod);
 public List<InceptionReportChangeRequestModel> FetchInceptionReportChangeRequest();
 public List<InceptionReportChangeRequestModel> FetchInceptionReportChangeRequest(bool InActive);
 public bool CrInceptionReportApproveReject(InceptionReportChangeRequestModel model, SqlTransaction transaction = null);
 public void ActiveInActive(int InceptionReportChangeRequestID,bool? InActive,int CurUserID);}
}
