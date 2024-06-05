using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApprovalProcessController : ControllerBase
    {
        private ISRVApprovalProcess srv = null;

        public ApprovalProcessController(ISRVApprovalProcess srv)
        {
            this.srv = srv;
        }

        // GET: ApprovalProcess
        [HttpGet]
        [Route("GetApprovalProcess")]
        public IActionResult GetApprovalProcess()
        {
            try
            {
                return Ok(srv.FetchApprovalProcess());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ApprovalProcess
        [HttpGet]
        [Route("RD_ApprovalProcess")]
        public IActionResult RD_ApprovalProcess()
        {
            try
            {
                return Ok(srv.FetchApprovalProcess(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ApprovalProcess
        [HttpPost]
        [Route("RD_ApprovalProcessBy")]
        public IActionResult RD_ApprovalProcessBy(ApprovalProcessModel mod)
        {
            try
            {
                return Ok(srv.FetchApprovalProcess(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ApprovalProcess/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ApprovalProcessModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveApprovalProcess(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ApprovalProcess/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ApprovalProcessModel d)
        {
            try
            {
                
                srv.ActiveInActive(d.ProcessKey, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}