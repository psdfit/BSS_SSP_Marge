using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchemeChangeRequestController : ControllerBase
    {
        ISRVSchemeChangeRequest srv = null;
        private readonly ISRVScheme srvScheme;

        public SchemeChangeRequestController(ISRVSchemeChangeRequest srv, ISRVScheme srvScheme)
        {
            this.srv = srv;
            this.srvScheme = srvScheme;
        }
        // GET: SchemeChangeRequest
        [HttpGet]
        [Route("GetSchemeChangeRequest")]
        public IActionResult GetSchemeChangeRequest()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchSchemeChangeRequest());

                ls.Add(srvScheme.FetchScheme(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: SchemeChangeRequest
        [HttpGet]
        [Route("RD_SchemeChangeRequest")]
        public IActionResult RD_SchemeChangeRequest()
        {
            try
            {
                return Ok(srv.FetchSchemeChangeRequest(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: SchemeChangeRequest
        [HttpPost]
        [Route("RD_SchemeChangeRequestBy")]
        public IActionResult RD_SchemeChangeRequestBy(SchemeChangeRequestModel mod)
        {
            try
            {
                return Ok(srv.FetchSchemeChangeRequest(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: SchemeChangeRequest/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(SchemeChangeRequestModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                List<object> ls = new List<object>();
                ls.Add(srv.SaveSchemeChangeRequest(D));
                //srv.Update_CR_SchemeApprovalHistory(D);

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: SchemeChangeRequest/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(SchemeChangeRequestModel d)
        {
            try
            {
                srv.ActiveInActive(d.SchemeChangeRequestID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

