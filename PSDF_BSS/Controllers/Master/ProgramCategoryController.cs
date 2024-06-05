using DataLayer.Classes;
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
    public class ProgramCategoryController : ControllerBase
    {
        private ISRVProgramCategory srv = null;

        public ProgramCategoryController(ISRVProgramCategory srv)
        {
            this.srv = srv;
        }

        // GET: ProgramCategory
        [HttpGet]
        [Route("GetProgramCategory")]
        public IActionResult GetProgramCategory()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchProgramCategory());

                ls.Add(new SRVProgramType().FetchProgramType(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ProgramCategory
        [HttpGet]
        [Route("RD_ProgramCategory")]
        public IActionResult RD_ProgramCategory()
        {
            try
            {
                return Ok(srv.FetchProgramCategory(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ProgramCategory
        [HttpPost]
        [Route("RD_ProgramCategoryBy")]
        public IActionResult RD_ProgramCategoryBy(ProgramCategoryModel mod)
        {
            try
            {
                return Ok(srv.FetchProgramCategory(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ProgramCategory/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ProgramCategoryModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveProgramCategory(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ProgramCategory/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ProgramCategoryModel d)
        {
            

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.PCategoryID, d.InActive, Convert.ToInt32(User.Identity.Name));
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