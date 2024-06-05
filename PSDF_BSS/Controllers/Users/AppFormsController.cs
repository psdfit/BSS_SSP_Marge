using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PSDF_BSS.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppFormsController : ControllerBase
    {
        private readonly ISRVAppForms srvAppForms ;
        private readonly ISRVModules srvSRVModules ;

        public AppFormsController(ISRVAppForms srvAppForms, ISRVModules srvSRVModules)
        {
            this.srvAppForms = srvAppForms;
            this.srvSRVModules = srvSRVModules;
        }

        // GET: AppForms
        [HttpGet]
        [Route("GetAppForms")]
        public IActionResult GetAppForms()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvAppForms.FetchAppForms());

                ls.Add(srvSRVModules.FetchModules(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: AppForms
        [HttpGet]
        [Route("RD_AppForms")]
        public IActionResult RD_AppForms()
        {
            try
            {
                return Ok(srvAppForms.FetchAppForms(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: AppForms
        [HttpPost]
        [Route("RD_AppFormsBy")]
        public IActionResult RD_AppFormsBy(AppFormsModel mod)
        {
            try
            {
                return Ok(srvAppForms.FetchAppForms(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: AppForms/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(AppFormsModel D)
        {
            try
            {
                return Ok(srvAppForms.SaveAppForms(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: AppForms/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(AppFormsModel d)
        {
            try
            {
                srvAppForms.ActiveInActive(d.FormID, d.InActive, 1);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}