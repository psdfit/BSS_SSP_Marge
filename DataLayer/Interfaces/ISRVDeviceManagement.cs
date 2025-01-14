using DataLayer.JobScheduler.Scheduler;
using DataLayer.Models;
using DataLayer.Models.DVV;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVDeviceManagement
    {
       DataTable Save(DeviceRegistrationModel data);
       DataTable UpdateDeviceStatus(DeviceRegistrationModel Param);
       DataTable GetDeviceRegistration(int? registrationID, int? userID, int? SchemeID, int? TSPID, int? ClassID);
       DataTable GetBiometricAttendanceTrainees(BiometricAttendanceTraineeModel model);
       DataTable GetBiometricAttendanceOnRollTrainees(BiometricAttendanceTraineeModel model);
       DataTable GetTSPDetailsByClassID(int classID);

    }
}
