/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers.TSPEmployment
{
    public class TspExcelModel
    {
        public List<TSPEmploymentModel> data { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TSPEmploymentController : ControllerBase
    {
        private readonly ISRVTSPEmployment srv;
        private readonly ISRVTraineeProfile srvTraineeProfile;
        private readonly ISRVVerificationMethod srvVerificationMethod;
        private readonly IWebHostEnvironment _env;
        private readonly ISRVClass srvClass;
        private readonly ISRVEmploymentStatus srvEmploymentStatus;
        private readonly ISRVPlacementType srvPlacementType;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTehsil srvTehsil;
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVScheme srvScheme;

        public TSPEmploymentController(ISRVTSPEmployment srv, ISRVEmploymentStatus srvEmploymentStatus,
        ISRVPlacementType srvPlacementType, ISRVDistrict srvDistrict, ISRVTehsil srvTehsil,
                ISRVTraineeProfile traineeProfileSRV, ISRVUsers srvUsers, ISRVScheme srvScheme,
                ISRVVerificationMethod verificationMethod, ISRVClass srvClass, ISRVOrgConfig srvOrgConfig,
        IWebHostEnvironment env)
        {
            this.srv = srv;
            srvTraineeProfile = traineeProfileSRV;
            srvVerificationMethod = verificationMethod;
            _env = env;
            this.srvClass = srvClass;
            this.srvEmploymentStatus = srvEmploymentStatus;

            this.srvPlacementType = srvPlacementType;
            this.srvDistrict = srvDistrict;
            this.srvTehsil = srvTehsil;
            this.srvOrgConfig = srvOrgConfig;
            this.srvUsers = srvUsers;
            this.srvScheme = srvScheme;
        }

        [HttpGet]
        [Route("GetClass")]
        public IActionResult GetClass()
        {
            try
            {
                var userID = Convert.ToInt32(User.Identity.Name);
                return Ok(new
                {
                    ClassList = srv.GetClasses(userID)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetDeoDashboardStats")]
        public IActionResult GetDeoDashboardStats()
        {
            try
            {
                return Ok(srv.GetDeoDashboardStats());
                //{
                //    ClassList = srv.GetClasses(userID)
                //});
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetFilteredClass/{filter}")]
        public IActionResult GetFilteredClass([FromQuery(Name = "filter")] int[] filter)
        {
            try
            {
                int userID = filter?[3] ?? 0;
                int oID = filter?[4] ?? 0;
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                List<object> ls = new List<object>();

                ls.Add(srv.FetchClassFilters(filter));
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    ls.Add(srvScheme.FetchSchemesByTSPUser(curUserID));
                }
                else
                {
                    ls.Add(srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true, OrganizationID = oID }));
                }
                //return Ok(srv.FetchMasterSheetByFilters(filter));
                //ls.Add(srvScheme.FetchSchemeForFilter(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("GetTraineeOfClass/{ClassID}")]
        public IActionResult GetTraineeOfClass(string ClassID)
        {
            try
            {
                return Ok(new
                {
                    PlacementStatus = Common.FilterPlacementDropdown(srvEmploymentStatus.FetchEmploymentStatus(false)),
                    TraineeList = srv.GetCompletedTraineeByClass(Convert.ToInt32(ClassID)),
                    PlacementTypes = srvPlacementType.FetchPlacementType(false),
                    District = srvDistrict.FetchAllPakistanDistrict(false),
                    Tehsil = srvTehsil.FetchAllPakistanTehsil(false),
                    VerificationMethods = srvVerificationMethod.FetchAll(),
                    PlacementData = srv.FetchPlacementFormE(new TSPEmploymentModel { ClassID = Convert.ToInt32(ClassID) })
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpGet]
        [Route("GetCompletedTraineesOfClassForEmployment/{ClassID}")]
        public IActionResult GetCompletedTraineesOfClassForEmployment(string ClassID)
        {
            try
            {
                return Ok(new
                {
                    TraineeList = srv.GetCompletedTraineeByClass(Convert.ToInt32(ClassID)),
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpGet]
        [Route("GetCompletedANDEmployedTraineesOfClass/{ClassID}")]
        public IActionResult GetCompletedANDEmployedTraineesOfClass(string ClassID)
        {
            try
            {
                return Ok(new
                {
                    TraineeList = srv.GetCompletedTraineeByClass(Convert.ToInt32(ClassID)),
                    PlacementData = srv.FetchPlacementFormE(new TSPEmploymentModel { ClassID = Convert.ToInt32(ClassID) })
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }    
        [HttpGet]
        [Route("GetEmploymentReportedTraineesOfClass/{ClassID}")]
        public IActionResult GetEmploymentReportedTraineesOfClass(string ClassID)
        {
            try
            {
                return Ok(new
                {
                    TraineeList = srv.GetCompletedTraineeByClass(Convert.ToInt32(ClassID)),
                    PlacementData = srv.FetchReportedPlacementFormE(new TSPEmploymentModel { ClassID = Convert.ToInt32(ClassID) })
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TSPEmploymentModel D)
        {
            try
            {
                // TSPEmploymentModel D = JsonConvert.DeserializeObject<TSPEmploymentModel>(data);

                if (!String.IsNullOrEmpty(D.FilePath))
                {
                    var path = "\\Documents\\TSPEmployment\\";

                    var fileName = Common.AddFile(D.FilePath, path);

                    D.FileName = fileName;
                    D.FilePath = fileName;
                }

                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SavePlacementFormE(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("SubmitClassEmployment")]
        public async Task<IActionResult> SubmitClassEmployment(TSPEmploymentModel m)
        {
            try
            {
                m.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SubmitClassEmployment(m.ClassID ?? 0, m.CurUserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("SaveExcelFile")]
        public IActionResult SaveExcelFile(List<TSPEmploymentExcelModel> list)
        {
            try
            {
                // var list = System.Text.Json.JsonSerializer.Deserialize<List<TSPEmploymentExcelModel>>(json.ToString());

                TraineeProfileModel M = new TraineeProfileModel();
                M.ClassID = list[0].ClassID ?? 0;
                var trainees = srvTraineeProfile.FetchTraineeProfile(M);
                var placementstatuses = srvEmploymentStatus.FetchEmploymentStatus(false);
                var placementtypes = srvPlacementType.FetchPlacementType(false);
                var districts = srvDistrict.FetchDistrict(false);
                var tehsils = srvTehsil.FetchTehsil(false);
                var classes = srvClass.GetByClassID(M.ClassID);

                foreach (var item in list)
                {
                    TSPEmploymentModel model = new TSPEmploymentModel();
                    model.PlacementID = item.PlacementID;
                    model.TraineeID = item.TraineeID;
                    model.TraineeCode = item.TraineeCode;
                    model.ContactNumber = item.ContactNumber;
                    model.ClassID = item.ClassID;
                    model.EmploymentStatus = item.EmploymentStatus;
                    model.EmploymentType = item.EmploymentType;
                    model.District = item.District;
                    model.EmploymentTehsil = item.EmploymentTehsil;
                    model.EmploymentStartDate = item.EmploymentStartDate;
                    model.EmployerBusinessType = item.EmployerBusinessType;
                    model.Designation = item.Designation;
                    model.Department = item.Department;
                    model.EmployerName = item.EmployerName;
                    model.EmploymentAddress = item.EmploymentAddress;
                    model.EmploymentDuration = item.EmploymentDuration;
                    model.EmploymentTiming = item.EmploymentTiming;
                    model.OfficeContactNo = item.OfficeContactNo;
                    model.Salary = item.Salary;
                    model.SupervisorContact = item.SupervisorContact;
                    model.SupervisorName = item.SupervisorName;
                    model.TradeName = item.TradeName;
                    model.TraineeName = item.TraineeName;
                    model.VerificationMethodId = item.VerificationMethodId;
                    model.VerifyStatus = item.VerifyStatus;
                    model.PlacementTypeID = item.PlacementTypeID;
                    model.IsTSP = item.IsTSP;
                    model.EOBI = item.EOBI;
                    model.EmployerNTN = item.EmployerNTN;
                    if (!String.IsNullOrEmpty(item.FilePath))
                    {
                        var path = "\\Documents\\TSPEmployment\\";

                        var fileName = Common.AddFile(item.FilePath, path);

                        model.FileName = fileName;
                        model.FilePath = fileName;
                    }

                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    srv.SavePlacementFormE(model);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("TraineeForVerificationExportExcel")]
        public IActionResult TraineeForVerificationExportExcel(int pId, int vmId, int tspId, int cId)
        {
            try
            {
                var data = srv.FetchPlacementsForDEOToExport(new TSPEmploymentExcelModel
                {
                    PlacementTypeID = pId,
                    VerificationMethodId = vmId,
                    TSPID = tspId,
                    ClassID = cId
                });
                return Ok(new
                {
                    SelfList = data,
                    TypeID = pId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPost]
        [Route("ReportedEmploymentExportExcel")]
        public IActionResult ReportedEmploymentExportExcel(QueryFilters filters)
        {
            try
            {
                var data = srv.FetchReportedPlacementToExport(filters);
                //{
                //    filters
                //});
                return Ok(data);
                //{
                //    SelfList = data,
                //    TypeID = pId
                //});
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPost]
        [Route("VerifiedEmploymentExportExcel")]
        public IActionResult VerifiedEmploymentExportExcel(QueryFilters filters)
        {
            try
            {
                var data = srv.FetchVerifiedPlacementToExport(filters);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPost]
        [Route("GetData")]
        public IActionResult GetData(TSPEmploymentModel model)
        {
            try
            {
                var Classid = Convert.ToInt32(model.ClassID);
                Dictionary<string, object> ls = new Dictionary<string, object>();
                if (model.ClassID != null && model.ClassID != 0)
                {
                    ClassModel Class = srvClass.GetByClassID(model.ClassID ?? -1);
                    OrgConfigModel orgConfig = srvOrgConfig.FetchOrgConfig(new OrgConfigModel() { ClassID = Convert.ToInt32(model.ClassID) })?[0];
                    string DeadlineStatus;

                    var classStatusDate = Convert.ToDateTime(Class.EndDate);
                    var currentDate = DateTime.Now;
                    var time = currentDate.Subtract(classStatusDate);
                    var days = Math.Floor(time.TotalDays);
                    if (orgConfig != null && days > orgConfig.EmploymentDeadline)
                    {
                        DeadlineStatus = "Date Passed";
                    }
                    else
                        DeadlineStatus = "Date Not Passed";

                    if (model.TraineeID != 0 && model.TraineeID != null)
                    {
                        Parallel.Invoke(() => ls.Add("Class", srvClass.GetByClassID(Classid)),
                                        //() => ls.Add("TraineeProfile", new SRVTraineeProfile().FetchTraineeProfile()),
                                        () => ls.Add("PlacementStatus", Common.FilterPlacementDropdown(srvEmploymentStatus.FetchEmploymentStatus(false))),
                                        () => ls.Add("PlacementTypes", srvPlacementType.FetchPlacementType(false)),
                                        () => ls.Add("District", srvDistrict.FetchAllPakistanDistrict(false)),
                                        () => ls.Add("Tehsil", srvTehsil.FetchAllPakistanTehsil(false)),
                                        () => ls.Add("EmploymentData", srv.FetchPlacementFormE(new TSPEmploymentModel { ClassID = model.ClassID })),
                                        () => ls.Add("OrgConfig", orgConfig != null ? orgConfig.EmploymentDeadline : 0),
                                        () => ls.Add("DeadlineStatus", DeadlineStatus),
                                        () => ls.Add("EmploymentSubmited", Class.EmploymentSubmited),
                                        () => ls.Add("TraineeProfile", srv.GetTraineeData((int)model.ClassID, (int)model.TraineeID)),
                                        () => ls.Add("VerificationMethods", srvVerificationMethod.FetchAll())
                                       );
                    }
                    else
                    {
                        if (model.ClassID != 0)
                            Parallel.Invoke(
                                () => ls.Add("Class", srvClass.GetByClassID(Classid)),
                                           //() => ls.Add("TraineeProfile", new SRVTraineeProfile().FetchTraineeProfile()),
                                           () => ls.Add("PlacementStatus", Common.FilterPlacementDropdown(srvEmploymentStatus.FetchEmploymentStatus(false))),
                                           () => ls.Add("PlacementTypes", srvPlacementType.FetchPlacementType(false)),
                                           () => ls.Add("District", srvDistrict.FetchAllPakistanDistrict(false)),
                                           () => ls.Add("Tehsil", srvTehsil.FetchAllPakistanTehsil(false)),
                                           () => ls.Add("EmploymentData", srv.FetchPlacementFormE(new TSPEmploymentModel { ClassID = model.ClassID })),
                                           () => ls.Add("OrgConfig", orgConfig != null ? orgConfig.EmploymentDeadline : 0),
                                           () => ls.Add("DeadlineStatus", DeadlineStatus),
                                           () => ls.Add("EmploymentSubmited", Class.EmploymentSubmited),

                                           () => ls.Add("VerificationMethods", srvVerificationMethod.FetchAll())
                                          );
                    }
                }

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("GetDataForEmployment")]
        public IActionResult GetDataForEmployment(TSPEmploymentModel model)
        {
            try
            {
                var Classid = Convert.ToInt32(model.ClassID);
                Dictionary<string, object> ls = new Dictionary<string, object>();
                if (model.ClassID != null && model.ClassID != 0)
                {
                    ClassModel Class = srvClass.GetByClassID(model.ClassID ?? -1);
                    OrgConfigModel orgConfig = srvOrgConfig.FetchOrgConfig(new OrgConfigModel() { ClassID = Convert.ToInt32(model.ClassID) })?[0];
                    string DeadlineStatus;

                    var classStatusDate = Convert.ToDateTime(Class.EndDate);
                    var currentDate = DateTime.Now;
                    var time = currentDate.Subtract(classStatusDate);
                    var days = time.TotalDays;
                    if (orgConfig != null && Math.Floor(days) > orgConfig.EmploymentDeadline)
                    {
                        DeadlineStatus = "Date Passed";
                    }
                    else
                        DeadlineStatus = "Date Not Passed";

                    if (model.TraineeID != 0 && model.TraineeID != null)
                    {
                        Parallel.Invoke(() => ls.Add("Class", srvClass.GetByClassID(Classid)),
                                        //() => ls.Add("TraineeProfile", new SRVTraineeProfile().FetchTraineeProfile()),
                                        () => ls.Add("PlacementStatus", Common.FilterPlacementDropdown(srvEmploymentStatus.FetchEmploymentStatus(false))),
                                        () => ls.Add("PlacementTypes", srvPlacementType.FetchPlacementType(false)),
                                        () => ls.Add("District", srvDistrict.FetchAllPakistanDistrict(false)),
                                        () => ls.Add("Tehsil", srvTehsil.FetchAllPakistanTehsil(false)),
                                        () => ls.Add("EmploymentData", srv.FetchEmployedTraineesForTSP(new TSPEmploymentModel { ClassID = model.ClassID })),
                                        () => ls.Add("OrgConfig", orgConfig != null ? orgConfig.EmploymentDeadline : 0),
                                        () => ls.Add("DeadlineStatus", DeadlineStatus),
                                        () => ls.Add("EmploymentSubmited", Class.EmploymentSubmited),
                                        () => ls.Add("TraineeProfile", srv.GetTraineeData((int)model.ClassID, (int)model.TraineeID)),
                                        () => ls.Add("VerificationMethods", srvVerificationMethod.FetchAll())
                                       );
                    }
                    else
                    {
                        if (model.ClassID != 0)
                            Parallel.Invoke(
                                () => ls.Add("Class", srvClass.GetByClassID(Classid)),
                                           //() => ls.Add("TraineeProfile", new SRVTraineeProfile().FetchTraineeProfile()),
                                           () => ls.Add("PlacementStatus", Common.FilterPlacementDropdown(srvEmploymentStatus.FetchEmploymentStatus(false))),
                                           () => ls.Add("PlacementTypes", srvPlacementType.FetchPlacementType(false)),
                                           () => ls.Add("District", srvDistrict.FetchAllPakistanDistrict(false)),
                                           () => ls.Add("Tehsil", srvTehsil.FetchAllPakistanTehsil(false)),
                                           () => ls.Add("EmploymentData", srv.FetchEmployedTraineesForTSP(new TSPEmploymentModel { ClassID = model.ClassID })),
                                           () => ls.Add("OrgConfig", orgConfig != null ? orgConfig.EmploymentDeadline : 0),
                                           () => ls.Add("DeadlineStatus", DeadlineStatus),
                                           () => ls.Add("EmploymentSubmited", Class.EmploymentSubmited),

                                           () => ls.Add("VerificationMethods", srvVerificationMethod.FetchAll())
                                          );
                    }
                }

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        
        [HttpPost]
        [Route("GetEmploymentDataByTraineeID")]
        public IActionResult GetDaGetEmploymentDataByTraineeIData(TSPEmploymentModel model)
        {
            try
            {

                List<object> ls = new List<object>();
                ls.Add(srv.FetchPlacementFormEByTraineeID(new TSPEmploymentModel { ClassID = model.ClassID, TraineeID = model.TraineeID }));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // POST: PlacementFormE/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TSPEmploymentModel d)
        {
            try
            {
                srv.ActiveInActive(d.PlacementID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("EmployeeVerificationDropdown")]
        public IActionResult EmployeeVerificationDropdown()
        {
            try
            {
                var placementTypes = srvPlacementType.FetchPlacementType(false);
                var verificationMethods = srvVerificationMethod.FetchAll();
                return Ok(new
                {
                    placementTypes,
                    verificationMethods
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSelfEmploymentList")]
        public IActionResult GetSelfEmploymentList(int pId, int vmId, int cId, int? TSPID)
        {
            try
            {
                var data = srv.FetchPlacementFormE(new TSPEmploymentModel
                {
                    PlacementTypeID = pId,
                    VerificationMethodId = vmId,
                    ClassID = cId,
                    TSPID = TSPID
                });
                return Ok(new
                {
                    SelfList = data,
                    TypeID = pId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }


        [HttpGet]
        [Route("GetDEOEmploymentClassesByTSP")]
        public IActionResult GetDEOEmploymentClassesByTSP(int pId, int vmId, int tspId)
        {
            try
            {
                var data = srv.FetchDEOEmploymentClassesByTSP(pId, vmId, tspId);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPost]
        [Route("ForwardedToTelephonic")]
        public IActionResult ForwardedToTelephonic(ForwardToTelephonicVerification Model)
        {
            try
            {
                var success = srv.ForwardedToTelephonic(Model);
                return Ok(success);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetTspClassese")]
        public IActionResult GetTspClassese()
        {
            try
            {
                return Ok(new
                {
                    ClassList = srv.GetClasses(0)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        [HttpGet]
        [Route("GetTspForEmploymentVerification")]
        public IActionResult GetTspForEmploymentVerification(int placementTypeId, int verificationMethodId)
        {
            try
            {
                return Ok(new
                {
                    TSPsList = srv.GetTPSDetailForEmploymentVerification(placementTypeId, verificationMethodId)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetEmploymentVerificationByCallCentreClasses")]
        public IActionResult GetEmploymentVerificationByCallCentreClasses()
        {
            try
            {
                return Ok(new
                {
                    ClassList = srv.GetTelephonicEmploymentVerificationClasses()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetEmploymentVerificationClasses")]
        public IActionResult GetEmploymentVerificationClasses(int pId, int vmId, int tspId, int cId)
        {
            try
            {
                return Ok(new
                {
                    ClassList = srv.GetClassesForEmploymentVerification(pId, vmId, tspId, cId)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        [HttpGet]
        [Route("GetTelephonicEmploymentVerificationClasses")]
        public IActionResult GetTelephonicEmploymentVerificationClasses(int pId, int vmId, int tspId, int cId)
        {
            try
            {
                return Ok(new
                {
                    ClassList = srv.GetTelephonicEmploymentVerificationClasses(pId, vmId, tspId, cId)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetTraineesForEmploymentVerification")]
        public IActionResult GetTraineesForEmploymentVerification(int pId, int vmId, int tspId, int cId)
        {
            try
            {
                return Ok(new
                {
                    PlacementData = srv.FetchPlacementFormEForVerification(new TSPEmploymentModel { PlacementTypeID = pId, VerificationMethodId = vmId, TSPID = tspId, ClassID = Convert.ToInt32(cId) })
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetTraineeForEmploymentVerification")]
        public IActionResult GetTraineeForEmploymentVerification(int traineeId)
        {
            try
            {
                return Ok(              
                    srv.FetchTraineeForEmploymentVerification(new TSPEmploymentModel { TraineeID = traineeId })
                );
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        [HttpGet]
        [Route("GetTelephonicTraineesForEmploymentVerification")]
        public IActionResult GetTelephonicTraineesForEmploymentVerification(int pId, int vmId, int tspId, int cId)
        {
            try
            {
                return Ok(new
                {
                    PlacementData = srv.FetchTelephonicPlacementFormE(new TSPEmploymentModel { PlacementTypeID = pId, VerificationMethodId = vmId, TSPID = tspId, ClassID = Convert.ToInt32(cId) })
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        [HttpGet]
        [Route("GetTelephonicEmploymentList")]
        public IActionResult GetTelephonicEmploymentList(int pId, int vmId, int cId, int? TSPID)
        {
            try
            {
                var data = srv.FetchTelephonicPlacementFormE(new TSPEmploymentModel
                {
                    PlacementTypeID = pId,
                    VerificationMethodId = vmId,
                    ClassID = cId,
                    TSPID = TSPID
                });
                return Ok(new
                {
                    SelfList = data,
                    TypeID = pId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }


        [HttpPost]
        [Route("GetEmploymentDataForVerification")]
        public IActionResult GetEmploymentDataForVerification(TSPEmploymentModel model)
        {
            try
            {
                Dictionary<string, object> ls = new Dictionary<string, object>();
                if (model.ClassID != null && model.ClassID != 0)
                {
                    if (model.TraineeID != 0 && model.TraineeID != null)
                    {
                        Parallel.Invoke(() => ls.Add("EmploymentData", srv.FetchPlacementFormE(new TSPEmploymentModel { ClassID = model.ClassID }))
                                       );
                    }
                    else
                    {
                        if (model.ClassID != 0)
                            Parallel.Invoke(() => ls.Add("EmploymentData", srv.FetchPlacementFormE(new TSPEmploymentModel { ClassID = model.ClassID }))
                                           );
                    }
                }
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }


        [HttpGet]
        [Route("GetFilteredClassOnJob/{filter}")]
        public IActionResult GetFilteredClassOnJob([FromQuery(Name = "filter")] int[] filter)
        {
            try
            {
                int userID = filter?[3] ?? 0;
                int oID = filter?[4] ?? 0;
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                List<object> ls = new List<object>();

                ls.Add(srv.FetchClassFiltersOnJob(filter));
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    ls.Add(srvScheme.FetchSchemesByTSPUserOnJob(curUserID));
                }
                else
                {
                    ls.Add(srvScheme.FetchSchemesByTSPUserOnJob(curUserID));
                }
                //return Ok(srv.FetchMasterSheetByFilters(filter));
                //ls.Add(srvScheme.FetchSchemeForFilter(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTraineesOfClassForOnJob/{ClassID}")]
        public IActionResult GetTraineesOfClassForOnJob(string ClassID)
        {
            try
            {
                return Ok(new
                {
                    TraineeList = srv.GetCompletedTraineeByClassOnJob(Convert.ToInt32(ClassID)),
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

    }
}