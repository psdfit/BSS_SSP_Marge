//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace DataLayer.Services
//{
//    public class BaseClass
//    {
//        public BaseClass()
//        {
//            TMData = DatabaseFactory.CreateDatabase("DBConnectionString");
//        }
//        public Database TMData { get; set; }
//    }

//    public class ClassMonitoringReport : BaseClass
//    {
//        public ClassMonitoringReport()
//        {
//            this.MinorCount = 0;
//            this.MajorCount = 0;
//            this.SeriousCount = 0;
//            this.ObservationCount = 0;

//            this.MrInfo = new Dictionary<int, Dictionary<string, string>>();
//            this.GhostTrainees = new List<Dictionary<string, string>>();
//            this.MarginalTrainees = new List<Dictionary<string, string>>();
//            this.FakeTrainees = new List<Dictionary<string, string>>();
//            this.DropoutTrainees = new List<Dictionary<string, string>>();
//            this.DeletedTrainees = new List<Dictionary<string, string>>();
//            this.AdditionalTrainees = new List<Dictionary<string, string>>();
//            this.ViolationsList = new List<Dictionary<string, string>>();
//            this.TraineeFeedback = new List<Dictionary<string, string>>();
//            this.ViolationType = new Dictionary<string, string>();
//        }

//        public string SchemeID { get; set; }
//        public string TSPName { get; set; }
//        public string CentreName { get; set; }
//        public string VisitNo { get; set; }
//        public string SchemeName { get; set; }
//        public string ClassCode { get; set; }
//        public string TradeName { get; set; }
//        public string TrainerName { get; set; }
//        public string ReportingMonth { get; set; }

//        public string IsLockRemarks { get; set; }
//        public string IsRelocatedRemarks { get; set; }
//        public string EqipmentAvailRemarks { get; set; }
//        public string InstructorRemarks { get; set; }

//        public string FMRemarks { get; set; }
//        public string TSPRemarks { get; set; }

//        public Dictionary<int, Dictionary<string, string>> MrInfo { get; set; }
//        public List<Dictionary<string, string>> GhostTrainees { get; set; }
//        public List<Dictionary<string, string>> FakeTrainees { get; set; }
//        public List<Dictionary<string, string>> MarginalTrainees { get; set; }
//        public List<Dictionary<string, string>> DropoutTrainees { get; set; }
//        public List<Dictionary<string, string>> DeletedTrainees { get; set; }
//        public List<Dictionary<string, string>> AdditionalTrainees { get; set; }
//        public List<Dictionary<string, string>> ViolationsList { get; set; }
//        public List<Dictionary<string, string>> TraineeFeedback { get; set; }
//        public Dictionary<string, string> ViolationType { get; set; }

//        public int MinorCount { get; set; }
//        public int MajorCount { get; set; }
//        public int SeriousCount { get; set; }
//        public int ObservationCount { get; set; }
//    }

//    public class ClassMonitoringRequest : BaseClass
//    {
//        Trade Trade;
//        public ClassMonitoringRequest()
//        {
//            this.Trade = new Trade();
//        }
//        public Scheme Scheme { set; get; }
//        public ServiceProvider ServiceProvider { set; get; }
//        public string ClassCode { get; set; }
//        public string ClassInspectionRequestID { get; set; }
//        public string TrainingCentreName { get; set; }
//        public string TrainingLocAddress { get; set; }
//        public string ClassCodeString
//        {
//            get
//            {
//                return string.Format("{0}-{1}-{2}", this.Scheme.Code, this.ServiceProvider.Code, this.ClassCode);
//            }
//        }
//        public Trade Trade { get; set; }
//        //Added by Pentaloop
//        public string ClassReportDownloadLink
//        {
//            get { return "Download"; }

//        }
//        /*******************************************************/
//    }

//    public class ProfileVerificationReport : BaseClass
//    {
//        public ProfileVerificationReport()
//        {
//            this.registeredTraineesCount = 0;
//            this.presentTraineesCount = 0;

//            this.TraineesList = new List<Dictionary<string, string>>();
//        }

//        public string SchemeName { get; set; }
//        public string TSPName { get; set; }
//        public string CentreAddress { get; set; }
//        public string ClassCode { get; set; }
//        public string TradeName { get; set; }
//        public string VisitDate { get; set; }
//        public string IsLock { get; set; }

//        public string TraineeCountRemarks { get; set; }
//        public int registeredTraineesCount { get; set; }
//        public int presentTraineesCount { get; set; }

//        public List<Dictionary<string, string>> TraineesList { get; set; }

//        public void AddTrainee(string srNo, string name, string fatherName, string cnic, string verificationStatus, string remarks)
//        {
//            Dictionary<string, string> dictObj = new Dictionary<string, string>()
//            {
//                {"sr_no" , srNo},
//                {"name" , name},
//                {"father_name", fatherName},
//                {"cnic", cnic},
//                {"verification_status", verificationStatus},
//                {"remarks", remarks}
//            };

//            this.TraineesList.Add(dictObj);
//        }

//    }

//    public class ViolationSummary : BaseClass
//    {
//        public ViolationSummary()
//        {
//            this.ClassViolationsList = new List<Dictionary<string, string>>();
//        }
//        public string TSPName { get; set; }
//        public string SchemeID { get; set; }
//        public string SchemeType { get; set; }
//        public string SchemeName { get; set; }
//        public string ReportingMonth { get; set; }
//        public string MonthlyVisitsInfo { get; set; }

//        public string AllMajorCount { get; set; }
//        public string AllMinorCount { get; set; }
//        public string AllSeriousCount { get; set; }
//        public string AllObservationsCount { get; set; }
//        public string AllViolationsCount { get; set; } // major, minor and servious count

//        public List<Dictionary<string, string>> ClassViolationsList { get; set; }

//        public void AddClassVioation(string srNo, string tspName, string classCode, string majorCount, string minorCount, string seriousCount, string totalViolation, string observationCount, string remarks)
//        {
//            Dictionary<string, string> dictObj = new Dictionary<string, string>()
//            {
//                {"sr_no" , srNo},
//                {"tsp_name" , tspName},
//                {"classcode", classCode},
//                {"major_count", majorCount},
//                {"minor_count", minorCount},
//                {"serious_count", seriousCount},
//                {"total_violations", totalViolation},
//                {"observation_count", observationCount},
//                {"remarks", remarks}
//            };

//            this.ClassViolationsList.Add(dictObj);
//        }


//    }

//    public class Class : BaseClass
//    {
//        public Class()
//        {
//            this.Trade = new Trade();
//        }
//        public Guid ID { set; get; }
//        public Scheme Scheme { set; get; }
//        public ServiceProvider ServiceProvider { set; get; }
//        public Instructor instructor { set; get; }
//        public string ClassCode { get; set; }
//        public string ClassCodeString
//        {
//            get
//            {
//                return string.Format("{0}-{1}-{2}", this.Scheme.Code, this.ServiceProvider.Code, this.ClassCode);
//            }
//        }

//        public Center Center { set; get; }
//        public Trade Trade { get; set; }
//        public Batch Batch { get; set; }
//        public Gender Gender { get; set; }
//        public Shift Shift { get; set; }
//        public Section Section { get; set; }
//        public Int32 TotalTrainingDays { get; set; }
//        public string ShiftTimeFrom { get; set; }
//        public string ShiftTimeTo { get; set; }
//        public DateTime Contract_stratingDate { get; set; }
//        public DateTime Contract_EndDate { get; set; }
//        public DateTime Master_studentProfileDueon { get; set; }
//        public Int32 Contract_NoOfTraineesEnrolled { get; set; }
//        public bool Contract_TrainingDeliveredSelf { get; set; }
//        public string Contract_TrainingDeliveredBy { get; set; }
//        public int Contract_TrainingDuration { get; set; }
//        public int Contract_TotalTrainingHours { get; set; }
//        public int Contract_TotalTrainingDays { get; set; }
//        public int Contract_TrainingHoursPerDay { get; set; }
//        public NoticeToProceed Master_NoticeToProceed { get; set; }
//        public RequestToProceed Master_RequestToProceed { get; set; }
//        public DateTime? Master_InceptionReportDueOn { get; set; }
//        public Boolean Master_TraineeProfilesReceived { get; set; }
//        public DateTime? Master_CompletionReportDueOn { get; set; }
//        public Boolean Master_InceptionReporReceived { get; set; }
//        public Boolean Master_InceptionReportDeliveredTP { get; set; }
//        public DateTime? Master_DeliveryDateToTp { get; set; }
//        public Int32 Master_TotalTraineeProfile { get; set; }
//        public CompletionReportStatus Master_CompletationReportStatus { get; set; }
//        public TestingAgency CertifyingAgency { get; set; }
//        public int ClassSize { get; set; }
//        public ClassStatus ClassStatus { get; set; }
//        public int ClaimedTrainees { get; set; }
//        public int MaxAttendance_VisitTPM { get; set; }
//        public int MarginalTrainees { get; set; }
//        public TraningDays TrainingDays { get; set; }
//        public DateTime? TraineeProfileReceiveddate { get; set; }
//        public int TotalHoursPerWeek { get; set; }
//        public int EmploymentContractValue { get; set; }
//        public string remarks { get; set; }
//        //Add by Rao ALi Haider 26-Aug-2019
//        public EmploymentInvoiceStatus EmploymentInvoiceStatus { get; set; }
//        //
//        public string genderstring
//        {
//            get
//            {
//                if (Gender == Gender.Male)
//                {
//                    return "Male";
//                    //                  Male=1,
//                    //Female=2,
//                    //both=3
//                }
//                else if (Gender == Gender.Female)
//                {
//                    return "Female";
//                }
//                else if (Gender == Gender.Transgender) //Added b y Rao Ali Haider 2_Sep_2019
//                {
//                    return "Transgender";
//                }
//                else
//                {
//                    return "both";
//                }
//            }
//        }

//        public string ClassStatusName
//        {
//            get
//            {
//                if (ClassStatus == ClassStatus.Planned)
//                {
//                    return "Planned";
//                }
//                else if (ClassStatus == ClassStatus.Ready)
//                {
//                    return "Ready";
//                }
//                else if (ClassStatus == ClassStatus.Active)
//                {
//                    return "Active";
//                }
//                else if (ClassStatus == ClassStatus.Completed)
//                {
//                    return "Completed";
//                }
//                else if (ClassStatus == ClassStatus.Cancelled)
//                {
//                    return "Cancelled";
//                }
//                else
//                {
//                    return "Suspended";
//                }
//            }
//        }
//        public string ClassToolsLink
//        {
//            get { return "Tools"; }

//        }//Just to use in Class Listing
//         /* Added By PentaLoop */
//        public string ClassReportDownloadLink
//        {
//            get { return "Download"; }

//        }
//        /* End */
//        public string Contract_stratingDateString
//        {
//            get { return this.Contract_stratingDate.ToString("dd-MM-yyyy"); }

//        }

//        public string Contract_EndDateString
//        {
//            get { return this.Contract_EndDate.ToString("dd-MM-yyyy"); }

//        }
//    }

//    public class CentreTrade : BaseClass
//    {
//        public string TradeName { get; set; }
//        public int ClassesPerBatch { get; set; }
//        public int TraineesPerClass { get; set; }
//        public int TraineesPerTrade { get; set; }
//        public int ContractualTrainees { get; set; }
//        public string QuantitySufficient { get; set; }
//        public string ItemsMissing { get; set; }
//        public string LabRooms { get; set; }
//        public string SpaceSufficient { get; set; }
//        public string PowerBackup { get; set; }
//    }

//    public class CentreReport : BaseClass
//    {
//        public CentreReport()
//        {
//            this.ClassList = new List<CentreClass>();
//            this.TradeList = new List<CentreTrade>();
//            this.TradeToolsList = new List<List<Dictionary<string, string>>>();
//        }

//        public string SchemeName { get; set; }
//        public string TSPName { get; set; }
//        public string TrainingCentreName { get; set; }
//        public string VisitDateTime { get; set; }
//        public string CentreAddress { get; set; }
//        public int ClassesInspectedCount { get; set; }
//        public string InchargeName { get; set; }
//        public string InchargeMob { get; set; }

//        public string LocSuitable { get; set; }
//        public string LocSuitableRemarks { get; set; }
//        public string LocSuitableRecom { get; set; }
//        public string PremisesSecurity { get; set; }
//        public string PremisesSecurityRemarks { get; set; }
//        public string PremisesSecurityRecom { get; set; }
//        public string StructIntegrity { get; set; }
//        public string StructIntegrityRemarks { get; set; }
//        public string StructIntegrityRecom { get; set; }
//        public string InchargeRoom { get; set; }
//        public string InchargeRoomRemarks { get; set; }
//        public string InchargeRoomRecom { get; set; }
//        public string ElecticSupply { get; set; }
//        public string ElecticSupplyRemarks { get; set; }
//        public string ElecticSupplyRecom { get; set; }
//        public string ToiletAvail { get; set; }
//        public string ToiletAvailRemarks { get; set; }
//        public string ToiletAvailRecom { get; set; }
//        public int ToiletCount { get; set; }
//        public string WaterAvail { get; set; }
//        public string WaterAvailRemarks { get; set; }
//        public string WaterAvailRecom { get; set; }
//        public string FirstAidAvail { get; set; }
//        public string FirstAidAvailRemarks { get; set; }
//        public string FirstAidAvailRecom { get; set; }

//        public string keyFacMissing { get; set; }
//        public string keyFacStructure { get; set; }
//        public string keyFacFurnitureAvail { get; set; }
//        public string keyFacElecAvail { get; set; }
//        public string keyFacEquipAvail { get; set; }

//        public List<CentreTrade> TradeList { get; set; }
//        public List<CentreClass> ClassList { get; set; }
//        public List<List<Dictionary<string, string>>> TradeToolsList { get; set; }

//        public string FieldMonitorName { get; set; }
//        public string TSPRepName { get; set; }
//        public string DistrictInchargeName { get; set; }

//        public string FMSubmissionDateTime { get; set; }
//        public string DIAssignDateTime { get; set; }

//        public string FieldMonitorRemarks { get; set; }
//        public string InchargeRemarks { get; set; }

//        public string TspSignatureImg { get; set; }
//        public string FmSignatureImg { get; set; }

//        // list of tools will be added later

//        public Dictionary<string, string> AddTradeTool(string tradeName, string tradeDuration, string headCount, string toolName, string quantityRequired, string actualQuantity)
//        {
//            Dictionary<string, string> dictObj = new Dictionary<string, string>()
//            {
//                {"trade_name" , tradeName},
//                {"trade_duration" , tradeDuration},
//                {"trainee_head_count" , headCount},
//                {"tool_name", toolName},
//                {"tool_qty", quantityRequired},
//                {"tool_actual_qty", actualQuantity},
//            };
//            return dictObj;

//        }
//    }

//    public class CentreClass : BaseClass
//    {
//        public CentreClass()
//        {

//        }

//        public string ClassCode { get; set; }
//        public string TradeName { get; set; }
//        public string ClassStartDate { get; set; }
//        public string BoardAvail { get; set; }
//        public string ChairsAvail { get; set; }
//        public string BulbsAvail { get; set; }
//        public string VentAvail { get; set; }
//        public string SpaceSufficient { get; set; }

//    }


//}
