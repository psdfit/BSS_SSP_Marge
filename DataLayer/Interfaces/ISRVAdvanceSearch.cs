using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVAdvanceSearch
    {

        List<AdvancedSearchModel> AdvanceSearch(AdvancedSearchModel search);
        DataSet GetTraineeProfile(int TraineeID);
        DataSet GetInstructorProfile(int InstructorID);
        DataSet GetClassDetail(int ClassID);
        DataSet GetTSPDetail(int TSPMasterID);
        DataSet GetSchemeDetail(int SchemeID);
        DataSet GetMPRDetail(int MPRID);
        DataSet GetPRNDetail(int PRNID);
        DataSet GetSRNDetail(int SRNID);
        DataSet GetInvoiceDetail(int InvoiceID);
    }
}
