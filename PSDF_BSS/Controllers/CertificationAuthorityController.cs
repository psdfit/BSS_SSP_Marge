/* **** Aamer Rehman Malik *****/

using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Interfaces;
using PSDF_BSS.Logging;

namespace PSDF_BSSMasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificationAuthorityController : ControllerBase
    {
        private readonly ISRVCertificationAuthority srvCertificationAuthority = null;
        private readonly ISRVCertificationCategory srvSRVCertificationCategory = null;

        public CertificationAuthorityController(ISRVCertificationAuthority srvCertificationAuthority, ISRVCertificationCategory srvSRVCertificationCategory)
        {
            this.srvCertificationAuthority = srvCertificationAuthority;
            this.srvSRVCertificationCategory = srvSRVCertificationCategory;
        }

        // GET: CertificationAuthority
        [HttpGet]
        [Route("GetCertificationAuthority")]
        public IActionResult GetCertificationAuthority()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvCertificationAuthority.FetchCertificationAuthority());

                ls.Add(srvSRVCertificationCategory.FetchCertificationCategory(false));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: CertificationAuthority
        [HttpGet]
        [Route("RD_CertificationAuthority")]
        public IActionResult RD_CertificationAuthority()
        {
            try
            {
                return Ok(srvCertificationAuthority.FetchCertificationAuthority(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: CertificationAuthority
        [HttpPost]
        [Route("RD_CertificationAuthorityBy")]
        public IActionResult RD_CertificationAuthorityBy(CertificationAuthorityModel mod)
        {
            try
            {
                return Ok(srvCertificationAuthority.FetchCertificationAuthority(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: CertificationAuthority/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(CertificationAuthorityModel D)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3], D.CertificationCategoryID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvCertificationAuthority.SaveCertificationAuthority(D));
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

        // POST: CertificationAuthority/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(CertificationAuthorityModel d)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srvCertificationAuthority.ActiveInActive(d.CertAuthID, d.InActive, Convert.ToInt32(User.Identity.Name));
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