using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSDF_BSS.Reports.Models;

namespace PSDF_BSS.Reports.Interfaces
{
    public interface ISRVProfileVerification
    {
        List<ProfileVerificationModel> GetProfileVerificationReportList(int? schemeId, int? tspId, int? classId, string date);
    }
}