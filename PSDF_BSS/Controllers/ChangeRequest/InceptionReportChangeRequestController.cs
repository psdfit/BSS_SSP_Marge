using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using PSDF_BSS.Logging;

namespace PSDF_BSSChangeRequest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InceptionReportChangeRequestController : ControllerBase
    {
        ISRVInceptionReportChangeRequest srv = null;
        public InceptionReportChangeRequestController(ISRVInceptionReportChangeRequest srv)
        {
            this.srv = srv;
        }
        // GET: InceptionReportChangeRequest
        [HttpGet]
        [Route("GetInceptionReportChangeRequest")]
        public IActionResult GetInceptionReportChangeRequest()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchInceptionReportChangeRequest());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InceptionReportChangeRequest
        [HttpGet]
        [Route("RD_InceptionReportChangeRequest")]
        public IActionResult RD_InceptionReportChangeRequest()
        {
            try
            {
                return Ok(srv.FetchInceptionReportChangeRequest(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InceptionReportChangeRequest
        [HttpPost]
        [Route("RD_InceptionReportChangeRequestBy")]
        public IActionResult RD_InceptionReportChangeRequestBy(InceptionReportChangeRequestModel mod)
        {
            try
            {
                return Ok(srv.FetchInceptionReportChangeRequest(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("GetFilteredInceptionReportChangeRequest")]
        public IActionResult GetFilteredInceptionReportChangeRequest(InceptionReportChangeRequestModel mod)
        {
            try
            {
                return Ok(srv.FetchInceptionReportChangeRequest(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: InceptionReportChangeRequest/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(InceptionReportChangeRequestModel D)
        {
            
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.InceptionReportChangeRequestID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveInceptionReportChangeRequest(D));
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

        // POST: InceptionReportChangeRequest/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(InceptionReportChangeRequestModel d)
        {
            try
            {
                srv.ActiveInActive(d.InceptionReportChangeRequestID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

    }
}

