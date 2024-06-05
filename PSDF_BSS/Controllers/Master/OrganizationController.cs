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
    public class OrganizationController : ControllerBase
    {
        private ISRVOrganization srv = null;

        public OrganizationController(ISRVOrganization srv)
        {
            this.srv = srv;
        }

        // GET: Organization
        [HttpGet]
        [Route("GetOrganization")]
        public IActionResult GetOrganization()
        {
            try
            {
                return Ok(srv.FetchOrganization());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // RD: Organization
        [HttpGet]
        [Route("RD_Organization")]
        public IActionResult RD_Organization()
        {
            try
            {
                return Ok(srv.FetchOrganization(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // RD: Organization
        [HttpPost]
        [Route("RD_OrganizationBy")]
        public IActionResult RD_OrganizationBy(OrganizationModel mod)
        {
            try
            {
                return Ok(srv.FetchOrganization(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // POST: Organization/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(OrganizationModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.OID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveOrganization(D));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        // POST: Organization/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(OrganizationModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.OID, d.InActive, Convert.ToInt32(User.Identity.Name));
                    return Ok(true);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
    }
}