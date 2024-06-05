using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace DataLayer.Services
{
    public class SRVWebsite : SRVBase, ISRVWebsite
    {
        public SRVWebsite() { }

        public DataTable GetDistricts(string ModifiedDate)
        {
            try
            {
                string query = "SELECT DistrictID, DistrictName, DistrictNameUrdu, CreatedDate, ModifiedDate from District where InActive=0";

                if (!String.IsNullOrEmpty(ModifiedDate))
                {
                    DateTime date;
                    if (!DateTime.TryParse(ModifiedDate, out date)) { throw new Exception("Date should be in YYYY-MM-DD"); }

                    query = query + " and cast(ISNULL(ModifiedDate, CreatedDate) as date) = " + "'" + ModifiedDate + "'";
                }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, query);
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetTehsils(string ModifiedDate)
        {
            try
            {
                string query = "select TehsilID, TehsilName, DistrictID, CreatedDate, ModifiedDate from Tehsil where InActive=0";

                if (!String.IsNullOrEmpty(ModifiedDate))
                {
                    DateTime date;
                    if (!DateTime.TryParse(ModifiedDate, out date)) { throw new Exception("Date should be in YYYY-MM-DD"); }

                    query = query + " and cast(ISNULL(ModifiedDate, CreatedDate) as date) = " + "'" + ModifiedDate + "'";
                }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, query);
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetSectors(string ModifiedDate)
        {
            try
            {
                string query = "select SectorID, SectorName, CreatedDate, ModifiedDate from Sector where InActive=0";

                if (!String.IsNullOrEmpty(ModifiedDate))
                {
                    DateTime date;
                    if (!DateTime.TryParse(ModifiedDate, out date)) { throw new Exception("Date should be in YYYY-MM-DD"); }

                    query = query + " and cast(ISNULL(ModifiedDate, CreatedDate) as date) = " + "'" + ModifiedDate + "'";
                }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, query);
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetSubSectors(string ModifiedDate)
        {
            try
            {
                string query = "select SubSectorID, SubSectorName, SectorID, CreatedDate, ModifiedDate from SubSector where InActive=0";

                if (!String.IsNullOrEmpty(ModifiedDate))
                {
                    DateTime date;
                    if (!DateTime.TryParse(ModifiedDate, out date)) { throw new Exception("Date should be in YYYY-MM-DD"); }

                    query = query + " and cast(ISNULL(ModifiedDate, CreatedDate) as date) = " + "'" + ModifiedDate + "'";
                }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, query);
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetTrades(string ModifiedDate)
        {
            try
            {
                string query = "select TradeID, TradeName, TradeCode, SectorID, SubSectorID, CreatedDate, ModifiedDate from Trade where IsApproved=1 and InActive=0";

                if (!String.IsNullOrEmpty(ModifiedDate))
                {
                    DateTime date;
                    if (!DateTime.TryParse(ModifiedDate, out date)) { throw new Exception("Date should be in YYYY-MM-DD"); }

                    query = query + " and cast(ISNULL(ModifiedDate, CreatedDate) as date) = " + "'" + ModifiedDate + "'";
                }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, query);
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable GetBSSTrades(string ModifiedDate)
        {
            try
            {
            //    SqlParameter[] param = new SqlParameter[1];
            //    param[0] = new SqlParameter("@ModifiedDate", ModifiedDate);

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ModifiedDate", ModifiedDate));

                DataTable ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_BSS_Trades_ForAPI", param.ToArray()).Tables[0];
                return ds;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public DataTable GetClasses(string ModifiedDate)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];

                if (!String.IsNullOrEmpty(ModifiedDate))
                {
                    DateTime date;
                    if (!DateTime.TryParse(ModifiedDate, out date)) { throw new Exception("Date should be in YYYY-MM-DD"); }
                    param[0] = new SqlParameter("@ModifiedDate", ModifiedDate);
                }
                else { param[0] = new SqlParameter("@ModifiedDate", ""); }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "Website_Classes", param);
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetGenders(string ModifiedDate)
        {
            try
            {
                string query = "select GenderID, GenderName, CreatedDate, ModifiedDate from Gender where InActive=0";

                if (!String.IsNullOrEmpty(ModifiedDate))
                {
                    DateTime date;
                    if (!DateTime.TryParse(ModifiedDate, out date)) { throw new Exception("Date should be in YYYY-MM-DD"); }

                    query = query + " and cast(ISNULL(ModifiedDate, CreatedDate) as date) = " + "'" + ModifiedDate + "'";
                }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, query);
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetEducationTypes(string ModifiedDate)
        {
            try
            {
                string query = "select EducationTypeID, Education, CreatedDate, ModifiedDate from EducationTypes where InActive=0";

                if (!String.IsNullOrEmpty(ModifiedDate))
                {
                    DateTime date;
                    if (!DateTime.TryParse(ModifiedDate, out date)) { throw new Exception("Date should be in YYYY-MM-DD"); }

                    query = query + " and cast(ISNULL(ModifiedDate, CreatedDate) as date) = " + "'" + ModifiedDate + "'";
                }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, query);
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public string SavePotentialTrainee(PotentialTraineesModel Trainee)
        {
            try
            {
                string errMsg = "";
                if (!IsValidCNICFormat(Trainee.TraineeCNIC, out errMsg))
                {
                    throw new Exception(errMsg);
                }
                if (!IsValidMobileNoFormat(Trainee.TraineePhone, out errMsg))
                {
                    throw new Exception(errMsg);
                }
                if (!IsValidEmail(Trainee.TraineeEmail, out errMsg))
                {
                    throw new Exception(errMsg);
                }

                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@TraineeName", Trainee.TraineeName);
                param[1] = new SqlParameter("@TraineeCNIC", Trainee.TraineeCNIC);
                param[2] = new SqlParameter("@TraineeEmail", Trainee.TraineeEmail);
                param[3] = new SqlParameter("@TraineePhone", Trainee.TraineePhone);
                param[4] = new SqlParameter("@GenderID", Trainee.GenderID);
                param[5] = new SqlParameter("@EducationID", Trainee.EducationID);
                param[6] = new SqlParameter("@DistrictID", Trainee.DistrictID);
                param[7] = new SqlParameter("@TehsilID", Trainee.TehsilID);
                param[8] = new SqlParameter("@TradeID", Trainee.TradeID);
                param[9] = new SqlParameter("@TimeStamp", Trainee.TimeStamp);
                param[10] = new SqlParameter("@ClassCode", Trainee.ClassCode);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), "[AU_PotentialTrainee]", param);

                return "Trainee saved successfully.";
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool IsValidCNICFormat(string cnic, out string errMsg)
        {
            Regex regex = new Regex(@"^[0-9]{5}-[0-9]{7}-[0-9]{1}$");
            bool isValid = regex.IsMatch(cnic);
            errMsg = isValid ? string.Empty : $"Invalid CNIC Format, it should be xxxxx-xxxxxxx-x";
            return isValid;

        }
        public bool IsValidMobileNoFormat(string mobileNo, out string errMsg)
        {
            Regex regex = new Regex(@"^[0-9]{4}-[0-9]{7}$");
            bool isValid = regex.IsMatch(mobileNo);
            errMsg = isValid ? string.Empty : $"Invalid Phone number Format, it should be xxxx-xxxxxxx";
            return isValid;

        }
        public bool IsValidEmail(string emailaddress, out string errMsg)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                errMsg = string.Empty;
                return true;
            }
            catch (FormatException)
            {
                errMsg = "Invalid Email Format";
                return false;
            }
        }
    }
}
