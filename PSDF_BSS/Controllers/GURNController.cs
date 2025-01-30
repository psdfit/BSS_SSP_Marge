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
    public class GURNController : ControllerBase
    {
        private readonly ISRVGURN srvGURN;
        private readonly ISRVGURNDetails srvGURNDetails;
        private readonly ISRVUsers srvUsers;

        public GURNController(ISRVGURN srvGURN, ISRVGURNDetails srvGURNDetails, ISRVUsers srvUsers)
        {
            this.srvGURN = srvGURN;
            this.srvGURNDetails = srvGURNDetails;
            this.srvUsers = srvUsers;
        }
        [HttpPost]
        [Route("GetGURN")]
        public IActionResult GetGURN(GURNModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvGURN.FetchGURN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetGURNDetails/{gurnID}")]
        public IActionResult GetGURNDetails(int gurnID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvGURNDetails.FetchGURNDetails(new GURNDetailsModel() { GURNID = gurnID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetGURNExcelExportByIDs")]
        public IActionResult GetGURNExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvGURNDetails.GetGURNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }


        [HttpGet]
        [Route("GetGURNDetailsFiltered/{gurnID}")]
        public IActionResult GetGURNDetailsFiltered(int gurnID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvGURNDetails.FetchGURNDetailsFiltered(new GURNDetailsModel() { GURNID = gurnID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdateGURNDetails")]
        public IActionResult UpdateGURNDetails(GURNDetailsModel mod)
        {
            try
            {
                return Ok(srvGURNDetails.UpdateGURNDetails(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetGURNExcelExport")]
        public IActionResult GetGURNExcelExport(GURNDetailsModel mod)
        {
            try
            {
                var k = srvGURNDetails.GetGURNExcelExport(mod);
                return Ok(k);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpPost]
        //[Route("GetVRN")]
        //public IActionResult GetVRN(GURNModel model)
        //{
        //    try
        //    {
        //        int curUserID = Convert.ToInt32(User.Identity.Name);
        //        int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

        //        if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
        //        {
        //            model.UserID = curUserID;
        //        }

        //        return Ok(srvGURN.FetchVRN(model));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
    }
}

