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
    public class SRVContactPerson : SRVBase, DataLayer.Interfaces.ISRVContactPerson
    {
        public SRVContactPerson() { }
        public ContactPersonModel GetByContactPersonID(int ContactPersonID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ContactPersonID", ContactPersonID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ContactPerson", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfContactPerson(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ContactPersonModel> SaveContactPerson(ContactPersonModel ContactPerson)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@ContactPersonID", ContactPerson.ContactPersonID);
                param[1] = new SqlParameter("@ContactPersonName", ContactPerson.ContactPersonName);
                param[2] = new SqlParameter("@ContactPersonEmail", ContactPerson.ContactPersonEmail);
                param[3] = new SqlParameter("@ContactPersonLandline", ContactPerson.ContactPersonLandline);
                param[4] = new SqlParameter("@ContactPersonMobile", ContactPerson.ContactPersonMobile);
                param[5] = new SqlParameter("@IncepReportID", ContactPerson.IncepReportID);

                param[6] = new SqlParameter("@CurUserID", ContactPerson.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ContactPerson]", param);
                return FetchContactPerson();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<ContactPersonModel> LoopinData(DataTable dt)
        {
            List<ContactPersonModel> ContactPersonL = new List<ContactPersonModel>();

            foreach (DataRow r in dt.Rows)
            {
                ContactPersonL.Add(RowOfContactPerson(r));

            }
            return ContactPersonL;
        }
        public List<ContactPersonModel> FetchContactPerson(ContactPersonModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ContactPerson", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ContactPersonModel> FetchContactPerson()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ContactPerson").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ContactPersonModel> GetByContactPersonMobile(string ContactPersonMobile)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ContactPerson", new SqlParameter("@ContactPersonMobile", ContactPersonMobile)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<ContactPersonModel> cp = LoopinData(dt);

                    return cp;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ContactPersonModel> FetchContactPerson(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ContactPerson", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ContactPersonModel> FetchContactPerson(int IncepReportId)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ContactPerson", new SqlParameter("@IncepReportId", IncepReportId)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public int BatchInsert(List<ContactPersonModel> ls, int @BatchFkey, int CurUserID)
        {

            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@IncepReportID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_ContactPerson]", param);
        }
        public void ActiveInActive(int ContactPersonID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ContactPersonID", ContactPersonID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ContactPerson]", PLead);
        }
        private ContactPersonModel RowOfContactPerson(DataRow r)
        {
            ContactPersonModel ContactPerson = new ContactPersonModel();
            ContactPerson.ContactPersonID = Convert.ToInt32(r["ContactPersonID"]);
            ContactPerson.ContactPersonType = r["ContactPersonType"].ToString();
            ContactPerson.ContactPersonName = r["ContactPersonName"].ToString();
            ContactPerson.ContactPersonEmail = r["ContactPersonEmail"].ToString();
            ContactPerson.ContactPersonLandline = r["ContactPersonLandline"].ToString();
            ContactPerson.ContactPersonMobile = r["ContactPersonMobile"].ToString();
            //ContactPerson.IncepReportID = Convert.ToInt32(r["IncepReportID"]);
            ContactPerson.InActive = Convert.ToBoolean(r["InActive"]);
            ContactPerson.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ContactPerson.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ContactPerson.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ContactPerson.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return ContactPerson;
        }
    }
}
