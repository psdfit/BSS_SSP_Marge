using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVComplaint : ISRVComplaint
    {
        public DataTable GetAllComplaintTypesAndSubTypes()
        {
            try
            {
                var dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AllComplaintTypesAndSubTypes");

                return (dt.Tables[0]);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ComplaintModel> FetchComplaintTypeForCRUD()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ComplaintType").Tables[0];
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);

                return (ComplaintModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ComplaintModel> FetchComplaintSubTypeForCRUD(int? ComplaintTypeID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ComplaintTypeID", ComplaintTypeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ComplaintSubType", param).Tables[0];
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);

                return (ComplaintModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public long saveComplaint(ComplaintModel model, out string errMsg)
        {
            try
            {
                errMsg = string.Empty;
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@ComplainantID", model.ComplainantID);
                param[1] = new SqlParameter("@CurUserID", model.CurUserID);
                param[2] = new SqlParameter("@ComplainantName", "null");
                param[3] = new SqlParameter("@ComplaintDescription", model.ComplaintDescription);
                param[4] = new SqlParameter("@ComplaintStatusTypeID", 1);
                param[5] = new SqlParameter("@ComplaintTypeID", model.ComplaintTypeID);
                param[6] = new SqlParameter("@ComplaintSubTypeID", model.ComplaintSubTypeID);
                param[7] = new SqlParameter("@FileGuid", model.FilePath);
                param[8] = new SqlParameter("@Submitted", model.Submitted);
                param[9] = new SqlParameter("@TraineeID", model.TraineeID);
                param[10] = new SqlParameter("@TSPMasterID", model.TSPMasterID);
                param[11] = new SqlParameter("@ComplaintRegisterType", model.ComplaintRegisterType);
                param[12] = new SqlParameter("@Returncomplaint_No", SqlDbType.Int);
                param[12].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Complaint]", param);
                long complaint_No = Convert.ToInt32(param[12].Value);
                return complaint_No;
            }
            catch (Exception ex) { throw new Exception(ex.Message); errMsg = ex.Message; }
        }
        public List<ComplaintModel> FetchComplaintForGridView(int? CurUserID, int? ComplaintStatusTypeID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Userid", CurUserID);
                param[1] = new SqlParameter("@ComplaintStatusTypeID", ComplaintStatusTypeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_Complaint]", param).Tables[0];
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);
                return (ComplaintModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ComplaintModel> FetchComplaintOfTSPSelf(int? CurUserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_ComplaintTSP]", new SqlParameter("@Userid", CurUserID)).Tables[0];
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);
                return (ComplaintModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int? ComplainantID, bool? InActive, int CurUserID)
        {

            try
            {
                SqlParameter[] PLead = new SqlParameter[3];
                PLead[0] = new SqlParameter("@ComplainantID", ComplainantID);
                PLead[1] = new SqlParameter("@InActive", InActive);
                PLead[2] = new SqlParameter("@CurUserID", CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Complaint]", PLead);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ComplaintModel> GetComplaintStatusType()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_ComplaintStatusType]").Tables[0];
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);

                return (ComplaintModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool complaintStatusChange(ComplaintModel model)
        {
            try
            {
                SqlParameter[] PLead = new SqlParameter[6];
                PLead[0] = new SqlParameter("@ComplainantID", model.ComplainantID);
                PLead[1] = new SqlParameter("@ComplaintStatusDetailID", model.ComplaintStatusDetailID);
                PLead[2] = new SqlParameter("@ComplaintStatusTypeID", model.ComplaintStatusTypeID);
                PLead[3] = new SqlParameter("@CurUserID", model.CurUserID);
                PLead[4] = new SqlParameter("@complaintStatusDetailComments", model.complaintStatusDetailComments);
                PLead[5] = new SqlParameter("@FilePath", model.FilePath);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ComplainStatusChange]", PLead);
                return true;

            }
            catch (Exception ex) { throw new Exception(ex.Message); return false; }
        }
        public List<ComplaintModel> FetchComplaintHistory(int Complaintid)
        {
            try
            {

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_ComplaintHistory]", new SqlParameter("@ComplaintID", Complaintid)).Tables[0];
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);
                return (ComplaintModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ComplaintModel> FetchComplaintAttachments(int? ComplainantID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_Complaint_AttachementsByID]", new SqlParameter("@ComplainantID", ComplainantID)).Tables[0];
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);
                ComplaintModel.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return (ComplaintModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TSPDetailModel> FetchTSPMasterDataByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));
                List<TSPDetailModel> TSPDetailModel = new List<TSPDetailModel>();
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_FetchTSPMasterDataByUser]", param.ToArray()).Tables[0];
                TSPDetailModel = Helper.ConvertDataTableToModel<TSPDetailModel>(dt);
                return (TSPDetailModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ComplaintModel> FetchTraineeAndTSPComplaintForKAM(int? CurUserID, int? ComplaintStatusTypeID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Userid", CurUserID);
                param[1] = new SqlParameter("@ComplaintStatusTypeID", ComplaintStatusTypeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TraineeComplaintForKAM]", param).Tables[0];
                List<ComplaintModel> TraineeComplaint = new List<ComplaintModel>();
                TraineeComplaint = Helper.ConvertDataTableToModel<ComplaintModel>(dt);
                SqlParameter[] param2 = new SqlParameter[3];
                param2[0] = new SqlParameter("@Userid", CurUserID);
                param2[1] = new SqlParameter("@ComplaintStatusTypeID", ComplaintStatusTypeID);
                DataTable dt2 = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPComplaintForKAM]", param2).Tables[0];
                List<ComplaintModel> TSPComplaint = new List<ComplaintModel>();
                TSPComplaint = Helper.ConvertDataTableToModel<ComplaintModel>(dt2);
                TraineeComplaint.AddRange(TSPComplaint);
                List<ComplaintModel> LIST = TraineeComplaint.OrderByDescending(x => x.CreatedDate).ToList();

                return (LIST);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ComplaintModel> FetchComplaintStatusAttachments(int? ComplaintStatusDetailID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_ComplaintStatus_AttachementsByID]", new SqlParameter("@ComplaintStatusDetailID", ComplaintStatusDetailID)).Tables[0];
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);
                ComplaintModel.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return (ComplaintModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public ComplaintModel GETTraineeByCnicAndTSPUserID(string TraineeCNIC, int? UserID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@TraineeCNIC", TraineeCNIC);
                param[1] = new SqlParameter("@UserID", UserID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GET_traineeByTSPUserIDAndCNIC", param).Tables[0];
                List<ComplaintModel> TraineeInfo = new List<ComplaintModel>();
                TraineeInfo = Helper.ConvertDataTableToModel<ComplaintModel>(dt);
                if (TraineeInfo.Count > 0)
                    return (TraineeInfo[0]);
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ComplaintModel> GetComplaintHistoryByCompaintNoApi(string ComplaintNo)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ComplaintNo", ComplaintNo);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetComplaintHistoryByCompaintNo]", param).Tables[0];
                List<ComplaintModel> ComplaintHistory = new List<ComplaintModel>();
                ComplaintHistory = Helper.ConvertDataTableToModel<ComplaintModel>(dt);

                return (ComplaintHistory);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
