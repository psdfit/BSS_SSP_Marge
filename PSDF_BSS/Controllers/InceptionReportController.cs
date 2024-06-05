using DataLayer.Classes;
using DataLayer.Models;
using DataLayer.Services;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MasterDataModule.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class InceptionReportController : ControllerBase
    {
        private readonly ISRVInceptionReport srvInceptionReport;
        private readonly ISRVClass srvClass;
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVSections srvSections;
        private readonly ISRVContactPerson srvContactPerson;
        private readonly ISRVInstructor srvInstructor;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVScheme srvScheme;


        public InceptionReportController(ISRVInceptionReport srvInceptionReport, ISRVClass srvClass, ISRVOrgConfig srvOrgConfig, ISRVSections srvSections, ISRVContactPerson srvContactPerson, ISRVInstructor srvInstructor, ISRVUsers srvUsers, ISRVScheme srvScheme)
        {
            this.srvInceptionReport = srvInceptionReport;
            this.srvClass = srvClass;
            this.srvOrgConfig = srvOrgConfig;
            this.srvSections = srvSections;
            this.srvContactPerson = srvContactPerson;
            this.srvInstructor = srvInstructor;
            this.srvUsers = srvUsers;
            this.srvScheme = srvScheme;
        }


    [HttpGet]
        [Route("GetDataByClass/{classId}")]
        public IActionResult GetDataByClass(int classId)
        {
            try
            {
                List<object> list = new List<object>();
                if (classId > 0)
                {
                    list.Add(srvClass.GetByClassID(classId));
                    list.Add(srvOrgConfig.FetchOrgConfig(new OrgConfigModel() { ClassID = classId })[0]);
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: InceptionReport
        [HttpGet]
        [Route("GetInceptionReport/{id}")]
        public IActionResult GetInceptionReport(int id)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvInceptionReport.FetchInceptionReport(id));
                ls.Add(srvSections.FetchSections(false));
                ls.Add(srvInstructor.GetByClassID(id));
                ls.Add(srvClass.GetByClassID(id));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetFilteredInceptionReport/{filter}")]
        public IActionResult GetFilteredInceptionReport([FromQuery(Name = "filter")] int[] filter)
        {
            try
            {
                int userID = filter?[3] ?? 0;
                int oID = filter?[4] ?? 0;
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                List<object> ls = new List<object>();

                ls.Add(srvInceptionReport.FetchInceptionReportByFilters(filter));
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
        [Route("GetInceptionReportByClass")]
        public IActionResult GetInceptionReportByClass([FromQuery] int classID)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvInceptionReport.FetchInceptionReport(new InceptionReportModel() { ClassID = classID }));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InceptionReport
        [HttpGet]
        [Route("RD_InceptionReport")]
        public IActionResult RD_InceptionReport()
        {
            try
            {
                return Ok(srvInceptionReport.FetchInceptionReport(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("RD_InceptionReportBy_ID/{id}")]
        public IActionResult RD_InceptionReportBy_ID(int id)    // To get current incetion report in the change request approvals
        {
            try
            {
                return Ok(srvInceptionReport.GetByInceptionReportID(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("RD_Instuctors_By_InceptionID/{id}")]
        public IActionResult RD_Instuctors_By_InceptionID(int id)    // To get current incetion report in the change request approvals
        {
            try
            {
                return Ok(srvInceptionReport.GetMappedInstructorsByID(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetInceptionReportByUser/{id}")]
        public IActionResult GetInceptionReportByUser(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvInceptionReport.FetchInceptionReportDataByUser(id));
                ls.Add(srvInstructor.GetByTSPUserID(id));
                ///ls.Add(srvGender.FetchGender(false));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }


        // RD: InceptionReport
        [HttpPost]
        [Route("RD_InceptionReportBy")]
        public IActionResult RD_InceptionReportBy(InceptionReportModel model)
        {
            try
            {
                List<object> list = new List<object>();
                list.Add(srvInceptionReport.CheckReportCriteria(model.ClassID));
                list.Add(srvInceptionReport.FetchInceptionReport(model));
                list.Add(srvClass.GetByClassID(model.ClassID));
                list.Add(srvOrgConfig.FetchOrgConfig(new OrgConfigModel() { ClassID = model.ClassID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: InceptionReport/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(InceptionReportModel D)
        {
            try
            {
                //D.CurUserID = Common.GetUserFromRequest(Request).UserID;
                //return Ok(srv.SaveInceptionReport(D));UpdateClassStatusByReport
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvInceptionReport.SaveInceptionReport(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetContactPerson/{id}")]
        public ActionResult GetContactPerson(int id)
        {
            try
            {
                return Ok(srvContactPerson.FetchContactPerson(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: InceptionReport/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(InceptionReportModel d)
        {
            try
            {

                srvInceptionReport.ActiveInActive(d.IncepReportID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetFilteredInceptionReportPaged")]
        public IActionResult GetFilteredInceptionReport([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                filterModel.UserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                List<object> ls = new List<object>();

                ls.Add(srvInceptionReport.FetchInceptionReportByPaged(pagingModel, filterModel, out int totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: InceptionReport
        [HttpGet]
        [Route("GetActiveClassTiming/{id}")]
        public IActionResult GetActiveClassTiming(string id)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvInceptionReport.FetchActiveClassInception(id));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: InceptionReport
        [HttpGet]
        [Route("GetActiveClassTimingCR/{id}")]
        public IActionResult GetActiveClassTimingCR(string id)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvInceptionReport.FetchActiveClassInceptionCR(id));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

    }
}