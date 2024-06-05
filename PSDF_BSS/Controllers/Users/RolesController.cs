using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace PSDF_BSS.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly  ISRVRoles srvRoles ;
        private readonly ISRVAppForms srvSRVAppForms;
       

        public RolesController(ISRVRoles srvRoles, ISRVAppForms srvSRVAppForms)
        {
            this.srvRoles = srvRoles;
            this.srvSRVAppForms = srvSRVAppForms;
            
        }

        // GET: Roles
        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetRoles()
        {
            try
            {
                List<object> ls = new List<object>
                {
                    srvRoles.FetchRoles(),

                    srvSRVAppForms.FetchAppForms(false)
                };
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Roles
        [HttpGet]
        [Route("RD_Roles")]
        public IActionResult RD_Roles()
        {
            try
            {
                return Ok(srvRoles.FetchRoles(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Roles
        [HttpPost]
        [Route("RD_RolesBy")]
        public IActionResult RD_RolesBy(RolesModel mod)
        {
            try
            {
                return Ok(srvRoles.FetchRoles(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Roles/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(RolesModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.RoleID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvRoles.SaveRoles(D));
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

        [HttpGet]
        [Route("GetRoleRights/{id}")]
        public ActionResult GetRoleRights(int id)
        {
            try
            {
                return Ok(new SRVRolesRights().GetByRoleID(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Roles/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(RolesModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {

                    srvRoles.ActiveInActive(d.RoleID, d.InActive, Convert.ToInt32(User.Identity.Name));
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