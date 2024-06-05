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
    public class DurationController : ControllerBase
    {
        private readonly ISRVDuration srvDuration;
        public DurationController(ISRVDuration srvDuration)
        {
            this.srvDuration = srvDuration;
        }
        // GET: Duration
        [HttpGet]
        [Route("GetDuration")]
        public IActionResult GetDuration()
        {
            try
            {

                return Ok(srvDuration.FetchDuration());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: Duration
        [HttpGet]
        [Route("RD_Duration")]
        public IActionResult RD_Duration()
        {
            try
            {
                return Ok(srvDuration.FetchDuration(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: Duration
        [HttpPost]
        [Route("RD_DurationBy")]
        public IActionResult RD_DurationBy(DurationModel mod)
        {
            try
            {
                return Ok(srvDuration.FetchDuration(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: Duration/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(DurationModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.DurationID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvDuration.SaveDuration(D));
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

        // POST: Duration/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(DurationModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srvDuration.ActiveInActive(d.DurationID, d.InActive, Convert.ToInt32(User.Identity.Name));
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

