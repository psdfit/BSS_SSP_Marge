using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class StipendStatusController : ControllerBase
    {
        private readonly ISRVStipendStatus srv;

        public StipendStatusController(ISRVStipendStatus _srv)
        {
            this.srv = _srv;
        }

        // GET: StipendStatus
        [HttpGet]
        [Route("GetStipendStatus")]
        public IActionResult GetStipendStatus()
        {
            try
            {
                return Ok(srv.FetchStipendStatus());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: StipendStatus
        [HttpGet]
        [Route("RD_StipendStatus")]
        public IActionResult RD_StipendStatus()
        {
            try
            {
                return Ok(srv.FetchStipendStatus(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: StipendStatus
        [HttpPost]
        [Route("RD_StipendStatusBy")]
        public IActionResult RD_StipendStatusBy(StipendStatusModel mod)
        {
            try
            {
                return Ok(srv.FetchStipendStatus(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: StipendStatus/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(StipendStatusModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.StipendStatusID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveStipendStatus(D));
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

        // POST: StipendStatus/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(StipendStatusModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.StipendStatusID, d.InActive, Convert.ToInt32(User.Identity.Name));
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