/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Classes;
using Newtonsoft.Json;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVPOLines : ISRVPOLines
    {
        public List<POLinesModel> FetchPOLines(int id)
        {
            try
            {
                SqlParameter p = new SqlParameter("@id", id);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_POLines", p).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<POLinesModel> GetPOLinesByPOHeaderID(int POHeaderID, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter p = new SqlParameter("@POHeaderID", POHeaderID);
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_POLines", p).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_POLines", p).Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<POLinesModel> LoopinData(DataTable dt)
        {
            List<POLinesModel> POLines = new List<POLinesModel>();

            foreach (DataRow r in dt.Rows)
            {
                POLines.Add(RowOfPOLines(r));
            }

            return POLines;
        }

        private POLinesModel RowOfPOLines(DataRow row)
        {
            POLinesModel POLines = new POLinesModel();

            POLines.POLineID = row.Field<int>("POLineID");
            POLines.POHeaderID = row.Field<int>("POHeaderID");
            POLines.DocEntry = row.Field<int>("DocEntry");
            POLines.DocNumber = row.Field<int>("DocNumber");
            POLines.LineNum = row.Field<int>("LineNum");
            POLines.U_Trainee_Per_Class = row.Field<int>("U_Trainee_Per_Class");
            POLines.LineTotal = row.Field<decimal?>("LineTotal");
            POLines.OcrCode2 = row.Field<string>("OcrCode2");
            POLines.U_Training_Cost = row.Field<decimal?>("U_Training_Cost");
            POLines.U_Stipend = row.Field<decimal?>("U_Stipend");
            POLines.U_Uniform_Bag = row.Field<decimal?>("U_Uniform_Bag");
            POLines.U_Testing_Fee = row.Field<decimal?>("U_Testing_Fee");
            POLines.U_Cost_Trainee_Month = row.Field<decimal?>("U_Cost_Trainee_Month");
            POLines.U_Cost_Trainee_LMont = row.Field<decimal?>("U_Cost_Trainee_LMont");
            POLines.U_Cost_Trainee_FMont = row.Field<decimal?>("U_Cost_Trainee_FMont");
            POLines.U_Cost_Trai_2nd_Last = row.Field<decimal?>("U_Cost_Trai_2nd_Last");
            POLines.U_Cost_Trainee_2Mont = row.Field<decimal?>("U_Cost_Trainee_2Mont");
            POLines.Dscription = row.Field<string>("Dscription");
            POLines.AcctCode = row.Field<string>("AcctCode");
            POLines.OcrCode = row.Field<string>("OcrCode");
            POLines.TaxCode = row.Field<string>("TaxCode");
            POLines.WtLiable = row.Field<string>("WtLiable");
            POLines.LineStatus = row.Field<string>("LineStatus");
            POLines.OcrCode3 = row.Field<string>("OcrCode3");
            POLines.U_Class_Code = row.Field<string>("U_Class_Code");
            POLines.U_Batch = row.Field<string>("U_Batch");
            POLines.U_Batch_Duration = row.Field<string>("U_Batch_Duration");
            POLines.U_Class_Start_Date = row.Field<DateTime>("U_Class_Start_Date");
            POLines.U_Class_End_Date = row.Field<DateTime>("U_Class_End_Date");

            POLines.CreatedDate = row.Field<DateTime?>("CreatedDate");
            POLines.ModifiedDate = row.Field<DateTime?>("ModifiedDate");
            POLines.CreatedUserID = row.Field<int>("CreatedUserID");
            POLines.ModifiedUserID = row.Field<int>("ModifiedUserID");
            //POLines.InActive = Convert.ToBoolean(r["InActive"]);

            return POLines;
        }

        public bool UpdateSAPInPOLines(int poHeaderId, List<Podetail> ls, List<POLinesModel> PoLines, SqlTransaction transaction = null)
        {
            try
            {
                for (int i = 0; i < PoLines.Count; i++)
                {
                    var line = PoLines[i];
                    var sapObj = ls.FirstOrDefault(x => x.LineNum == i);
                    UpdateLines(PoLines[i].POLineID,sapObj.LineNum,transaction);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateLines(int lineId, int lineNum, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@lineId", lineId);
                param[1] = new SqlParameter("@lineNum", lineNum);
                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[AU_POLinesSAPID]", param);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_POLinesSAPID]", param);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}