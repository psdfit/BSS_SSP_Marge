using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace PSDF_BSS.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomFinancialYearController : ControllerBase
    {
        private ISRVCustomFinancialYear srv = null;

        // GET: api/CustomFinancialYear
        public CustomFinancialYearController(ISRVCustomFinancialYear srv)
        {
            this.srv = srv;
        }

        [HttpGet]
        [Route("GetCustomFinancialYear")]
        public IActionResult GetCustomFinancialYear()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchCustomFinancialYear());

                ls.Add(new SRVOrganization().FetchOrganization(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: CustomFinancialYear
        [HttpGet]
        [Route("RD_CustomFinancialYear")]
        public IActionResult RD_CustomFinancialYear()
        {
            try
            {
                return Ok(srv.FetchCustomFinancialYear(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: CustomFinancialYear
        [HttpPost]
        [Route("RD_CustomFinancialYearBy")]
        public IActionResult RD_CustomFinancialYear(CustomFinancialYearModel mod)
        {
            try
            {
                return Ok(srv.FetchCustomFinancialYear(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: CustomFinancialYear/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(CustomFinancialYearModel D)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3], D.Id);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);

                    return Ok(srv.SaveCustomFinancialYear(D));
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        // POST: CustomFinancialYear/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(CustomFinancialYearModel d)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.Id, d.InActive, Convert.ToInt32(User.Identity.Name));
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