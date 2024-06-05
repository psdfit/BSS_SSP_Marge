using PSDF_AMSReports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_AMSReports.Interfaces
{
    public interface IAMSReportService
    {
        Task<AMSFormThreeViewModel> GetAMSFormThreeReport(int? schemeId, int? tspId, int? classId, string dateTime);
        Task<UnverifiedTraineeModel> GetUnverifiedTraineeReport(int? schemeId, int? tspId, int? classId, string dateTime);
    }
}
