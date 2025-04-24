using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OTRNController:ControllerBase 
    {
        private readonly ISRVOTRN srvOTRN;
        private readonly ISRVOTRNDetails srvOTRNDetails;
        private readonly ISRVUsers srvUsers;

        public OTRNController(ISRVOTRN srvOTRN, ISRVOTRNDetails srvOTRNDetails, ISRVUsers srvUsers)
        {
            this.srvOTRN = srvOTRN;
            this.srvOTRNDetails = srvOTRNDetails;
            this.srvUsers = srvUsers;
        }
        [HttpPost]
        [Route("GetOTRN")]
        public IActionResult GetOTRN(OTRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvOTRN.FetchOTRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetOTRNDetails/{OTRNID}")]
        public IActionResult GetOTRNDetails(int OTRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvOTRNDetails.FetchOTRNDetails(new OTRNDetailsModel() { OTRNID = OTRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetOTRNExcelExportByIDs")]
        public IActionResult GetOTRNExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvOTRNDetails.GetOTRNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }


        [HttpGet]
        [Route("GetOTRNDetailsFiltered/{OTRNID}")]
        public IActionResult GetOTRNDetailsFiltered(int OTRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvOTRNDetails.FetchOTRNDetailsFiltered(new OTRNDetailsModel() { OTRNID = OTRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdateOTRNDetails")]
        public IActionResult UpdateOTRNDetails(OTRNDetailsModel mod)
        {
            try
            {
                return Ok(srvOTRNDetails.UpdateOTRNDetails(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetOTRNExcelExport")]
        public IActionResult GetOTRNExcelExport(OTRNDetailsModel mod)
        {
            try
            {
                var k = srvOTRNDetails.GetOTRNExcelExport(mod);
                return Ok(k);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetVRN")]
        public IActionResult GetVRN(OTRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvOTRN.FetchVRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
