using DataLayer.Models;
using DataLayer.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVManualNoteGeneration : ISRVManualNoteGeneration
    {

        public DataTable FetchEligibleClassDataForPVRN(QueryFilters model)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_IsEligibleClassesForPVRN", param).Tables[0];
            return dt;
        }

        public DataTable GeneratePVRN(QueryFilters model, out string IsGenerated)
        {
            SqlParameter[] param = new SqlParameter[2];
            IsGenerated = null;
            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PVRN", param.ToArray());
            IsGenerated = ds.Tables.Count>0 ? true.ToString() : false.ToString();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            return null;
        }

        public DataTable FetchEligibleClassDataForMRN(QueryFilters model)
        {
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_IsEligibleClassesForMRN", param).Tables[0];
            return dt;
        }

        public DataTable GenerateMRN(QueryFilters model, out string IsGenerated)
        {
            SqlParameter[] param = new SqlParameter[5];
            IsGenerated = null;
            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_MRN", param.ToArray());
            IsGenerated = ds.Tables.Count > 0 ? true.ToString() : false.ToString();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            return null;
        }
        public DataTable FetchEligibleClassDataForPCRN(QueryFilters model)
        {
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_IsEligibleClassesForPCRN", param).Tables[0];
            return dt;
        }

        public DataTable GeneratePCRN(QueryFilters model, out string IsGenerated)
        {
            SqlParameter[] param = new SqlParameter[5];
            IsGenerated = null;
            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_PCRN", param.ToArray());
            IsGenerated = ds.Tables.Count > 0 ? true.ToString() : false.ToString();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            return null;
        }
        public DataTable FetchEligibleClassDataForOTRN(QueryFilters model)
        {
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_IsEligibleClassesForOTRN", param).Tables[0];
            return dt;
        }

        public DataTable GenerateOTRN(QueryFilters model, out string IsGenerated)
        {
            SqlParameter[] param = new SqlParameter[5];
            IsGenerated = null;
            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_OTRN", param.ToArray());
            IsGenerated = ds.Tables.Count > 0 ? true.ToString() : false.ToString();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            return null;
        }
    }
}
