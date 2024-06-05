/* **** Aamer Rehman Malik *****/

using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using PSDF_BSS.Logging;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitPlanController : ControllerBase
    {
        private readonly ISRVVisitPlan srvVisitPlan;
        private readonly ISRVClass srvClass;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVUserEventMap srvUserEventMap;
        //private readonly ISRVClassEventMap srvEventClasses;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVCluster srvCluster;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVRegion srvRegion;
        private readonly ISRVClassEventMap srvClassEventMap;

        public VisitPlanController(ISRVVisitPlan srvVisitPlan, ISRVClass srvClass, ISRVUsers srvUsers, ISRVUserEventMap srvUserEventMap, ISRVClassEventMap srvClassEventMap, ISRVDistrict srvDistrict, ISRVCluster srvCluster, ISRVScheme srvScheme, ISRVRegion srvRegion)
        {
            this.srvVisitPlan = srvVisitPlan;
            this.srvClass = srvClass;
            this.srvUsers = srvUsers;
            this.srvUserEventMap = srvUserEventMap;
            this.srvClassEventMap = srvClassEventMap;
            this.srvDistrict = srvDistrict;
            this.srvCluster = srvCluster;
            this.srvScheme = srvScheme;
            this.srvRegion = srvRegion;
        }

        // GET: VisitPlan
        [HttpGet]
        [Route("GetVisitPlan")]
        public IActionResult GetVisitPlan()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvVisitPlan.FetchVisitPlan());

                ls.Add(srvUsers.FetchUsers(false));
                ls.Add(srvClass.FetchClass(false));
                ls.Add(srvCluster.FetchCluster(false));
                ls.Add(srvDistrict.FetchDistrict(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // GET: VisitPlan
        [HttpGet]
        [Route("GetCallCenterVisitPlan")]
        public IActionResult GetCallCenterVisitPlan()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvVisitPlan.FetchCallCenterVisitPlan());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetEventMappedData/{id}")]
        public ActionResult GetEventMappedData(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                //TradeDetailMapModel Du = new TradeDetailMapModel();
                UserEventMapModel Ue = new UserEventMapModel();
                ClassEventMapModel Ce = new ClassEventMapModel();
                SchemeEventMapModel Se = new SchemeEventMapModel();
                //TradeConsumableMaterialMapModel Cs = new TradeConsumableMaterialMapModel();
                //TradeSourceOfCurriculumMapModel Sc = new TradeSourceOfCurriculumMapModel();
                //Du.TradeID = id;
                Ue.VisitPlanID = id;
                Ce.VisitPlanID = id;
                Se.VisitPlanID = id;
                //Cs.TradeID = id;
                //Sc.TradeID = id;
                //ls.Add(srvTradeDetail.FetchTradeDetailMap(Du));

                ls.Add(srvUserEventMap.FetchUserEventMap(Ue));
                ls.Add(srvClassEventMap.FetchClassEventMap(Ce));
                ls.Add(srvUserEventMap.FetchSchemeEventMap(Se));

                //ls.Add(srvTradeConsumableMaterial.FetchTradeConsumableMaterialMap(Cs));

                //ls.Add(srvTradeSourceOfCurriculum.FetchTradeSourceOfCurriculumMap(Sc));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: VisitPlan
        [HttpGet]
        [Route("RD_VisitPlan")]
        public IActionResult RD_VisitPlan()
        {
            try
            {
                return Ok(srvVisitPlan.FetchVisitPlan(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: VisitPlan
        [HttpGet]
        [Route("GetTSPUsersByScheme/{id}")]
        public IActionResult GetTSPUsersByScheme(string id)
        {
            try
            {

                List<object> list = new List<object>();

                list.Add(srvVisitPlan.FetchTSPUsers(id));

                return Ok(list);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }  // RD: VisitPlan
        [HttpGet]
        [Route("GetVisitPlanBy/{classId}")]
        public IActionResult GetVisitPlanBy(int classId)
        {
            try
            {
                //SRVUserEventMap u = new SRVUserEventMap();
                //SRVClassEventMap c = new SRVClassEventMap();
                List<object> list = new List<object>();

                list.Add(srvVisitPlan.FetchVisitPlan(new VisitPlanModel() { ClassID = classId }));
                list.Add(srvClass.GetByClassID(classId));
                list.Add(srvUsers.FetchUsers(false));
                list.Add(srvClass.FetchClass(false));
                list.Add(srvUserEventMap.FetchUserEventMapAll(0));
                list.Add(srvClassEventMap.FetchClassEventMapAll(0));
                list.Add(srvCluster.FetchCluster(false));
                list.Add(srvDistrict.FetchDistrict(false));
                list.Add(srvScheme.FetchScheme(false));
                list.Add(srvRegion.FetchRegion(false));
                list.Add(srvUserEventMap.FetchSchemeEventMapAll(0));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetVisitPlanByDate")]
        public IActionResult GetVisitPlanByDate(VisitPlanModel mod)
        {
            try
            {
                List<object> list = new List<object>();
                //SRVUserEventMap u = new SRVUserEventMap();
                //SRVClassEventMap c = new SRVClassEventMap();

                //mod.UserID = Convert.ToInt32(User.Identity.Name);
                list.Add(srvVisitPlan.FetchCalendarVisitPlan(mod));
                list.Add(srvUsers.FetchUsers(false));
                list.Add(srvClass.FetchClass(false));
                list.Add(srvUserEventMap.FetchUserEventMapAll(0));
                list.Add(srvClassEventMap.FetchClassEventMapAll(0));
                list.Add(srvCluster.FetchCluster(false));
                list.Add(srvDistrict.FetchDistrict(false));
                list.Add(srvScheme.FetchScheme(false));
                list.Add(srvRegion.FetchRegion(false));
                list.Add(srvUserEventMap.FetchSchemeEventMapAll(0));


                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetVisitsByUser")]
        public IActionResult GetVisitsByUser(VisitPlanModel mod)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvVisitPlan.FetchCalendarVisitPlan(mod));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateUserEventStatus")]
        public IActionResult UpdateUserEventStatus(UserEventMapModel uvm)
        {
            try
            {
                srvVisitPlan.UpdateUserEventStatus(uvm);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateCallCenterAgentEventStatus")]
        public IActionResult UpdateCallCenterAgentEventStatus(UserEventMapModel vpm)
        {
            try
            {
                srvVisitPlan.UpdateCallCenterAgentEventStatus(vpm);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetUserEventReportData/{visitplanid}")]
        public IActionResult GetUserEventReportData(int visitplanid)
        {
            try
            {
                //srvVisitPlan.GetUserEventReport(vp);
                return Ok(srvVisitPlan.GetUserEventReport(visitplanid));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetTPMVisitPlanBy/{userLevel}")]
        public IActionResult GetTPMVisitPlanBy(int userLevel)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvVisitPlan.GetByVisitType(userLevel));
                //list.Add(srvClass.GetByClassID(classId));
                list.Add(srvClass.FetchClass(false));
                list.Add(srvUsers.FetchUsers(false));
                list.Add(srvCluster.FetchCluster(false));
                list.Add(srvDistrict.FetchDistrict(false));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetEventUsers/{visitplanid}")]
        public IActionResult GetEventUsers(int visitplanid)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvVisitPlan.GetEventUsers(visitplanid));
                //list.Add(srvClass.GetByClassID(classId));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetPlanBy")]
        public IActionResult GetPlanBy(VisitPlanModel D)
        {
            try
            {
                //D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvVisitPlan.FetchCalendarVisitPlan(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("SaveNewCallCenterAgent")]
        public IActionResult SaveNewCallCenterAgent(UserEventMapModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                srvVisitPlan.SaveNewCallCenterAgent(D);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save(VisitPlanModel D)
        {
            
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.VisitPlanID);
            if (IsAuthrized == true)
            {
                try
                {
                    //D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvVisitPlan.SaveVisitPlan(D));
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

        // POST: VisitPlan/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(VisitPlanModel d)
        {
            try
            {
                //
                srvVisitPlan.ActiveInActive(d.VisitPlanID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}