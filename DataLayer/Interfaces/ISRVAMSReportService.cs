using DataLayer.Models;
using DataLayer.Models.AMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ISRVAMSReportService
    {
        Task<AMSFormThreeViewModel> GetAMSFormThreeReport(int? schemeId, int? tspId, int? classId, string dateTime);
        Task<UnverifiedTraineeModel> GetUnverifiedTraineeReport(int? schemeId, int? tspId, int? classId, string dateTime);
        Task<FormFourModel> GetFormFourReport(int? schemeId, int? tspId, int? classId, string dateTime);
        Task<ProfileVerificationModel> GetProfileVerificationReport(int? schemeId, int? tspId, int? classId, string dateTime);
    }
}
