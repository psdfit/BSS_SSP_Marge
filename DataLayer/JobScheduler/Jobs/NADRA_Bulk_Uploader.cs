using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;
using System.Data;
using DataLayer.Services;
using DataLayer.Models;
using System.Net.Mail;
using System.Threading;
using System.Drawing.Imaging;
using DataLayer.Interfaces;
using OfficeOpenXml;
using ExcelDataReader;
using System.Linq;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace DataLayer.Scheduler.Jobs
{
    public class NADRA_Bulk_Uploader : IJob
    {
        private readonly ISRVTraineeProfile _srvTraineeProfile;
        private readonly ISRVDistrict _srvdistrict;
        private readonly ISRVOrgConfig _srvorgconfig;
        private readonly ISRVClass _srvclass;
        private readonly ISRVTraineeStatus _srvTraineeStatus;
        private readonly IConfigurationRoot configuration;
        private readonly ISRVSendEmail srvSendEmail;
        public NADRA_Bulk_Uploader(ISRVSendEmail srvSendEmail, ISRVTraineeProfile srvTraineeProfile, ISRVDistrict srvdistrict, ISRVOrgConfig srvorgconfig, ISRVClass srvclass
            , ISRVTraineeStatus srvTraineeStatus)
        {
            _srvTraineeProfile = srvTraineeProfile;
            _srvdistrict = srvdistrict;
            _srvorgconfig = srvorgconfig;
            _srvclass = srvclass;
            this.srvSendEmail = srvSendEmail;
            _srvTraineeStatus = srvTraineeStatus;
            configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        }
        public async void Run()
        {
            await JobTask();
            ///SET Secudle
            var cancellationToken = new CancellationTokenSource().Token;
            _ = Task.Delay(TimeSpan.FromMinutes(1)).ContinueWith(async (t) =>
            {
                Run();
            }, cancellationToken);

            /// 2ND WAY
            //Timer timer = new Timer();
            //timer.Interval = TimeSpan.FromSeconds(60).TotalMilliseconds;
            //timer.Elapsed += (Object source, ElapsedEventArgs e) => Task.Run(() => Main());
            ////timer.AutoReset = true;
            //timer.Start();// = true;
        }
        public async Task JobTask()
        {
            string sourceFolder = FilePaths.NADRA_VERYSISFILES_DIR;
            string targetFolder = FilePaths.NADRA_TARGETFILES_DIR;
            ///Job
            await Task.Run(() =>
            {
                IConfigurationSection verisysJob = configuration.GetSection("AppSettings").GetSection("VerisysJob");
                if (Convert.ToBoolean(verisysJob.GetSection("IsEnable")?.Value))
                {
                    Upload(sourceFolder, targetFolder);
                    ProcessExcelFiles(sourceFolder, targetFolder);
                }
            });
        }
        //public void PuShNotification(string logFilePath, string message)
        //{
        //    try
        //    {
        //        ApprovalModel approvalModel = new ApprovalModel();
        //        ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
        //        approvalModel.ProcessKey = EnumApprovalProcess.NADRA_VERIFICATION;
        //        approvalModel.CustomComments = message;
        //        approvalModel.logFilePath = logFilePath;
        //        approvalModel.isUserMapping = true;
        //        srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);

        //    }
        //    catch (Exception ex)
        //    { throw new Exception(ex.Message); }
        //}
        private void ProcessExcelFiles(string sourceFolder, string targetFolder)
        {
            string line = string.Empty;
            string todayDate = DateTime.Now.ToString("dd-MMM-yyyy");
            string todayDateTime = DateTime.Now.ToString("dd-MMM-yyyy-HH:mm:ss");
            string logFilePath = Path.Combine(sourceFolder, $"{todayDate}_ExcelBulkLogFile.txt");
            List<int> TSPIDs = new List<int>();
            if (!Directory.Exists(sourceFolder)) Directory.CreateDirectory(sourceFolder);

            using (StreamWriter sw = new StreamWriter(logFilePath))
            {
                try
                {
                    ///Terminate execution If source Directory is not found
                    if (!System.IO.Directory.Exists(sourceFolder)) return;
                    string[] sourcefiles = Directory.GetFiles(sourceFolder, "*.xlsx");
                    if (sourcefiles.Length > 0)
                    {
                        ///Create Sub Directory
                        if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
                        targetFolder = Path.Combine(targetFolder, todayDate);
                        if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

                        //string logFilePath = Path.Combine(targetFolder, $"{todayDate}_LogFile.txt");
                        string cnicDir = Path.Combine(targetFolder, $"CNIC Images");
                        string htmlDir = Path.Combine(targetFolder, $"HTML Files");
                        string afterReadFileDir = Path.Combine(sourceFolder, todayDate);
                        string appRoot = string.Empty;
                        if (AppContext.BaseDirectory.IndexOf("bin") > 0)
                            appRoot = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin") - 1);
                        else
                            appRoot = AppContext.BaseDirectory;
                        string appNadraImageDir = FilePaths.TRAINEE_PROFILE_NADRA_DIR;

                        //using (StreamWriter sw = new StreamWriter(logFilePath))
                        //{
                        //string line = string.Empty;
                        foreach (string filePath in sourcefiles)
                        {

                            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                            //var streamtest = File.Open(filePath, FileMode.Open, FileAccess.Read);
                            //var readertest = ExcelReaderFactory.CreateReader(streamtest);
                            var existingFile = new FileInfo(filePath);

                            var readertestForUpdate = new ExcelPackage(existingFile);

                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            ExcelWorkbook WorkBook = readertestForUpdate.Workbook;
                            ExcelWorksheet workSheet = WorkBook.Worksheets.First();
                            //ExcelWorksheet workSheet = readertestForUpdate.Workbook.Worksheets[0];
                            var start = workSheet.Dimension.Start;
                            var end = workSheet.Dimension.End;
                            for (int row = start.Row; row <= end.Row; row++)
                            { // Row by row...
                                if (row > 1)
                                {

                                    List<DistrictModel> districtList = new List<DistrictModel>();
                                    districtList = _srvdistrict.FetchDistrict();

                                    workSheet.Cells[row, 11].Style.Numberformat.Format = "dd-mm-yy";
                                    string DOB = workSheet.Cells[row, 11].Text;
                                    string[] DOBArray = DOB.Split(new string[] { "-" }, StringSplitOptions.None);
                                    string testDateFormat = workSheet.Cells[row, 11].Text;
                                    //var BorthDate = DateTime.ParseExact(DOB, "dd-MMM-yy", CultureInfo.InvariantCulture);
                                    if (DOBArray.Length == 3)
                                    {
                                        string DOBDate = DOBArray[0];
                                        string DOBMonth = DOBArray[1];
                                        string DOBYear = DOBArray[2];
                                        //DateTime oDate = Convert.ToDateTime(DOB);
                                        if (Convert.ToInt32(DOBDate) < 10 && DOBDate.Length < 2)
                                        {
                                            DOBDate = "0" + DOBDate;
                                        }
                                        if (Convert.ToInt32(DOBMonth) < 10 && DOBMonth.Length < 2)
                                        {
                                            DOBMonth = "0" + DOBMonth;
                                        }
                                        if (Convert.ToInt32(DOBYear) < 40)
                                        {
                                            DOBYear = "20" + DOBYear;
                                        }
                                        else if (Convert.ToInt32(DOBYear) >= 40)
                                        {
                                            DOBYear = "19" + DOBYear;
                                        }

                                        var BorthDate = DateTime.ParseExact(DOBMonth + "/" + DOBDate + "/" + DOBYear, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                                        //string DOBFormatted = DOB.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                                        string CNICNumber = workSheet.Cells[row, 10].Text;
                                        string TraineeName = workSheet.Cells[row, 7].Text;
                                        string FatherNAME = workSheet.Cells[row, 8].Text;
                                        string Gender = workSheet.Cells[row, 15].Text;
                                        string CNICIssueDate = workSheet.Cells[row, 9].Text;
                                        string ResidenceDistrict = workSheet.Cells[row, 14].Text;
                                        string ResidenceTehsil = workSheet.Cells[row, 13].Text;
                                        string MonitoringStatus = workSheet.Cells[row, 20].Text;
                                        string CNICVerified = workSheet.Cells[row, 16].Text;
                                        string TraineeStatus = workSheet.Cells[row, 17].Text;
                                        string CNICUnVerifiedReason = workSheet.Cells[row, 19].Text;
                                        string DistrictVerified = workSheet.Cells[row, 18].Text;
                                        string BSSStatus = workSheet.Cells[row, 21].Text;
                                        string TraineeReligion = workSheet.Cells[row, 22].Text;
                                        string permanentAddress = workSheet.Cells[row, 23].Text;
                                        string permanentTehsil = workSheet.Cells[row, 24].Text;
                                        string permanentDistrict = workSheet.Cells[row, 25].Text;

                                        TraineeProfileModel traineeProfile = _srvTraineeProfile.GetTraineeProfileByCNIC(CNICNumber);

                                        if (traineeProfile != null)
                                        {
                                            if (traineeProfile.AgeVerified==false|| traineeProfile.DistrictVerified==false|| traineeProfile.CNICVerified==false)
                                            {
                                                TSPIDs.Add(traineeProfile.TSPID);
                                            }

                                            int traineeStatusTypeID = traineeProfile.TraineeStatusTypeID.Value;
                                            //if(traineeStatusName.ToLower() != "expelled" && traineeStatusName.ToLower() != "drop-out")
                                            if (traineeStatusTypeID != (int)EnumTraineeStatusType.Expelled && traineeStatusTypeID != (int)EnumTraineeStatusType.DropOut)
                                            {
                                                if (MonitoringStatus.ToLower() == "yes" && string.IsNullOrEmpty(BSSStatus))
                                                {
                                                    int traineeProfileId = traineeProfile.TraineeID;
                                                    //updating trainee profile and verification status
                                                    int ClassId = traineeProfile.ClassID;
                                                    OrgConfigModel orgconfig = _srvorgconfig.GetByClassID(ClassId);
                                                    ClassModel classModel = _srvclass.GetByClassID(ClassId);
                                                    int minAge = orgconfig.MinAge;
                                                    int maxAge = orgconfig.MaxAge;
                                                    DateTime classStartDate = classModel.StartDate.Value;
                                                    DateTime classEndDate = classModel.EndDate.Value;
                                                    //DateTime dateofbirth = Convert.ToDateTime(DOB);

                                                    // DateTime dateofbirth = DateTime.ParseExact(DOBDate + "/" + DOBMonth + "/" + DOBYear, "dd/mm/yy", CultureInfo.InvariantCulture);

                                                    string startDateAge = CalculateTraineeAge(BorthDate.AddDays(1), classStartDate);
                                                    string[] startDateArray = startDateAge.Split(new string[] { "," }, StringSplitOptions.None);
                                                    int startYears = Convert.ToInt32(startDateArray[0]);
                                                    int startMonths = Convert.ToInt32(startDateArray[1]);
                                                    int startDays = Convert.ToInt32(startDateArray[2]);
                                                    string endDateAge = CalculateTraineeAge(BorthDate.AddDays(1), classEndDate);
                                                    string[] endDateArray = endDateAge.Split(new string[] { "," }, StringSplitOptions.None);
                                                    int endYears = Convert.ToInt32(endDateArray[0]);
                                                    int endMonths = Convert.ToInt32(endDateArray[1]);
                                                    int endDays = Convert.ToInt32(endDateArray[2]);
                                                    bool isAgeVerified = true;
                                                    string UnderOverAge = "";
                                                    if (startYears > maxAge || (startYears == maxAge && (startMonths + startDays) > 0))
                                                    {
                                                        //if(Convert.ToInt32(startDateAge) > maxAge)
                                                        isAgeVerified = false;
                                                        UnderOverAge = "Overage";
                                                        _srvTraineeStatus.SaveTraineeStatus(new TraineeStatusModel()
                                                        {
                                                            TraineeStatusID = 0,
                                                            TraineeProfileID = traineeProfile.TraineeID,
                                                            TraineeStatusTypeID = (int)EnumTraineeStatusType.Expelled,
                                                            CreatedUserID = (int)EnumUsers.System,
                                                            Comments = "Overage"
                                                        });

                                                    }
                                                    else if (startYears < minAge)
                                                    {
                                                        if (endYears < minAge)
                                                        {
                                                            isAgeVerified = false;
                                                            UnderOverAge = "Underage";
                                                            _srvTraineeStatus.SaveTraineeStatus(new TraineeStatusModel()
                                                            {
                                                                TraineeStatusID = 0,
                                                                TraineeProfileID = traineeProfile.TraineeID,
                                                                TraineeStatusTypeID = (int)EnumTraineeStatusType.Expelled,
                                                                CreatedUserID = (int)EnumUsers.System,
                                                                Comments = "Underage"
                                                            });
                                                        }
                                                        else if (endYears >= minAge)
                                                        {
                                                            isAgeVerified = true;
                                                            UnderOverAge = "";
                                                        }
                                                    }
                                                    string disrictUnverifiedReason = "";
                                                    int intDistrictVerified = 1;
                                                    bool bitCnicVerified = true;
                                                    if (DistrictVerified.ToLower() == "no")
                                                    {
                                                        intDistrictVerified = 2;
                                                        disrictUnverifiedReason = "Out Of Punjab";
                                                        _srvTraineeStatus.SaveTraineeStatus(new TraineeStatusModel()
                                                        {
                                                            TraineeStatusID = 0,
                                                            TraineeProfileID = traineeProfile.TraineeID,
                                                            TraineeStatusTypeID = (int)EnumTraineeStatusType.Expelled,
                                                            CreatedUserID = (int)EnumUsers.System,
                                                            Comments = "Out Of Punjab"
                                                        });
                                                    }
                                                    if (string.IsNullOrEmpty(DistrictVerified))
                                                    {
                                                        intDistrictVerified = 3;
                                                    }


                                                    if (CNICVerified.ToLower() == "no")
                                                    {
                                                        bitCnicVerified = false;
                                                    }

                                                    //string formattedDOB = DOBYear + "-" + DOBMonth + "-" + DOBDate;
                                                    if (_srvTraineeProfile.NadraVerysisExcel(traineeProfileId, CNICNumber, TraineeName, FatherNAME, BorthDate, Gender, bitCnicVerified,
                                                        CNICUnVerifiedReason, intDistrictVerified, isAgeVerified, UnderOverAge, disrictUnverifiedReason,
                                                        ResidenceDistrict, ResidenceTehsil, TraineeReligion, permanentAddress, permanentDistrict, permanentTehsil))
                                                    {
                                                        //readertest.MergeCells
                                                        //workSheet.Cells[row, 24].Text;
                                                        DateTime currentDatetime = DateTime.Now;
                                                        workSheet.Cells[row, 21].Value = "Processed: " + currentDatetime;
                                                        readertestForUpdate.Save();
                                                    }
                                                    else
                                                    {
                                                        DateTime currentDatetime = DateTime.Now;
                                                        workSheet.Cells[row, 21].Value = "ERROR Found: " + currentDatetime;
                                                        readertestForUpdate.Save();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                DateTime currentDatetime = DateTime.Now;
                                                workSheet.Cells[row, 21].Value = "Trainees Status in BSS is : " + Enum.GetName(typeof(EnumTraineeStatusType), traineeStatusTypeID) + ": " + currentDatetime;
                                                readertestForUpdate.Save();
                                            }
                                        }

                                        else
                                        {
                                            workSheet.Cells[row, 21].Value = $"{todayDateTime} - CNIC not found";
                                            readertestForUpdate.Save();
                                        }
                                    }
                                    //else
                                    //{
                                    //    workSheet.Cells[row, 21].Value = "DOB Format is not incorrect";
                                    //    readertestForUpdate.Save();

                                    //}

                                }
                            }
                            readertestForUpdate.Save();
                            if (TSPIDs.Count>0)
                            {
                                ApprovalHistoryModel model = new ApprovalHistoryModel();
                                ApprovalModel approvalsModelForNotification = new ApprovalModel();
                                approvalsModelForNotification.UserIDs= _srvTraineeStatus.GetTSPUserByClassID_NADRA_Notification(TSPIDs);
                                approvalsModelForNotification.ProcessKey = EnumApprovalProcess.NADRA_VERIFICATION;
                                approvalsModelForNotification.isUserMapping = true;
                                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                            }
                        }
                        // }
                        line = !string.IsNullOrEmpty(line) ? line : $"{todayDateTime} - NADRA Bulk Verisys Excel file(s) executed successfully.";
                        //PuShNotification(logFilePath, line);
                    }
                    else
                    {
                        line = $"{todayDateTime} - Not found any excel file. " + sourceFolder;
                    }
                }
                catch (Exception ex)
                {
                    line = $"{todayDateTime} - {ex.Message} \n {ex.ToString()}";
                }
                line = !string.IsNullOrEmpty(line) ? line : $"{todayDateTime} - NADRA Bulk Verisys Excel file(s) executed successfully.";
                sw.WriteLine(line);

            }
            SendEmail(logFilePath);
        }
        static string CalculateTraineeAge(DateTime Dob, DateTime paramDate)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(paramDate.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == paramDate)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= paramDate)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = paramDate.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = paramDate.Subtract(PastYearDate).Hours;
            int Minutes = paramDate.Subtract(PastYearDate).Minutes;
            int Seconds = paramDate.Subtract(PastYearDate).Seconds;
            return String.Format("{0},{1},{2}",
            Years, Months, Days);
            // return String.Format("{0}",
            //Years);
        }


        #region  
        #region Execution Details
        /// Read html files from sourceFolder 
        /// Scrap and update records
        /// -----Create sub directories
        /// --------*moveDir = sourceFolder/sourceFolder+DateTime
        /// --------*target  = targetFolder
        /// --------*logDir  = targetFolder/{todayDate}_LogFile
        /// --------*cnicDir = targetFolder/targetFolder{todayDate}/CNIC Images/
        /// --------*htmlDir = targetFolder/sourceFolder{todayDate}/HTML Files/
        /// --------*AppRoot = appRoot/{FilePaths.TRAINEE_PROFILE_NADRA_DIR}
        /// -----GetTraineeProfile
        /// --------Save Image to *AppRoot
        /// --------Update DB Record
        /// --------After successfully update 
        /// ------------save image to *cnicDir
        /// ------------save html files to *htmlDir
        /// ------------Move html files to *moveDir
        /// ------Manual Verify the trainee and update DB records
        /// save logFile to *logDir
        /// Send email to the user with log attachement
        #endregion Execution Details

        /// <summary>
        /// Uploading html files from Specific Directory to System Directory <br></br>
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="targetFolder"></param>
        /// <returns>void</returns>
        private void Upload(string sourceFolder, string targetFolder)
        {
            string errorMsg = string.Empty;
            string todayDate = DateTime.Now.ToString("dd-MMM-yyyy");
            string todayDateTime = DateTime.Now.ToString("dd-MMM-yyyy-HH:mm:ss");
            try
            {
                ///Terminate execution If source Directory is not found
                if (!System.IO.Directory.Exists(sourceFolder)) return;
                string[] sourcefiles = Directory.GetFiles(sourceFolder, "*.html");

                if (sourcefiles.Length > 0)
                {
                    ///Create Sub Directory
                    if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
                    targetFolder = Path.Combine(targetFolder, todayDate);
                    if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

                    string logFilePath = Path.Combine(targetFolder, $"{todayDate}_LogFile.txt");
                    string cnicDir = Path.Combine(targetFolder, $"CNIC Images");
                    string htmlDir = Path.Combine(targetFolder, $"HTML Files");
                    string afterReadFileDir = Path.Combine(sourceFolder, todayDate);
                    string appRoot = string.Empty;
                    if (AppContext.BaseDirectory.IndexOf("bin") > 0)
                        appRoot = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin") - 1);
                    else
                        appRoot = AppContext.BaseDirectory;
                    string appNadraImageDir = FilePaths.TRAINEE_PROFILE_NADRA_DIR;

                    using (StreamWriter sw = new StreamWriter(logFilePath))
                    {
                        string line = string.Empty;
                        foreach (string filePath in sourcefiles)
                        {
                            var scrapData = FilesScraping(filePath, out errorMsg);
                            if (scrapData != null)
                            {
                                if (!string.IsNullOrEmpty(scrapData.ImageBase64))
                                {
                                    TraineeProfileModel trainee = _srvTraineeProfile.GetTraineeProfileByCNIC(scrapData.CNIC);
                                    if (trainee != null)
                                    {
                                        //string imagePath = appNadraImageDir + scrapData.CNIC + ".jpg";
                                        string fileName = Path.GetFileName(filePath);
                                        string rootImagePath = appRoot + appNadraImageDir;
                                        ///Save Image to AppRoot Directory
                                        string imgFullPath = SaveImageToDirectory(scrapData.ImageBase64, $"{trainee.TraineeID}_{scrapData.CNIC}", rootImagePath, out errorMsg);
                                        if (!string.IsNullOrEmpty(imgFullPath))
                                        {
                                            ///Update DB Record
                                            string imgPath = imgFullPath.Replace(appRoot, "");
                                            if (UpdateTraineeCNICImage(trainee.TraineeID, imgPath, out errorMsg))
                                            {
                                                ///After successfully update 
                                                //////save image to tragetFolder/tragetFolder{today}/CNIC Images/
                                                SaveImageToDirectory(scrapData.ImageBase64, scrapData.CNIC, cnicDir, out errorMsg);
                                                if (!Directory.Exists(htmlDir)) Directory.CreateDirectory(htmlDir);
                                                //////save html files to tragetFolder/tragetFolder{today}/HTML Files/
                                                File.Copy(filePath, Path.Combine(htmlDir, fileName), true);
                                                if (!Directory.Exists(afterReadFileDir)) Directory.CreateDirectory(afterReadFileDir);
                                                //////Move html files to sourceFolder/sourceFolder{today}/
                                                File.Move(filePath, Path.Combine(afterReadFileDir, fileName), true);
                                                line = $"{ todayDateTime }--[Success]---[Update Record]----------[Against][CNIC = {scrapData.CNIC } , TraineeID ={ trainee.TraineeID } , Class Code ={ trainee.ClassCode } & ImageFullPath = { imgFullPath} ]";
                                                /// 
                                            }
                                        }
                                        /////Manual Verify the trainee and update DB records
                                        //if (TraineeManualVerification(trainee, scrapData, out errorMsg))
                                        //{

                                        //}
                                        //else
                                        //{
                                        //    line = $"{ todayDateTime }--[Error]--[Verification Error]---[Against][CNIC = { scrapData.CNIC } ]---\n[ErrorMessage] : {errorMsg}";
                                        //}
                                    }
                                    else
                                    {
                                        line = $"{ todayDateTime }--[Error]-----[Trainee Not Found]------[Against][CNIC = { scrapData.CNIC } ]";
                                    }
                                }
                                else
                                {
                                    line = $"{ todayDateTime }--[Error]-----[Image Upload]-----------[Against][File = { filePath } , ErrorMessage= CNIC Image Not Found ]";
                                }
                            }
                            else
                            {
                                line = $"{ todayDateTime }--[Error]-----[Image Upload]-----------[Against][File = { filePath } , ErrorMessage= {errorMsg} ]";
                            }
                            sw.WriteLine(line);
                        }
                    }
                    /////Send logFile to given user email address
                    SendEmail(logFilePath);
                }
                else
                {
                    errorMsg = "Not found any html file." + sourceFolder;
                }
            }
            catch (Exception ex)
            {
                //errorMsg = ex.Message;

                using (StreamWriter sw = new StreamWriter(Path.Combine(sourceFolder, "log.txt")))
                {
                    string line = $"{ todayDateTime }--[Error]--\n[Exception] : {ex.ToString()}";
                    sw.WriteLine(line);
                }

            }
        }
        /// <summary>
        /// Return <see langword="ScrapModel"/> if it execute successfully <br></br>
        /// scrap the person information from DOM 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns> ScrapModel</returns>
        private ScrapModel FilesScraping(string filePath, out string errorMsg)
        {
            ScrapModel result = null;
            errorMsg = string.Empty;
            try
            {
                string CNIC = string.Empty;
                string Issuedate = string.Empty;
                string district = string.Empty;
                string line = string.Empty;
                string base64String = string.Empty;
                int CNindex;
                string chr;
                string Jogaght = string.Empty;
                string CNICB1 = string.Empty;
                StreamReader sr = new StreamReader(filePath);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("Identity Card Number:"))
                    {
                        Jogaght = line;
                        line = sr.ReadLine();
                        while (line.Trim().ToString() == "<br>")
                        {
                            line = sr.ReadLine();
                        }
                        //   line = sr.ReadLine();
                        chr = "\">";
                        CNindex = line.IndexOf(chr);



                        CNIC = line.Substring(CNindex + 2, 15);
                        CNICB1 = CNIC.ToString();

                    }
                    //if (line.Contains("Issue Date: (YYYY-MM-DD)"))
                    //{
                    //    line = sr.ReadLine();
                    //    line = sr.ReadLine();
                    //    chr = "\">";
                    //    CNindex = line.IndexOf(chr);

                    //    Issuedate = line.Substring(CNindex + 2, 10);
                    //    string ISSUEB1 = Issuedate.ToString();
                    //}
                    if (line.Contains("<img src=") && Jogaght.Contains("Identity Card Number:"))
                    {
                        line = line.Trim();
                        chr = "=";
                        CNindex = line.IndexOf(chr);

                        base64String = line.Substring(CNindex + 25, line.Length - 35);
                        base64String = base64String.Replace("\"", "");
                    }
                    //if (line.Contains("Place of Birth"))
                    //{
                    //    line = sr.ReadLine();
                    //    line = sr.ReadLine();
                    //    line = sr.ReadLine();
                    //    line = sr.ReadLine();
                    //    //chr = "\">";
                    //    //CNindex = line.IndexOf(chr);
                    //    district = line.Split(',')[1].Replace("</span>", "").Trim();
                    //}
                }
                result = new ScrapModel()
                {
                    CNIC = CNIC,
                    CNICIssueDate = Issuedate,
                    District = district,
                    ImageBase64 = base64String
                };
                sr.Close();
                sr.Dispose();
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// Send an email with logfile attachment
        /// </summary>
        /// <param name="logfilePath"></param>
        /// <returns></returns>
        private bool SendEmail(string logfilePath)
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            IConfigurationSection email = configuration.GetSection("AppSettings").GetSection("Email");

            string mailServer = email.GetSection("MailServer").Value;
            string mailAddress = email.GetSection("MailAddress").Value;
            string userName = email.GetSection("UserName").Value;
            string password = email.GetSection("Password").Value;

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");
            mail.From = new MailAddress(mailAddress);
            //mail.To.Add("m.asim@psdf.org.pk");
            //mail.To.Add("azhar.iqbal@psdf.org.pk");
            mail.To.Add("bssoperations@psdf.org.pk");
            //mail.CC.Add("hammad.gis@gmail.com");
            //mail.CC.Add("numan.tariq@abacus-global.com");
            mail.Subject = "CNIC Bulkupload Log File";
            mail.Body = "Please find attachment.";

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(logfilePath);
            mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(userName, password);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            return true;
        }
        private bool TraineeManualVerification(TraineeProfileModel trainee, ScrapModel scrapData, out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                if (trainee != null && scrapData != null)
                {
                    bool AgeVerified = true;
                    bool DistrictVerified = false;
                    bool CNICVerified = false;
                    string verifiedReason = "Verified By NADRA Verisys";
                    string UnVerifiedReason = "Unverified By NADRA Verisys";

                    if (trainee.TraineeCNIC.Trim() == scrapData.CNIC.Trim())
                    {
                        CNICVerified = true;
                    }

                    if ((trainee.DistrictName.Trim().ToLower() == scrapData.District.Trim().ToLower()
                        || trainee.DistrictNameUrdu?.Trim() == scrapData.District?.Trim())
                        //&& trainee.ProvinceID == (int)EnumProvince.Punjab
                        )
                    {
                        DistrictVerified = true;
                    }
                    var model = new TraineeProfileModel()
                    {
                        TraineeID = trainee.TraineeID,
                        CNICVerified = CNICVerified,
                        AgeVerified = AgeVerified,
                        DistrictVerified = DistrictVerified,
                        CNICUnVerifiedReason = CNICVerified ? verifiedReason : UnVerifiedReason,
                        AgeUnVerifiedReason = AgeVerified ? verifiedReason : UnVerifiedReason,
                        ResidenceUnVerifiedReason = DistrictVerified ? verifiedReason : UnVerifiedReason,
                        CurUserID = (int)EnumUsers.System
                    };
                    var verifiy = _srvTraineeProfile.TraineeManualVerification(model);
                    result = true;
                }
                else
                {
                    errorMsg = "trainee or scrap data is null";
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
            }
            return result;
        }
        private bool UpdateTraineeCNICImage(int traineeID, string imgPath, out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@TraineeID", traineeID);
                param[1] = new SqlParameter("@CNICImgNADRA", imgPath);
                param[2] = new SqlParameter("@CurUserID", (int)EnumUsers.System);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[TraineeProfile_UpdateNadraImage]", param);
                result = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return result;
        }
        private string SaveImageToDirectory(string base64, string fileName, string directoryPath, out string errorMsg)
        {
            string imgPath = string.Empty;
            errorMsg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(base64))
                {
                    imgPath = Path.Combine(directoryPath, fileName) + ".jpg";
                    //OverWrite image( delete image IF already exist)
                    if (File.Exists(imgPath)) File.Delete(imgPath);
                    //Create direcotry if not Exist
                    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                    Image img = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64)));
                    img.Save(imgPath);
                }
                else
                {
                    errorMsg = "Base64 is empty.";
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return imgPath;
        }
        #endregion
    }
    class ScrapModel
    {
        public string CNIC { get; set; }
        public string CNICIssueDate { get; set; }
        public string District { get; set; }
        public string ImageBase64 { get; set; }
    }
}
