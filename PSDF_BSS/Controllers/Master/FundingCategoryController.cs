/* **** Aamer Rehman Malik *****/

using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundingCategoryController : ControllerBase
    {
        private ISRVFundingCategory srv = null;

        public FundingCategoryController(ISRVFundingCategory srv)
        {
            this.srv = srv;
        }

        // GET: FundingCategory
        [HttpGet]
        [Route("GetFundingCategory")]
        public IActionResult GetFundingCategory()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchFundingCategory());

                ls.Add(new SRVFundingSource().FetchFundingSource(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: FundingCategory
        [HttpGet]
        [Route("RD_FundingCategory")]
        public IActionResult RD_FundingCategory()
        {
            try
            {
                return Ok(srv.FetchFundingCategory(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: FundingCategory
        [HttpPost]
        [Route("RD_FundingCategoryBy")]
        public IActionResult RD_FundingCategoryBy(FundingCategoryModel mod)
        {
            try
            {
                return Ok(srv.FetchFundingCategory(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: FundingCategory/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(FundingCategoryModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.FundingCategoryID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveFundingCategory(D));
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
        public IActionResult CheckName(FundingCategoryModel mod)
        {
            if (!String.IsNullOrEmpty(mod.FundingCategoryName))
            {
                int ID = mod.FundingCategoryID;
                mod.FundingCategoryID = 0;
                List<FundingCategoryModel> u = srv.FetchFundingCategory(mod);
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
                        if (u[0].FundingCategoryID == ID)
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

        // POST: FundingCategory/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(FundingCategoryModel d)
        {
           

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.FundingCategoryID, d.InActive, Convert.ToInt32(User.Identity.Name));
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