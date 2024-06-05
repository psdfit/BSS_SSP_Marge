/* **** Aamer Rehman Malik *****/

using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PSDF_BSSMasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly ISRVClass srvClass;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVSector srvSector;
        private readonly ISRVGender srvGender;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTehsil srvTehsil;
        private readonly ISRVCluster srvCluster;
        private readonly ISRVEducationTypes srvEducationTypes;
        private readonly ISRVCertificationAuthority srvCertificationAuthority;
        private readonly ISRVTrade srvTrade;
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVTradeDetailMap srvTradeDetailMap;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVDuration srvDuration;
        private readonly ISRVSourceOfCurriculum srvSourceOfCurriculum;
        private readonly ISRVProvinces srvProvinces;
        private readonly ISRVBSSReports srvDropDown;

        public ClassController(ISRVClass srvClass, ISRVUsers srvUsers, ISRVSector srvSector, ISRVGender srvGender, ISRVDistrict srvDistrict, ISRVTehsil srvTehsil
            , ISRVCluster srvCluster, ISRVEducationTypes srvEducationTypes, ISRVCertificationAuthority srvCertificationAuthority
            , ISRVTrade srvTrade, ISRVTSPMaster srvTSPMaster, ISRVTradeDetailMap srvTradeDetailMap, ISRVScheme srvScheme, ISRVDuration srvDuration
            , ISRVSourceOfCurriculum srvSourceOfCurriculum, ISRVProvinces srvProvinces, ISRVBSSReports srvBSSReports)
        {
            this.srvClass = srvClass;
            this.srvUsers = srvUsers;
            this.srvSector = srvSector;
            this.srvGender = srvGender;
            this.srvDistrict = srvDistrict;
            this.srvTehsil = srvTehsil;
            this.srvCluster = srvCluster;
            this.srvEducationTypes = srvEducationTypes;
            this.srvCertificationAuthority = srvCertificationAuthority;
            this.srvTrade = srvTrade;
            this.srvTSPMaster = srvTSPMaster;
            this.srvTradeDetailMap = srvTradeDetailMap;
            this.srvScheme = srvScheme;
            this.srvDuration = srvDuration;
            this.srvSourceOfCurriculum = srvSourceOfCurriculum;
            this.srvProvinces = srvProvinces;
            this.srvDropDown = srvBSSReports;
        }

        // GET: Class
        [HttpGet]
        [Route("GetClass")]
        public IActionResult GetClass()
        {
            try
            {
                List<object> ls = new List<object>();

                //ls.Add(srvClass.FetchClass());
                ls.Add(srvClass.FetchApprovedClass());
                ls.Add(srvSector.FetchSector(false));
                ls.Add(srvGender.FetchGender(false));
                ls.Add(srvDistrict.FetchDistrict(false));
                ls.Add(srvTehsil.FetchTehsil(false));
                ls.Add(srvCluster.FetchCluster(false));
                ls.Add(srvEducationTypes.FetchEducationTypes(false));
                ls.Add(srvCertificationAuthority.FetchCertificationAuthority(false));
                ls.Add(srvTrade.FetchTrade(new TradeModel() { IsApproved = true }));
                ls.Add(srvTSPMaster.FetchTSPMaster(false));
                ls.Add(srvTradeDetailMap.FetchTradeDetailMap());
                ls.Add(srvDuration.FetchDuration(false));
                ls.Add(srvSourceOfCurriculum.FetchSourceOfCurriculum(false));
                ls.Add(srvCluster.FetchCluster(false));
                ls.Add(srvProvinces.FetchProvince(false));

                ls.Add(this.FetchRegistrationAuthority());
                ls.Add(this.FetchProgramFocus());
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }

        public IActionResult FetchRegistrationAuthority()
        {
            try
            {
                var SpDesc = new
                {
                    SpName = "RD_RegistrationAuthority",
                    SpParamValue = "InActive=0"
                    //ParmNameAndValue = "FirstParmName=FirstParamValue/SecondParmName=SecondParamValue/"
                };

                // Serialize the object to a JSON string
                string jsonString = JsonConvert.SerializeObject(SpDesc);

                return Ok(srvDropDown.FetchDropDownList(jsonString));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        public IActionResult FetchProgramFocus()
        {
            try
            {
                var SpDesc = new
                {
                    SpName = "RD_ProgramFocus",
                    SpParamValue = "InActive=0"
                    //ParmNameAndValue = "FirstParmName=FirstParamValue/SecondParmName=SecondParamValue/"
                };

                // Serialize the object to a JSON string
                string jsonString = JsonConvert.SerializeObject(SpDesc);

                return Ok(srvDropDown.FetchDropDownList(jsonString));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: Class
        [HttpGet]
        [Route("RD_Class")]
        public IActionResult RD_Class()
        {
            try
            {
                return Ok(srvClass.FetchClass(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetFilteredClasses/{filter}")]
        public IActionResult GetFilteredClasses([FromQuery(Name = "filter")] int[] filter)
        {
            //List<TraineeProfileModel> list = new List<TraineeProfileModel>();
            try
            {
                List<object> ls = new List<object>();
                int userLevel;
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                UsersModel loginuser = new UsersModel();
                loginuser = srvUsers.GetByUserID(CurUserID);
                userLevel = loginuser.UserLevel;

                ls.Add(srvClass.FetchClassByFilters(filter));
                ls.Add(userLevel);
                return Ok(ls);
                //return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: Class
        [HttpPost]
        [Route("RD_ClassBy")]
        public IActionResult RD_ClassBy(ClassModel mod)
        {
            try
            {
                return Ok(srvClass.FetchClass(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: Class
        [HttpGet]
        [Route("RD_ClassByStatus")]
        public IActionResult RD_ClassByStatus()
        {
            List<object> ls = new List<object>();

            //ls.Add(srv.FetchClass(new ClassModel() { ClassStatusID = 1 }));

            ls.Add(srvClass.FetchClass().OrderByDescending(p => p.StartDate).Where(p => p.ClassStatusID != 1).ToList());
            ls.Add(new SRVClassStatus().FetchClassStatus());

            try
            {
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: Class/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody] string str)
        {
            try
            {
                ClassModel[] model = JsonConvert.DeserializeObject<ClassModel[]>(str);
                var InvalidClassStartDate = model.FirstOrDefault(x => (x.StartDate.Value.Day > 5 && x.StartDate.Value.Day < 15) || (x.StartDate.Value.Day > 16));
                if (InvalidClassStartDate != null)
                {
                    return BadRequest($"Invalid StartDate {InvalidClassStartDate.StartDate.Value} , it must be (in-between 1 to 5 ) or (15 or 16) date of month.");
                }

                List<ClassModel> ls = new List<ClassModel>();
                foreach (var item in model)
                {
                    item.CurUserID = Convert.ToInt32(User.Identity.Name);
                    ls.Add(srvClass.SaveClass(item));
                }
                // D.CurUserID= Common.GetUserFromRequest(Request).UserID;
                //return Ok(srv.SaveClass(D));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: Class/Save
        [HttpPost]
        [Route("UpdateClassStatus")]
        public IActionResult UpdateClassStatus(ClassModel model)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.ClassID);
            if (IsAuthrized == true)
            {
                try
                {
                    srvClass.UpdateClassStatus(model.ClassID, model.ClassStatusID);

                    return Ok();
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

        [HttpGet]
        [Route("GetClassSequence/{noOfSeqs}")]
        public IActionResult GetClassSequence(int noOfSeqs)
        {
            try
            {
                int[] seqs = new int[noOfSeqs];

                for (int i = 0; i < noOfSeqs; i++)
                {
                    seqs[i] = srvClass.GetClassSequence();
                }

                return Ok(seqs);
            }
            catch (Exception e)
            { return BadRequest(e.Message); }
        }

        // POST: Class/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ClassModel d)
        {
            try
            {
                srvClass.ActiveInActive(d.ClassID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("RTP")]
        public IActionResult RTP(ClassModel d)
        {
            try
            {
                //
                srvClass.RTP(d.ClassID, d.RTP, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetClassesBySchemeID/{id}")]
        public IActionResult GetClassesBySchemeID(int id)
        {
            try
            {
                List<ClassModel> m = new List<ClassModel>();

                m = srvClass.FetchClassByScheme(id);

                return Ok(m);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }

        [HttpPost]
        [Route("FetchClassesByUser")]
        public IActionResult FetchClassesByUser(QueryFilters filters)
        {
            try
            {
                return Ok(srvClass.FetchClassesByUser(filters));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost]
        [Route("FetchPendingInceptionReportClassesByUser")]
        public IActionResult FetchPendingInceptionReportClassesByUser(QueryFilters filters)
        {
            try
            {
                return Ok(srvClass.FetchPendingInceptionReportClassesByUser(filters));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost]
        [Route("FetchPendingRegistertionClassesByUser")]
        public IActionResult FetchPendingRegistertionClassesByUser(QueryFilters filters)
        {
            try
            {
                return Ok(srvClass.FetchPendingRegisterationClassesByUser(filters));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost]
        [Route("FetchPendingRTPClassesByUser")]
        public IActionResult FetchPendingRTPClassesByUser(QueryFilters filters)
        {
            try
            {
                return Ok(srvClass.FetchPendingRTPClassesByUser(filters));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost]
        [Route("FetchPendingInceptionReportClassesByKAMUser")]
        public IActionResult FetchPendingInceptionReportClassesByKAMUser(QueryFilters filters)
        {
            try
            {
                return Ok(srvClass.FetchPendingInceptionReportClassesByKAMUser(filters));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost]
        [Route("FetchPendingRegistertionClassesByKAMUser")]
        public IActionResult FetchPendingRegistertionClassesByKAMUser(QueryFilters filters)
        {
            try
            {
                return Ok(srvClass.FetchPendingRegisterationClassesByKAMUser(filters));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost]
        [Route("FetchPendingRTPClassesByKAMUser")]
        public IActionResult FetchPendingRTPClassesByKAMUser(QueryFilters filters)
        {
            try
            {
                return Ok(srvClass.FetchPendingRTPClassesByKAMUser(filters));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpGet]
        [Route("GetClassesByUser/{id}")]
        public IActionResult GetClassesByUser(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvClass.FetchClassesDataByUser(id));
                ls.Add(srvTrade.FetchTrade(new TradeModel() { IsApproved = true }));
                ls.Add(srvTradeDetailMap.FetchTradeDetailMap());
                ls.Add(srvEducationTypes.FetchEducationTypes(false));
                ls.Add(srvCertificationAuthority.FetchCertificationAuthority(false));
                ls.Add(srvCluster.FetchCluster(false));
                ls.Add(srvTehsil.FetchTehsil(false));
                ls.Add(srvDistrict.FetchDistrict(false));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpGet]
        [Route("GetClassesByTsp/{tspId}")]
        public IActionResult GetClassesByTsp(int tspId)
        {
            try
            {
                return Ok(srvClass.FetchClassByTsp(tspId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("FetchClassesByUserPaged")]
        public IActionResult FetchClassesByUserPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();
                List<object> ls = new List<object>();
                ls.Add(srvClass.FetchClassesByUserPaged(pagingModel, filterModel, out int totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        #region ClassProceedingStatus

        // GET: MasterSheet
        [HttpGet]
        [Route("GetClassProceeedingStatusData")]
        public IActionResult GetClassProceeedingStatusData()
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvScheme.FetchScheme(false));
                ls.Add(srvClass.FetchClassProceeedingStatusDataByFilters(new int[] { 0, 0, 0 }));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetFilteredClassProceeedingStatusData/{filters}")]
        public IActionResult GetFilteredClassProceeedingStatusData([FromQuery(Name = "filters")] int[] filters)
        {
            try
            {
                return Ok(srvClass.FetchClassProceeedingStatusDataByFilters(filters));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion ClassProceedingStatus
    }
}