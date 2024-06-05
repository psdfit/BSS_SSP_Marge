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
    [ApiController]
    [Route("api/[controller]")]
    public class TestingAgencyController : ControllerBase
    {
        private ISRVTestingAgency srv = null;

        public TestingAgencyController(ISRVTestingAgency srv)
        {
            this.srv = srv;
        }

        // GET: TestingAgency
        [HttpGet]
        [Route("GetTestingAgency")]
        public IActionResult GetTestingAgency()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchTestingAgency());

                ls.Add(new SRVCertificationCategory().FetchCertificationCategory(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TestingAgency
        [HttpGet]
        [Route("RD_TestingAgency")]
        public IActionResult RD_TestingAgency()
        {
            try
            {
                return Ok(srv.FetchTestingAgency(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TestingAgency
        [HttpPost]
        [Route("RD_TestingAgencyBy")]
        public IActionResult RD_TestingAgencyBy(TestingAgencyModel mod)
        {
            try
            {
                return Ok(srv.FetchTestingAgency(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TestingAgency/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TestingAgencyModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.TestingAgencyID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveTestingAgency(D));
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

        // POST: TestingAgency/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TestingAgencyModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.TestingAgencyID, d.InActive, Convert.ToInt32(User.Identity.Name));
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