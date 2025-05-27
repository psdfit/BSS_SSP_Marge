using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PVRNController : ControllerBase
    {
        private readonly ISRVPVRN srvPVRN;
        private readonly ISRVPVRNDetails srvPVRNDetails;
        private readonly ISRVUsers srvUsers;

        public PVRNController(ISRVPVRN srvPVRN, ISRVPVRNDetails srvPVRNDetails, ISRVUsers srvUsers)
        {
            this.srvPVRN = srvPVRN;
            this.srvPVRNDetails = srvPVRNDetails;
            this.srvUsers = srvUsers;
        }
        [HttpPost]
        [Route("GetPVRN")]
        public IActionResult GetPVRN(PVRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvPVRN.FetchPVRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetPVRNDetails/{PVRNID}")]
        public IActionResult GetPVRNDetails(int PVRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPVRNDetails.FetchPVRNDetails(new PVRNDetailsModel() { PVRNID = PVRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetPVRNExcelExportByIDs")]
        public IActionResult GetPVRNExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvPVRNDetails.GetPVRNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }


        [HttpGet]
        [Route("GetPVRNDetailsFiltered/{PVRNID}")]
        public IActionResult GetPVRNDetailsFiltered(int PVRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPVRNDetails.FetchPVRNDetailsFiltered(new PVRNDetailsModel() { PVRNID = PVRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdatePVRNDetails")]
        public IActionResult UpdatePVRNDetails(PVRNDetailsModel mod)
        {
            try
            {
                return Ok(srvPVRNDetails.UpdatePVRNDetails(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetPVRNExcelExport")]
        public IActionResult GetPVRNExcelExport(PVRNDetailsModel mod)
        {
            try
            {
                var k = srvPVRNDetails.GetPVRNExcelExport(mod);
                return Ok(k);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetVRN")]
        public IActionResult GetVRN(PVRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvPVRN.FetchVRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
