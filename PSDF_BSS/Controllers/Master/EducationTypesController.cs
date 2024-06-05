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
    public class EducationTypesController : ControllerBase
    {
        private ISRVEducationTypes srv = null;

        public EducationTypesController(ISRVEducationTypes srv)
        {
            this.srv = srv;
        }

        // GET: EducationTypes
        [HttpGet]
        [Route("GetEducationTypes")]
        public IActionResult GetEducationTypes()
        {
            try
            {
                return Ok(srv.FetchEducationTypes());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: EducationTypes
        [HttpGet]
        [Route("RD_EducationTypes")]
        public IActionResult RD_EducationTypes()
        {
            try
            {
                return Ok(srv.FetchEducationTypes(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: EducationTypes
        [HttpPost]
        [Route("RD_EducationTypesBy")]
        public IActionResult RD_EducationTypesBy(EducationTypesModel mod)
        {
            try
            {
                return Ok(srv.FetchEducationTypes(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EducationTypes/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(EducationTypesModel D)
        {
            


            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.EducationTypeID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveEducationTypes(D));
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

        // POST: EducationTypes/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(EducationTypesModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.EducationTypeID, d.InActive, Convert.ToInt32(User.Identity.Name));
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