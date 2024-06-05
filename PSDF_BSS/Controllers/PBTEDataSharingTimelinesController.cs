using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PBTEDataSharingTimelinesController : ControllerBase
    {
        private readonly ISRVPBTEDataSharingTimelines srvPBTEDataSharingTimelines;
        public PBTEDataSharingTimelinesController(ISRVPBTEDataSharingTimelines srvPBTEDataSharingTimelines)
        {
            this.srvPBTEDataSharingTimelines = srvPBTEDataSharingTimelines;
        }
        // GET: PBTEDataSharingTimelines
        [HttpGet]
        [Route("GetPBTEDataSharingTimelines")]
        public IActionResult GetPBTEDataSharingTimelines()
        {
            try
            {

                return Ok(srvPBTEDataSharingTimelines.FetchPBTEDataSharingTimelines());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: PBTEDataSharingTimelines
        [HttpGet]
        [Route("RD_PBTEDataSharingTimelines")]
        public IActionResult RD_PBTEDataSharingTimelines()
        {
            try
            {
                return Ok(srvPBTEDataSharingTimelines.FetchPBTEDataSharingTimelines(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: PBTEDataSharingTimelines
        [HttpPost]
        [Route("RD_PBTEDataSharingTimelinesBy")]
        public IActionResult RD_PBTEDataSharingTimelinesBy(PBTEDataSharingTimelinesModel mod)
        {
            try
            {
                return Ok(srvPBTEDataSharingTimelines.FetchPBTEDataSharingTimelines(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: PBTEDataSharingTimelines/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(PBTEDataSharingTimelinesModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.ID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvPBTEDataSharingTimelines.SavePBTEDataSharingTimelines(D));
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

        // POST: PBTEDataSharingTimelines/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(PBTEDataSharingTimelinesModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srvPBTEDataSharingTimelines.ActiveInActive(d.ID, d.InActive, Convert.ToInt32(User.Identity.Name));
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

