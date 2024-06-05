using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmploymentStatusController : ControllerBase
    {
        private ISRVEmploymentStatus srv = null;

        public EmploymentStatusController(ISRVEmploymentStatus srv)
        {
            this.srv = srv;
        }

        // GET: EmploymentStatus
        [HttpGet]
        [Route("GetEmploymentStatus")]
        public IActionResult GetEmploymentStatus()
        {
            try
            {
                return Ok(srv.FetchEmploymentStatus());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: EmploymentStatus
        [HttpGet]
        [Route("RD_EmploymentStatus")]
        public IActionResult RD_EmploymentStatus()
        {
            try
            {
                return Ok(srv.FetchEmploymentStatus(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: EmploymentStatus
        [HttpPost]
        [Route("RD_EmploymentStatusBy")]
        public IActionResult RD_EmploymentStatusBy(EmploymentStatusModel mod)
        {
            try
            {
                return Ok(srv.FetchEmploymentStatus(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EmploymentStatus/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(EmploymentStatusModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveEmploymentStatus(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EmploymentStatus/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(EmploymentStatusModel d)
        {
            
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.EmploymentStatusID, d.InActive, Convert.ToInt32(User.Identity.Name));
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