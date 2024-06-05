using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSSChangeRequest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorReplaceChangeRequestController : ControllerBase
    {
        ISRVInstructorReplaceChangeRequest srv = null;
        public InstructorReplaceChangeRequestController(ISRVInstructorReplaceChangeRequest srv)
        {
            this.srv = srv;
        }
        // GET: InstructorReplaceChangeRequest
        [HttpGet]
        [Route("GetInstructorReplaceChangeRequest")]
        public IActionResult GetInstructorReplaceChangeRequest()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchInstructorReplaceChangeRequest());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InstructorReplaceChangeRequest
        [HttpGet]
        [Route("RD_InstructorReplaceChangeRequest")]
        public IActionResult RD_InstructorReplaceChangeRequest()
        {
            try
            {
                return Ok(srv.FetchInstructorReplaceChangeRequest(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InstructorReplaceChangeRequest
        [HttpPost]
        [Route("RD_InstructorReplaceChangeRequestBy")]
        public IActionResult RD_InstructorReplaceChangeRequestBy(InstructorReplaceChangeRequestModel mod)
        {
            try
            {
                return Ok(srv.FetchInstructorReplaceChangeRequest(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        //// POST: InstructorReplaceChangeRequest/Save
        //[HttpPost]
        //[Route("Save")]
        //public IActionResult Save(InstructorReplaceChangeRequestModel D)
        //{
        //    try
        //    {
        //        D.CurUserID = Convert.ToInt32(User.Identity.Name);
        //        return Ok(srv.SaveInstructorReplaceChangeRequest(D));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        // POST: InstructorReplaceChangeRequest/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(InstructorReplaceChangeRequestModel d)
        {
            try
            {
                srv.ActiveInActive(d.InstructorReplaceChangeRequestID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

