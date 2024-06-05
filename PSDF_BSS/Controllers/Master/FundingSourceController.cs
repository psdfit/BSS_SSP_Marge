/* **** Aamer Rehman Malik *****/

using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundingSourceController : ControllerBase
    {
        private ISRVFundingSource srv = null;

        public FundingSourceController(ISRVFundingSource srv)
        {
            this.srv = srv;
        }

        // GET: FundingSource
        [HttpGet]
        [Route("GetFundingSource")]
        public IActionResult GetFundingSource()
        {
            try
            {
                return Ok(srv.FetchFundingSource());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: FundingSource
        [HttpGet]
        [Route("RD_FundingSource")]
        public IActionResult RD_FundingSource()
        {
            try
            {
                return Ok(srv.FetchFundingSource(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: FundingSource
        [HttpPost]
        [Route("RD_FundingSourceBy")]
        public IActionResult RD_FundingSourceBy(FundingSourceModel mod)
        {
            try
            {
                return Ok(srv.FetchFundingSource(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: FundingSource/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(FundingSourceModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.FundingSourceID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveFundingSource(D));
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

        // CheckName
        [HttpPost]
        [Route("CheckName")]
        public IActionResult CheckName(FundingSourceModel mod)
        {
            if (!String.IsNullOrEmpty(mod.FundingSourceName))
            {
                int ID = mod.FundingSourceID;
                mod.FundingSourceID = 0;
                List<FundingSourceModel> u = srv.FetchFundingSource(mod);
                if (u == null || u.Count == 0)
                {
                    return Ok(true);
                }
                else
                {
                    if (u.Count > 1)
                        return Ok(false);
                    else
                    {
                        if (u[0].FundingSourceID == ID)
                        {
                            return Ok(true);
                        }
                        else
                            return Ok(false);
                    }
                }
            }
            else
                return BadRequest("Bad Request");
        }

        // POST: FundingSource/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(FundingSourceModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.FundingSourceID, d.InActive, Convert.ToInt32(User.Identity.Name));
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