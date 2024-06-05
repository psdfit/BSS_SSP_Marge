using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
    public interface ISRVDeletedDropout
    {
        List<DeletedDropoutModel> GetDeletedDropoutList(AMSReportsParamModel model);

    }
}
