using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using DataLayer.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DataLayer.Services
{
    public class SRVPaymentSchedule: ISRVPaymentSchedule
    {
        public List<PaymentScheduleModel> GetPaymentSchedules()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SAP_Scheme").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<PaymentScheduleModel> LoopinData(DataTable dt)
        {
            List<PaymentScheduleModel> ps = new List<PaymentScheduleModel>();

            foreach (DataRow r in dt.Rows)
            {
                ps.Add(RowOfPaymentSchedule(r));
            }

            return ps;
        }

        private PaymentScheduleModel RowOfPaymentSchedule(DataRow r)
        {
            PaymentScheduleModel ps = new PaymentScheduleModel();

            ps.SAP_SchemeID = Convert.ToInt32(r["SAP_SchemeID"]);

            ps.Description = r["Description"].ToString();
            ps.PaymentStructure = r["PaymentStructure"].ToString();
            ps.SchemeCode = r["SchemeCode"].ToString();

            return ps;
        }
    }
}
