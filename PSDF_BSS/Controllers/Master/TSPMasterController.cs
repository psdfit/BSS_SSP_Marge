using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace PSDF_BSSMasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TSPMasterController : ControllerBase
    {
        private ISRVTSPMaster srv = null;
        private ISRVDistrict srvDist = null; //added by hammad to return district list

        public TSPMasterController(ISRVTSPMaster srv, ISRVDistrict srvDist)
        {
            this.srv = srv;
            this.srvDist = srvDist;
        }

        // GET: TSPMaster
        [HttpGet]
        [Route("GetTSPMaster")]
        public IActionResult GetTSPMaster()
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srv.FetchTSPMaster());
                ls.Add(srvDist.FetchDistrict()); // added by hammad to return district list

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TSPMaster
        [HttpGet]
        [Route("RD_TSPMaster")]
        public IActionResult RD_TSPMaster()
        {
            try
            {
                return Ok(srv.FetchTSPMaster(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TSPMaster
        [HttpPost]
        [Route("RD_TSPMasterBy")]
        public IActionResult RD_TSPMasterBy(TSPMasterModel mod)
        {
            try
            {
                return Ok(srv.FetchTSPMaster(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("CheckTspIsNew")]
        public IActionResult CheckTspIsNew(TSPMasterModel mod)
        {
            try
            {
                //TSPMasterModel model1 = srv.CheckDupplicateTspByNTN(mod.NTN, mod.TSPName);
                //TSPMasterModel model2 = srv.CheckDupplicateTspByNTNAlert(mod.NTN, mod.TSPName);
                //if (model1 == null && model2 == null)
                //    Msg = 1;
                //else if (model1 == null && model2 != null)
                //    Msg = 2;
                //else
                //    Msg = 3;
                return Ok(srv.CheckDupplicateTspByNTNAlert(mod.NTN, mod.TSPName));
               // return Ok(srv.CheckDupplicateTspByNTN(mod.NTN,mod.TSPName) == null ? true : false);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TSPMaster/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TSPMasterModel D)
        {
           
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.TSPMasterID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveTSPMaster(D));
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

        // POST: TSPMaster/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TSPMasterModel d)
        {
            
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {

                    srv.ActiveInActive(d.TSPMasterID, d.InActive, Convert.ToInt32(User.Identity.Name));
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
    }
}