/* **** Aamer Rehman Malik *****/

using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PSDF_BSSd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MPRController : ControllerBase
    {
        private readonly ISRVMPR srvMPR;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVMPRTraineeDetail srvSRVMPRTraineeDetail;
        private readonly ISRVUsers srvUsers;



        public MPRController(ISRVMPR srvMPR, ISRVScheme srvScheme, ISRVMPRTraineeDetail srvSRVMPRTraineeDetail, ISRVUsers srvUsers)
        {
            this.srvMPR = srvMPR;
            this.srvScheme = srvScheme;
            this.srvSRVMPRTraineeDetail = srvSRVMPRTraineeDetail;
            this.srvUsers = srvUsers;
        }

        // GET: MPR
        [HttpGet]
        [Route("GetMPR")]
        public IActionResult GetMPR()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvMPR.FetchMPR());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("MPRTraineeDetail/{id}")]
        public IActionResult MPRTraineeDetail(int id)
        {
            try
            {
                return Ok(srvSRVMPRTraineeDetail.GetByMPRID(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetMPRExcelExportByIDs")]
        public IActionResult GetMPRExcelExportByIDs([FromBody]string ids)
        {
            try
            {
                return Ok(srvSRVMPRTraineeDetail.GetMPRExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        // RD: MPR
        [HttpGet]
        [Route("RD_MPR")]
        public IActionResult RD_MPR()
        {
            try
            {
                return Ok(srvMPR.FetchMPR(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: MPR
        [HttpPost]
        [Route("RD_MPRBy")]
        public IActionResult RD_MPRBy(MPRModel mod)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;
                Dictionary<string, object> list = new Dictionary<string, object>();

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    mod.UserID = curUserID;
                    list.Add("Schemes", srvScheme.FetchSchemesByTSPUser(mod.UserID));
                    list.Add("MPR", srvMPR.FetchMPR(mod));
                }
                else if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.KAM)))
                {
                    mod.UserID = curUserID;
                    list.Add("Schemes", srvScheme.FetchSchemesByKAMUser(mod.UserID));
                    list.Add("MPR", srvMPR.FetchMPRByKAM(mod));
                }
                else
                {
                    list.Add("Schemes", srvScheme.FetchSchemesByTSPUser(mod.UserID));
                    list.Add("MPR", srvMPR.FetchMPR(mod));
                }

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetClassMonthview")]
        public IActionResult GetClassMonthview(MPRModel mod)
        {
            try
            {
                return Ok(srvMPR.GetClassMonthview(mod.ClassID, mod.Month, mod.Batch));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: MPR/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(MPRModel D)
        {
            try
            {
                return Ok(srvMPR.SaveMPR(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: MPR/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(MPRModel d)
        {
            try
            {
                srvMPR.ActiveInActive(d.MPRID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}