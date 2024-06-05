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
    public class CertificationCategoryController : ControllerBase
    {
        private ISRVCertificationCategory srv = null;

        public CertificationCategoryController(ISRVCertificationCategory srv)
        {
            this.srv = srv;
        }

        // GET: CertificationCategory
        [HttpGet]
        [Route("GetCertificationCategory")]
        public IActionResult GetCertificationCategory()
        {
            try
            {
                return Ok(srv.FetchCertificationCategory());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: CertificationCategory
        [HttpGet]
        [Route("RD_CertificationCategory")]
        public IActionResult RD_CertificationCategory()
        {
            try
            {
                return Ok(srv.FetchCertificationCategory(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: CertificationCategory
        [HttpPost]
        [Route("RD_CertificationCategoryBy")]
        public IActionResult RD_CertificationCategoryBy(CertificationCategoryModel mod)
        {
            try
            {
                return Ok(srv.FetchCertificationCategory(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: CertificationCategory/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(CertificationCategoryModel D)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3], D.CertificationCategoryID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveCertificationCategory(D));
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

        // POST: CertificationCategory/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(CertificationCategoryModel d)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.CertificationCategoryID, d.InActive, Convert.ToInt32(User.Identity.Name));
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