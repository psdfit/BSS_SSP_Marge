using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Models.DVV;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVDeviceManagement : ISRVDeviceManagement
    {



        public DataTable Save(DeviceRegistrationModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@UserID", data.UserID));
            param.Add(new SqlParameter("@Brand", data.Brand));
            param.Add(new SqlParameter("@Model", data.Model));
            param.Add(new SqlParameter("@SerialNumber", data.SerialNumber));
            param.Add(new SqlParameter("@StatusRequest", data.StatusRequest));
            param.Add(new SqlParameter("@TSPLocation", data.TSPLocation));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_DVVDeviceRegistration", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable UpdateDeviceStatus(DeviceRegistrationModel data)
        {
           
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@RegistrationID", data.RegistrationID));
            param.Add(new SqlParameter("@StatusRequest", data.StatusRequest));
            param.Add(new SqlParameter("@RequestRemarks", data.Remarks));
            param.Add(new SqlParameter("@ApprovalStatus", ""));
            param.Add(new SqlParameter("@InActive", 1));
            param.Add(new SqlParameter("@UserID", data.UserID));
            param.Add(new SqlParameter("@CreatedUserID", data.UserID));

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_DVVDeviceActivationLog", param.ToArray()).Tables[0];
            return dt;
        }

        public DataTable GetDeviceRegistration(int? registrationID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            if (registrationID.HasValue)
            {
                param.Add(new SqlParameter("@RegistrationID", registrationID.Value));
            }
            else
            {
                param.Add(new SqlParameter("@RegistrationID", DBNull.Value));
            }

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_DVVDeviceRegistration", param.ToArray()).Tables[0];
            return dt;
        }

        public DataTable GetBiometricAttendanceTrainees(BiometricAttendanceTraineeModel model)
{
    List<SqlParameter> param = new List<SqlParameter>
    {
        new SqlParameter("@TraineeID", model.TraineeID ?? 0),
        new SqlParameter("@SchemeID", model.SchemeID ?? 0),
        new SqlParameter("@TspID", model.TspID ?? 0),
        new SqlParameter("@ClassID", model.ClassID ?? 0),
        new SqlParameter("@UserID", model.UserID ?? 0),
    };

    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_DVVBiometricAttandanceTrainees", param.ToArray()).Tables[0];
    return dt;
}

    }

}