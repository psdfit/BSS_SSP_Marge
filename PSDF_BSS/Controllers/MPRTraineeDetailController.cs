using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSSd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MPRTraineeDetailController : ControllerBase
    {
        private readonly ISRVMPRTraineeDetail srvMPRTraineeDetail;
        private readonly ISRVTraineeProfile srvTraineeProfile;
        private readonly ISRVMPR srvSRVMPR;
        public MPRTraineeDetailController(ISRVMPRTraineeDetail srvMPRTraineeDetail, ISRVTraineeProfile srvTraineeProfile, ISRVMPR srvSRVMPR)
        {
            this.srvMPRTraineeDetail = srvMPRTraineeDetail;
            this.srvTraineeProfile = srvTraineeProfile;
            this.srvSRVMPR = srvSRVMPR;
        }
        // GET: MPRTraineeDetail
        [HttpGet]
        [Route("GetMPRTraineeDetail")]
        public IActionResult GetMPRTraineeDetail()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvMPRTraineeDetail.FetchMPRTraineeDetail());

                ls.Add(srvSRVMPR.FetchMPR(false));

                ls.Add(srvTraineeProfile.FetchTraineeProfile(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: MPRTraineeDetail
        [HttpGet]
        [Route("RD_MPRTraineeDetail")]
        public IActionResult RD_MPRTraineeDetail()
        {
            try
            {
                return Ok(srvMPRTraineeDetail.FetchMPRTraineeDetail(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: MPRTraineeDetail
        [HttpPost]
        [Route("RD_MPRTraineeDetailBy")]
        public IActionResult RD_MPRTraineeDetailBy(MPRTraineeDetailModel mod)
        {
            try
            {
                return Ok(srvMPRTraineeDetail.FetchMPRTraineeDetail(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: MPRTraineeDetail/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(MPRTraineeDetailModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvMPRTraineeDetail.SaveMPRTraineeDetail(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: MPRTraineeDetail/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(MPRTraineeDetailModel d)
        {
            try
            {

                srvMPRTraineeDetail.ActiveInActive(d.MPRDetailID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

