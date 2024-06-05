using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
    public interface ISRVCovidMaskViolation
    {
        List<CovidMaskViolationModel> GetCovidMaskViolationList(AMSReportsParamModel model);

    }
}
