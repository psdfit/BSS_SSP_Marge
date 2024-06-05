using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVInstructorMaster : SRVBase, ISRVInstructorMaster
    {
        public SRVInstructorMaster() { }
        public InstructorMasterModel GetByInstrMasterID(int InstrMasterID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@InstrMasterID", InstrMasterID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorMaster", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfInstructorMaster(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorMasterModel> SaveInstructorMaster(InstructorMasterModel InstructorMaster)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@InstrMasterID", InstructorMaster.InstrMasterID);
                param[1] = new SqlParameter("@InstructorName", InstructorMaster.InstructorName);
                param[2] = new SqlParameter("@CNICofInstructor", InstructorMaster.CNICofInstructor);
                param[3] = new SqlParameter("@QualificationHighest", InstructorMaster.QualificationHighest);
                param[4] = new SqlParameter("@TotalExperience", InstructorMaster.TotalExperience);
                param[5] = new SqlParameter("@GenderID", InstructorMaster.GenderID);
                param[6] = new SqlParameter("@TradeID", InstructorMaster.TradeID);
                param[7] = new SqlParameter("@LocationAddress", InstructorMaster.LocationAddress);
                param[8] = new SqlParameter("@NameOfOrganization", InstructorMaster.NameOfOrganization);
                param[9] = new SqlParameter("@PicturePath", InstructorMaster.PicturePath);

                param[10] = new SqlParameter("@CurUserID", InstructorMaster.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_InstructorMaster]", param);
                return FetchInstructorMaster();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<InstructorMasterModel> LoopinData(DataTable dt)
        {
            List<InstructorMasterModel> InstructorMasterL = new List<InstructorMasterModel>();

            foreach (DataRow r in dt.Rows)
            {
                InstructorMasterL.Add(RowOfInstructorMaster(r));

            }
            return InstructorMasterL;
        }
        public List<InstructorMasterModel> FetchInstructorMaster(InstructorMasterModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorMaster", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InstructorMasterModel> FetchInstructorMaster()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorMaster").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorMasterModel> FetchInstructorMaster(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorMaster", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorMasterModel> GetByGenderID(int GenderID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorMaster", new SqlParameter("@GenderID", GenderID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InstructorMasterModel> GetByTradeID(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorMaster", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int InstrMasterID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@InstrMasterID", InstrMasterID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_InstructorMaster]", PLead);
        }
        private InstructorMasterModel RowOfInstructorMaster(DataRow r)
        {
            InstructorMasterModel InstructorMaster = new InstructorMasterModel();
            InstructorMaster.InstrMasterID = Convert.ToInt32(r["InstrMasterID"]);
            InstructorMaster.InstructorName = r["InstructorName"].ToString();
            InstructorMaster.CNICofInstructor = r["CNICofInstructor"].ToString();
            InstructorMaster.QualificationHighest = r["QualificationHighest"].ToString();
            InstructorMaster.TotalExperience = r["TotalExperience"].ToString();
            InstructorMaster.GenderID = Convert.ToInt32(r["GenderID"]);
            InstructorMaster.GenderName = r["GenderName"].ToString();
            InstructorMaster.TradeID = Convert.ToInt32(r["TradeID"]);
            InstructorMaster.TradeName = r["TradeName"].ToString();
            InstructorMaster.LocationAddress = r["LocationAddress"].ToString();
            InstructorMaster.NameOfOrganization = r["NameOfOrganization"].ToString();
            InstructorMaster.PicturePath = r["PicturePath"].ToString();
            InstructorMaster.InActive = Convert.ToBoolean(r["InActive"]);
            InstructorMaster.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            InstructorMaster.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            InstructorMaster.CreatedDate = r["CreatedDate"].ToString().GetDate();
            InstructorMaster.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return InstructorMaster;
        }
    }
}
