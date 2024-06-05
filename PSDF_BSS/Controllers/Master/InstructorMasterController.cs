using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorMasterController : ControllerBase
    {
        ISRVInstructorMaster srv = null;
        public InstructorMasterController(ISRVInstructorMaster srv)
        {
            this.srv = srv;
        }
        // GET: InstructorMaster
        [HttpGet]
        [Route("GetInstructorMaster")]
        public IActionResult GetInstructorMaster()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchInstructorMaster());

                ls.Add(new SRVGender().FetchGender(false));

                ls.Add(new SRVTrade().FetchTrade(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InstructorMaster
        [HttpGet]
        [Route("RD_InstructorMaster")]
        public IActionResult RD_InstructorMaster()
        {
            try
            {
                return Ok(srv.FetchInstructorMaster(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InstructorMaster
        [HttpPost]
        [Route("RD_InstructorMasterBy")]
        public IActionResult RD_InstructorMasterBy(InstructorMasterModel mod)
        {
            try
            {
                return Ok(srv.FetchInstructorMaster(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: InstructorMaster/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(InstructorMasterModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveInstructorMaster(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: InstructorMaster/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(InstructorMasterModel d)
        {
            try
            {
               
                srv.ActiveInActive(d.InstrMasterID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

