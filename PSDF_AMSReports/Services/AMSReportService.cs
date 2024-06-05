using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.SqlClient;
using PSDF_AMSReports.Dapper;
using PSDF_AMSReports.Interfaces;
using PSDF_AMSReports.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace PSDF_AMSReports.Services
{
    public class AMSReportService : IAMSReportService
    {
        public IDapperConfig _dapper { get; set; }
        public const string SignatureImageDirectory = @"c:\inetpub\wwwroot\BssAMSApi\Uploads\class";
        public AMSReportService(IDapperConfig dapper){
            _dapper = dapper;
        }
        public async Task<AMSFormThreeViewModel> GetAMSFormThreeReport(int? schemeId, int? tspId, int? classId, string dateTime)
        {
            try
            {

                //using (HttpClient client = new HttpClient())
                //{
                //    HttpResponseMessage response = await client.GetAsync("http://119.159.234.172:51600/AMS_API/AMS/Uploads/class/W4-254-005/mr-class-3222-fm_signature-1516428362832817047.jpeg");
                //    byte[] content = await response.Content.ReadAsByteArrayAsync();
                //    //return "data:image/png;base64," + Convert.ToBase64String(content);
                //}

                DateTime? date = null;
                if (!string.IsNullOrEmpty(dateTime))
                {
                    date = DateTime.ParseExact(dateTime, "dd/MM/yyyy", null);
                }
                schemeId = schemeId > 0 ? schemeId : null;
                tspId = tspId > 0 ? tspId : null;
                classId = classId > 0 ? classId : null;
                int? month = null;
                int? year = null;
                if (date != null)
                {
                    month = date.Value.Month;
                    year = date.Value.Year;
                }
                var model = new AMSFormThreeViewModel();
                var visitCount = await _dapper.QueryAsync<AMSFormThreeViewModel>("dbo.RD_AMSVisitCountReport", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                model.VisitCount = visitCount.First().VisitCount;
                var list = await _dapper.QueryAsync<AMSVisitListModel>("dbo.RD_AMSFormThreeVisitsReport", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                if (list.Any())
                {
                    var tspDetails = new AMSTSPDetailsModel();
                    tspDetails.TSPName = list.First().TSPName;
                    tspDetails.VisitInMonths = 0;
                    tspDetails.CentreName = list.First().CentreName;
                    tspDetails.SchemeName = list.First().SchemeName;
                    tspDetails.ClassCode = list.First().ClassCode;
                    tspDetails.TradeName = list.First().TradeName;
                    tspDetails.InstructorsInfo = list.First().InstructorsInfo;
                    tspDetails.ReportingMonth = list.First().VisitDateTime.Value.ToString("MMM") + "-" + list.First().VisitDateTime.Value.ToString("yy");
                    model.TSPDetails = tspDetails;
                    model.VisitDetails = list.ToList();

                    //var viLi = new List<AMSVisitModel>();
                    //foreach (var item in list)
                    //{
                    //    var v1 = new AMSVisitModel();
                    //    v1.IsLock = item.IsLock == "y" ? "Yes" : "No";
                    //    v1.IsRelocated = item.IsRelocated == "y" ? "Yes" : "No";
                    //    v1.EquipmentAvailable = item.IsEquipmentAvailable == "y" ? "Yes" : "No";
                    //    v1.SameInstructor = item.InstructorChanged == "y" ? "Yes" : "No";
                    //    v1.IsLockRemarks = item.IsLockRemarks;
                    //    v1.IsRelocatedRemarks = item.IsRelocatedRemarks;
                    //    v1.EquipmentAvailableRemarks = item.IsEquipmentAvailableRemarks;
                    //    v1.SameInstructorRemarks = item.IsAllocatedTrainerRemarks;
                    //    viLi.Add(v1);
                    //}
                    //model.VisitsList = viLi;

                    for (int i = 0; i < list.Count(); i++)
                    {
                        var item = list.ToList()[i];
                        var obj = new AMSVisitModel();
                        obj.IsLock = item.IsLock == "y" ? "Yes" : "No";
                        obj.IsRelocated = item.IsRelocated == "y" ? "Yes" : "No";
                        obj.EquipmentAvailable = item.IsEquipmentAvailable == "y" ? "Yes" : "No";
                        obj.SameInstructor = item.InstructorChanged == "y" ? "Yes" : "No";
                        obj.IsLockRemarks = item.IsLockRemarks;
                        obj.IsRelocatedRemarks = item.IsRelocatedRemarks;
                        obj.EquipmentAvailableRemarks = item.IsEquipmentAvailableRemarks;
                        obj.SameInstructorRemarks = item.IsAllocatedTrainerRemarks;
                        obj.FMSignOffRemarks = item.SignOffFmRemarks;
                        obj.TspSignOffRemarks = item.SignOffTspRemarks;

                        var fmSignImg = Path.Combine(SignatureImageDirectory,item.ClassCode, item.FMSignatureImgPath);
                        if (File.Exists(fmSignImg))
                        {
                            obj.FMSignImage = fmSignImg;
                        }
                        var tspSignImg = Path.Combine(SignatureImageDirectory,item.ClassCode, item.TspSignatureImgPath);
                        if (File.Exists(tspSignImg))
                        {
                            obj.TSPSignImage = tspSignImg;
                        }
                        if (i == 0)
                        {
                            model.Visit1 = obj;
                        }
                        if (i == 1)
                        {
                            model.Visit2 = obj;
                        }
                        if (i == 2)
                        {
                            model.Visit3 = obj;
                        }
                        if (i == 3)
                        {
                            model.Visit4 = obj;
                        }
                    }
                    //var visit1 = list.FirstOrDefault(x => x.VisitNo == 1);
                    //if (visit1 != null)
                    //{
                    //    var v1 = new AMSVisitModel();
                    //    v1.IsLock = visit1.IsLock == "y" ? "Yes" : "No";
                    //    v1.IsRelocated = visit1.IsRelocated == "y" ? "Yes" : "No";
                    //    v1.EquipmentAvailable = visit1.IsEquipmentAvailable == "y" ? "Yes" : "No";
                    //    v1.SameInstructor = visit1.InstructorChanged == "y" ? "Yes" : "No";
                    //    v1.IsLockRemarks = visit1.IsLockRemarks;
                    //    v1.IsRelocatedRemarks = visit1.IsRelocatedRemarks;
                    //    v1.EquipmentAvailableRemarks = visit1.IsEquipmentAvailableRemarks;
                    //    v1.SameInstructorRemarks = visit1.IsAllocatedTrainerRemarks;
                    //    model.Visit1 = v1;
                    //}
                    //var visit2 = list.FirstOrDefault(x => x.VisitNo == 2);
                    //if (visit2 != null)
                    //{
                    //    var v2 = new AMSVisitModel();
                    //    v2.IsLock = visit2.IsLock == "y" ? "Yes" : "No";
                    //    v2.IsRelocated = visit2.IsRelocated == "y" ? "Yes" : "No";
                    //    v2.EquipmentAvailable = visit2.IsEquipmentAvailable == "y" ? "Yes" : "No";
                    //    v2.SameInstructor = visit2.InstructorChanged == "y" ? "Yes" : "No";
                    //    v2.IsLockRemarks = visit2.IsLockRemarks;
                    //    v2.IsRelocatedRemarks = visit2.IsRelocatedRemarks;
                    //    v2.EquipmentAvailableRemarks = visit2.IsEquipmentAvailableRemarks;
                    //    v2.SameInstructorRemarks = visit2.IsAllocatedTrainerRemarks;
                    //    model.Visit2 = v2;
                    //}
                    //var visit3 = list.FirstOrDefault(x => x.VisitNo == 3);
                    //if (visit3 != null)
                    //{
                    //    var v3 = new AMSVisitModel();
                    //    v3.IsLock = visit3.IsLock == "y" ? "Yes" : "No";
                    //    v3.IsRelocated = visit3.IsRelocated == "y" ? "Yes" : "No";
                    //    v3.EquipmentAvailable = visit3.IsEquipmentAvailable == "y" ? "Yes" : "No";
                    //    v3.SameInstructor = visit3.InstructorChanged == "y" ? "Yes" : "No";
                    //    v3.IsLockRemarks = visit3.IsLockRemarks;
                    //    v3.IsRelocatedRemarks = visit3.IsRelocatedRemarks;
                    //    v3.EquipmentAvailableRemarks = visit3.IsEquipmentAvailableRemarks;
                    //    v3.SameInstructorRemarks = visit3.IsAllocatedTrainerRemarks;
                    //    model.Visit3 = v3;
                    //}
                    //var visit4 = list.FirstOrDefault(x => x.VisitNo == 4);
                    //if (visit4 != null)
                    //{
                    //    var v4 = new AMSVisitModel();
                    //    v4.IsLock = visit4.IsLock == "y" ? "Yes" : "No";
                    //    v4.IsRelocated = visit4.IsRelocated == "y" ? "Yes" : "No";
                    //    v4.EquipmentAvailable = visit4.IsEquipmentAvailable == "y" ? "Yes" : "No";
                    //    v4.SameInstructor = visit4.InstructorChanged == "y" ? "Yes" : "No";
                    //    v4.IsLockRemarks = visit4.IsLockRemarks;
                    //    v4.IsRelocatedRemarks = visit4.IsRelocatedRemarks;
                    //    v4.EquipmentAvailableRemarks = visit4.IsEquipmentAvailableRemarks;
                    //    v4.SameInstructorRemarks = visit4.IsAllocatedTrainerRemarks;
                    //    model.Visit4 = v4;
                    //}


                }
                var fakeGhostList = await _dapper.QueryAsync<AMSFakeGhostTraineesModel>("dbo.RD_AMSReportFakeGhostTrainees", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                if (fakeGhostList.Any())
                {
                    model.FakeGhostTrainees = fakeGhostList.ToList();
                }
                else
                {
                    model.FakeGhostTrainees = new List<AMSFakeGhostTraineesModel>();
                }
                var marginalTrainees = await _dapper.QueryAsync<AMSMarginalTraineesModel>("dbo.RD_AMSReportMarginalTrainees", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                if (marginalTrainees.Any())
                {
                    model.MarginalTrainees = marginalTrainees.ToList();
                }
                else
                {
                    model.MarginalTrainees = new List<AMSMarginalTraineesModel>();
                }
                var violations = await _dapper.QueryAsync<AMSClassViolationModel>("dbo.RD_AMSClassViolationsReport", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                if (violations.Any())
                {
                    var vitem = violations.First();
                    var vio = new AMSClassViolationModel();
                    vio.major_count = vitem.major_count;
                    vio.minor_count = vitem.minor_count;
                    vio.observation_count = vitem.observation_count;
                    vio.serious_count = vitem.serious_count;
                    model.ClassViolations = vio;
                }
                else
                {
                    model.ClassViolations = new AMSClassViolationModel()
                    {
                        major_count = 0,
                        minor_count = 0,
                        observation_count = 0,
                        serious_count = 0
                    };
                }
                var traineesCountList = await _dapper.QueryAsync<AMSCMTraineesCountModel>("dbo.RD_AMSCMTraineesCount", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                if (traineesCountList.Any())
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        var item = traineesCountList.ToList()[i];
                        var obj = new AMSCMTraineesCountModel();
                        obj.TraineesPresentCount = item.TraineesPresentCount;
                        obj.TraineesCountOne = item.TraineesCountOne;
                        obj.TraineesCountTwo = item.TraineesCountTwo;
                        obj.TraineesCountThree = item.TraineesCountThree;
                        obj.VDate = (item.VisitDateTime) != null ? item.VisitDateTime.Value.ToString("dd MMMM yyyy") : "N/A";
                        obj.VTime = (item.VisitDateTime) != null ? item.VisitDateTime.Value.ToString("hh:mm tt") : "N/A";
                        if (i == 0)
                        {
                            model.CMVisit1 = obj;
                        }
                        if (i == 1)
                        {
                            model.CMVisit2 = obj;
                        }
                        if (i == 2)
                        {
                            model.CMVisit3 = obj;
                        }
                        if (i == 3)
                        {
                            model.CMVisit4 = obj;
                        }
                    }
                    var visit1 = traineesCountList.FirstOrDefault(x => x.VisitNo == 1);
                    if (visit1 != null)
                    {
                        var v1 = new AMSCMTraineesCountModel();
                        v1.TraineesPresentCount = visit1.TraineesPresentCount;
                        v1.TraineesCountOne = visit1.TraineesCountOne;
                        v1.TraineesCountTwo = visit1.TraineesCountTwo;
                        v1.TraineesCountThree = visit1.TraineesCountThree;
                        v1.VDate = (visit1.VisitDateTime) != null ? visit1.VisitDateTime.Value.ToString("dd MMMM yyyy") : "N/A";
                        v1.VTime = (visit1.VisitDateTime) != null ? visit1.VisitDateTime.Value.ToString("hh:mm tt") : "N/A";
                        model.CMVisit1 = v1;
                    }
                    var visit2 = traineesCountList.FirstOrDefault(x => x.VisitNo == 2);
                    if (visit2 != null)
                    {
                        var v2 = new AMSCMTraineesCountModel();
                        v2.TraineesPresentCount = visit2.TraineesPresentCount;
                        v2.TraineesCountOne = visit2.TraineesCountOne;
                        v2.TraineesCountTwo = visit2.TraineesCountTwo;
                        v2.TraineesCountThree = visit2.TraineesCountThree;
                        v2.VDate = (visit2.VisitDateTime) != null ? visit2.VisitDateTime.Value.ToString("dd MMMM yyyy") : "N/A";
                        v2.VTime = (visit2.VisitDateTime) != null ? visit2.VisitDateTime.Value.ToString("hh:mm tt") : "N/A";
                        model.CMVisit2 = v2;
                    }
                    var visit3 = traineesCountList.FirstOrDefault(x => x.VisitNo == 3);
                    if (visit3 != null)
                    {
                        var v3 = new AMSCMTraineesCountModel();
                        v3.TraineesPresentCount = visit3.TraineesPresentCount;
                        v3.TraineesCountOne = visit3.TraineesCountOne;
                        v3.TraineesCountTwo = visit3.TraineesCountTwo;
                        v3.TraineesCountThree = visit3.TraineesCountThree;
                        v3.VDate = (visit3.VisitDateTime) != null ? visit3.VisitDateTime.Value.ToString("dd MMMM yyyy") : "N/A";
                        v3.VTime = (visit3.VisitDateTime) != null ? visit3.VisitDateTime.Value.ToString("hh:mm tt") : "N/A";
                        model.CMVisit3 = v3;
                    }
                    var visit4 = traineesCountList.FirstOrDefault(x => x.VisitNo == 4);
                    if (visit4 != null)
                    {
                        var v4 = new AMSCMTraineesCountModel();
                        v4.TraineesPresentCount = visit4.TraineesPresentCount;
                        v4.TraineesCountOne = visit4.TraineesCountOne;
                        v4.TraineesCountTwo = visit4.TraineesCountTwo;
                        v4.TraineesCountThree = visit4.TraineesCountThree;
                        v4.VDate = (visit4.VisitDateTime) != null ? visit4.VisitDateTime.Value.ToString("dd MMMM yyyy") : "N/A";
                        v4.VTime = (visit4.VisitDateTime) != null ? visit4.VisitDateTime.Value.ToString("hh:mm tt") : "N/A";
                        model.CMVisit4 = v4;
                    }
                }

                var traineesPreceptionList = await _dapper.QueryAsync<AMSTraineesPreceptionModel>("dbo.RD_AMSTraineesPreceptionReport", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                if (traineesPreceptionList.Any())
                {
                    model.TraineesPreceptions = traineesPreceptionList.ToList();
                }
                var violationTypeList = await _dapper.QueryAsync<AMSViolationTypeModel>("dbo.RD_AMSViolationTypeReport", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                if (violationTypeList.Any())
                {
                    model.ViolationTypeList = violationTypeList.ToList();
                }
                var delDoTrainees = await _dapper.QueryAsync<AMSFakeGhostTraineesModel>("dbo.RD_AMSDeleteDropOutTrainees", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                if (fakeGhostList.Any())
                {
                    model.DeleteDropOutTrainees = delDoTrainees.ToList();
                }
                else
                {
                    model.DeleteDropOutTrainees = new List<AMSFakeGhostTraineesModel>();
                }
                return model;
            }
            catch (Exception ex)
            {
                return new AMSFormThreeViewModel();
            }
        }

        public async Task<UnverifiedTraineeModel> GetUnverifiedTraineeReport(int? schemeId, int? tspId, int? classId, string dateTime)
        {
            try
            {
                DateTime? date = null;
                if (!string.IsNullOrEmpty(dateTime))
                {
                    date = DateTime.ParseExact(dateTime, "dd/MM/yyyy", null);
                }
                schemeId = schemeId > 0 ? schemeId : null;
                tspId = tspId > 0 ? tspId : null;
                classId = classId > 0 ? classId : null;
                int? month = null;
                int? year = null;
                if (date != null)
                {
                    month = date.Value.Month;
                    year = date.Value.Year;
                }
                var ClassDetail = await _dapper.QueryAsync<Table1>("dbo.RTP_UnverifiedTraineeClassDetail", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @month = month, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                var UserDetails = await _dapper.QueryAsync<Table2>("dbo.RTP_UnverifiedTraineeUersDetails", new { @schemeId = schemeId, @tspId = tspId, @classId = classId, @year = year }, CommandType.StoredProcedure).ConfigureAwait(true);
                UnverifiedTraineeModel model = new UnverifiedTraineeModel();
                model.ClassDetail = new Table1();
                model.UserDetails = new List<Table2>();
                if (ClassDetail.Any())
                {
                    var Table1 = new Table1
                    {
                        ServiceProviderName = ClassDetail.ToList()[0].ServiceProviderName,
                        TrainingScheme = ClassDetail.ToList()[0].TrainingScheme,
                        TrainingCentreAddress = ClassDetail.ToList()[0].TrainingCentreAddress,
                        DistrictofTrainingLocation = ClassDetail.ToList()[0].DistrictofTrainingLocation,
                        ClassCode = ClassDetail.ToList()[0].ClassCode,
                        Trade = ClassDetail.ToList()[0].Trade,
                        TotalNumberOfRegisteredTrainees = UserDetails.Count()
                    };
                    model.ClassDetail = Table1;
                }
                if(UserDetails.Any())
                {
                    model.UserDetails = UserDetails.ToList();
                    int presentInClassCount = 0;
                    foreach (var item in model.UserDetails)
                    {
                        if (item.CNICVerified == "Yes")
                            presentInClassCount += 1;
                    }
                    model.ClassDetail.TotalTraineesPresentInClass = presentInClassCount;
                }
                return model;
            }
            catch (Exception ex)
            {
                return new UnverifiedTraineeModel();
            }
        }
    }
}
