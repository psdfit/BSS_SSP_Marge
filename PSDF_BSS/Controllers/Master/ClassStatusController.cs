using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using PSDF_BSS.Logging;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassStatusController : ControllerBase
    {
        ISRVClassStatus srv = null;
        public ClassStatusController(ISRVClassStatus srv)
        {
            this.srv = srv;
        }
        // GET: ClassStatus
        [HttpGet]
        [Route("GetClassStatus")]
        public IActionResult GetClassStatus()
        {
            try
            {

                return Ok(srv.FetchClassStatus());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: ClassReason
        [HttpGet]
        [Route("GetClassReason")]
        public IActionResult GetClassReason()
        {
            try
            {

                return Ok(srv.FetchClassReason());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: ClassStatus
        [HttpGet]
        [Route("RD_ClassStatus")]
        public IActionResult RD_ClassStatus()
        {
            try
            {
                return Ok(srv.FetchClassStatus(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: ClassStatus
        [HttpPost]
        [Route("RD_ClassStatusBy")]
        public IActionResult RD_ClassStatusBy(ClassStatusModel mod)
        {
            try
            {
                return Ok(srv.FetchClassStatus(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: ClassStatus/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ClassStatusModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveClassStatus(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ClassStatus/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ClassStatusModel d)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {       /**/
                    srv.ActiveInActive(d.ClassStatusID, d.InActive, Convert.ToInt32(User.Identity.Name));
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

