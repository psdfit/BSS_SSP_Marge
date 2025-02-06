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
    public class SRVGurnDisbursementStatus : SRVBase, ISRVGurnDisbursementStatus
    {
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVClass srvClass;
        private readonly IDapperConfig _dapper;

        public SRVGurnDisbursementStatus(ISRVSendEmail srvSendEmail, ISRVOrgConfig srvOrgConfig, ISRVClass srvClass, IDapperConfig _dapper)
        {
            this.srvOrgConfig = srvOrgConfig;
            this.srvClass = srvClass;
        }

        private List<GurnDisbursementModel> LoopinData(DataTable dt)
        {
            List<GurnDisbursementModel> GURN = new List<GurnDisbursementModel>();

            foreach (DataRow r in dt.Rows)
            {
                GURN.Add(RowOfGurnDisbursement(r));
            }
            return GURN;
        }
        public List<GurnDisbursementModel> FetchGurnDisbursementStatus()
        {
            try
            {
                //var list = _dapper.Query<GurnDisbursementModel>("RD_TraineesForGurnDisbursment", CommandType.StoredProcedure);
                //return list;

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineesForGurnDisbursment").Tables[0];
                return LoopinData(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<GurnDisbursementModel> FetchGurnDisbursementStatusByFilters(GurnDisbursementFiltersModel model)
        {
            try
            {
                //var list = _dapper.Query<GurnDisbursementModel>("RD_TraineesForGurnDisbursment", CommandType.StoredProcedure);
                //return list;

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Month", model.Month));
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@ClassID", model.ClassID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineesForGurnDisbursment",  param.ToArray()).Tables[0];
                return LoopinData(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void UpdateTraineesDisbursement(List<GurnDisbursementModel> ls)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));


                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "Update_Trainees_Gurn_Disbursement", param);


            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private GurnDisbursementModel RowOfGurnDisbursement(DataRow row)
        {
            GurnDisbursementModel GURN = new GurnDisbursementModel();
            GURN.TraineeID = row.Field<int>("TraineeID");
            GURN.TraineeName = row.Field<string>("TraineeName");
            GURN.TraineeCode = row.Field<string>("TraineeCode");
            GURN.FatherName = row.Field<string>("FatherName");
            GURN.TradeName = row.Field<string>("TradeName");
            GURN.ClassCode = row.Field<string>("ClassCode");
            GURN.ContactNumber = row.Field<string>("ContactNumber");
            GURN.Batch = row.Field<int>("Batch");
            GURN.Amount = row.Field<decimal>("Amount");
            GURN.DistrictName = row.Field<string>("DistrictName");
            GURN.Comments = row.Field<string>("Comments");
            GURN.NumberOfMonths = row.Field<int>("NumberOfMonths");
            GURN.Month = row.Field<DateTime>("Month");
            GURN.SchemeName = row.Field<string>("SchemeName");
            GURN.TSPName = row.Field<string>("TSPName");
            GURN.TraineeCNIC = row.Field<string>("TraineeCNIC");
            GURN.TokenNumber = row.Field<string>("TokenNumber");
            GURN.TransactionNumber = row.Field<string>("TransactionNumber");
            GURN.Redeem = row.Field<string>("Redeem");

            return GURN;
        }


    }
}
