using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Services
{
    public class SRVAAdvanceSearch : SRVBase, ISRVAdvanceSearch
    {
        public SRVAAdvanceSearch() { }
       
        public List<AdvancedSearchModel> AdvanceSearch(AdvancedSearchModel search)
        {
            try
            {
                var param = new[] {
                new SqlParameter("@SearchString",search.SearchString),
                new SqlParameter("@SearchTables", search.SearchTables)
                };

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AdvanceSearch", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        private List<AdvancedSearchModel> LoopinData(DataTable dt)
        {
            List<AdvancedSearchModel> search = new List<AdvancedSearchModel>();

            foreach (DataRow r in dt.Rows)
            {
                search.Add(RowOfSearchStatus(r));

            }
            return search;
        }
        private AdvancedSearchModel RowOfSearchStatus(DataRow r)
        {
            AdvancedSearchModel adsearch = new AdvancedSearchModel();

            adsearch.SchemaName = r.Field<string>("SchemaName");
            adsearch.TableName = r.Field<string>("TableName");
            adsearch.TablePKName = r.Field<string>("TablePKName");
            adsearch.TablePKValue = r.Field<int>("TablePKValue");
            adsearch.ColumnName = r.Field<string>("ColumnName");
            adsearch.ColumnValue = r.Field<string>("ColumnValue");
            adsearch.JsonRow = r.Field<string>("JsonRow");
            return adsearch;
        }
        public DataSet GetTraineeProfile(int TraineeID) {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@TraineeID", TraineeID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "ADS_Trainee", param);
            
            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ds.Tables[0].Rows[0]["TraineeImg"] = string.IsNullOrEmpty(ds.Tables[0].Rows[0].Field<string>("TraineeImg")) ? string.Empty : Common.GetFileBase64(ds.Tables[0].Rows[0]["TraineeImg"].ToString());
            ds.Tables[0].Rows[0]["CNICImgNADRA"] = string.IsNullOrEmpty(ds.Tables[0].Rows[0].Field<string>("CNICImgNADRA")) ? string.Empty : Common.GetFileBase64(ds.Tables[0].Rows[0]["CNICImgNADRA"].ToString());
            
            ds.Tables[0].TableName = "TraineeProfile";
            ds.Tables[1].TableName = "MPRDetail";

            return ds;
        }
        public DataSet GetInstructorProfile(int InstructorID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@InstructorID", InstructorID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "ADS_Instructor", param);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ds.Tables[0].Rows[0]["PicturePath"] = string.IsNullOrEmpty(ds.Tables[0].Rows[0].Field<string>("PicturePath")) ? Common.GetFileBase64(@"\Documents\Instructor\default.png") : Common.GetFileBase64(ds.Tables[0].Rows[0]["PicturePath"].ToString());

            ds.Tables[0].TableName = "InstructorProfile";
            ds.Tables[1].TableName = "ClassesDetail";

            return ds;
        }
        public DataSet GetClassDetail(int ClassID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@ClassID", ClassID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "ADS_Class", param);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ds.Tables[0].TableName = "ClassDetail";
            ds.Tables[1].TableName = "ClassStatuses";
            ds.Tables[2].TableName = "ClassInception";
            ds.Tables[3].TableName = "ClassInstructors";
            ds.Tables[4].TableName = "ClassMPR";
            ds.Tables[5].TableName = "ClassSRN";
            ds.Tables[6].TableName = "ClassInvoices";
            ds.Tables[7].TableName = "ClassPO";

            return ds;
        }
        public DataSet GetTSPDetail(int ClassID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@TSPMasterID", ClassID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "ADS_TSP", param);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ds.Tables[0].TableName = "TSPDetail";
            ds.Tables[1].TableName = "TSPSchemes";
            ds.Tables[2].TableName = "TSPClasses";
            ds.Tables[3].TableName = "TSPInstructors";
            ds.Tables[4].TableName = "TSPPRN";
            ds.Tables[5].TableName = "TSPINV";

            return ds;
        }
        public DataSet GetSchemeDetail(int ClassID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@SchemeID", ClassID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "ADS_Scheme", param);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ds.Tables[0].TableName = "SchemeDetail";
            ds.Tables[1].TableName = "SchemeTSPs";
            ds.Tables[2].TableName = "SchemeClasses";

            return ds;
        }
        public DataSet GetMPRDetail(int MPRID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@MPRID", MPRID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "ADS_RD_MPR", param);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ds.Tables[0].TableName = "MPR";

            return ds;
        }
        public DataSet GetPRNDetail(int PRNID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@PRNMasterID", PRNID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "ADS_RD_PRNMasterByMasterID", param);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ds.Tables[0].TableName = "PRN";

            return ds;
        }
        public DataSet GetSRNDetail(int SRNID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@SRNID", SRNID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "ADS_RD_SRN", param);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ds.Tables[0].TableName = "SRN";

            return ds;
        }
        public DataSet GetInvoiceDetail(int InvoiceID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@InvoiceID", InvoiceID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "ADS_RD_Invoice", param);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ds.Tables[0].TableName = "Invoice";

            return ds;
        }
    }
}

