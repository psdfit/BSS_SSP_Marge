using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSSTraineeReports.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SRNController : ControllerBase
    {
        private readonly ISRVSRN srvSRN;
        private readonly ISRVSRNDetails srvSRNDetails;
        private readonly ISRVUsers srvUsers;

        public SRNController(ISRVSRN srvSRN, ISRVSRNDetails srvSRNDetails, ISRVUsers srvUsers)
        {
            this.srvSRN = srvSRN;
            this.srvSRNDetails = srvSRNDetails;
            this.srvUsers = srvUsers;
        }
        [HttpPost]
        [Route("GetSRN")]
        public IActionResult GetSRN(SRNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvSRN.FetchSRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetSRNDetails/{srnID}")]
        public IActionResult GetSRNDetails(int srnID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvSRNDetails.FetchSRNDetails(new SRNDetailsModel() { SRNID = srnID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetSRNExcelExportByIDs")]
        public IActionResult GetSRNExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvSRNDetails.GetSRNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }


        [HttpGet]
        [Route("GetSRNDetailsFiltered/{srnID}")]
        public IActionResult GetSRNDetailsFiltered(int srnID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvSRNDetails.FetchSRNDetailsFiltered(new SRNDetailsModel() { SRNID = srnID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdateSRNDetails")]
        public IActionResult UpdateSRNDetails(SRNDetailsModel mod)
        {
            try
            {
                return Ok(srvSRNDetails.UpdateSRNDetails(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetSRNExcelExport")]
        public IActionResult GetSRNExcelExport(SRNDetailsModel mod)
        {
            try
            {
                var k = srvSRNDetails.GetSRNExcelExport(mod);
                return Ok(k);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

