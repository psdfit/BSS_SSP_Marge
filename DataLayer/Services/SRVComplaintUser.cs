using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Services
{
  public  class SRVComplaintUser: ISRVComplaintUser
    {

        public bool saveComplaintUser(ComplaintModel model)
        {
            bool bit;
            try
            {
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                if (model.ComplaintTypeID!=null && model.ComplaintSubTypeID!=null&&(model.ComplaintUserID==null|| model.ComplaintUserID == 0))
                {
                    SqlParameter[] param2 = new SqlParameter[6];
                    param2[0] = new SqlParameter("@ComplaintTypeID", model.ComplaintTypeID);
                    param2[1] = new SqlParameter("@ComplaintSubTypeID", model.ComplaintSubTypeID);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_ComplaintUser]", param2).Tables[0];
                  
                    ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);
                }
                if (ComplaintModel.Count==0)
                {
                    SqlParameter[] param = new SqlParameter[6];
                    param[0] = new SqlParameter("@ComplaintUserID", model.ComplaintUserID);
                    param[1] = new SqlParameter("@CurUserID", model.CurUserID);
                    param[2] = new SqlParameter("@UserIDs", model.UserIDs);
                    param[3] = new SqlParameter("@ComplaintTypeID", model.ComplaintTypeID);
                    param[4] = new SqlParameter("@ComplaintSubTypeID", model.ComplaintSubTypeID);
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ComplaintUser]", param);
                    return true;
                }
                return false;

            }
            catch (Exception ex) { throw new Exception(ex.Message); return false; }
        }
        public List<ComplaintModel> FetchComplaintUserForGridView()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_ComplaintUser]").Tables[0];
                List<ComplaintModel> ComplaintModel = new List<ComplaintModel>();
                ComplaintModel = Helper.ConvertDataTableToModel<ComplaintModel>(dt);
                return (ComplaintModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
