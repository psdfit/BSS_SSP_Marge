using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PCRNController:ControllerBase 
    {
        private readonly ISRVPCRN srvPCRN;
        private readonly ISRVPCRNDetails srvPCRNDetails;
        private readonly ISRVUsers srvUsers;

        public PCRNController(ISRVPCRN srvPCRN, ISRVPCRNDetails srvPCRNDetails, ISRVUsers srvUsers)
        {
            this.srvPCRN = srvPCRN;
            this.srvPCRNDetails = srvPCRNDetails;
            this.srvUsers = srvUsers;
        }
        [HttpPost]
        [Route("GetPCRN")]
        public IActionResult GetPCRN(PCRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvPCRN.FetchPCRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetPCRNDetails/{PCRNID}")]
        public IActionResult GetPCRNDetails(int PCRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPCRNDetails.FetchPCRNDetails(new PCRNDetailsModel() { PCRNID = PCRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetPCRNExcelExportByIDs")]
        public IActionResult GetPCRNExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvPCRNDetails.GetPCRNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }


        [HttpGet]
        [Route("GetPCRNDetailsFiltered/{PCRNID}")]
        public IActionResult GetPCRNDetailsFiltered(int PCRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPCRNDetails.FetchPCRNDetailsFiltered(new PCRNDetailsModel() { PCRNID = PCRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdatePCRNDetails")]
        public IActionResult UpdatePCRNDetails(PCRNDetailsModel mod)
        {
            try
            {
                return Ok(srvPCRNDetails.UpdatePCRNDetails(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetPCRNExcelExport")]
        public IActionResult GetPCRNExcelExport(PCRNDetailsModel mod)
        {
            try
            {
                var k = srvPCRNDetails.GetPCRNExcelExport(mod);
                return Ok(k);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetVRN")]
        public IActionResult GetVRN(PCRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvPCRN.FetchVRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
