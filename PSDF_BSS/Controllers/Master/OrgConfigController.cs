/* **** Aamer Rehman Malik *****/

using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrgConfigController : ControllerBase
    {
        private ISRVOrgConfig srv = null;
        private ISRVProgramType PTypesrv = null;
        private ISRVOrganization srvOrg = null;
        private ISRVGender srvGen = null;
        private ISRVEducationTypes srvEdTp= null;
        private ISRVScheme srvSch = null;
        private ISRVTSPDetail srvTSPD = null;
        private ISRVClass srvClass = null;

        public OrgConfigController(ISRVOrgConfig srv, ISRVProgramType PTypesrv, ISRVOrganization srvOrg, ISRVGender srvGen,ISRVEducationTypes srvEdTp, ISRVScheme srvSch, ISRVTSPDetail srvTSPD, ISRVClass srvClass)
        {
            this.srv = srv;
            this.PTypesrv = PTypesrv;
            this.srvOrg = srvOrg;
            this.srvGen = srvGen;
            this.srvEdTp = srvEdTp;
            this.srvSch = srvSch;
            this.srvTSPD = srvTSPD;
            this.srvClass = srvClass;
        }

        // GET: OrgConfig
        [HttpGet]
        [Route("GetOrgConfig")]
        public IActionResult GetOrgConfig()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvOrg.FetchOrganization(false));

                ls.Add(srvGen.FetchGender(false));

                ls.Add(srvEdTp.FetchEducationTypes(false));
                ls.Add(PTypesrv.FetchProgramType(false));
                //ls.Add(srvSch.FetchScheme(false));
               // ls.Add(srvSch.FetchSchemeBusinessRuleType("Community"));
                //ls.Add(srvTSPD.FetchTSPDetailByScheme(22414));
                //ls.Add(srvClass.FetchClassByTsp(11308));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // GET: GETOrgConfigScheme
        [HttpGet]
        [Route("GetOrgConfigScheme/{ruletype}")]
        public IActionResult GetOrgConfigScheme(string ruletype)
        {
            try
            {
                List<object> ls = new List<object>();
                               
                ls.Add(srvSch.FetchSchemeBusinessRuleType(ruletype));
               
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // GET: GETOrgConfigTSP
        [HttpGet]
        [Route("GetOrgConfigTSP/{sid}")]
        public IActionResult GetOrgConfigTSP(int sid)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvTSPD.FetchTSPDetailByScheme(sid));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // GET: GETOrgConfigClass
        [HttpGet]
        [Route("GetOrgConfigClass/{tid}")]
        public IActionResult GetOrgConfigClass(int tid)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvClass.FetchClassByTsp(tid));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        //[Route("GetOrgConfig/{id}/{ruletype}/{sid}/{tid}/{cid}")]
        [Route("GetOrgConfig/{id}/{ruletype}/{sid}/{tid}")]
        //public IActionResult GetOrgConfig(int id, string ruletype, int sid, int tid, int cid)
         public IActionResult GetOrgConfig(int id, string ruletype, int sid, int tid)
        {
            try
            {
                //return Ok(srv.FetchOrgConfig(id, ruletype,sid,tid,cid));
                return Ok(srv.FetchOrgConfig(id, ruletype, sid, tid));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // RD: OrgConfig
        [HttpGet]
        [Route("RD_OrgConfig")]
        public IActionResult RD_OrgConfig()
        {
            try
            {
                return Ok(srv.FetchOrgConfig(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: OrgConfig
        [HttpPost]
        [Route("RD_OrgConfigBy")]
        public IActionResult RD_OrgConfigBy(OrgConfigModel mod)
        {
            try
            {
                return Ok(srv.FetchOrgConfig(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: OrgConfig/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(List<OrgConfigModel> D)
        {
           


            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], 1);
            if (IsAuthrized == true)
            {
                try
                {
                    //List<OrgConfigModel> PreviousOrgConfigList = srv.FetchOrgConfig(D[0].OID, D[0].BusinessRuleTypeForGetPreviousList, D[0].SchemeID,D[0].TSPID,D[0].ClassID);
                    List<OrgConfigModel> PreviousOrgConfigList = srv.FetchOrgConfig(D[0].OID, D[0].BusinessRuleTypeForGetPreviousList, D[0].SchemeID, D[0].TSPID);
                    int ID = srv.BatchInsert(D, D[0].OID, Convert.ToInt32(User.Identity.Name));
                    if (ID > 0)
                    {
                        srv.ComparreNewAndPreviousListOfOrgConfig(D, PreviousOrgConfigList, Convert.ToInt32(User.Identity.Name));
                    }
                    return Ok(ID);
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
    }
}