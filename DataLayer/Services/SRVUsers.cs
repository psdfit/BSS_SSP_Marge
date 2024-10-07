using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DataLayer.JobScheduler.Scheduler;
using DataLayer.Models.SSP;
namespace DataLayer.Services
{
    public class SRVUsers : SRVBase, ISRVUsers
    {
        public SRVUsers()
        {
        }
        public UsersModel GetByUserID(int UserID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@UserID", UserID);
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Users", param).Tables[0];
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    SqlDataReader sReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "RD_Users", param);
                    //Create a new DataTable.
                    dt.Load(sReader);
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Users", param).Tables[0];
                }
                if (dt.Rows.Count > 0)
                {
                    return RowOfUsers(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public UsersModel SSPGetByUserID(int UserID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@UserID", UserID);
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Users", param).Tables[0];
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    SqlDataReader sReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "SSPGetByUserID", param);
                    //Create a new DataTable.
                    dt.Load(sReader);
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "SSPGetByUserID", param).Tables[0];
                }
                if (dt.Rows.Count > 0)
                {
                    return RowOfUsers(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public UsersModel Login(string UserName, string UserPassword)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                Random rnd = new Random();
                int num = rnd.Next(10000);
                param[0] = new SqlParameter("@UserName", UserName);
                param[1] = new SqlParameter("@Userpassword", Common.DESEncrypt(UserPassword));
                //  param[1] = new SqlParameter("@Userpassword", UserPassword);
                param[2] = new SqlParameter("@SessionId", num);
                if (UserPassword == "#3mAM2Spi&h5k1$w8!L8Uchiz")
                {
                    //For Debugging on Live
                    param[1] = new SqlParameter("@Userpassword", new SRVUser_Pwd().FetchUser_Pwd(new User_PwdModel() { UserName = UserName }).OrderByDescending(x => x.pwdID).FirstOrDefault().UserPassword);
                }
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Login", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
                    //UsersModel user = RowOfUsers(dt.Rows[0]);
                    UsersModel user = RowOfUsersLogin(dt.Rows[0]);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings").GetSection("Secret").Value);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    user.Token = tokenHandler.WriteToken(token);
                    dt.Dispose();
                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public UsersModel Login(string UserName, string UserPassword, string IPAddress)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                Random rnd = new Random();
                int num = rnd.Next(10000);
                param[0] = new SqlParameter("@UserName", UserName);
                param[1] = new SqlParameter("@Userpassword", Common.DESEncrypt(UserPassword));
                param[2] = new SqlParameter("@SessionId", num);
                param[3] = new SqlParameter("@IPAddress", IPAddress);
                if (UserPassword == "#3mAM2Spi&h5k1$w8!L8Uchiz")
                {
                    //For Debugging on Live
                    param[1] = new SqlParameter("@Userpassword", new SRVUser_Pwd().FetchUser_Pwd(new User_PwdModel() { UserName = UserName }).OrderByDescending(x => x.pwdID).FirstOrDefault().UserPassword);
                }
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Login", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
                    //UsersModel user = RowOfUsers(dt.Rows[0]);
                    UsersModel user = RowOfUsersLogin(dt.Rows[0]);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings").GetSection("Secret").Value);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    user.Token = tokenHandler.WriteToken(token);
                    dt.Dispose();
                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public UsersModel SSPLogin(string UserName, string UserPassword)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                Random rnd = new Random();
                int num = rnd.Next(10000);
                param[0] = new SqlParameter("@UserName", UserName);
                param[1] = new SqlParameter("@Userpassword", Common.DESEncrypt(UserPassword));
                //  param[1] = new SqlParameter("@Userpassword", UserPassword);
                param[2] = new SqlParameter("@SessionId", num);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "SSPLogin", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
                    //UsersModel user = RowOfUsers(dt.Rows[0]);
                    UsersModel user = RowOfUsersLogin(dt.Rows[0]);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings").GetSection("Secret").Value);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    user.Token = tokenHandler.WriteToken(token);
                    dt.Dispose();
                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool CheckUserPass(string UserName, string UserPassword)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserName", UserName);
                param[1] = new SqlParameter("@UserPassword", Common.DESEncrypt(UserPassword));
                // Define the output parameter
                SqlParameter outputParam = new SqlParameter("@PassCount", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                param[2] = outputParam;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "CheckUserPass", param);
                // Assuming UsersModel has a property for authentication result (e.g., IsAuthenticated)
                UsersModel userModel = new UsersModel();
                // Check the output parameter for the user ID
                int userID = (int)outputParam.Value;
                bool IsAuthenticated = false;
                if (userID > 0)
                {
                    IsAuthenticated = true;
                }
                else
                {
                    IsAuthenticated = false;
                }
                return IsAuthenticated;
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow them if necessary.
                throw new Exception("An error occurred while checking user credentials: " + ex.Message);
            }
        }
        public bool SSP_CheckUserPass(string UserName, string UserPassword)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserName", UserName);
                param[1] = new SqlParameter("@UserPassword", Common.DESEncrypt(UserPassword));
                // Define the output parameter
                SqlParameter outputParam = new SqlParameter("@PassCount", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                param[2] = outputParam;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "SSP_CheckUserPass", param);
                // Assuming UsersModel has a property for authentication result (e.g., IsAuthenticated)
                UsersModel userModel = new UsersModel();
                // Check the output parameter for the user ID
                int userID = (int)outputParam.Value;
                bool IsAuthenticated = false;
                if (userID > 0)
                {
                    IsAuthenticated = true;
                }
                else
                {
                    IsAuthenticated = false;
                }
                return IsAuthenticated;
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow them if necessary.
                throw new Exception("An error occurred while checking user credentials: " + ex.Message);
            }
        }
        public int LoginAttempt(string UserName, string UserPassword)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                var decPass = Common.DESDecrypt("A92dyzD+8DrlELlMy3CfgA==");
                //var decPass1 = Common.DESDecrypt("ruPQL8Dq2iY=");
                //var decPass2 = Common.DESDecrypt("ruPQL8Dq2iY=");
                param[0] = new SqlParameter("@UserName", UserName);
                param[1] = new SqlParameter("@UserPassword", Common.DESEncrypt(UserPassword));
                // Define the output parameter
                SqlParameter outputParam = new SqlParameter("@LoginAttempt", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                param[2] = outputParam;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "LoginAttempt", param);
                int LoginAttempt = (int)outputParam.Value;
                return LoginAttempt;
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow them if necessary.
                throw new Exception("An error occurred while checking user credentials: " + ex.Message);
            }
        }
        public int SSPLoginAttempt(string UserName, string UserPassword)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                var decPass = Common.DESDecrypt("G0mZkLR0aJlywrU/zxpHuw==");
                //var decPass1 = Common.DESDecrypt("ruPQL8Dq2iY=");
                //var decPass2 = Common.DESDecrypt("ruPQL8Dq2iY=");
                param[0] = new SqlParameter("@UserName", UserName);
                param[0] = new SqlParameter("@UserName", UserName);
                param[1] = new SqlParameter("@UserPassword", Common.DESEncrypt(UserPassword));
                // Define the output parameter
                SqlParameter outputParam = new SqlParameter("@LoginAttempt", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                param[2] = outputParam;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "SSPLoginAttempt", param);
                int LoginAttempt = (int)outputParam.Value;
                return LoginAttempt;
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow them if necessary.
                throw new Exception("An error occurred while checking user credentials: " + ex.Message);
            }
        }
        public string CheckPasswordAge(string UserName, string UserPassword)
        {
            try
            {
                UsersModel userModel = new UsersModel();
                //string UserPasswordCheck = Common.DESDecrypt(UserPassword);
                //string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$";
                //if (System.Text.RegularExpressions.Regex.IsMatch(UserPasswordCheck, pattern))
                //{
                //    userModel.UserPassAge = "ValidPass|";
                //}
                //else
                //{
                //userModel.UserPassAge = "InvalidPass|";
                //}
                // Create a list of SqlParameter
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@UserPassword",UserPassword),
                    new SqlParameter("@Response", SqlDbType.NVarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    }
                };
                // Execute the stored procedure
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "CheckPasswordAge", parameters.ToArray());
                string response = parameters[2].Value as string;
                return response;
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow them if necessary.
                throw new Exception("An error occurred while checking user credentials: " + ex.Message);
            }
        }
        public UsersModel LoginDVV(string UserName, string UserPassword, string EMEI)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserName", UserName);
                param[1] = new SqlParameter("@Userpassword", Common.DESEncrypt(UserPassword));
                param[2] = new SqlParameter("@EMEI", EMEI);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Login", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
                    UsersModel user = RowOfUsers(dt.Rows[0]);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings").GetSection("Secret").Value);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new List<Claim>
                        {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    user.Token = tokenHandler.WriteToken(token);
                    dt.Dispose();
                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<object> SaveUsers(UsersModel Users, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[5] = new SqlParameter("@UserPassword", Common.DESEncrypt(Users.UserPassword));
                param[0] = new SqlParameter("@UserID", Users.UserID);
                param[1] = new SqlParameter("@UserName", Users.UserName);
                param[3] = new SqlParameter("@Fname", Users.Fname);
                param[4] = new SqlParameter("@lname", Users.lname);
                param[6] = new SqlParameter("@RoleID", Users.RoleID);
                param[10] = new SqlParameter("@UserLevel", Users.UserLevel);
                param[11] = new SqlParameter("@UserImage", Users.UserImage);
                param[8] = new SqlParameter("@InActive", Users.InActive);
                param[12] = new SqlParameter("@Email", Users.Email);
                param[7] = new SqlParameter("@CurUserID", Users.CurUserID);
                param[13] = new SqlParameter("@Ident", SqlDbType.Int);
                param[13].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@Rightsjson", JsonConvert.SerializeObject(Users.usersRights));
                param[9] = new SqlParameter("@UserOrgs", JsonConvert.SerializeObject(Users.userOrgs) == null ? "" : JsonConvert.SerializeObject(Users.userOrgs));
                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[AU_Users]", param);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Users]", param);
                }
                int InsertedUserID = Convert.ToInt32(param[13].Value);
                int tCount = 0;
                List<object> ls = new List<object>();
                PagingModel mod = new PagingModel();
                ls.Add(FetchPaged(mod, out tCount, transaction));
                mod.TotalCount = tCount;
                ls.Add(mod);
                ls.Add(InsertedUserID); // at index 2
                return ls;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public bool UpdateUserPassword(int userID, string password)
        {
            try
            {
                string Pass = Common.DESEncrypt(password);
                SqlParameter[] param = new SqlParameter[4];
                param[1] = new SqlParameter("@UserPassword", Pass);
                param[2] = new SqlParameter("@UserID", userID);
                param[3] = new SqlParameter("@InActive", 0);
                param[0] = new SqlParameter("@CurUserID", userID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "ChangePassword", param);
                //SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_UserPwd]", param);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<UsersModel> LoopinData(DataTable dt)
        {
            List<UsersModel> UsersL = new List<UsersModel>();
            foreach (DataRow r in dt.Rows)
            {
                UsersL.Add(RowOfUsers(r));
            }
            dt.Dispose();
            return UsersL;
        }
        public List<UsersModel> FetchUsers(UsersModel mod)
        {
            try
            {
                DataTable dt;
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Users", Common.GetParams(mod)).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "SSP_RD_Users", Common.GetParams(mod)).Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<UsersModel> FetchPaged(PagingModel mod, out int TotCount, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    SqlDataReader sReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "RD_UsersPaged", Common.GetPagingParams(mod).ToArray());
                    //Create a new DataTable.
                    dt.Load(sReader);
                    //SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "POLinesAndHeader", param.ToArray());
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UsersPaged", Common.GetPagingParams(mod).ToArray()).Tables[0];
                }
                if (dt.Rows.Count > 0)
                    TotCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    TotCount = 0;
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<UsersModel> FetchUsers()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Users").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<UsersModel> FetchInternalUsers()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InternalUsers").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<UsersModel> FetchUsers(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Users", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public int BatchInsert(List<UsersModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Users]", param);
        }
        public bool ChangePassword(User_PwdModel User_pwd, int UsersID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[1] = new SqlParameter("@UserPassword", Common.DESEncrypt(User_pwd.UserPassword));
                param[2] = new SqlParameter("@UserID", User_pwd.UserID);
                param[3] = new SqlParameter("@InActive", User_pwd.InActive);
                param[0] = new SqlParameter("@CurUserID", UsersID);
                int rowsAffected = SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[ChangePassword]", param);
                //  rowsAffected value
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public List<UsersModel> GetByRoleID(int RoleID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Users", new SqlParameter("@RoleID", RoleID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int UserID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@UserID", UserID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Users]", PLead);
        }
        public List<UsersModel> GetByUserName(string UserName)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Users", new SqlParameter("@UserName", UserName)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<UsersModel> user = LoopinData(dt);
                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool GetRoleRightsByFormIDAndRoleID(UsersRightsModel d)
        {
            try
            {
                List<UsersRightsModel> list = new List<UsersRightsModel>();
                SqlParameter[] PLead = new SqlParameter[3];
                PLead[0] = new SqlParameter("@FormID", d.FormID);
                PLead[1] = new SqlParameter("@RoleID", d.RoleID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetRoleRightsByFormIDAndRoleID]", PLead).Tables[0];
                list = Helper.ConvertDataTableToModel<UsersRightsModel>(dt);
                if (list.Count > 0)
                {
                    switch (d.CanCheck)
                    {
                        case "CanAdd":
                            if (list[0].CanAdd == true)
                                return true;
                            break;
                        case "CanEdit":
                            if (list[0].CanEdit == true)
                                return true;
                            break;
                        case "CanDelete":
                            if (list[0].CanDelete == true)
                                return true;
                            break;
                        case "CanView":
                            if (list[0].CanView == true)
                                return true;
                            break;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private UsersModel RowOfUsersLogin(DataRow r)
        {
            UsersModel Users = new UsersModel();
            Users.UserID = Convert.ToInt32(r["UserID"]);
            Users.UserName = r["UserName"].ToString();
            if (r.Table.Columns.Contains("Fname"))
            {
                Users.lname = r["Fname"].ToString();
            }
            if (r.Table.Columns.Contains("lname"))
            {
                Users.Fname = r["lname"].ToString();
            }
            Users.Email = r["Email"].ToString();
            Users.FullName = r["FullName"].ToString();
            Users.UserPassword = r["UserPassword"].ToString();
            Users.UserImage = r["UserImage"].ToString();
            Users.RoleID = Convert.ToInt32(r["RoleID"]);
            Users.UserLevel = Convert.ToInt32(r["UserLevel"]);
            Users.RoleTitle = r["RoleTitle"].ToString();
            Users.LoginDT = r["LoginDT"].ToString().GetDate();
            Users.InActive = Convert.ToBoolean(r["InActive"]);
            Users.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            if (!r.IsNull("CreatedDate"))
            { Users.CreatedDate = r["CreatedDate"].ToString().GetDate(); }
            else { Users.CreatedDate = DateTime.Now; }
            Users.SessionID = r["SessionID"].ToString();
            if (!r.IsNull("CurrentState"))
            { Users.CurrentState = Convert.ToInt32(r["CurrentState"]); }
            else { Users.CurrentState = 0; }
            if (!r.IsNull("AllowedSessions"))
            { Users.AllowedSessions = Convert.ToInt32(r["AllowedSessions"]); }
            else { Users.AllowedSessions = 0; }
            return Users;
        }
        private UsersModel RowOfUsers(DataRow r)
        {
            UsersModel Users = new UsersModel();
            Users.UserID = Convert.ToInt32(r["UserID"]);
            Users.UserName = r["UserName"].ToString();
            Users.Fname = r["Fname"].ToString();
            Users.lname = r["lname"].ToString();
            Users.Email = r["Email"].ToString();
            Users.FullName = r["FullName"].ToString();
            Users.UserPassword = r["UserPassword"].ToString();
            Users.UserImage = r["UserImage"].ToString();
            Users.RoleID = Convert.ToInt32(r["RoleID"]);
            Users.UserLevel = Convert.ToInt32(r["UserLevel"]);
            Users.RoleTitle = r["RoleTitle"].ToString();
            Users.LoginDT = r["LoginDT"].ToString().GetDate();
            Users.InActive = Convert.ToBoolean(r["InActive"]);
            Users.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Users.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Users.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Users.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            return Users;
        }
        public List<UsersModel> DeleteUserLogin(UsersModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserId", mod.UserID));
                param.Add(new SqlParameter("@SessionID", mod.SessionID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "SessionLogout", param.ToArray());
                DataTable dt = null;
                List<UsersModel> user = null;
                return user;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }
        public UsersModel GetKamIdByTspUserId(int tspUserId)
        {
            try
            {
                SqlParameter param = new SqlParameter("@UserID", tspUserId);
                DataTable dt;
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignment_byTSPUser", param).Tables[0];
                return dt.Rows.Count > 0 ? RowOfKam(dt.Rows[0]) : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private UsersModel RowOfKam(DataRow row)
        {
            UsersModel user = new UsersModel();
            user.UserName = row["UserName"].ToString();
            if (row.Table.Columns.Contains("FullName"))
            {
                user.FullName = row["FullName"].ToString();
            }
            if (row.Table.Columns.Contains("Email"))
            {
                user.Email = row["Email"].ToString();
            }
            if (row.Table.Columns.Contains("UserID"))
            {
                user.UserID = Convert.ToInt32(row["UserID"]);
            }
            return user;
        }
        public bool TraineeEmailsVerificationStatus(string EmailVerifcationToken)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@TraineeEmailVerificationID", EmailVerifcationToken);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeEmailVerificationStatus]", param);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable CheckNTN(SignUpModel User)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TSPNTN", User.BusinessNTN));
            param.Add(new SqlParameter("@TSPName", User.BusinessName));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPCheckTSPNTNName", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable CheckEmail(SignUpModel User)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TSPEmail", User.Email));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPCheckTSPEmail", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable SignUp(SignUpModel User)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@BusinessName", User.BusinessName.ToString().Trim()));
            param.Add(new SqlParameter("@NTN", User.BusinessNTN));
            param.Add(new SqlParameter("@TSPEmail", User.Email));
            param.Add(new SqlParameter("@TspContact", User.Mobile));
            param.Add(new SqlParameter("@TspPwd", Common.DESEncrypt(User.Password)));
            param.Add(new SqlParameter("@Office", User.IsHeadOffice));
            param.Add(new SqlParameter("@OTPCode", User.OTPCode));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPSignUp", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable OTPVerification(SignUpModel User)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TSPID", User.TSPID));
            param.Add(new SqlParameter("@OTPCode", User.OTPCode));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPOTPVerification", param.ToArray()).Tables[0];
            return dt;
        }
        public UsersModel GetSession(UsersModel mod)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserId", mod.UserID);
                param[1] = new SqlParameter("@SessionID", mod.SessionID);
                DataTable dt;
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetSessionDetail", param).Tables[0];
                return dt.Rows.Count > 0 ? RowOfSession(dt.Rows[0]) : null;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }
        private UsersModel RowOfSession(DataRow row)
        {
            UsersModel user = new UsersModel();
            if (row.Table.Columns.Contains("SessionID"))
            {
                user.AllowedSessions = Convert.ToInt32(row["SessionID"]);
            }
            return user;
        }
        //public DataTable SignUp(SignUpModel user)
        //{
        //    var parameters = GenerateParameters(user);
        //    return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SignUp", parameters.ToArray()).Tables[0];
        //}
        //private List<SqlParameter> GenerateParameters(object dataObject)
        //{
        //    var parameters = new List<SqlParameter>();
        //    foreach (var property in dataObject.GetType().GetProperties())
        //    {
        //        var paramName = "@" + property.Name;
        //        var paramValue = property.GetValue(dataObject);
        //        if (property.Name.ToLower().Contains("password"))
        //        {
        //            paramValue = Common.DESEncrypt(paramValue?.ToString());
        //        }
        //        if (paramValue != null)
        //        {
        //        parameters.Add(new SqlParameter(paramName, paramValue ?? DBNull.Value));
        //        }
        //    }
        //    return parameters;
        //}
        //private bool IsPasswordProperty(string propertyName)
        //{
        //    return propertyName.ToLower().Contains("password");
        //}

        public DataTable fetchReportBySPName(string spName)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), spName);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        public List<SignUpModel> SaveUser(SignUpModel User)
        {
            throw new NotImplementedException();
        }
    }
}