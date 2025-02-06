using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TPRNController : ControllerBase
    {
        private readonly ISRVTPRN srvTPRN;
        private readonly ISRVTPRNDetails srvTPRNDetails;
        private readonly ISRVUsers srvUsers;

        public TPRNController(ISRVTPRN srvTPRN, ISRVTPRNDetails srvTPRNDetails, ISRVUsers srvUsers)
        {
            this.srvTPRN = srvTPRN;
            this.srvTPRNDetails = srvTPRNDetails;
            this.srvUsers = srvUsers;
        }
        [HttpPost]
        [Route("GetTPRN")]
        public IActionResult GetTPRN(TPRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvTPRN.FetchTPRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetTPRNDetails/{TPRNID}")]
        public IActionResult GetTPRNDetails(int TPRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvTPRNDetails.FetchTPRNDetails(new TPRNDetailsModel() { TPRNID = TPRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetTPRNExcelExportByIDs")]
        public IActionResult GetTPRNExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvTPRNDetails.GetTPRNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }


        [HttpGet]
        [Route("GetTPRNDetailsFiltered/{TPRNID}")]
        public IActionResult GetTPRNDetailsFiltered(int TPRNID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvTPRNDetails.FetchTPRNDetailsFiltered(new TPRNDetailsModel() { TPRNID = TPRNID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdateTPRNDetails")]
        public IActionResult UpdateTPRNDetails(TPRNDetailsModel mod)
        {
            try
            {
                return Ok(srvTPRNDetails.UpdateTPRNDetails(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetTPRNExcelExport")]
        public IActionResult GetTPRNExcelExport(TPRNDetailsModel mod)
        {
            try
            {
                var k = srvTPRNDetails.GetTPRNExcelExport(mod);
                return Ok(k);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetVRN")]
        public IActionResult GetVRN(TPRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvTPRN.FetchVRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
