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
    public class SourceOfCurriculumController : ControllerBase
    {
        ISRVSourceOfCurriculum srv = null;
        ISRVCertificationAuthority srvCertAuth = null;
        public SourceOfCurriculumController(ISRVSourceOfCurriculum srv, ISRVCertificationAuthority srvCertAuth)
        {
            this.srv = srv;
            this.srvCertAuth = srvCertAuth;
        }
        // GET: SourceOfCurriculum
        [HttpGet]
        [Route("GetSourceOfCurriculum")]
        public async Task<IActionResult> GetSourceOfCurriculum()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchSourceOfCurriculum());

                ls.Add(srvCertAuth.FetchCertificationAuthority(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: SourceOfCurriculum
        [HttpGet]
        [Route("RD_SourceOfCurriculum")]
        public async Task<IActionResult> RD_SourceOfCurriculum()
        {
            try
            {
                return Ok(srv.FetchSourceOfCurriculum(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: SourceOfCurriculum
        [HttpPost]
        [Route("RD_SourceOfCurriculumBy")]
        public async Task<IActionResult> RD_SourceOfCurriculumBy(SourceOfCurriculumModel mod)
        {
            try
            {
                return Ok(srv.FetchSourceOfCurriculum(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: SourceOfCurriculum/Save
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SourceOfCurriculumModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.SourceOfCurriculumID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveSourceOfCurriculum(D));
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

        // POST: SourceOfCurriculum/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<IActionResult> ActiveInActive(SourceOfCurriculumModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.SourceOfCurriculumID, d.InActive, Convert.ToInt32(User.Identity.Name));
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

