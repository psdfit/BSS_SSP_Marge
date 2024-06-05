/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataLayer.Services
{
    public class SRVUser_Pwd : SRVBase, DataLayer.Interfaces.ISRVUser_Pwd
    {
        public SRVUser_Pwd()
        {
        }

        public User_PwdModel GetBypwdID(int pwdID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@pwdID", pwdID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_User_Pwd", param).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return RowOfUser_Pwd(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<User_PwdModel> SaveUser_Pwd(User_PwdModel User_Pwd)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@pwdID", User_Pwd.pwdID);
                param[1] = new SqlParameter("@UserPassword", User_Pwd.UserPassword);
                param[2] = new SqlParameter("@UserID", User_Pwd.UserID);

                param[3] = new SqlParameter("@CurUserID", User_Pwd.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_User_Pwd]", param);
                return FetchUser_Pwd();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<User_PwdModel> LoopinData(DataTable dt)
        {
            List<User_PwdModel> User_PwdL = new List<User_PwdModel>();

            foreach (DataRow r in dt.Rows)
            {
                User_PwdL.Add(RowOfUser_Pwd(r));
            }
            return User_PwdL;
        }

        public List<User_PwdModel> FetchUser_Pwd(User_PwdModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_User_Pwd", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<User_PwdModel> FetchUser_Pwd()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_User_Pwd").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<User_PwdModel> FetchUser_Pwd(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_User_Pwd", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<User_PwdModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_User_Pwd]", param);
        }

        public List<User_PwdModel> GetByUserID(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_User_Pwd", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int pwdID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@pwdID", pwdID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_User_Pwd]", PLead);
        }

        private User_PwdModel RowOfUser_Pwd(DataRow r)
        {
            User_PwdModel User_Pwd = new User_PwdModel();
            User_Pwd.pwdID = Convert.ToInt32(r["pwdID"]);
            User_Pwd.UserPassword = r["UserPassword"].ToString();
            User_Pwd.UserID = Convert.ToInt32(r["UserID"]);
            User_Pwd.UserName = r["UserName"].ToString();
            User_Pwd.InActive = Convert.ToBoolean(r["InActive"]);
            User_Pwd.CreatedUserID = Convert.ToInt32(r["CreatedUserID"].ToString());
            User_Pwd.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"].ToString() == "" ? 0 : r["ModifiedUserID"]);
            User_Pwd.CreatedDate = r["CreatedDate"].ToString().GetDate();
            User_Pwd.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return User_Pwd;
        }

        public string GeneratePass(int size)
        {
            string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numericChars = "0123456789";
            string specialChars = "!@#$%^&*(){}[]";

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            // Add at least one character from each category
            password.Append(lowercaseChars[random.Next(lowercaseChars.Length)]);
            password.Append(uppercaseChars[random.Next(uppercaseChars.Length)]);
            password.Append(numericChars[random.Next(numericChars.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            int requiredLength = size - password.Length; // Minimum length of 8 characters

            // Add the remaining characters randomly
            for (int i = 0; i < requiredLength; i++)
            {
                string charSet = lowercaseChars + uppercaseChars + numericChars + specialChars;
                password.Append(charSet[random.Next(charSet.Length)]);
            }

            // Shuffle the password to randomize the character order
            string shuffledPassword = new string(password.ToString().ToCharArray().OrderBy(x => random.Next()).ToArray());

            return shuffledPassword;
        }

        public string GeneratePassword(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}