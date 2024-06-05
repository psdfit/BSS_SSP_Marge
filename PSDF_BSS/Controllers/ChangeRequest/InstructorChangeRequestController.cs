using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorChangeRequestController : ControllerBase
    {
        ISRVInstructorChangeRequest srv = null;
        ISRVInstructor srvInstructor;
        public InstructorChangeRequestController(ISRVInstructorChangeRequest srv, ISRVInstructor srvInstructor)
        {
            this.srv = srv;
            this.srvInstructor = srvInstructor;
        }

        // GET: InstructorChangeRequest
        [HttpGet]
        [Route("GetInstructorChangeRequest")]
        public IActionResult GetInstructorChangeRequest()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchInstructorChangeRequest());

                //ls.Add(new SRVInstructor().FetchInstructor(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        
        // GET: InstructorChangeRequest
        [HttpPost]
        [Route("GeFilteredtInstructorChangeRequest")]
        public IActionResult GeFilteredtInstructorChangeRequest(QueryFilters filters)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchFilteredInstructorChangeRequest(filters));

                //ls.Add(new SRVInstructor().FetchInstructor(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        // GET: NEw InstructorChangeRequest
        [HttpGet]
        [Route("GetNewInstructorRequest")]
        public IActionResult GetNewInstructorRequest()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchNewInstructorRequest());

                //ls.Add(new SRVInstructor().FetchInstructor(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        // GET: NEw InstructorChangeRequest
        [HttpPost]
        [Route("GetFilteredNewInstructorRequest")]
        public IActionResult GetFilteredNewInstructorRequest(QueryFilters filters)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchFilteredNewInstructorRequest(filters));

                //ls.Add(new SRVInstructor().FetchInstructor(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetNewInstructorRequestAttachments")]
        public IActionResult GetNewInstructorRequestAttachments(InstructorChangeRequestModel mod)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchNewInstructorRequestAttachments(mod.CRNewInstructorID));

                //ls.Add(new SRVInstructor().FetchInstructor(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InstructorChangeRequest
        [HttpGet]
        [Route("RD_InstructorChangeRequest")]
        public IActionResult RD_InstructorChangeRequest()
        {
            try
            {
                return Ok(srv.FetchInstructorChangeRequest(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InstructorChangeRequest
        [HttpPost]
        [Route("RD_InstructorChangeRequestBy")]
        public IActionResult RD_InstructorChangeRequestBy(InstructorChangeRequestModel mod)
        {
            try
            {
                return Ok(srv.FetchInstructorChangeRequest(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: InstructorChangeRequest/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(InstructorChangeRequestModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.InstrID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveInstructorChangeRequest(D));
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

        // POST: InstructorChangeRequest/SaveNewInstructor
        [HttpPost]
        [Route("SaveNewInstructor")]
        public IActionResult SaveNewInstructor(InstructorChangeRequestModel D)
        {

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.CRNewInstructorID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);

                    if (!String.IsNullOrEmpty(D.FilePath))
                    {
                        var path = "\\Documents\\InstructorFiles\\";

                        var fileName = Common.AddFile(D.FilePath, path);

                        D.FileName = fileName;
                        D.FilePath = fileName;
                    }

                    //srv.SaveInstructorByCR(D);
                    return Ok(srv.SaveInstructorByCR(D));
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

        // POST: InstructorChangeRequest/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(InstructorChangeRequestModel d)
        {
            try
            {
                srv.ActiveInActive(d.InstructorChangeRequestID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

