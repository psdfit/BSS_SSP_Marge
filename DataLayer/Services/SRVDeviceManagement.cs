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
            param.Add(new SqlParameter("@TSPID", data.TSPID));
            param.Add(new SqlParameter("@ClassID", data.ClassID));
            param.Add(new SqlParameter("@SchemeID", data.SchemeID));
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

        public DataTable GetDeviceRegistration(int? registrationID, int? userID, int? SchemeID, int? TSPID, int? ClassID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            // Add RegistrationID filter if provided
            param.Add(new SqlParameter("@RegistrationID", registrationID ?? (object)DBNull.Value));

            // Add UserID filter if provided
            param.Add(new SqlParameter("@UserID", userID ?? (object)DBNull.Value));

            // Add SchemeID filter if provided
            param.Add(new SqlParameter("@SchemeID", SchemeID ?? (object)DBNull.Value));

            // Add TSPID filter if provided
            param.Add(new SqlParameter("@TSPID", TSPID ?? (object)DBNull.Value));

            // Add ClassID filter if provided
            param.Add(new SqlParameter("@ClassID", ClassID ?? (object)DBNull.Value));

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
        
        public DataTable GetBiometricAttendanceOnRollTrainees(BiometricAttendanceTraineeModel model)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@TraineeID", model.TraineeID ?? 0),
                new SqlParameter("@SchemeID", model.SchemeID ?? 0),
                new SqlParameter("@TspID", model.TspID ?? 0),
                new SqlParameter("@ClassID", model.ClassID ?? 0),
                new SqlParameter("@UserID", model.UserID ?? 0),
            };

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_DVVBiometricAttendanceEnrolledTrainees", param.ToArray()).Tables[0];
            return dt;
        }

        public DataTable GetTSPDetailsByClassID(int classID)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@ClassID", classID)
            };

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailByClassID_DVV", param.ToArray()).Tables[0];
            return dt;
        }
    }

}