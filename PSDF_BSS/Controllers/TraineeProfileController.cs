using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSSRegistration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TraineeProfileController : ControllerBase
    {
        private readonly ISRVTraineeProfile srvTraineeProfile;
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVTraineeStatus srvTraineeStatus;
        private readonly ISRVClass srvClass;
        private readonly ISRVTraineeAttendance srvTraineeAttendance;
        private readonly ISRVGender srvGender;
        private readonly ISRVTrade srvTrade;
        private readonly ISRVEducationTypes srvEducationTypes;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTehsil srvTehsil;
        private readonly ISRVTraineeDisability srvTraineeDisability;
        private readonly ISRVSections srvSections;
        private readonly ISRVTSPDetail srvTSPDetail;
        private readonly ISRVReligion srvReligion;
        private readonly ISRVEmploymentStatus srvEmploymentStatus;
        private readonly ISRVIncomeRange srvIncomeRange;
        private readonly ISRVInceptionReport srvInceptionReport;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVReferralSource srvReferralSource;
        private readonly ISRVProvinces sRVProvinces;

        public TraineeProfileController(ISRVTraineeProfile srvTraineeProfile
            , ISRVOrgConfig srvOrgConfig
            , ISRVTraineeStatus srvTraineeStatus
            , ISRVClass srvClass
            , ISRVTraineeAttendance srvTraineeAttendance
            , ISRVGender srvGender
            , ISRVTrade srvTrade
            , ISRVEducationTypes srvEducationTypes
            , ISRVDistrict srvDistrict
            , ISRVTehsil srvTehsil
            , ISRVTraineeDisability srvTraineeDisability
            , ISRVSections srvSections
            , ISRVTSPDetail srvTSPDetail
            , ISRVReligion srvReligion
            , ISRVEmploymentStatus srvEmploymentStatus
            , ISRVIncomeRange srvIncomeRange
            , ISRVInceptionReport srvInceptionReport
            , ISRVScheme srvScheme
            , ISRVReferralSource srvReferralSource
            , ISRVProvinces sRVProvinces
            )
        {
            this.srvTraineeProfile = srvTraineeProfile;
            this.srvOrgConfig = srvOrgConfig;
            this.srvTraineeStatus = srvTraineeStatus;
            this.srvClass = srvClass;
            this.srvTraineeAttendance = srvTraineeAttendance;
            this.srvGender = srvGender;
            this.srvTrade = srvTrade;
            this.srvEducationTypes = srvEducationTypes;
            this.srvDistrict = srvDistrict;
            this.srvTehsil = srvTehsil;
            this.srvTraineeDisability = srvTraineeDisability;
            this.srvSections = srvSections;
            this.srvTSPDetail = srvTSPDetail;
            this.srvReligion = srvReligion;
            this.srvEmploymentStatus = srvEmploymentStatus;
            this.srvIncomeRange = srvIncomeRange;
            this.srvInceptionReport = srvInceptionReport;
            this.srvScheme = srvScheme;
            this.srvReferralSource = srvReferralSource;
            this.sRVProvinces = sRVProvinces;
        }

        // GET: TraineeProfile
        [HttpGet]
        [Route("GetTraineeProfile")]
        public IActionResult GetTraineeProfile()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvTraineeProfile.FetchTraineeProfile());

                ls.Add(srvGender.FetchGender(false));

                ls.Add(srvTrade.FetchTrade(false));

                //ls.Add(new SRVSections().FetchSections(false));

                ls.Add(srvScheme.FetchScheme(false));

                //ls.Add(new SRVTSPDetail().FetchTSPDetail(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeProfile
        [HttpGet]
        [Route("RD_TraineeProfile")]
        public IActionResult RD_TraineeProfile()
        {
            try
            {
                return Ok(srvTraineeProfile.FetchTraineeProfile(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetFilteredTrainees/{filters}")]
        public IActionResult GetFilteredTrainees([FromQuery(Name = "filters")] int[] filters)
        {
            try
            {
                return Ok(srvTraineeProfile.FetchTraineesByFilters(filters));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTraineesByUser/{id}")]
        public IActionResult GetTraineesByUser(int id)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvTraineeProfile.FetchTraineesDataByUser(id));
                ls.Add(srvDistrict.FetchDistrict(false));
                ls.Add(srvTehsil.FetchTehsil(false));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        // RD: TraineeProfile
        [HttpPost]
        [Route("RD_TraineeProfileBy")]
        public IActionResult RD_TraineeProfileBy(TraineeProfileModel mod)
        {
            try
            {
                return Ok(srvTraineeProfile.FetchTraineeProfile(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TraineeProfile/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TraineeProfileModel model)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.TraineeID);
            if (IsAuthrized == true)
            {
                //string errMsg = string.Empty;
                try
                {
                    //srvTraineeProfile.CalculateAgeEligibility(model.DateOfBirth.Value, model.ClassID, out errMsg);
                    //if (!string.IsNullOrEmpty(errMsg))
                    //{
                    //    return BadRequest(errMsg);

                    //}
                    //if (!srvTraineeProfile.isEligibleTrainee(model, out errMsg))
                    //{
                    //    return BadRequest(errMsg);
                    //}

                    //manual Process entry is directly from FORM so in that case process is manual
                    //in case of DVV data in the traineeprofile table is inserted via other method and IsManual will be false from that method entry
                    if (model.TraineeID == 0)
                    {
                        model.IsManual = true;
                        model.TraineeVerified = false; // In case of DVV trainee is verified but in case of manual process trainee by default is not verified
                        model.CNICVerified = false; // All manual entered trainees verification will be done by DEO so this status in case of manual is not verified.
                        model.DistrictVerified = false; // All manual entered trainees verification will be done by DEO so this status in case of manual is not verified.
                        model.ResultStatusID = 3; // initial ResultStatus is "None = 3" in DB
                    }

                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    var list = srvTraineeProfile.SaveTraineeProfile(model);

                    return Ok(list);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TraineeProfileModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    //
                    int userID = Convert.ToInt32(User.Identity.Name);
                    srvTraineeProfile.ActiveInActive(d.TraineeID, d.InActive, userID);
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

        [HttpGet]
        [Route("GetData")]
        public IActionResult GetData([FromQuery] int OID)
        {
            try
            {
                Dictionary<string, object> ls = new Dictionary<string, object>();
                Parallel.Invoke(
                              () => ls.Add("Genders", srvGender.FetchGender(false)),
                          () => ls.Add("Trade", srvTrade.FetchTrade(false)),
                          () => ls.Add("EducationTypes", srvEducationTypes.FetchEducationTypes(false)),
                          () => ls.Add("District", srvDistrict.FetchDistrict(false)),
                          () => ls.Add("Tehsil", srvTehsil.FetchTehsil(false)),
                          () => ls.Add("TempDistrict", srvDistrict.FetchAllPakistanDistrict(false)),//Get Temporary District
                          () => ls.Add("TempTehsil", srvTehsil.FetchAllPakistanTehsil(false)), //Get Temporary Tehsil

                          () => ls.Add("TraineeDisability", srvTraineeDisability.FetchTraineeDisability(false)),
                          () => ls.Add("IncomeRange", srvIncomeRange.FetchIncomeRanges(new IncomeRangeModel() { InActive = false })),
                          () => ls.Add("ReferralSource", srvReferralSource.FetchReferralSources(new ReferralSourceModel() { InActive = false })),
                          () => ls.Add("Province", sRVProvinces.FetchProvince(false)),
                          () => ls.Add("TemporaryProvince", sRVProvinces.FetchProvince(false)),
                          () => ls.Add("Sections", srvSections.FetchSections(false)),
                          () => ls.Add("TSPDetail", srvTSPDetail.FetchTSPDetail(false)),
                          () => ls.Add("Class", srvClass.FetchClass(new ClassModel() { OrganizationID = OID, InActive = false, ClassStatusID = (int)EnumClassStatus.Active })),    //this methos
                                                                                                                                                                                   //() => ls.Add("Class", srvClass.FetchApprovadClassesByModel(new ClassModel() { OrganizationID = OID, InActive = false, ClassStatusID = (int)EnumClassStatus.Active })),
                          () => ls.Add("Religion", srvReligion.FetchReligion(false)),
                          () => ls.Add("EmploymentStatus", srvEmploymentStatus.FetchEmploymentStatus(false))
                                );

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetDataByClass/{classId}")]
        public IActionResult GetDataByClass(int classId)
        {
            try
            {
                //List<object> list = new List<object>();
                IDictionary<string, Object> list = new Dictionary<string, Object>();
                if (classId > 0)
                {
                    var criteria = srvClass.CheckRegistrationCriteria(classId);
                    list.Add("CheckRegistrationCriteria", criteria);
                    list.Add("ListTraineeProfile", srvTraineeProfile.FetchTraineeProfileByClass(classId));
                    var classModel = srvClass.GetByClassID(classId);
                    list.Add("ClassModel", classModel);
                    list.Add("OrgConfigModel", srvOrgConfig.FetchOrgConfig(new OrgConfigModel() { ClassID = classId }));
                    list.Add("InceptionReportModel", srvInceptionReport.FetchInceptionReport(new InceptionReportModel() { ClassID = classId }));
                    list.Add("NextTraineeCode", srvTraineeProfile.FetchTraineeProfileNextCodeVal(classId));
                    list.Add("Schemes", srvScheme.GetBySchemeID(classModel.SchemeID));
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTraineeDraftDataByTsp")]
        public IActionResult GetTraineeDraftDataByTsp(int? TspId)
        {
            try
            {
                //List<object> list = new List<object>();
                IDictionary<string, Object> list = new Dictionary<string, Object>();
                if (TspId > 0)
                {
                    list.Add("ListTraineeProfile", srvTraineeProfile.FetchTraineeDraftDataByTsp(TspId));
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTraineeDraftDataByKam")]
        public IActionResult GetTraineeDraftDataByKamId(int? KamId)
        {
            try
            {
                //List<object> list = new List<object>();
                IDictionary<string, Object> list = new Dictionary<string, Object>();
                if (KamId > 0)
                {
                    list.Add("ListTraineeProfile", srvTraineeProfile.FetchTraineeDraftDataByKam(KamId));
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("CheckRegistrationCriteria")]
        public IActionResult CheckRegistrationCriteria([FromQuery] int classId)
        {
            try
            {
                var criteria = srvClass.CheckRegistrationCriteria(classId);

                return Ok(criteria);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("CalculateAgeEligibility")]
        public IActionResult CalculateAgeEligibility(TraineeProfileModel model)
        {
            int age = 0;
            string errMsg = string.Empty;
            try
            {
                age = srvTraineeProfile.CalculateAgeEligibility(model.DateOfBirth.Value, model.ClassID, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return new JsonResult(new { age, errMsg });
        }

        [HttpGet]
        [Route("isEligibleTrainee")]
        public IActionResult isEligibleTrainee([FromQuery] int traineeId, [FromQuery] string cnic, [FromQuery] int classId)
        {
            bool isValid = false;
            string errMsg = string.Empty;
            try
            {
                //it will return true if CNIC already exist
                isValid = srvTraineeProfile.isEligibleTrainee(new TraineeProfileModel()
                {
                    TraineeID = traineeId,
                    TraineeCNIC = cnic,
                    ClassID = classId
                }, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return new JsonResult(new { isValid, errMsg });
        }

        [HttpGet]
        [Route("isEligibleTraineeEmail")]
        public IActionResult isEligibleTraineeEmail([FromQuery] int traineeId, [FromQuery] string email, [FromQuery] int classId)
        {
            bool isValid = false;
            string errMsg = string.Empty;
            try
            {
                //it will return true if CNIC already exist
                isValid = srvTraineeProfile.isEligibleTraineeEmail(new TraineeProfileModel()
                {
                    TraineeID = traineeId,
                    TraineeEmail = email,
                    ClassID = classId
                }, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return new JsonResult(new { isValid, errMsg });
        }

        /// <summary>
        /// this region was developed but not used
        /// </summary>
        /// <returns></returns>

        #region Trainee Attendance

        [HttpGet]
        [Route("AddTraineeAttendance")]
        public IActionResult AddTraineeAttendance()
        {
            // get visits
            AMS_Visit visit = new AMS_Visit() { VisitorId = 1, VisitDate = new DateTime(2020, 01, 01), AbsentTrainees = new int[2] { 547065, 547066 } };
            try
            {
                // save into Attendance Table
                foreach (var traineeId in visit.AbsentTrainees)
                {
                    srvTraineeAttendance.SaveTraineeAttendance(new TraineeAttendanceModel()
                    {
                        IsManual = true,
                        TraineeProfileID = traineeId,
                        AttendanceDate = visit.VisitDate,
                        IsAbsent = true,
                        CurUserID = Convert.ToInt32(User.Identity.Name),
                        CreatedUserID = Convert.ToInt32(User.Identity.Name)
                    });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("SetTraineeStatus")]
        public IActionResult SetTraineeStatusByMonthlyAttendance()
        {
            /// assign date is end of month when we calculate stipend
            var date = new DateTime(2020, 01, 01);
            try
            {
                //get Manual Trainee's attendance list by month
                var monthlyAttendance = srvTraineeAttendance.FetchTraineeAttendanceByMonth(
                    new TraineeAttendanceModel() { IsManual = true, AttendanceDate = date }
                    ).GroupBy(x => x.TraineeProfileID);

                foreach (var group in monthlyAttendance)
                {
                    int traineeId = group.Key;
                    var previousMonth = date.AddMonths(-1);
                    TraineeStatusModel previousMonthStatus = srvTraineeStatus.GetTraineeStatusByMonth(new TraineeStatusModel() { TraineeProfileID = traineeId, CreatedDate = previousMonth });
                    int statusTypeId = 0;
                    string comments = string.Empty;
                    ///Add New Status
                    if (previousMonthStatus.TraineeStatusTypeID == (int)EnumTraineeStatusType.Marginal)
                    {
                        if (!group.Any(x => x.IsAbsent = false))
                        {
                            statusTypeId = (int)EnumTraineeStatusType.DropOut;
                            comments = "Due to previous's month status is Marginal.";
                        }
                        else
                        {
                            statusTypeId = (int)EnumTraineeStatusType.OnRoll;
                            comments = "Trainee is reappear in class after one month.";
                        }
                    }
                    else
                    {
                        if (!group.Any(x => x.IsAbsent = false))
                        {
                            statusTypeId = (int)EnumTraineeStatusType.Marginal;
                            comments = "Trainee is not appear in the class since one month.";
                        }
                    }
                    if (statusTypeId > 0)
                    {
                        srvTraineeStatus.SaveTraineeStatus(new TraineeStatusModel()
                        {
                            TraineeProfileID = traineeId,
                            TraineeStatusTypeID = statusTypeId,
                            CreatedUserID = Convert.ToInt32(User.Identity.Name),
                            Comments = comments
                        });
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion Trainee Attendance

        [HttpGet]
        [Route("GetTraineeStatusHistory/{traineeID}")]
        public IActionResult GetTraineeStatusHistory(int traineeID)
        {
            try
            {
                if (traineeID == 0)
                {
                    return BadRequest();
                }
                return Ok(srvTraineeStatus.FetchTraineeStatusByTraineeID(traineeID));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTraineeHistory/{traineeID}")]
        public IActionResult GetTraineeHistory(int traineeID)
        {
            try
            {
                if (traineeID == 0)
                {
                    return BadRequest();
                }
                return Ok(srvTraineeProfile.FetchTraineeHistoryByTraineeID(traineeID));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("FetchTraineesByUser")]
        public IActionResult FetchTraineesByUser(QueryFilters filters)
        {
            try
            {
                return Ok(srvTraineeProfile.FetchTraineesByUser(filters));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("FetchTraineesForCRFilterByUser")]
        public IActionResult FetchTraineesForCRFilterByUser(QueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();
                list.Add(srvTraineeProfile.FetchCRTraineesByUser(filters));
                list.Add(srvDistrict.FetchDistrict(false));
                list.Add(srvTehsil.FetchTehsil(false));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("BatchUpdateExtraTrainees")]
        public IActionResult BatchUpdateExtraTrainees([FromBody] string json)
        {
            try
            {
                //
                //List<TraineeProfileModel> list=JsonConvert.DeserializeObject<List<TraineeProfileModel>>(json);
                int userID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvTraineeProfile.BatchUpdateExtraTrainees(json, userID));
                //return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetDataForTraineeUpdation")]
        public IActionResult GetDataForTraineeUpdation()
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                list.Add("Schemes", srvScheme.FetchSchemesByTSPUser(0));
                list.Add("Data", srvTraineeProfile.FetchTraineesByFilters(new int[] { 0, 0, 0 }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("UpdateTraineeStatus")]
        public IActionResult UpdateTraineeStatus([FromQuery] int TraineeID, [FromQuery] int TraineeStatusTypeID, [FromQuery] string TraineeStatusReason)
        {
            try
            {
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvTraineeProfile.UpdateTraineeStatus(TraineeID, TraineeStatusTypeID, CurUserID, TraineeStatusReason));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateTraineeResult")]
        public IActionResult UpdateTraineeResult([FromBody] string str)
        {
            try
            {
                TraineeProfileModel[] model = JsonConvert.DeserializeObject<TraineeProfileModel[]>(str);
                List<TraineeProfileModel> ls = new List<TraineeProfileModel>();
                foreach (var item in model)
                {
                    item.CurUserID = Convert.ToInt32(User.Identity.Name);
                    ls.Add(srvTraineeProfile.UpdateTraineeResultStatus(item));
                }
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdateTraineeCNICImg")]
        public IActionResult UpdateTraineeCNICImg([FromBody] string str)
        {
            try
            {
                TraineeProfileModel[] model = JsonConvert.DeserializeObject<TraineeProfileModel[]>(str);
                //List<TraineeProfileModel> ls = new List<TraineeProfileModel>();
                foreach (var item in model)
                {
                    item.CurUserID = Convert.ToInt32(User.Identity.Name);
                    srvTraineeProfile.UpdateTraineeCNICImg(item);
                }
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("CheckTraineeCNIC")]
        public IActionResult CheckTraineeCNIC(TraineeProfileModel trainee)
        {
            if (!String.IsNullOrEmpty(trainee.TraineeCNIC))
            {
                List<TraineeProfileModel> u = srvTraineeProfile.GetByTraineeCNIC(trainee.TraineeCNIC);
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
                        if (u[0].TraineeID == trainee.TraineeID)
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

        [HttpPost]
        [Route("GetTraineesFromFile")]
        public IActionResult GetTraineesFromFile(List<CNICStatusModel> cnic)
        {
            try
            {
                return Ok(srvTraineeProfile.GetTraineesFromFile(cnic));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteDraftTrainee")]
        public IActionResult DeleteDraftTrainee([FromQuery] int TraineeID)
        {
            try
            {
                srvTraineeProfile.DeleteDraftTrainee(TraineeID);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("FetchTraineesByUserPaged")]
        public IActionResult FetchTraineesByUserPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();
                List<object> ls = new List<object>();
                ls.Add(srvTraineeProfile.FetchTraineesByUserPaged(pagingModel, filterModel, out int totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("FetchTraineesForCRFilterByUserPaged")]
        public IActionResult FetchCRTraineesByUserPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();
                List<object> ls = new List<object>();
                ls.Add(srvTraineeProfile.FetchCRTraineesByUserPaged(pagingModel, filterModel, out int totalCount));
                ls.Add(srvDistrict.FetchDistrict(false));
                ls.Add(srvTehsil.FetchTehsil(false));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Added by Rao Ali Haider
        /// 29-Mar-2024
        /// </summary>
        /// <param name="TspId"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetSubmittedTraineesByTsp/{TraineeFilter}/{programid}/{districtid}/{tradeid}/{traininglocationid}")]
        public IActionResult GetSubmittedTraineesByTsp(int TraineeFilter, int programid, int districtid, int tradeid, int traininglocationid)
        {
            try
            {
                //List<object> list = new List<object>();
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                IDictionary<string, Object> list = new Dictionary<string, Object>();
                return Ok(srvTraineeProfile.FetchSubmittedTraineeDataByTsp(TraineeFilter, programid, districtid, tradeid, CurUserID, traininglocationid));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTSPTradeCriteria/{programid}/{tradeid}")]
        public IActionResult GetTSPTradeCriteria(int programid, int tradeid)
        {
            try
            {
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                var criteria = srvTraineeProfile.checkTSPTradeCriteria(programid, tradeid, CurUserID);
                   
                   
                return Ok(criteria);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: TraineeProfile/SaveSubmitted
        [HttpPost]
        [Route("SaveSubmitted")]
        public IActionResult SaveSubmitted(TraineeProfileModel model)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.TraineeID);
            if (IsAuthrized == true)
            {
                //string errMsg = string.Empty;
                try
                {
                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    var list = srvTraineeProfile.SaveTraineeIntrestProfile(model);

                    return Ok(null);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
        // POST: TraineeProfile/SaveInterview
        [HttpPost]
        [Route("SaveInterview")]
        public IActionResult SaveInterview(TraineeProfileModel model)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.TraineeID);
            if (IsAuthrized == true)
            {
                //string errMsg = string.Empty;
                try
                {
                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    var list = srvTraineeProfile.SaveTraineeInterviewSubmission(model);

                    return Ok(null);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
    }
}