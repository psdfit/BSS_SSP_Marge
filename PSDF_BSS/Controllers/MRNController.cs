using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MRNController : ControllerBase
    {
        private readonly ISRVMRN srvMRN;
        private readonly ISRVMRNDetails srvMRNDetails;
        private readonly ISRVUsers srvUsers;

        public MRNController(ISRVMRN srvMRN, ISRVMRNDetails srvMRNDetails, ISRVUsers srvUsers)
        {
            this.srvMRN = srvMRN;
            this.srvMRNDetails = srvMRNDetails;
            this.srvUsers = srvUsers;
        }
        [HttpPost]
        [Route("GetMRN")]
        public IActionResult GetMRN(MRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvMRN.FetchMRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetMRNDetails/{MRNID}")]
        public IActionResult GetMRNDetails(int MRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvMRNDetails.FetchMRNDetails(new MRNDetailsModel() { MRNID = MRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetMRNExcelExportByIDs")]
        public IActionResult GetMRNExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvMRNDetails.GetMRNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }


        [HttpGet]
        [Route("GetMRNDetailsFiltered/{MRNID}")]
        public IActionResult GetMRNDetailsFiltered(int MRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvMRNDetails.FetchMRNDetailsFiltered(new MRNDetailsModel() { MRNID = MRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdateMRNDetails")]
        public IActionResult UpdateMRNDetails(MRNDetailsModel mod)
        {
            try
            {
                return Ok(srvMRNDetails.UpdateMRNDetails(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetMRNExcelExport")]
        public IActionResult GetMRNExcelExport(MRNDetailsModel mod)
        {
            try
            {
                var k = srvMRNDetails.GetMRNExcelExport(mod);
                return Ok(k);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetVRN")]
        public IActionResult GetVRN(MRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvMRN.FetchVRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
