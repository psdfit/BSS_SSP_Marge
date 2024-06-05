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
    public class ProgramTypeController : ControllerBase
    {
        private ISRVProgramType srv = null;

        public ProgramTypeController(ISRVProgramType srv)
        {
            this.srv = srv;
        }

        // GET: ProgramType
        [HttpGet]
        [Route("GetProgramType")]
        public IActionResult GetProgramType()
        {
            try
            {
                return Ok(srv.FetchProgramType());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ProgramType
        [HttpGet]
        [Route("RD_ProgramType")]
        public IActionResult RD_ProgramType()
        {
            try
            {
                return Ok(srv.FetchProgramType(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ProgramType
        [HttpPost]
        [Route("RD_ProgramTypeBy")]
        public IActionResult RD_ProgramTypeBy(ProgramTypeModel mod)
        {
            try
            {
                return Ok(srv.FetchProgramType(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ProgramType/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ProgramTypeModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveProgramType(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ProgramType/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ProgramTypeModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.PTypeID, d.InActive, Convert.ToInt32(User.Identity.Name));
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