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
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_DVVDeviceRegistration", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable UpdateDeviceStatus(DeviceRegistrationModel data)
        {
           
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@ActivationLogID", data.ActivationLogID));
            param.Add(new SqlParameter("@RegistrationID", data.RegistrationID));
            param.Add(new SqlParameter("@StatusRequest", data.DeviceStatusRequest));
            param.Add(new SqlParameter("@RequestRemarks", data.Remarks));
            param.Add(new SqlParameter("@ApprovalStatus", ""));
            param.Add(new SqlParameter("@InActive", 1));
            param.Add(new SqlParameter("@UserID", data.UserID));

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_DVVDeviceActivationLog", param.ToArray()).Tables[0];
            return dt;
        }
    }

}