using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataLayer.Dapper;

namespace DataLayer.Services
{
    public class SRVSrnDisbursementStatus : SRVBase, ISRVSrnDisbursementStatus
    {
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVClass srvClass;
        private readonly IDapperConfig _dapper;

        public SRVSrnDisbursementStatus(ISRVSendEmail srvSendEmail, ISRVOrgConfig srvOrgConfig, ISRVClass srvClass, IDapperConfig _dapper)
        {
            this.srvOrgConfig = srvOrgConfig;
            this.srvClass = srvClass;
        }

        private List<SrnDisbursementModel> LoopinData(DataTable dt)
        {
            List<SrnDisbursementModel> srn = new List<SrnDisbursementModel>();

            foreach (DataRow r in dt.Rows)
            {
                srn.Add(RowOfSrnDisbursement(r));
            }
            return srn;
        }
        public List<SrnDisbursementModel> FetchSrnDisbursementStatus()
        {
            try
            {
                //var list = _dapper.Query<SrnDisbursementModel>("RD_TraineesForSrnDisbursment", CommandType.StoredProcedure);
                //return list;

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineesForSrnDisbursment").Tables[0];
                return LoopinData(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SrnDisbursementModel> FetchSrnDisbursementStatusByFilters(SrnDisbursementFiltersModel model)
        {
            try
            {
                //var list = _dapper.Query<SrnDisbursementModel>("RD_TraineesForSrnDisbursment", CommandType.StoredProcedure);
                //return list;

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Month", model.Month));
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@ClassID", model.ClassID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineesForSrnDisbursment",  param.ToArray()).Tables[0];
                return LoopinData(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void UpdateTraineesDisbursement(List<SrnDisbursementModel> ls)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "Update_Trainees_Srn_Disbursement", param);


            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private SrnDisbursementModel RowOfSrnDisbursement(DataRow row)
        {
            SrnDisbursementModel srn = new SrnDisbursementModel();
            srn.TraineeID = row.Field<int>("TraineeID");
            srn.TraineeName = row.Field<string>("TraineeName");
            srn.TraineeCode = row.Field<string>("TraineeCode");
            srn.FatherName = row.Field<string>("FatherName");
            srn.TradeName = row.Field<string>("TradeName");
            srn.ClassCode = row.Field<string>("ClassCode");
            srn.ContactNumber = row.Field<string>("ContactNumber");
            srn.Batch = row.Field<int>("Batch");
            srn.Amount = row.Field<decimal>("Amount");
            srn.DistrictName = row.Field<string>("DistrictName");
            srn.Comments = row.Field<string>("Comments");
            srn.NumberOfMonths = row.Field<int>("NumberOfMonths");
            srn.Month = row.Field<DateTime>("Month");
            srn.SchemeName = row.Field<string>("SchemeName");
            srn.TSPName = row.Field<string>("TSPName");
            srn.TraineeCNIC = row.Field<string>("TraineeCNIC");
            srn.TokenNumber = row.Field<string>("TokenNumber");
            srn.TransactionNumber = row.Field<string>("TransactionNumber");
            srn.Redeem = row.Field<string>("Redeem");

            return srn;
        }


    }
}
