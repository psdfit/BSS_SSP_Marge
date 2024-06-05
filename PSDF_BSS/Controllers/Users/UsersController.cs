/* **** Aamer Rehman Malik *****/

using DataLayer;
using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Models.SSP;
using DataLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers.Users
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [System.Serializable]
    public class UsersController : ControllerBase
    {
        private readonly ISRVUsers srvUsers;
        private readonly ISRVRoles srvRoles;
        private readonly ISRVOrganization srvOrganization;
        private readonly ISRVUsersRights srvUsersRights;
        private readonly ISRVUserOrganizations srvUserOrganizations;
        private readonly ISRVUser_Pwd srvUser_Pwd;
        private readonly ISRVSAPApi srvSAPApi;
        private readonly ISRVTSPColor srvTspColor;
        private readonly ISRVSendEmail srvSendEmail;
        private readonly IHubContext<NotificationsHub, INotificationsHub> _hubContext;
        private readonly ISRVNotificationDetail iSRVNotificationDetail;

        public UsersController(ISRVNotificationDetail iSRVNotificationDetail, ISRVSendEmail srvSendEmail, ISRVUsers srvUsers, ISRVRoles srvRoles, ISRVOrganization srvOrganization, ISRVUsersRights srvUsersRights,
            ISRVUserOrganizations srvUserOrganizations, ISRVUser_Pwd srvUser_Pwd, ISRVSAPApi srvSAPApi, ISRVTSPColor srvTspColor, IHubContext<NotificationsHub, INotificationsHub> hubContext)
        {
            this.iSRVNotificationDetail = iSRVNotificationDetail;
            this.srvUsers = srvUsers;
            this.srvRoles = srvRoles;
            this.srvSendEmail = srvSendEmail;
            this.srvOrganization = srvOrganization;
            this.srvUsersRights = srvUsersRights;
            this.srvUserOrganizations = srvUserOrganizations;
            this.srvUser_Pwd = srvUser_Pwd;
            this.srvSAPApi = srvSAPApi;
            this.srvTspColor = srvTspColor;

            this._hubContext = hubContext;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("CheckNTN")]
        public IActionResult CheckNTN(SignUpModel User)
        {
            try
            {
                DataTable dt = srvUsers.CheckNTN(User);
                return Ok(Convert.ToInt32(dt.Rows[0]["Result"]));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }


        }

        [AllowAnonymous]
        [HttpPost]
        [Route("CheckEmail")]
        public IActionResult CheckEmail(SignUpModel UserEmail)
        {
            try
            {
                DataTable dt = srvUsers.CheckEmail(UserEmail);
                return Ok(dt.Rows.Count > 0 ? 1 : 0);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }


        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel User)
        {
            try
            {
                string randomCode = GenerateRandomCode();
                User.OTPCode = randomCode;
                DataTable dt = srvUsers.SignUp(User);
                if (dt.Rows.Count > 0)
                {
                    string subject = "OTP Verification";
                    string msg = $@"Your OTP Verification Code is:{randomCode}.";
                    await SendSMS(User.Mobile, msg);
                    Common.SendEmail(User.Email, subject, msg);
                }

                return Ok(dt.Rows[0].Table);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }


        }


        static string GenerateRandomCode()
        {
            Random random = new Random();
            int randomValue = random.Next(1000, 10000);
            return randomValue.ToString("D6");
        }



        public async Task<IActionResult> SendSMS(string mobile, string msg)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    var requestData = new
                    {
                        id = "rcpsdf",
                        brandname = "PSDF.",
                        password = "rcpsdf1281",
                    };

                    string apiUrl = $"http://api.lowcostsms.co.uk/api/sendsms?" +
                        $"id={requestData.id}" +
                        $"&pass={requestData.password}" +
                        $"&mobile={Regex.Replace(mobile, @"[^0-9]", "")}" +
                        $"&brandname={requestData.brandname}" +
                        $"&msg={msg}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("SMS sent successfully!");
                    }
                    else
                    {
                        return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Sending OTP Error+{ex.Message}");
                }
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("OTPVerification")]
        public async Task<IActionResult> OTPVerification(SignUpModel UserOTP)
        {
            try
            {
                DataTable dt = srvUsers.OTPVerification(UserOTP);
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[0]["UserPassword"] = Common.DESDecrypt(dt.Rows[0]["UserPassword"].ToString());
                    string subject = "Email Verification";
                    string msg = $@"Your Email has been verified.UserName is: {dt.Rows[0]["UserName"].ToString()}.";
                    if (dt.Rows != null)
                    {
                        await SendSMS(dt.Rows[0]["TspContact"].ToString(), msg);

                    }
                    Common.SendEmail(dt.Rows[0]["TspEmail"].ToString(), subject, msg);
                    return Ok(dt.Rows[0].Table);

                }
                else
                {
                    return BadRequest("Enter Valid OTP Code");

                }



            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }


        }

        // GET: Users
        [HttpGet]
        [Route("GetUsers")]
        public ActionResult GetUsers()
        {
            try
            {
                //PagingModel P = new PagingModel()
                //{
                //    PageNo = 1,
                //    PageSize = 10,
                //    SortColumn = "UserID",
                //    SortOrder = "DESC"
                //};
                //int tCount = 0;
                //List<UsersModel> users = srv.FetchPaged(P, out tCount);
                //P.TotalCount = tCount;
                Dictionary<string, object> ls = new Dictionary<string, object>();
                Parallel.Invoke(
              //() => ls.Add("Users", users),

              //() => ls.Add("Paging", P),

              () => ls.Add("Roles", srvRoles.FetchRoles(false)),
              () => ls.Add("Orgs", srvOrganization.FetchOrganization(false)));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Logout")]
        public IActionResult DeleteUserLogin(int? SessionID, int? UserId)
        {
            try
            {
                UsersModel mod = new UsersModel();
                mod.UserID = UserId.Value; // = ' + sessionStorage.getItem("UserId") + ' &;
                mod.SessionID = SessionID.ToString();
                return Ok(srvUsers.DeleteUserLogin(mod));
                // srvUsers.DeleteUserLogin(UserId, SessionID);
                // return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetSession")]
        public IActionResult GetLoginSession(int? SessionID, int? UserId)
        {
            try
            {
                UsersModel mod = new UsersModel();
                mod.UserID = UserId.Value; // = ' + sessionStorage.getItem("UserId") + ' &;
                mod.SessionID = SessionID.ToString();
                return Ok(srvUsers.GetSession(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("CheckPasswordAge")]
        public IActionResult CheckPasswordAge(UsersModel user)
        {
            string UserPassAge = srvUsers.CheckPasswordAge(user.UserName, user.UserPassword);
            //string[] PassError = us.UserPassAge.Split('|');

            if (UserPassAge == "0" || UserPassAge == "1")
            {
                return Ok(UserPassAge);
            }
            else
            {
                return BadRequest(UserPassAge);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UsersModel user)
        {
            try
            {
                //int LoginAttempt = srvUsers.LoginAttempt(user.UserName, user.UserPassword);

                ////7 Invalid User
                //if (LoginAttempt == 7)
                //{
                //    return BadRequest("Please enter a valid username.");
                //}
                //Wrong password

                int LoginAttempt = srvUsers.LoginAttempt(user.UserName, user.UserPassword);

                //7 Invalid User
                if (LoginAttempt == 7)
                {
                    int SSPLoginAttempt = srvUsers.SSPLoginAttempt(user.UserName, user.UserPassword);

                    if (SSPLoginAttempt == 7)
                    {
                        return BadRequest("Please enter a valid username.");
                    }
                    if (SSPLoginAttempt > 4)
                    {
                        return BadRequest("Your account has been locked due to five consecutive unsuccessful login attempts. Please use the forgot password option to regain access.");
                    }

                    if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.UserPassword))
                    {
                        return BadRequest("Invalid username or password");
                    }

                    UsersModel SSPUser = srvUsers.SSPLogin(user.UserName, user.UserPassword);

                    if (SSPUser == null)
                    {
                        return BadRequest($"Invalid username or password. You have only {5 - SSPLoginAttempt} attempts remaining. After that, your account will be locked.");
                    }

                    List<string> list = new List<string>
                    {
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(SSPUser))),
                        JsonConvert.SerializeObject(srvUsersRights.SSPGetUsersRightsByUser(SSPUser.UserID)),
                        JsonConvert.SerializeObject(srvUserOrganizations.GetByUserIDForSSP(SSPUser.UserID))
                    };
                    return Ok(list);

                }



                if (LoginAttempt > 4)
                {
                    return BadRequest("Your account has been locked due to five consecutive unsuccessful login attempts. Please use the forgot password option to regain access.");
                }

                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.UserPassword))
                {
                    return BadRequest("Invalid username or password");
                }
                if (!String.IsNullOrEmpty(user.UserName) && !String.IsNullOrEmpty(user.UserPassword))
                {
                    //Developed by Ali Haider 10-10-2023
                    string privateIpAddress = GetPrivateIpAddress();
                    UsersModel u = srvUsers.Login(user.UserName, user.UserPassword, privateIpAddress);

                    if (u != null)

                    {
                        if (u.CurrentState != null)
                        {
                            if (u.CurrentState >= u.AllowedSessions)
                            {
                                return BadRequest("User already have '" + u.AllowedSessions + "' active sessions!");
                            }

                            if (u.CurrentState != null && u.CurrentState >= u.AllowedSessions)
                            {
                                return BadRequest($"User already has '{u.AllowedSessions}' active sessions!");
                            }

                            if (u.UserLevel.Equals((int)EnumUserLevel.TSP))
                            {
                                List<TSPColorModel> tspuser = srvTspColor.FetchTSPColorByID(u.UserID);
                                if (tspuser.Count > 0 && tspuser[0].TSPColorID == (int)EnumTSPColor.Black)
                                {
                                    return BadRequest("Blacklisted TSPs are not allowed to login");
                                }
                            }
                        }
                        List<string> ls = new List<string>
                    {
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(u))),
                        JsonConvert.SerializeObject(srvUsersRights.GetByUserID(u.UserID)),
                        JsonConvert.SerializeObject(srvUserOrganizations.GetByUserID(u.UserID))
                    };
                        return Ok(ls);
                    }
                    return BadRequest("Invalid username or password");
                }
                return BadRequest("Invalid username or password");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString() + " in Login Controller");

            }
        }

        //public IActionResult Login(UsersModel user)
        //{
        //    UsersModel us = srvUsers.LoginAttempt(user.UserName, user.UserPassword);

        //    if (us.LoginAttempt <=4)
        //    {
        //    if (!String.IsNullOrEmpty(user.UserName) && !String.IsNullOrEmpty(user.UserPassword))
        //    {
        //        UsersModel u = srvUsers.Login(user.UserName, user.UserPassword);

        //        if (u != null)
        //        {
        //            if (u.CurrentState != null)
        //            {
        //                if (u.CurrentState >= u.AllowedSessions)
        //                {
        //                    return BadRequest("User already have '" + u.AllowedSessions + "' active sessions!");
        //                }
        //            }

        //            if (u.UserLevel.Equals((int)EnumUserLevel.TSP))
        //            {
        //                List<TSPColorModel> tspuser = srvTspColor.FetchTSPColorByID(u.UserID);
        //                if (tspuser.Count != 0)
        //                {
        //                    int? tspcolorid = tspuser[0].TSPColorID;
        //                    if (tspcolorid.Equals((int)EnumTSPColor.Black))
        //                    {
        //                        return BadRequest("Blacklisted TSPs are not allowed to login");
        //                    }
        //                }
        //            }

        //            List<string> ls = new List<string>
        //            {
        //                Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(u))),
        //                JsonConvert.SerializeObject(srvUsersRights.GetByUserID(u.UserID)),
        //                JsonConvert.SerializeObject(srvUserOrganizations.GetByUserID(u.UserID))
        //            };
        //            return Ok(ls);
        //        }
        //        else
        //        {
        //                return BadRequest($"Invalid username or password. You have only {us.LoginAttempt - 5} attempts remaining. After that, your account will be locked.");

        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("Invalid username or password");
        //    }

        //    }
        //    else
        //    {
        //            return BadRequest($"Your account has been locked due to five consecutive unsuccessful login attempts. Please use the password reset option to regain access.");

        //    }
        //}

        // RD: Users
        [HttpGet]
        [Route("RD_Users")]
        public IActionResult RD_Users()
        {
            try
            {
                return Ok(srvUsers.FetchUsers(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Users
        [HttpPost]
        [Route("RD_UsersBy")]
        public IActionResult RD_UsersBy(UsersModel mod)
        {
            try
            {
                return Ok(srvUsers.FetchUsers(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("RD_UsersByUserName/{id}")]
        public IActionResult RD_UsersByUserName(string id)
        {
            try
            {
                UsersModel mod = new UsersModel();
                mod.UserName = id.Replace("|||", "/");
                return Ok(srvUsers.FetchUsers(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Document/{filePath}")]


        public IActionResult Document(string filePath)
        {
            try
            {
                var relativePath = filePath.Replace("||", "\\");

                if (System.IO.File.Exists(relativePath))
                {
                    var contentTypeProvider = new FileExtensionContentTypeProvider();
                    if (!contentTypeProvider.TryGetContentType(filePath, out var contentType))
                    {
                        contentType = "application/octet-stream";
                    }
                    return File(System.IO.File.ReadAllBytes(relativePath), contentType, $"{filePath}.pdf");
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message?.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetDocument/{id}")]
        public IActionResult GetDocument(string id)



        {
            try
            {
                if (id != null)
                {
                    var filePath = id.Replace("||", "\\");
                    var relativePath = $"Documents\\Trade\\CurriculumAttachments\\{filePath}";

                    if (System.IO.File.Exists(relativePath))
                    {
                        var contentType = "application/pdf";
                        return File(System.IO.File.ReadAllBytes(relativePath), contentType, $"{id}.pdf");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    // If fileResult is null, handle it accordingly
                    return NotFound();
                }

            }
            catch (Exception e)
            {
                // Log the exception, and return a 500 Internal Server Error with the exception message
                // You might want to log the exception using a logging library like Serilog or log4net
                return StatusCode(500, e.InnerException?.ToString() ?? e.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ForgotPassword/{id}")]
        public IActionResult ForgotPassword(string id)
        {
            try
            {
                UsersModel mod = new UsersModel();
                mod.UserName = id.Replace("|||", "/");
                mod = srvUsers.FetchUsers(mod)?[0];
                if (mod != null)
                {
                    User_PwdModel Pmod = new User_PwdModel();
                    Pmod.InActive = false;
                    Pmod.UserID = mod.UserID;
                    //string newPwd = srvUser_Pwd.GeneratePassword(8, true)
                    int PassLen = 8;
                    string newPwd = Common.GeneratePass(PassLen);
                    if (srvUsers.UpdateUserPassword(mod.UserID, newPwd))
                    {
                        string subject = "Password Reset Request";
                        string username = mod.UserName;
                        string newPassword = newPwd;

                        string emailBody = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <title>Password Reset</title>
                        </head>
                        <body>
                            <p>Hello {username},</p>
                            <p>We received a request to reset your password for your PSDF BSS User account.</p>
                            <p>Your new password is: <strong>{newPassword}</strong></p>
                            <p>We recommend that you change your password after logging in.</p>
                            <p>If you did not request this password reset, please contact our support team immediately.</p>
                        </body>
                        </html>";


                        string body = "Your new password is : " + newPassword;
                        Common.SendEmail(mod.Email, subject, body);
                        //Common.sendEmailtoUser(mod.Email, subject, body);

                        //string Password = srvUser_Pwd.FetchUser_Pwd(Pmod)[0].UserPassword;
                        //Common.SendEmail(mod.Email, "Forgot Password", "Your new password for PSDF BSS User '" + mod.UserName + "' is " + newPwd);
                    }
                }

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("RD_UsersPaged")]
        public IActionResult RD_UsersPaged(PagingModel mod)
        {
            try
            {
                int tCount = 0;
                List<object> ls = new List<object>();

                ls.Add(srvUsers.FetchPaged(mod, out tCount));
                mod.TotalCount = tCount;
                ls.Add(mod);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetUserRights/{id}")]
        public IActionResult GetUserRights(int id)
        {
            try
            {
                Dictionary<string, object> ls = new Dictionary<string, object>();
                Parallel.Invoke(
              () => ls.Add("Rights", srvUsersRights.GetByUserID(id)),
              () => ls.Add("UserOrgs", srvUserOrganizations.GetByUserID(id)));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("CheckFinanceUserRights/{userId}/{formId}")]
        public IActionResult CheckFinanceUserRights(int userId, int formId)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvUsersRights.GetRightsByUserAndForm(userId, formId));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Users/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(UsersModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.UserID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvUsers.SaveUsers(D));
                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        // POST: Users/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(UsersModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srvUsers.ActiveInActive(d.UserID, d.InActive, Convert.ToInt32(User.Identity.Name));
                    return Ok(true);
                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        [HttpPost]
        [Route("CheckCanAlready")]
        public IActionResult CheckCanAlready(UsersRightsModel d)
        {
            try
            {
                bool ok = new bool();
                ok = srvUsers.GetRoleRightsByFormIDAndRoleID(d);
                return Ok(ok);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        public ActionResult ChangePassword(User_PwdModel user)
        {
            bool ok = new bool();
            ok = srvUsers.ChangePassword(user, Convert.ToInt32(User.Identity.Name));
            if (ok == true)
            {
                ApprovalModel approvalModel = new ApprovalModel();
                ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                approvalModel.ProcessKey = EnumApprovalProcess.CHANGE_PWD;
                approvalModel.UserIDs = Convert.ToInt32(User.Identity.Name).ToString();
                approvalModel.CurUserID = Convert.ToInt32(User.Identity.Name);
                srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);
            }

            return Ok(ok);
        }

        [HttpPost]
        [Route("CheckUser")]
        public IActionResult CheckUser(UsersModel user)
        {
            if (!String.IsNullOrEmpty(user.UserName) && !String.IsNullOrEmpty(user.UserPassword))
            {
                string privateIpAddress = GetPrivateIpAddress();
                UsersModel u = srvUsers.Login(user.UserName, user.UserPassword, privateIpAddress);
                if (u != null)
                {
                    return Ok(true);
                }
                else
                    return BadRequest("Invalid Old password");
            }
            else
                return BadRequest("Invalid Old password");
        }

        [HttpPost]
        [Route("CheckUserPass")]
        public IActionResult CheckUserPass(UsersModel user)
        {
            if (!String.IsNullOrEmpty(user.UserName) && !String.IsNullOrEmpty(user.UserPassword))
            {
                bool IsAuthenticated = srvUsers.CheckUserPass(user.UserName, user.UserPassword);
                if (IsAuthenticated == false)
                {
                    return Ok(false);
                }
                else
                    return Ok(true);
            }
            else
                return BadRequest("Invalid Old password");
        }

        [HttpPost]
        [Route("CheckUserName")]
        public IActionResult CheckUserName(UsersModel user)
        {
            if (!String.IsNullOrEmpty(user.UserName))
            {
                List<UsersModel> u = srvUsers.GetByUserName(user.UserName);
                if (u == null)
                {
                    return Ok(true);
                }
                else
                {
                    if (u.Count > 1)
                        return Ok(false);
                    else
                    {
                        if (u[0].UserID == user.UserID)
                        {
                            return Ok(true);
                        }
                        else
                            return Ok(false);
                    }
                }
            }
            else
                return BadRequest("Bad Request");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("EmailVerification/{id}")]
        public IActionResult EmailVerification(string id)
        {
            try
            {
                srvUsers.TraineeEmailsVerificationStatus(id);

                string htmlContent = "\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n\r\n<head>\r\n    <title>Email Verification</title>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <style type=\"text/css\">\r\n        @media screen {\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: normal;\r\n                font-weight: 400;\r\n                src: local('Lato Regular'), local('Lato-Regular'),\r\n                    url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: normal;\r\n                font-weight: 700;\r\n                src: local('Lato Bold'), local('Lato-Bold'),\r\n                    url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: italic;\r\n                font-weight: 400;\r\n                src: local('Lato Italic'), local('Lato-Italic'),\r\n                    url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: italic;\r\n                font-weight: 700;\r\n                src: local('Lato Bold Italic'), local('Lato-BoldItalic'),\r\n                    url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');\r\n            }\r\n        }\r\n\r\n        /* CLIENT-SPECIFIC STYLES */\r\n        body,\r\n        table,\r\n        td,\r\n        a {\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n        }\r\n\r\n        table,\r\n        td {\r\n            mso-table-lspace: 0pt;\r\n            mso-table-rspace: 0pt;\r\n        }\r\n\r\n        img {\r\n            -ms-interpolation-mode: bicubic;\r\n        }\r\n\r\n        /* RESET STYLES */\r\n        img {\r\n            border: 0;\r\n            height: auto;\r\n            line-height: 100%;\r\n            outline: none;\r\n            text-decoration: none;\r\n        }\r\n\r\n        table {\r\n            border-collapse: collapse !important;\r\n        }\r\n\r\n        body {\r\n            height: 100% !important;\r\n            margin: 0 !important;\r\n            padding: 0 !important;\r\n            width: 100% !important;\r\n            background-color: #f4f4f4;\r\n        }\r\n\r\n        /* iOS BLUE LINKS */\r\n        a[x-apple-data-detectors] {\r\n            color: inherit !important;\r\n            text-decoration: none !important;\r\n            font-size: inherit !important;\r\n            font-family: inherit !important;\r\n            font-weight: inherit !important;\r\n            line-height: inherit !important;\r\n        }\r\n\r\n        /* MOBILE STYLES */\r\n        @media screen and (max-width:600px) {\r\n            h1 {\r\n                font-size: 32px !important;\r\n                line-height: 32px !important;\r\n            }\r\n        }\r\n\r\n        /* ANDROID CENTER FIX */\r\n        div[style*=\"margin: 16px 0;\"] {\r\n            margin: 0 !important;\r\n        }\r\n    </style>\r\n</head>\r\n\r\n<body>\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n        <!-- LOGO -->\r\n        <tr>\r\n            <td align=\"center\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"></td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td  bgcolor=\"#ffffff\" align=\"center\" valign=\"top\"  style=\" width:100%;border-bottom:3px solid #0693e3;padding: 40px 20px 20px 20px; border-radius: 35px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n                            <img  src=\"https://bss.psdf.org.pk/assets/images/logo.png\"\r\n                            width=\"130\" height=\"125\" style=\"display: block; border: 0px;\" />\r\n                            <h1 style=\"font-size: 35px; font-weight: 400; margin: 2;\">Thank You for  email verification</h1>\r\n                            <img src=\"https://bss.psdf.org.pk/assets/images/footer_email.jpg\"   style=\"width: 100%;display: block; border: 0px;\"  alt=\"\">\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n\r\n</html>\r\n";

                var contentResult = new ContentResult
                {
                    Content = htmlContent,
                    ContentType = "text/html",
                    StatusCode = 200
                };

                return contentResult;

                //return contentResult;
                //return Content("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n\r\n<head>\r\n    <title>Email Verification</title>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <style type=\"text/css\">\r\n        @media screen {\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: normal;\r\n                font-weight: 400;\r\n                src: local('Lato Regular'), local('Lato-Regular'),\r\n                    url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: normal;\r\n                font-weight: 700;\r\n                src: local('Lato Bold'), local('Lato-Bold'),\r\n                    url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: italic;\r\n                font-weight: 400;\r\n                src: local('Lato Italic'), local('Lato-Italic'),\r\n                    url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: italic;\r\n                font-weight: 700;\r\n                src: local('Lato Bold Italic'), local('Lato-BoldItalic'),\r\n                    url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');\r\n            }\r\n        }\r\n\r\n        /* CLIENT-SPECIFIC STYLES */\r\n        body,\r\n        table,\r\n        td,\r\n        a {\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n        }\r\n\r\n        table,\r\n        td {\r\n            mso-table-lspace: 0pt;\r\n            mso-table-rspace: 0pt;\r\n        }\r\n\r\n        img {\r\n            -ms-interpolation-mode: bicubic;\r\n        }\r\n\r\n        /* RESET STYLES */\r\n        img {\r\n            border: 0;\r\n            height: auto;\r\n            line-height: 100%;\r\n            outline: none;\r\n            text-decoration: none;\r\n        }\r\n\r\n        table {\r\n            border-collapse: collapse !important;\r\n        }\r\n\r\n        body {\r\n            height: 100% !important;\r\n            margin: 0 !important;\r\n            padding: 0 !important;\r\n            width: 100% !important;\r\n            background-color: #f4f4f4;\r\n        }\r\n\r\n        /* iOS BLUE LINKS */\r\n        a[x-apple-data-detectors] {\r\n            color: inherit !important;\r\n            text-decoration: none !important;\r\n            font-size: inherit !important;\r\n            font-family: inherit !important;\r\n            font-weight: inherit !important;\r\n            line-height: inherit !important;\r\n        }\r\n\r\n        /* MOBILE STYLES */\r\n        @media screen and (max-width:600px) {\r\n            h1 {\r\n                font-size: 32px !important;\r\n                line-height: 32px !important;\r\n            }\r\n        }\r\n\r\n        /* ANDROID CENTER FIX */\r\n        div[style*=\"margin: 16px 0;\"] {\r\n            margin: 0 !important;\r\n        }\r\n    </style>\r\n</head>\r\n\r\n<body>\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n        <!-- LOGO -->\r\n        <tr>\r\n            <td align=\"center\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"></td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td  bgcolor=\"#ffffff\" align=\"center\" valign=\"top\"  style=\" width:100%;border-bottom:3px solid #0693e3;padding: 40px 20px 20px 20px; border-radius: 35px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n                            <img  src=\"https://bss.psdf.org.pk/assets/images/logo.png\"\r\n                            width=\"130\" height=\"125\" style=\"display: block; border: 0px;\" />\r\n                            <h1 style=\"font-size: 35px; font-weight: 400; margin: 2;\">Thank You for  email verification</h1>\r\n                            <img src=\"https://bss.psdf.org.pk/assets/images/footer_email.jpg\"   style=\"width: 100%;display: block; border: 0px;\"  alt=\"\">\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n\r\n</html>\r\n");


                ///return Redirect("http://localhost:4200/#/email-verification");
                //return Ok("Email Verified.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        //[AllowAnonymous]
        [HttpGet]
        [Route("TestCall")]
        public IActionResult TestCall()
        {
            try
            {
                //string body = @"<h1>Hello BSS <h1/>";
                //Common.SendEmail("numanexpt@gmail.com","BSS-Test",body,true);
                //srvSAPApi.FetchSAPBranches();

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        static string GetPrivateIpAddress()
        {
            string privateIpAddress = null;

            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                    foreach (UnicastIPAddressInformation ipInfo in ipProperties.UnicastAddresses)
                    {
                        if (ipInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(ipInfo.Address))
                        {
                            privateIpAddress = ipInfo.Address.ToString();
                            break;
                        }
                    }

                    if (privateIpAddress != null)
                        break;
                }
            }

            return privateIpAddress;
        }
    }
}