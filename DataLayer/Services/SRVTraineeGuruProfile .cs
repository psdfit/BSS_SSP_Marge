using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;

namespace DataLayer.Services
{
    public class SRVTraineeGuruProfile : ISRVTraineeGuruProfile
    {
        // Get profiles by TraineeProfileID using SqlHelper
        public  DataTable GetByTraineeProfileID(int UserID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GuruProfile", param.ToArray()).Tables[0];
            return dt;
        }

         public  DataTable GetTraineeGuruDetail(int traineeID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TraineeID", traineeID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeGuruProfiles", param.ToArray()).Tables[0];
            return dt;
        }

        // Insert function using SqlHelper
        public  bool SaveTraineeGuruProfile(TraineeGuruModel model)
        {
                var param = new List<SqlParameter>();

                param.Add(new SqlParameter("@GuruProfileID", model.GuruProfileID));
                param.Add(new SqlParameter("@FullName", model.FullName));
                param.Add(new SqlParameter("@ContactNumber", model.ContactNumber));
                param.Add( new SqlParameter("@CNIC", model.CNIC));
                param.Add( new SqlParameter("@CNICIssuedDate", model.CNICIssuedDate));
                param.Add( new SqlParameter("@UserID", model.CurUserID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_GuruProfile", param.ToArray());
            return true;
        }

        // Update function using SqlHelper
        //public async bool UpdateTraineeGuruProfile(TraineeGuruModel model)
        //{
        //    var param = new List<SqlParameter>
        //    {
        //        new SqlParameter("@FullName", model.FullName),
        //        new SqlParameter("@ContactNumber", model.ContactNumber),
        //        new SqlParameter("@CNIC", model.CNIC),
        //        new SqlParameter("@CNICIssuedDate", model.CNICIssuedDate),
        //        //new SqlParameter("@TraineeProfileID", model.TraineeProfileID),
        //        new SqlParameter("@UserID", model.CreatedUserID),
        //        new SqlParameter("@TraineeGuruProfileID", model.TraineeGuruProfileID)
        //    };

        //    try
        //    {
        //        int result = await Task.Run(() =>
        //            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_TraineeGuruProfile", param.ToArray()));

        //        return result > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error updating TraineeGuruProfile data", ex);
        //    }
        //}

        //// Delete function using SqlHelper
        //public async bool DeleteByTraineeProfileID(int traineeProfileID)
        //{
        //    var param = new List<SqlParameter>
        //    {
        //        new SqlParameter("@TraineeProfileID", traineeProfileID)
        //    };

        //    try
        //    {
        //        int result = await Task.Run(() =>
        //            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "SP_DeleteTraineeGuruProfile", param.ToArray()));

        //        return result > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error deleting TraineeGuruProfile data", ex);
        //    }
        //}
    }
}
