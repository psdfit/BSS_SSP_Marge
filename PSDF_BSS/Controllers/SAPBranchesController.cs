using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSSMasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SAPBranchesController : ControllerBase
    {
        private readonly ISRVSAPBranches srvSAPBranches;
        public SAPBranchesController(ISRVSAPBranches srvSAPBranches)
        {
            this.srvSAPBranches = srvSAPBranches;
        }
        // GET: SAPBranches
        [HttpGet]
        [Route("GetSAPBranches")]
        public IActionResult GetSAPBranches()
        {
            try
            {

                return Ok(srvSAPBranches.FetchSAPBranches());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: SAPBranches
        [HttpGet]
        [Route("RD_SAPBranches")]
        public IActionResult RD_SAPBranches()
        {
            try
            {
                return Ok(srvSAPBranches.FetchSAPBranches(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: SAPBranches
        [HttpPost]
        [Route("RD_SAPBranchesBy")]
        public IActionResult RD_SAPBranchesBy(SAPBranchesModel mod)
        {
            try
            {
                return Ok(srvSAPBranches.FetchSAPBranches(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: SAPBranches/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(SAPBranchesModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvSAPBranches.SaveSAPBranches(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: SAPBranches/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(SAPBranchesModel d)
        {
            try
            {       //UsersModel u = Common.GetUserFromRequest(Request);
                    //srv.ActiveInActive(d.Id,d.InActive, u.UserID);
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

