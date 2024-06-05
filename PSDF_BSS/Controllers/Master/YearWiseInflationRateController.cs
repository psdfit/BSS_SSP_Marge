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
    public class YearWiseInflationRateController : ControllerBase
    {
        private ISRVYearWiseInflationRate srv = null;

        public YearWiseInflationRateController(ISRVYearWiseInflationRate srv)
        {
            this.srv = srv;
        }

        // GET: YearWiseInflationRate
        [HttpGet]
        [Route("GetYearWiseInflationRate")]
        public IActionResult GetYearWiseInflationRate()
        {
            try
            {
                return Ok(srv.FetchYearWiseInflationRate());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: YearWiseInflationRate
        [HttpGet]
        [Route("RD_YearWiseInflationRate")]
        public IActionResult RD_YearWiseInflationRate()
        {
            try
            {
                return Ok(srv.FetchYearWiseInflationRate(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: YearWiseInflationRate
        [HttpPost]
        [Route("RD_YearWiseInflationRateBy")]
        public IActionResult RD_YearWiseInflationRateBy(YearWiseInflationRateModel mod)
        {
            try
            {
                return Ok(srv.FetchYearWiseInflationRate(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: YearWiseInflationRate/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(YearWiseInflationRateModel D)
        {
            


            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.IRID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveYearWiseInflationRate(D));
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

        // POST: YearWiseInflationRate/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(YearWiseInflationRateModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.IRID, d.InActive, Convert.ToInt32(User.Identity.Name));
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