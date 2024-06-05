using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
   public interface ISRVComplaint
    {
        void ActiveInActive(int? ComplainantID, bool? InActive, int CurUserID);
        long saveComplaint(ComplaintModel model, out string errMsg);
        DataTable GetAllComplaintTypesAndSubTypes();
        List<ComplaintModel> FetchComplaintTypeForCRUD();
        List<ComplaintModel> FetchComplaintSubTypeForCRUD(int? ComplaintTypeID);
        List<ComplaintModel> FetchComplaintForGridView(int? CurUserID, int? ComplaintStatusTypeID);
        List<ComplaintModel> FetchComplaintOfTSPSelf(int? CurUserID);
        List<ComplaintModel> GetComplaintStatusType();
        bool complaintStatusChange(ComplaintModel model);
        List<ComplaintModel> FetchComplaintHistory(int Complaintid);
        List<ComplaintModel> FetchComplaintAttachments(int? id);
        List<TSPDetailModel> FetchTSPMasterDataByUser(int UserID);
        List<ComplaintModel> FetchTraineeAndTSPComplaintForKAM(int? CurUserID, int? ComplaintStatusTypeID);
        List<ComplaintModel> FetchComplaintStatusAttachments(int? ComplaintStatusDetailID);
        ComplaintModel GETTraineeByCnicAndTSPUserID(string TraineeCNIC, int? UserID);
        List<ComplaintModel> GetComplaintHistoryByCompaintNoApi(string? ComplaintNo);
        //long SaveComplaintFromApi(ComplaintModel model, out string errMsg);
    }
}
