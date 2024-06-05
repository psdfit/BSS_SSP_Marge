/* **** Aamer Rehman Malik *****/

using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVMPR
    {
        MPRModel GetByMPRID(int MPRID);

        List<MPRModel> SaveMPR(MPRModel MPR);

        List<MPRModel> FetchMPR(MPRModel model);

        List<MPRModel> FetchMPR();

        List<MPRModel> FetchMPR(bool InActive);

        void ActiveInActive(int MPRID, bool? InActive, int CurUserID);

        public DataSet GetClassMonthview(int ClassID, DateTime? Month,string Type);

        public DataTable GetMPRview(int MPRID);
        public List<MPRModel> FetchMPRByKAM(MPRModel model);


        //public DataTable GetPRNview(int PRNID);
        //public DataTable GetInvoiceview(int InvoiceID);
    }
}