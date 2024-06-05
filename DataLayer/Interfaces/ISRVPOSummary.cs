using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVPOSummary
    {
        public List<POSummaryModel> GetPOSummary(DateTime Month,int SchemeID,int TSPID, string ProcessKey);
    }
}
