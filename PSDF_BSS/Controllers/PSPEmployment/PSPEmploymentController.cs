/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers.PSPEmployment
{
    public class TspExcelModel
    {
        public List<TSPEmploymentModel> data { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PSPEmploymentController : ControllerBase
    {
        private readonly ISRVPSPEmployment srv;
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
        private readonly ISRVTrade srvTrade;
        private readonly ISRVTSPEmployment srvTSPEmployment;

        public PSPEmploymentController(ISRVPSPEmployment srv, ISRVEmploymentStatus srvEmploymentStatus,
        ISRVPlacementType srvPlacementType, ISRVDistrict srvDistrict, ISRVTehsil srvTehsil,
                ISRVTraineeProfile srvTraineeProfile, ISRVUsers srvUsers, ISRVScheme srvScheme, ISRVTrade srvTrade,
                ISRVVerificationMethod verificationMethod, ISRVClass srvClass, ISRVOrgConfig srvOrgConfig, ISRVTSPEmployment srvTSPEmployment,
        IWebHostEnvironment env)
        {
            this.srv = srv;
            this.srvTraineeProfile = srvTraineeProfile;
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
            this.srvTrade = srvTrade;
            this.srvTSPEmployment = srvTSPEmployment;
        }

        [HttpGet]
        [Route("GetPSPBatchTraineeList")]
        public IActionResult GetPSPBatchTraineeList(int pspBatchId)
        {
            try
            {
                var data = srv.FetchPSPBatchTraineeByID(new PSPBatchModel
                {
                    PSPBatchID = pspBatchId
                });
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }


        [HttpGet]
        [Route("GetPSPBatches")]

        public IActionResult GetPSPBatches()
        {
            try
            {

                List<object> ls = new List<object>();

                ls.Add(srv.FetchPSPBatches());


                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetPSPTraineeAssignment/{id}")]

        public IActionResult GetPSPTraineeAssignment(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvUsers.FetchUsers(false));
                ls.Add(srv.FetchPSPBatches());
                ls.Add(srv.FetchPSPInterestedTraineesForAssignment(id));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("PSPAssignedTraineesByID")]

        public IActionResult PSPAssignedTraineesByID(PSPBatchModel mod)
        {
            try
            {
                List<object> ls = new List<object>();
                //ls.Add(srvUsers.FetchUsers(false));
                //ls.Add(srv.FetchPSPBatches());
                ls.Add(srv.FetchPSPAssignedTrainees(mod));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetPSPSelfEmploymentList")]
        public IActionResult GetPSPSelfEmploymentList(int pId, int vmId, int bId)
        {
            try
            {
                var data = srv.FetchPlacementFormE_PSP(new PSPEmploymentModel
                {
                    PlacementTypeID = pId,
                    VerificationMethodId = vmId,
                    PSPBatchID = bId
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
        [Route("GetPSPBatchTraineesForExport")]
        public IActionResult GetPSPBatchTraineesForExport([FromBody] string TraineeIDs)
        {
            try
            {
                return Ok(new
                {
                    PlacementStatus = Common.FilterPlacementDropdown(srvEmploymentStatus.FetchEmploymentStatus(false)),
                    TraineeList = srv.GetCompletedTraineeByTraineeIDs(TraineeIDs),
                    PlacementTypes = srvPlacementType.FetchPlacementType(false),
                    District = srvDistrict.FetchDistrict(false),
                    Tehsil = srvTehsil.FetchTehsil(false),
                    VerificationMethods = srvVerificationMethod.FetchAll(),
                    //PlacementData = srv.FetchPlacementFormE(new TSPEmploymentModel { ClassID = Convert.ToInt32(ClassID) })
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }



        [HttpPost]
        [Route("GetData")]
        public IActionResult GetData(PSPEmploymentModel model)
        {
            try
            {
                Dictionary<string, object> ls = new Dictionary<string, object>();


                if (model.TraineeID != 0 && model.TraineeID != null)
                {
                    Parallel.Invoke(() => ls.Add("PSPBatch", srv.FetchPSPBatches()),
                                    () => ls.Add("TraineeProfile", srvTraineeProfile.GetByTraineeID(model.TraineeID)),
                                    () => ls.Add("PlacementStatus", Common.FilterPlacementDropdown(srvEmploymentStatus.FetchEmploymentStatus(false))),
                                    () => ls.Add("PlacementTypes", srvPlacementType.FetchPlacementType(false)),
                                    () => ls.Add("District", srvDistrict.FetchDistrict(false)),
                                    () => ls.Add("Tehsil", srvTehsil.FetchTehsil(false)),
                                    () => ls.Add("EmploymentData", srv.FetchPlacementFormE(new PSPEmploymentModel { PSPBatchID = model.PSPBatchID , TraineeID = model.TraineeID})),
                                    //() => ls.Add("OrgConfig", orgConfig != null ? orgConfig.EmploymentDeadline : 0),
                                    //() => ls.Add("DeadlineStatus", DeadlineStatus),
                                    //() => ls.Add("TraineeProfile", srv.GetTraineeData((int)model.ClassID, (int)model.TraineeID)),
                                    () => ls.Add("VerificationMethods", srvVerificationMethod.FetchAll())
                                   );
                }
                else
                {
                    if (model.PSPBatchID != 0)
                        Parallel.Invoke(() => ls.Add("PSPBatch", srv.FetchPSPBatches()),
                                       //() => ls.Add("TraineeProfile", new SRVTraineeProfile().FetchTraineeProfile()),
                                       () => ls.Add("PlacementStatus", Common.FilterPlacementDropdown(srvEmploymentStatus.FetchEmploymentStatus(false))),
                                       () => ls.Add("PlacementTypes", srvPlacementType.FetchPlacementType(false)),
                                       () => ls.Add("District", srvDistrict.FetchDistrict(false)),
                                       () => ls.Add("Tehsil", srvTehsil.FetchTehsil(false)),
                                       () => ls.Add("EmploymentData", srv.FetchPlacementFormE(new PSPEmploymentModel { PSPBatchID = model.PSPBatchID })),
                                       //() => ls.Add("OrgConfig", orgConfig != null ? orgConfig.EmploymentDeadline : 0),
                                       //() => ls.Add("DeadlineStatus", DeadlineStatus),

                                       () => ls.Add("VerificationMethods", srvVerificationMethod.FetchAll())
                                      );
                }



                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }



        [HttpPost]
        [Route("GetTraineesFiltersData")]

        public IActionResult GetTraineesFiltersData(QueryFilters filters)
        {
            try
            {
                int userID = filters.UserID;
                int oID = filters.OID;
                int classID = filters.ClassID;
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                List<object> ls = new List<object>();

                ls.Add(srvTrade.FetchTrade(new TradeModel() { IsApproved = true }));
                ls.Add(srv.FetchClassFilters(filters));
                //if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                //{
                //    ls.Add(srvScheme.FetchSchemesByTSPUser(curUserID));
                //}
                //else
                //{
                //    ls.Add(srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true, OrganizationID = oID }));
                //}

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Route("GetFilteredPSPTrainees")]
        public IActionResult GetFilteredPSPTrainees(QueryFilters filters)
        {
            try
            {
                int userID = filters.UserID;
                int oID = filters.OID;
                int classID = filters.ClassID;
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                List<object> ls = new List<object>();

                ls.Add(srv.GetCompletedTraineeByClass(filters));

                //return Ok(srv.FetchMasterSheetByFilters(filter));
                //ls.Add(srvScheme.FetchSchemeForFilter(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetFilteredPSPBatchTrainees")]
        public IActionResult GetFilteredPSPBatchTrainees(QueryFilters filters)
        {
            try
            {
                int userID = filters.UserID;
                int oID = filters.OID;
                int classID = filters.ClassID;
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                List<object> ls = new List<object>();

                ls.Add(srv.GetCompletedTraineeByClass(filters));

                //return Ok(srv.FetchMasterSheetByFilters(filter));
                //ls.Add(srvScheme.FetchSchemeForFilter(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        
        [HttpPost]
        [Route("GetPSPTraineeForDEOVerification")]
        public IActionResult GetPSPTraineeForDEOVerification(QueryFilters filters)
        {
            try
            {
                int userID = filters.UserID;
                int oID = filters.OID;
                int classID = filters.ClassID;
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                List<object> ls = new List<object>();

                ls.Add(srv.GetCompletedTraineeByClass(filters));

                //return Ok(srv.FetchMasterSheetByFilters(filter));
                //ls.Add(srvScheme.FetchSchemeForFilter(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("SavePSPBatch")]
        public async Task<IActionResult> SavePSPBatch(PSPBatchModel D)
        { 
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.PSPBatchID);
            if (IsAuthrized == true)
            {
                // TSPEmploymentModel D = JsonConvert.DeserializeObject<TSPEmploymentModel>(data);
                //srv.SavePSPBatch(D);
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    srv.SavePSPBatch(D);
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
        [Route("SavePSPBatchTrainees")]
        public async Task<IActionResult> SavePSPBatchTrainees(List<PSPBatchModel> ls)
        {

            // TSPEmploymentModel D = JsonConvert.DeserializeObject<TSPEmploymentModel>(data);
            //D.CurUserID = Convert.ToInt32(User.Identity.Name);
            //srv.SavePSPBatchTrainees(ls);
            try
            {
                //D.CurUserID = Convert.ToInt32(User.Identity.Name);
                srv.SavePSPBatchTrainees(ls);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }

        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save(PSPEmploymentModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.PlacementID);
            if (IsAuthrized == true)
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
                    return BadRequest(e.InnerException.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        [HttpPost]
        [Route("SubmitClassEmployment")]
        public async Task<IActionResult> SubmitClassEmployment(TSPEmploymentModel m)
        {
            try
            {
                return Ok(srv.SubmitClassEmployment(m.ClassID ?? 0));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("SubmitPSPAssignedTrainees")]
        public async Task<IActionResult> SubmitPSPAssignedTrainees(PSPBatchModel mod)
        {
           
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], mod.TraineeID);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.UpdateTraineesPSP(mod);
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



        [HttpPost]
        [Route("ForwardedToTelephonic")]
        public IActionResult ForwardedToTelephonic(int[] list)
        {
            try
            {
                var success = srv.ForwardedToTelephonic(list);
                return Ok(success);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

    }
}