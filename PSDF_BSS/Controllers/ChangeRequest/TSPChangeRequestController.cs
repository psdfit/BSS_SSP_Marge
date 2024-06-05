using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PSDF_BSS.Logging;

namespace PSDF_BSS_Master.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TSPChangeRequestController : ControllerBase
    {
        ISRVTSPChangeRequest srv = null;
        public TSPChangeRequestController(ISRVTSPChangeRequest srv)
        {
            this.srv = srv;
        }
        // GET: TSPChangeRequest
        [HttpGet]
        [Route("GetTSPChangeRequest")]
        public IActionResult GetTSPChangeRequest()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchTSPChangeRequest());

                ls.Add(new SRVTSPDetail().FetchTSPDetail(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: TSPChangeRequest
        [HttpGet]
        [Route("RD_TSPChangeRequest")]
        public IActionResult RD_TSPChangeRequest()
        {
            try
            {
                return Ok(srv.FetchTSPChangeRequest(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: TSPChangeRequest
        [HttpPost]
        [Route("RD_TSPChangeRequestBy")]
        public IActionResult RD_TSPChangeRequestBy(TSPChangeRequestModel mod)
        {
            try
            {
                return Ok(srv.FetchTSPChangeRequest(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: TSPChangeRequest/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TSPChangeRequestModel D)
        {
           

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.TSPChangeRequestID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveTSPChangeRequest(D));
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



        // POST: TSPChangeRequest/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TSPChangeRequestModel d)
        {
            try
            {
                srv.ActiveInActive(d.TSPChangeRequestID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

