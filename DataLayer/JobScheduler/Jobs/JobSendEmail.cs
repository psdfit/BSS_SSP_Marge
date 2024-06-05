using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.JobScheduler.Jobs
{
    public class JobSendEmail : IJobSendEmail
    {
        private readonly ISRVNotificationDetail iSRVNotificationDetail;

        public JobSendEmail(ISRVNotificationDetail iSRVNotificationDetail)
        {
            this.iSRVNotificationDetail = iSRVNotificationDetail;
        }

        public async Task SendAsync(CancellationToken stoppingToken)
        {
            var UnverifiedTraineeEmaillist = iSRVNotificationDetail.GetUnverifiedTraineeEmails();

            foreach (var i in UnverifiedTraineeEmaillist)
            {
                //string verificationLink = $"http://bss.psdf.org.pk/#/email-verification/9920E741-C698-40B6-97E3-D730AD037345{i.TraineeEmailVerificationID}";
                string verificationLink = $"https://bss.psdf.org.pk:51599/api/Users/EmailVerification/{i.TraineeEmailVerificationID}";
                //string verificationLink = $"http://172.19.1.20:51599/api/Users/EmailVerification/{i.TraineeEmailVerificationID}";
                //string verificationLink = $"http://172.19.1.20:51599/api/Users/EmailVerification/{i.TraineeEmailVerificationID}";
                string subject = "Email Verification";
                string body = $@"
                                <!DOCTYPE html>
                                <html>
                                <head>
                                </head>
                                <body>
                                    <h2>Email Verification</h2>
                                    <p>Dear {i.TraineeName},</p>
                                    <p>Thank you for signing up with (Punjab Skills Development Fund). To complete your registration, please click the button below to verify your email address:</p>
                                    <p>
                                        <a href='{verificationLink}' target='_parent' style='background-color: #007BFF; color: #fff; padding: 10px 20px; text-decoration: none;'>Verify Email</a>
                                    </p>
                                    <p>Best regards,<br>Punjab Skills Development Fund</p>
                                </body>
                                </html>";

                // Send the email
                iSRVNotificationDetail.UpdateTraineeEmailsVerification(i.TraineeEmailVerificationID);
                await Common.SendEmailAsync(i.EmailAddress, subject, body, stoppingToken);

                // Update the verification status
            }
            var list = iSRVNotificationDetail.FetchNotificationDetailsForEmail();
            List<Task> emailTasks = new List<Task>(); // Declare the list here

            foreach (var item in list)
            {
                string subject = item.Subject;

                StringBuilder body = new StringBuilder();
                //body.AppendLine($"Dear {item.FullName}, <br>");
                //body.AppendLine($"{item.Body} <br>");
                body.Append(item.CustomComments);

                await Common.SendEmailAsync(item.UserEmail, subject, body.ToString(), stoppingToken);

                iSRVNotificationDetail.UpdateNotificationDetails(item);
            }
            await Task.WhenAll(emailTasks);
        }
    }
}