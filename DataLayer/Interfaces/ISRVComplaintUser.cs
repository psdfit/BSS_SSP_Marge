using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
  public  interface ISRVComplaintUser
    {
        bool saveComplaintUser(ComplaintModel model);
        List<ComplaintModel> FetchComplaintUserForGridView();



    }
}
