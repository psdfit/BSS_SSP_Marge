using DataLayer.Models;
using DataLayer.Services;
using DataLayer.Classes;
using DataLayer.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVGenerateInvoice : ISRVGenerateInvoice
    {
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVClass srvClass;
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVUser_Pwd srvUser_Pwd;
        private readonly ISRVSAPApi srvSAPApi;
        private readonly ISRVTSPDetail srvTSPDetail;
        private readonly ISRVClassInvoiceMap srvClassInvoiceMap;
        public SRVGenerateInvoice(ISRVTSPMaster srvTSPMaster, ISRVScheme srvScheme, ISRVClass srvClass, ISRVOrgConfig srvOrgConfig, ISRVUsers srvUsers, ISRVUser_Pwd srvUser_Pwd, ISRVSAPApi srvSAPApi, ISRVTSPDetail srvTSPDetail, ISRVClassInvoiceMap srvClassInvoiceMap)
        {
            this.srvTSPMaster = srvTSPMaster;
            this.srvScheme = srvScheme;
            this.srvClass = srvClass;
            this.srvOrgConfig = srvOrgConfig;
            this.srvUsers = srvUsers;
            this.srvSAPApi = srvSAPApi;
            this.srvTSPDetail = srvTSPDetail;
            this.srvClassInvoiceMap = srvClassInvoiceMap;
        }
        List<ClassModel> classes;
        SchemeModel scheme;
        OrgConfigModel org;
        int LimitDayNo;
        int DaysInAMonth;
        DateTime StartDate;
        DateTime EndDate;
        DateTime POStartDate;
        DateTime POEndDate;
        //string InvoiceType;
        string[] PaymentStructure;
        decimal Percentage;
        decimal NoOfInvoices;
        decimal DurationInDays;
        //decimal TotalPerTraineeCostExTax;
        //decimal CostPerDay;
        decimal MonthlyCostRegular;
        decimal CostPerDayRegular;
        decimal Cost2ndLast;
        decimal CostFinal;
        int startday;
        List<ClassInvoiceMapModel> ls;
        int InvoiceNo;
        decimal BalanceDays;

        public void GenerateInvoices(int SchemeID, SqlTransaction transaction = null)
        {
            classes = srvClass.FetchClassByScheme(SchemeID, transaction);
            scheme = srvScheme.GetBySchemeID(SchemeID, transaction);

            try
            {
                foreach (ClassModel c in classes)
                {
                    Initializations(c, transaction);

                    // Duration is 1 month or less
                    if (c.Duration <= 1)
                    {
                        CreateRegularInvoicesFor1MonthOrLess(c);
                    }
                    else
                    {
                        // handle for duration value in decimal and greater than 1 (1.5, 1.7, 2.8, ... etc)
                        // 2.5 * 30 = 75 days

                        CreateRegularInvoicesForGreaterThan1Month(c);
                    }

                    if (PaymentStructure.Length == 2)
                    {
                        ClassInvoiceMapModel m = new ClassInvoiceMapModel();

                        m.ClassID = c.ClassID;
                        m.InvoiceNo = ++InvoiceNo;
                        m.InvoiceType = "Final";
                        m.Amount = GetCostPercentage(PaymentStructure[1], (c.TotalPerTraineeCostInTax / (1 + c.SalesTaxRate)), "Final");
                        m.Month = EndDate.AddDays(org.EmploymentDeadline);

                        m.InvoiceDays = 30;
                        m.InvoiceStartDate = m.Month;
                        m.InvoiceEndDate = GetMonthEndDate(m.Month);
                        m.POStartDate = m.InvoiceStartDate;
                        m.POEndDate = m.InvoiceEndDate;

                        ls.Add(m);
                    }
                    else if (PaymentStructure.Length == 3)
                    {
                        ClassInvoiceMapModel m = new ClassInvoiceMapModel();
                        m.ClassID = c.ClassID;
                        //int paymentValue;
                        //if (int.TryParse(PaymentStructure[2], out paymentValue))
                        //{
                        //    if (paymentValue == 0)
                        //    {
                        //        m.InvoiceNo = InvoiceNo;
                        //    }
                        //    else
                        //    {
                        //        m.InvoiceNo = ++InvoiceNo;
                        //    }
                        //}
                        m.InvoiceNo = InvoiceNo;
                        m.InvoiceType = "2nd Last";
                        m.Amount = GetCostPercentage(PaymentStructure[1], (c.TotalPerTraineeCostInTax / (1 + c.SalesTaxRate)), "2nd Last");
                        m.Month = EndDate.AddDays(org.TSROpeningDays);

                        m.InvoiceDays = ls.Where(x => x.InvoiceType == "Regular").Sum(x => x.InvoiceDays);
                        m.InvoiceStartDate = m.Month;
                        m.InvoiceEndDate = GetMonthEndDate(m.Month);
                        m.POStartDate = m.InvoiceStartDate;
                        m.POEndDate = m.InvoiceEndDate;

                        ls.Add(m);

                        if (PaymentStructure.Length == 3)
                        {
                            int paymentValue1;
                            if (int.TryParse(PaymentStructure[2], out paymentValue1))
                            {
                                if (paymentValue1 != 0)
                                {
                                    m = new ClassInvoiceMapModel();
                                    m.ClassID = c.ClassID;
                                    m.InvoiceNo = ++InvoiceNo;
                                    m.InvoiceType = "Final";
                                    m.Amount = GetCostPercentage(PaymentStructure[2], (c.TotalPerTraineeCostInTax / (1 + c.SalesTaxRate)), "Final");
                                    m.Month = EndDate.AddDays(org.EmploymentDeadline);

                                    m.InvoiceDays = 30;
                                    m.InvoiceStartDate = m.Month;
                                    m.InvoiceEndDate = GetMonthEndDate(m.Month);
                                    m.POStartDate = m.InvoiceStartDate;
                                    m.POEndDate = m.InvoiceEndDate;

                                    ls.Add(m);
                                }
                            }
                        }
                    }

                    srvClassInvoiceMap.BatchInsert(ls, transaction);
                }
            }
            catch (Exception e)
            { throw new Exception(e.InnerException.ToString()); }
        }

        private void Initializations(ClassModel c, SqlTransaction transaction)
        {
            org = srvOrgConfig.GetByClassID(c.ClassID, transaction);

            LimitDayNo = 5; //  this should be in configuration
            DaysInAMonth = 30; // can be in configuration
            StartDate = c.StartDate.ToString().GetDate();
            EndDate = c.EndDate.ToString().GetDate();
            //InvoiceType = "Regular";

            PaymentStructure = scheme.PaymentSchedule.Split('/');
            Percentage = Convert.ToDecimal(PaymentStructure[0]); // eg. 70 from 70/10/20
            Percentage = Percentage / 100;
            DurationInDays = c.Duration * DaysInAMonth; // (3 * 30 = 90) or (0.5 * 30 = 15)

            //TotalPerTraineeCostExTax = c.TotalPerTraineeCostInTax - (c.TotalPerTraineeCostInTax *  c.SalesTaxRate);
            //CostPerDay = (TotalPerTraineeCostExTax * Percentage) / DurationInDays; // eg. 22000 / 90 = 244.44

            MonthlyCostRegular = GetCostPercentage(PaymentStructure[0], c.TrainingCostPerTraineePerMonthExTax, "Regular"); // 3448 * 0.7 = 2414
            Cost2ndLast = 0;
            CostFinal = 0;

            startday = Convert.ToInt32(StartDate.Day);

            ls = new List<ClassInvoiceMapModel>();
            InvoiceNo = 1;
            BalanceDays = 0;
        }

        private void CreateRegularInvoicesFor1MonthOrLess(ClassModel c)
        {
            NoOfInvoices = 1;

            if (startday >= 15 && c.Duration == 1) // eg If class starts from 15-16 and its 1 month class
            {
                NoOfInvoices += 1;
            }

            for (int i = 0; i < NoOfInvoices; i++)
            {
                ClassInvoiceMapModel m = new ClassInvoiceMapModel();

                m.ClassID = c.ClassID;
                m.InvoiceNo = InvoiceNo;
                m.InvoiceType = "Regular";
                m.Month = StartDate.AddMonths(i);

                decimal PerMonthInvoiceCost = MonthlyCostRegular;

                if (NoOfInvoices > 1)
                {
                    if (i == 0)
                    {
                        int LessDays = (DaysInAMonth - startday) + 1; // Start 15, End 14 of next month. 1st invoice of 16 days, 2nd 14 days
                        BalanceDays = DurationInDays - LessDays; // 14 = 30 - 16;

                        m.InvoiceStartDate = m.Month;                           // Start date of invoice
                        m.InvoiceEndDate = GetMonthEndDate(m.Month);    // End of month
                        m.InvoiceDays = LessDays;

                    }
                    else if (i == 1)
                    {
                        m.InvoiceStartDate = new DateTime(m.Month.Year, m.Month.Month, 1);  // 1st day of month
                        m.InvoiceEndDate = new DateTime(m.Month.Year, m.Month.Month, startday - 1);
                        m.InvoiceDays = (int)BalanceDays;
                    }
                }
                else // If single invoice
                {
                    m.InvoiceStartDate = m.Month;          // Start date of invoice
                    if (c.Duration == 1 || startday >= 15) // Full Month or Class start from 15-16
                    {
                        m.InvoiceEndDate = GetMonthEndDate(m.Month);    // End of month
                    }
                    else // Less than month eg 0.5
                    {
                        m.InvoiceEndDate = new DateTime(m.Month.Year, m.Month.Month, 15);
                    }

                    m.InvoiceDays = 30; //(int)DurationInDays;
                }

                Cost2ndLast += PerMonthInvoiceCost;
                CostFinal += PerMonthInvoiceCost;

                m.Amount = PerMonthInvoiceCost;

                ls.Add(m);

                InvoiceNo += 1;
            }

            if (ls.Count > 0)
            {
                POStartDate = ls[0].InvoiceStartDate;
                POEndDate = ls[ls.Count - 1].InvoiceEndDate;

                ls.ForEach(x => x.POStartDate = POStartDate);
                ls.ForEach(x => x.POEndDate = POEndDate);
            }
        }

        private void CreateRegularInvoicesForGreaterThan1Month(ClassModel c)
        {
            CostPerDayRegular = MonthlyCostRegular / DaysInAMonth; // eg. 2400 / 30 = 80

            if ((c.Duration % Math.Floor(c.Duration)) != 0) // 2.5 % 2 = 0.5 (!= 0)
            {
                GetRegularInvoicesDecimalDuration(c);
            }
            else // handle for normal case when duration is a whole number eg. 3
            {
                GetRegularInvoicesWholeNumberDuration(c);
            }
        }

        private void GetRegularInvoicesDecimalDuration(ClassModel c)
        {
            NoOfInvoices = Math.Ceiling(c.Duration); // (1.5) = 2

            for (int i = 0; i < NoOfInvoices; i++)
            {
                ClassInvoiceMapModel m = new ClassInvoiceMapModel();

                m.ClassID = c.ClassID;
                m.InvoiceNo = InvoiceNo;
                m.InvoiceType = "Regular";
                m.Month = StartDate.AddMonths(i);
                m.Amount = MonthlyCostRegular;

                if (i == 0)
                {
                    m.InvoiceStartDate = m.Month;
                    m.InvoiceEndDate = GetMonthEndDate(m.Month); // End of month

                    if (startday >= 15) // Class Start Date
                    {
                        m.InvoiceDays = 15;
                    }
                    else
                    {
                        m.InvoiceDays = 30;
                    }
                }
                else if (i == (NoOfInvoices - 1))
                {
                    if (startday >= 15) //  Class Start Date. Last invoice will be of 30 days
                    {
                        m.InvoiceStartDate = new DateTime(m.Month.Year, m.Month.Month, 1);
                        m.InvoiceEndDate = GetMonthEndDate(m.Month); // End of month
                        m.InvoiceDays = 30;
                    }
                    else // Last 15 days
                    {
                        m.InvoiceStartDate = new DateTime(m.Month.Year, m.Month.Month, 1);
                        m.InvoiceEndDate = new DateTime(m.Month.Year, m.Month.Month, 15); // End 15th of month
                        m.InvoiceDays = 15;
                    }
                }
                else
                {
                    m.InvoiceStartDate = new DateTime(m.Month.Year, m.Month.Month, 1);
                    m.InvoiceEndDate = GetMonthEndDate(m.Month); // End of month
                    m.InvoiceDays = 30;
                }

                Cost2ndLast += MonthlyCostRegular;
                CostFinal += MonthlyCostRegular;



                ls.Add(m);

                InvoiceNo += 1;
            }

            if (ls.Count > 0)
            {
                POStartDate = ls[0].InvoiceStartDate;
                POEndDate = ls[ls.Count - 1].InvoiceEndDate;

                ls.ForEach(x => x.POStartDate = POStartDate);
                ls.ForEach(x => x.POEndDate = POEndDate);
            }
        }

        private void GetRegularInvoicesWholeNumberDuration(ClassModel c)
        {
            NoOfInvoices = c.Duration;

            if (startday > LimitDayNo) // 12 > 5 // 15 > 5 . Class start from 15-16
            {
                NoOfInvoices += 1;
            }

            for (int i = 0; i < NoOfInvoices; i++)
            {
                ClassInvoiceMapModel m = new ClassInvoiceMapModel();

                m.ClassID = c.ClassID;
                m.InvoiceNo = InvoiceNo;
                m.InvoiceType = "Regular";
                m.Month = StartDate.AddMonths(i);
                m.Amount = MonthlyCostRegular;

                if (NoOfInvoices > c.Duration) // Class start from 15-16. There will be an extra invoice
                {
                    if (i == 0) // 1st half month invoice
                    {
                        int LessDays = (DaysInAMonth - startday) + 1; // Start 15, End 14 of next month. 1st invoice of 16 days, 2nd 14 days
                        BalanceDays = DaysInAMonth - LessDays; // 14 = 30 - 16;

                        m.InvoiceStartDate = m.Month;
                        m.InvoiceEndDate = GetMonthEndDate(m.Month); // End of month
                        m.InvoiceDays = LessDays;
                    }
                    else if (i == (NoOfInvoices - 1)) // Last half month invoice
                    {
                        m.InvoiceStartDate = new DateTime(m.Month.Year, m.Month.Month, 1);
                        m.InvoiceEndDate = new DateTime(m.Month.Year, m.Month.Month, startday - 1);
                        m.InvoiceDays = (int)BalanceDays;
                    }
                    else // Whole month(s) invoice
                    {
                        m.InvoiceStartDate = new DateTime(m.Month.Year, m.Month.Month, 1);
                        m.InvoiceEndDate = GetMonthEndDate(m.Month); // End of month
                        m.InvoiceDays = 30;
                    }
                }
                else // All invoices of whole months case. Class start from month start and end on month end
                {
                    if (i == 0)
                    {
                        m.InvoiceStartDate = m.Month;
                    }
                    else
                    {
                        m.InvoiceStartDate = new DateTime(m.Month.Year, m.Month.Month, 1);
                    }

                    m.InvoiceEndDate = GetMonthEndDate(m.Month); // End of month
                    m.InvoiceDays = 30;
                }

                Cost2ndLast += MonthlyCostRegular;
                CostFinal += MonthlyCostRegular;

                ls.Add(m);

                InvoiceNo += 1;
            }

            if (ls.Count > 0)
            {
                POStartDate = ls[0].InvoiceStartDate;
                POEndDate = ls[ls.Count - 1].InvoiceEndDate;

                ls.ForEach(x => x.POStartDate = POStartDate);
                ls.ForEach(x => x.POEndDate = POEndDate);
            }
        }

        private decimal GetCostPercentage(string Percentage, decimal Cost, string InvoiceType)
        {
            return (Convert.ToDecimal(Percentage) / 100) * Cost;
        }

        public List<EmailUsersModel> CreateTSPsAccounts(int SchemeID, int curUserID, SqlTransaction transaction = null)
        {
            List<TSPDetailModel> tsps = new List<TSPDetailModel>();
            List<EmailUsersModel> TspsToEmail = new List<EmailUsersModel>();

            try
            {
                tsps = srvTSPDetail.FetchTSPDetailByScheme(SchemeID, transaction);
                
                foreach (TSPDetailModel tsp in tsps)
                {
                    List<Object> u = CreateAccount(tsp, curUserID, transaction);

                    string subject, body;

                    if (u != null)
                    {
                        TspsToEmail.Add(new EmailUsersModel()
                        {
                            EmailAddress = tsp.CPEmail,
                            UserName = u[0].ToString(),
                            Password = u[1].ToString(),
                            IsAlreadyExist = (bool)u[2],
                            TSPContectPersonEmail = tsp.CPEmail,
                            FullName = tsp.TSPName
                        });
                    }
                    //else // already existing account
                    //{
                    //    TspsToEmail.Add(new EmailUsersModel()
                    //    {
                    //        EmailAddress = tsp.CPEmail,
                    //        UserName = "",
                    //        Password = "",
                    //        Existing = true, // existing account
                    //        FullName = tsp.TSPName
                    //    });
                    //}
                }

                return TspsToEmail;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public void GenerateEmailsToUsers(List<EmailUsersModel> user)
        {
            foreach (EmailUsersModel u in user)
            {
                string subject = "", body = "";

                //if (!u.IsAlreadyExist) // new account
                //{
                subject = "PSDF Training Service Provider Account Creation";
                //body = "Dear User, \nYour account for PSDF BSS has been created. Use the following credentials to login: http://mis.psdf.org.pk/test/" +
                //    " \nUsername: " + u.UserName + " \nPassword: " + u.Password;
                //body = $@"
                //            <html>
                //            <head>
                //            </head>
                //            <body>
                //                <p style='text-align:left;'><img src=cid:Header></p>
                //                   <p>Dear <b>{u.FullName} ,</b><p>
                //                    On behalf of Punjab Skills Development Fund (PSDF), we would like to take this opportunity and welcome you on board to our Business Support System (BSS). We at PSDF work tirelessly to provide best-in-class experience for all our partners.
                //                    <br />
                //                <p>Please find the below credentials:</p>
                //                <p>
                //                    User Name : <b>{u.UserName}</b><br />
                //                    Password: <b>{u.Password}</b ><br />
                //                   URL: <a href = 'http://bss.psdf.org.pk/'> http://bss.psdf.org.pk/</a>
                //                </p >
                //                <p>
                //                    Regards,<br />
                //                    Punjab Skills Development Fund
                //                </p>
                //                <p style='text-align:right;'><img src=cid:Footer></p>
                //             </ body >
                //             </ html > ";

                body = $@"
                                Dear {u.FullName},

                                On behalf of Punjab Skills Development Fund (PSDF), we would like to take this opportunity and welcome you on board to our Business Support System (BSS). We at PSDF work tirelessly to provide best-in-class experience for all our partners.

                                Please find the below credentials:

                                User Name: {u.UserName}
                                Password: {u.Password}
                                URL: http://bss.psdf.org.pk/

                                Regards,
                                Punjab Skills Development Fund
                                ";


                Common.SendEmail(u.EmailAddress, subject, body, true);
            }
        }

        public void GenerateEmailsToUsersB2C(List<EmailUsersModel> user)
        {
            foreach (EmailUsersModel u in user)
            {
                string subject = "", body = "";

                //if (!u.IsAlreadyExist) // new account
                //{
                subject = "Skills Scholarship Registration kick-off";
                //body = "Dear User, \nYour account for PSDF BSS has been created. Use the following credentials to login: http://mis.psdf.org.pk/test/" +
                //    " \nUsername: " + u.UserName + " \nPassword: " + u.Password;
                body = $@"
                            <html>
                            <head>
                            </head>
                            <body>
                                <p style='text-align:left;'><img src=cid:Header></p>
                                   <p>Dear <b>{u.FullName} ,</b><p>
                                    We are excited to welcome you to the kick-off of Skills Scholarship Initiative Scheme. Pl. note that trainee registration for this scheme will start on
                                    [03/04/2023] at sharp [09:00 am] 
                                    <br />
                                   URL: <a href = 'http://bss.psdf.org.pk/'> http://bss.psdf.org.pk/</a>
                                </p >
                                <p>
                                    Regards,<br />
                                    Punjab Skills Development Fund
                                </p>
                                <p style='text-align:right;'><img src=cid:Footer></p>
                             </ body >
                             </ html > ";


                Common.SendEmail(u.TSPContectPersonEmail, subject, body, true);
            }
        }

        private List<Object> CreateAccount(TSPDetailModel TSP, int curUserID, SqlTransaction transaction = null)
        {
            try
            {
                TSPMasterModel tSPMaster = srvTSPMaster.GetByTSPMasterID(TSP.TSPMasterID, transaction);
                if (tSPMaster.UserID != null && tSPMaster.UserID != 0) // account already exists, return
                {
                    //string tspSapCode = tSPMaster.SAPID;
                    int existingUserID = tSPMaster.UserID ?? default(int);
                    UsersModel usersModel = srvUsers.GetByUserID(existingUserID, transaction);
                    List<Object> ls = new List<Object>();
                    ls.Add(usersModel.UserName);
                    ls.Add(Common.DESDecrypt(usersModel.UserPassword));
                    ls.Add(true);
                    return ls;
                    //return null;
                }

                UsersModel user = new UsersModel();

                //user.UserName = TSP.TSPCode;
                user.UserName = GenerateUserName(TSP);
                //user.UserPassword = GeneratePassword(5, true);
                user.UserPassword =  Common.GeneratePass(8);

                //user.Fname = TSP.TSPName.Substring(0, TSP.TSPName.IndexOf(" "));
                user.Fname = TSP.TSPName;
                //user.lname = TSP.TSPName.Substring(TSP.TSPName.IndexOf(" ") + 1);
                user.lname = "";
                user.FullName = TSP.TSPName;
                user.Email = TSP.CPEmail;
                user.RoleID = (int)EnumRoles.TSP;
                user.UserLevel = (int)EnumUserLevel.TSP;
                user.InActive = false;
                user.CurUserID = curUserID;
                user.usersRights = new SRVRolesRights().GetByRoleIDForDefaultTSP((int)EnumRoles.TSP);
                // List<UsersRightsModel> list= new SRVRolesRights().GetByRoleIDForDefaultTSP((int)EnumRoles.TSP);
                user.userOrgs = new List<UserOrganizationsModel>();

                //user.usersRights.Add(new UsersRightsModel() { FormID = (int)EnumAppForms.MasterSheet, CanDelete = true, CanAdd = true, CanEdit = true, CreatedUserID = curUserID });
                //user.usersRights.Add(new UsersRightsModel() { FormID = (int)EnumAppForms.EventCalender, CanDelete = true, CanAdd = true, CanEdit = true, CreatedUserID = curUserID });
                ////user.usersRights.Add(new UsersRightsModel() { FormID = 28, CanDelete = true, CanAdd = true, CanEdit = true, CreatedUserID = 0}); // Registration Form
                //user.usersRights.Add(new UsersRightsModel() { FormID = (int)EnumAppForms.TSR, CanDelete = true, CanAdd = true, CanEdit = true, CreatedUserID = curUserID });
                //user.usersRights.Add(new UsersRightsModel() { FormID = (int)EnumAppForms.TCR, CanDelete = true, CanAdd = true, CanEdit = true, CreatedUserID = curUserID });
                //user.usersRights.Add(new UsersRightsModel() { FormID = (int)EnumAppForms.TSU, CanDelete = true, CanAdd = true, CanEdit = true, CreatedUserID = curUserID });
                //user.usersRights.Add(new UsersRightsModel() { FormID = (int)EnumAppForms.ChangeRequest, CanDelete = true, CanAdd = true, CanEdit = true, CreatedUserID = curUserID });

                List<object> l = srvUsers.SaveUsers(user, transaction);
                int UserID = Convert.ToInt32(l[2]);

                if (l.Count > 0)
                {
                    srvTSPMaster.SaveTSPMaster(new TSPMasterModel()
                    {
                        TSPMasterID = TSP.TSPMasterID,
                        TSPName = TSP.TSPName,
                        Address = TSP.Address,
                        NTN = TSP.NTN,
                        PNTN = TSP.PNTN,
                        GST = TSP.GST,
                        FTN = TSP.FTN,
                        UserID = UserID,
                    }, transaction);

                    //TSPMasterModel tSPMaster = srvTSPMaster.GetByTSPMasterID(TSP.TSPMasterID,transaction);
                    //string tspSapCode = tSPMaster.SAPID;
                    if (string.IsNullOrEmpty(tSPMaster.SAPID))
                    {
                        var response = srvSAPApi.SaveSAPBusinessPartnerForTSP(TSP, transaction).Result;
                        if (!string.IsNullOrEmpty(response?.SapObjId))
                        {
                            srvTSPMaster.UpdateTSPSAPId(TSP.TSPMasterID, response.SapObjId, transaction);
                        }
                    }
                    List<Object> ls = new List<Object>();
                    ls.Add(user.UserName);
                    ls.Add(user.UserPassword);
                    // Existing=false
                    ls.Add(false);

                    return ls;
                }
                else
                {
                    List<Object> ls = new List<Object>();
                    ls.Add(user.UserName);
                    ls.Add(user.UserPassword);
                    // Existing=true
                    ls.Add(true);

                    return ls;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private string GenerateUserName(TSPDetailModel TSP)
        {
            int i = 0;
            string username = "";

            foreach (char c in TSP.TSPName)
            {
                if (i == 5)
                    break;

                if (c == ' ')
                    continue;

                username += c.ToString();

                i++;
            }
            username = username + "-" + TSP.TSPMasterID;

            return username;
        }

       

        private string GeneratePassword()
        {
            string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numericChars = "0123456789";
            string specialChars = "!@#$%^&*()-_=+[]{}|;:'\",.<>?";

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            // Add at least one character from each category
            password.Append(lowercaseChars[random.Next(lowercaseChars.Length)]);
            password.Append(uppercaseChars[random.Next(uppercaseChars.Length)]);
            password.Append(numericChars[random.Next(numericChars.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            int requiredLength = 8 - password.Length; // Minimum length of 8 characters

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

        private DateTime GetMonthEndDate(DateTime Date)
        {
            var startDate = new DateTime(Date.Year, Date.Month, 1);
            return startDate.AddMonths(1).AddDays(-1);
        }
        private void GetRegularInvoicesDecimalDuration_Old(ClassModel c)
        {
            NoOfInvoices = Math.Ceiling(c.Duration); // (2.5) = 3

            if (startday > (DurationInDays % DaysInAMonth)) // eg. 6 > (75 % 30 = 15)
            {
                NoOfInvoices += 1;
            }

            for (int i = 0; i < NoOfInvoices; i++)
            {
                ClassInvoiceMapModel m = new ClassInvoiceMapModel();

                m.ClassID = c.ClassID;
                m.InvoiceNo = InvoiceNo;
                m.InvoiceType = "Regular";
                m.Month = StartDate.AddMonths(i);

                decimal PerMonthInvoiceCost = 0;

                if (i == 0)
                {
                    int LessDays = DaysInAMonth - (startday > LimitDayNo ? startday : 0); // 30 - 18 = 12 // 30 - 15 = 15
                    BalanceDays = Math.Abs((DurationInDays % DaysInAMonth) - LessDays); // (75 % 30 = 15) - 12 = 3 // 15 - 15 = 0

                    PerMonthInvoiceCost = CostPerDayRegular * (LessDays == 0 ? DaysInAMonth : LessDays); // 80.47 * 12 = 965.64 // 80.47 * 15 = 1207.05
                }
                else if (i == (NoOfInvoices - 1))
                {
                    PerMonthInvoiceCost = CostPerDayRegular * (BalanceDays == 0 ? DaysInAMonth : BalanceDays); // 80.47 * 3 = 241.41 // 80.47 * 30 = 2414
                }
                else
                {
                    //PerMonthInvoiceCost = CostPerDay * DaysInAMonth; // 80.47 * 30 = 2414
                    PerMonthInvoiceCost = MonthlyCostRegular; // 2414, same as above statement but simplified
                }

                Cost2ndLast += PerMonthInvoiceCost;
                CostFinal += PerMonthInvoiceCost;

                m.Amount = PerMonthInvoiceCost;

                ls.Add(m);

                InvoiceNo += 1;
            }
        }
    }
}
