using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVSendEmail : ISRVSendEmail
    {
        private readonly ISRVUsers srvUsers;
        private ISRVNotificationMap srv = null;
        public readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private readonly IHubContext<NotificationsHub, INotificationsHub> _hubContext;
        public SRVSendEmail(IHubContext<NotificationsHub, INotificationsHub> hubContext, ISRVUsers srvUsers, ISRVNotificationMap srv)
        {
            this.srvUsers = srvUsers;
            this.srv = srv;
            this._hubContext = hubContext;
        }
        public void GenerateEmailToApprovers(ApprovalModel currentApproval, ApprovalHistoryModel historyModel)
        {
            if (currentApproval != null)
            {
                string[] UserIDs = currentApproval.UserIDs.Split(',');

                /// send mail to approvers
                foreach (int userID in UserIDs.Select(int.Parse))
                {
                    //int ID = Convert.ToInt32(UserID);
                    UsersModel user = srvUsers.GetByUserID(userID);

                    var emailFormat = GeneratEmailFormat(user, currentApproval, historyModel);
                    Common.SendEmail(user.Email, emailFormat.Subject, emailFormat.BodyHTML, true);
                }
            }
        }



        public void GenerateEmailAndSendNotification(ApprovalModel approvalModel, ApprovalHistoryModel historyModel)
        {
            try
            {
                List<NotificationDetailModel> NotificationDetail = new List<NotificationDetailModel>();
                if (approvalModel != null)
                {
                    //GetProcess Info By ProcessKey
                    NotificationsMapModel Emailmodel = new NotificationsMapModel();
                    UserNotificationMapModel userNotification = new UserNotificationMapModel();
                    List<NotificationsMapModel> ProcessInfo = new List<NotificationsMapModel>();
                    userNotification.CustomComments = approvalModel.CustomComments;
                    ProcessInfo = srv.GetProcessInfoByProcessKey(approvalModel.ProcessKey);
                    if (ProcessInfo.Count > 0)
                    {
                        string NotificationUserIDs = "";
                        Emailmodel.Subject = ProcessInfo[0].Subject;
                        Emailmodel.Body = ProcessInfo[0].Body;

                        if (approvalModel.isUserMapping == true)
                        {
                            NotificationUserIDs = ProcessInfo[0].UserID + "," + approvalModel.UserIDs;

                        }
                        else
                        {
                            NotificationUserIDs = approvalModel.UserIDs;
                        }

                        List<string> uniqueValues = NotificationUserIDs.ToLower().Split(',').Distinct().ToList();
                        string UniqueString = string.Join(",", uniqueValues);
                        string[] UserIDs = UniqueString.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                        /// send mail to approvers
                        foreach (int userID in UserIDs.Select(int.Parse))
                        {
                            UsersModel user = srvUsers.GetByUserID(userID);
                            if (user != null)
                            {
                                userNotification.UserID = userID;
                                userNotification.NotificationMapID = ProcessInfo[0].NotificationMapID;
                                userNotification.NotificationTypeId = 1; //PushNotification
                                //if (historyModel.EmailSentOrNotBit != false && user != null)
                                //{
                                //    var emailFormat = GeneratEmailGenericFormate(Emailmodel, user, approvalModel, historyModel);
                                //    Common.SendEmailPushNotification(approvalModel.logFilePath, user.Email, emailFormat.Subject, emailFormat.BodyHTML, true);
                                //}
                                var emailFormat = GeneratEmailGenericFormate(Emailmodel, user, approvalModel, historyModel);

                                var emailNotifiction = new UserNotificationMapModel()
                                {
                                    NotificationMapID = ProcessInfo[0].NotificationMapID,
                                    CustomComments = emailFormat.BodyHTML,
                                    UserID = userID,
                                    NotificationTypeId = 2 //email Notification

                                };

                                //Start KamNotification
                                if (historyModel.ProcessKey == "CR_NEW_INSTRUCTOR" && historyModel.ApprovalStatusID == 3)
                                {
                                    UsersModel kamUser = srvUsers.GetKamIdByTspUserId(userID);
                                    if (kamUser != null)
                                    {
                                        var emailNotifictionForKam = new UserNotificationMapModel()
                                        {
                                            NotificationMapID = ProcessInfo[0].NotificationMapID,
                                            CustomComments = emailFormat.BodyHTML,
                                            UserID = kamUser.UserID,
                                            NotificationTypeId = 2 //email Notification

                                        };
                                        int kamNotificationId = SRVNotificationDetail.saveSendNotification(emailNotifictionForKam, approvalModel.CurUserID);
                                        NotificationDetail = srv.GetNotificationDetasilByID(kamNotificationId);
                                        NotificafationManager mngKam = new NotificafationManager(_hubContext);
                                        mngKam.Send(kamUser.UserID.ToString(), NotificationDetail[0]);
                                    }
                                }
                                //End KamNotification

                                _ = SRVNotificationDetail.saveSendNotification(emailNotifiction, approvalModel.CurUserID);
                                int NotificationDetailID = SRVNotificationDetail.saveSendNotification(userNotification, approvalModel.CurUserID);
                                NotificationDetail = srv.GetNotificationDetasilByID(NotificationDetailID);
                                NotificafationManager mng = new NotificafationManager(_hubContext);
                                mng.Send(userID.ToString(), NotificationDetail[0]);
                            }
                        }

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }





        private EmailModel GeneratEmailGenericFormate(NotificationsMapModel Emailmodel, UsersModel user, ApprovalModel approvalModel, ApprovalHistoryModel approvalHistoryModel)
        {

            var email = new EmailModel();
            string ApprovalStatus = "";
            string Remarks = "";
            if (approvalHistoryModel.ApprovalStatusID == (int)EnumApprovalStatus.Pending)
                ApprovalStatus = "Pending :";
            else if (approvalHistoryModel.ApprovalStatusID == (int)EnumApprovalStatus.Approved)
                ApprovalStatus = "Approved :";
            else if (approvalHistoryModel.ApprovalStatusID == (int)EnumApprovalStatus.SendBack)
                ApprovalStatus = "SendBack :";
            else if (approvalHistoryModel.ApprovalStatusID == (int)EnumApprovalStatus.Rejected)
                ApprovalStatus = "Rejected :";
            Remarks = $"<p><b>Class Code:</b> {approvalModel.ClassCodeBYIncClassId}</p><p><b>Remarks:</b> {approvalHistoryModel.Comments}</p>";

            if (approvalModel.ProcessKey != null)
            {
                email.Subject = "PSDF_BSS :" + ApprovalStatus + "  " + Emailmodel.Subject;
                //email.BodyHTML = $@"<html>
                //    <body>
                //        <p>Dear {user.FullName},</p>
                //        <p>
                //           {Emailmodel.Body}, {approvalModel.CustomComments}
                //        </p>
                //        <p>Thanks</p>
                //    </body>
                //</html>";

                email.BodyHTML = $@"<html>
                            <head>
                            </head>
                            <body>
                                <p style='text-align:left;'><img src=cid:Header></p>
                                   <p>Dear <b>{user.FullName},</b></p>
                                   <p>{Emailmodel.Body} {approvalModel.CustomComments}</p>
                                   {Remarks}
                                <p>
                                    Regards,<br />
                                    Punjab Skills Development Fund
                                </p>
                                <p style='text-align:right;'><img src=cid:Footer></p>
                             </ body >
                             </ html > ";
            }


            return email;
        }






        private EmailModel GeneratEmailFormat(UsersModel user, ApprovalModel approvalModel, ApprovalHistoryModel approvalHistoryModel)
        {
            var email = new EmailModel();
            switch (approvalHistoryModel.ApprovalStatusID)
            {
                case (int)EnumApprovalStatus.Pending:
                case (int)EnumApprovalStatus.Approved:
                    switch (approvalModel.ProcessKey)
                    {
                        case EnumApprovalProcess.AP_PD:
                        case EnumApprovalProcess.AP_BD:
                            email.Subject = "PSDF: New Appendix Approval";
                            email.BodyHTML = "Dear User, \nA new appendix was created which requires your approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                        case EnumApprovalProcess.PRN_T:
                            email.Subject = "PSDF: New Approval Assignment";
                            email.BodyHTML = "Dear User, \nA new Trainig Recomendation Note is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;
                        case EnumApprovalProcess.PRN_R:
                            email.Subject = "PSDF: New Approval Assignment";
                            email.BodyHTML = "Dear User, \nA new Payment Recomendation Note is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                        case EnumApprovalProcess.SRN:
                            email.Subject = "PSDF: New Approval Assignment";
                            email.BodyHTML = $@"<html>
                                                <body>
                                                    <p>Dear {user.FullName},</p>
                                                    <p>
                                                        A new <b>Stipend Recomendation Note</b> is assigned to you for approval.
                                                        Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                    </p>
                                                    <p>Thanks</p>
                                                </body>
                                             </html>";
                            break;

                        case EnumApprovalProcess.PO_SRN:
                            email.Subject = "PSDF: New Approval Assignment";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    A new <b>Purchase Order Of Stipend Recomendation Note</b> is assigned to you for approval.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;

                        case EnumApprovalProcess.PO_TSP:
                            email.Subject = "PSDF: New Approval Assignment";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    A new <b>Purchase Order</b> is assigned to you for approval.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;

                        case EnumApprovalProcess.INV_SRN:
                            email.Subject = "PSDF: New Approval Assignment";
                            email.BodyHTML = "Dear User, \n A new Invoice Of Stipend Recomendation Note is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                        case EnumApprovalProcess.CR_CLASS_LOCATION:
                            email.Subject = "PSDF: New Class Location Change Request";
                            email.BodyHTML = "Dear User, \n A new change request of Class Location is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                        case EnumApprovalProcess.CR_CLASS_DATES:
                            email.Subject = "PSDF: New Class Date Change Request";
                            email.BodyHTML = "Dear User, \n A new change request of Class Date is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                        case EnumApprovalProcess.CR_TRAINEE_UNVERIFIED:
                            email.Subject = "PSDF: New Trainee Profile Change Request";
                            email.BodyHTML = "Dear User, \n A new change request of Trainee Profile is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                        case EnumApprovalProcess.CR_TRAINEE_VERIFIED:
                            email.Subject = "PSDF: New Trainee Profile Change Request";
                            email.BodyHTML = "Dear User, \n A new change request of Trainee Profile is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                        case EnumApprovalProcess.CR_INSTRUCTOR:
                            email.Subject = "PSDF: New Instructor Change Request";
                            email.BodyHTML = "Dear User, \n A new change request of Instructor is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;
                        case EnumApprovalProcess.CR_NEW_INSTRUCTOR:
                            email.Subject = "PSDF:Approval for New Instructor through Change Request";
                            email.BodyHTML = "Dear User, \n A new change request of Instructor is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;
                        case EnumApprovalProcess.CR_INCEPTION:
                            email.Subject = "PSDF: New Inception Report Change Request";
                            email.BodyHTML = "Dear User, \n A new change request of Inception Report is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;
                        case EnumApprovalProcess.CR_INSTRUCTOR_REPLACE:
                            email.Subject = "PSDF: New Instructor Replace Change Request";
                            email.BodyHTML = "Dear User, \n A new change request of Inctructor Replacement is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                        default:
                            email.Subject = "PSDF: New Approval Assignment";
                            email.BodyHTML = "Dear User, \n A request is assigned to you for approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                    }
                    break;

                case (int)EnumApprovalStatus.SendBack:
                    switch (approvalModel.ProcessKey)
                    {
                        case EnumApprovalProcess.AP_PD:
                        case EnumApprovalProcess.AP_BD:
                            email.Subject = "PSDF: SendBack Appendix Approval";
                            email.BodyHTML = "Dear User, \n Your 'Appendix' approving request is sent back to you with Remarks : \" " + approvalHistoryModel.Comments + " \" .\n Kindly login to your PSDF BSS account to view details and further actions.";
                            break;

                        case EnumApprovalProcess.PRN_T:
                            email.Subject = "PSDF: SendBack TRN Approval Request";
                            email.BodyHTML = "Dear User, \nYour 'Testing Recomendation Note' approving request is sent back to you with Remarks : \" " + approvalHistoryModel.Comments + " \" .\n Kindly login to your PSDF BSS account to view details and further actions.";
                            break;
                        case EnumApprovalProcess.PRN_R:
                            email.Subject = "PSDF: SendBack Approval Assignment";
                            email.BodyHTML = "Dear User, \nYour 'Payment Recomendation Note' approving request is sent back to you with Remarks : \" " + approvalHistoryModel.Comments + " \" .\n Kindly login to your PSDF BSS account to view details and further actions.";
                            break;

                        case EnumApprovalProcess.SRN:
                            email.Subject = "PSDF: SendBack Approval Assignment";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                   Your <b>Stipend Recomendation Note</b> request is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                   Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;

                        case EnumApprovalProcess.INV_SRN:
                            email.Subject = "PSDF: SendBack Approval Assignment";
                            email.BodyHTML = "Dear User, \n Your Incvoice Of Stipend Recomendation Note approving request is send back to you with Remarks : \" " + approvalHistoryModel.Comments + " \" .\n Kindly login to your PSDF BSS account to view details and further actions";
                            break;

                        case EnumApprovalProcess.PO_SRN:
                            email.Subject = "PSDF: SendBack Approval Assignment";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Purchase Order Of Stipend Recomendation Note</b> request is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;
                        case EnumApprovalProcess.PO_TSP:
                            email.Subject = "PSDF: SendBack Approval Assignment";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Purchase Order Of TSP</b> request is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;

                        case EnumApprovalProcess.CR_CLASS_LOCATION:
                            email.Subject = "PSDF: SendBack Class Location Change Request";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Change request of Class Location</b> is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;
                        case EnumApprovalProcess.CR_CLASS_DATES:
                            email.Subject = "PSDF: SendBack Class Dates Change Request";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Change request of Class Dates</b> is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;
                        case EnumApprovalProcess.CR_TRAINEE_UNVERIFIED:
                            email.Subject = "PSDF: SendBack Trainee Profile Change Request";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Change request of Trainee Profile</b> is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;
                        case EnumApprovalProcess.CR_TRAINEE_VERIFIED:
                            email.Subject = "PSDF: SendBack Trainee Profile Change Request";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Change request of Trainee Profile</b> is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;

                        case EnumApprovalProcess.CR_INSTRUCTOR:
                            email.Subject = "PSDF: SendBack Instructor Change Request";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Change request of Instructor</b> is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;

                        case EnumApprovalProcess.CR_NEW_INSTRUCTOR:
                            email.Subject = "PSDF:Send Back New Instructor Request";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Change request of New Instructor</b> is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;
                        case EnumApprovalProcess.CR_INCEPTION:
                            email.Subject = "PSDF: SendBack Inception Report Change Request";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Change request Of Inception Report</b> is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;
                        case EnumApprovalProcess.CR_INSTRUCTOR_REPLACE:
                            email.Subject = "PSDF: SendBack Instructor Replace Change Request";
                            email.BodyHTML = $@"<html>
                                            <body>
                                                <p>Dear {user.FullName},</p>
                                                <p>
                                                    Your <b>Change request Of Instructor Replacement</b> is send back to you with Remarks :{approvalHistoryModel.Comments}.
                                                    Kindly <b>login</b> to your PSDF BSS account to view details and further actions.
                                                </p>
                                                <p>Thanks</p>
                                            </body>
                                         </html>";
                            break;

                        default:
                            email.Subject = "PSDF: SendBack Approval Assignment";
                            email.BodyHTML = "Dear User, \nYour request is send back to you with Remarks.\n Kindly login to your PSDF BSS account to view details and further actions.";
                            break;
                    }
                    break;

                case (int)EnumApprovalStatus.Rejected:
                    //if (currentApproval.ProcessKey == EnumApprovalProcess.AP)
                    //{
                    //    subject = "PSDF: SendBack Appendix Approval";
                    //    body = "Dear User, \nA new appendix was created which requires your approval.\n Kindly login to your PSDF BSS account to view details and further actions";
                    //}
                    break;

                default:
                    break;
            }
            return email;
        }
    }
}
