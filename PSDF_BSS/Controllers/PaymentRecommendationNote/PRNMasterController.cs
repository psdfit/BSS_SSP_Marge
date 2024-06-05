using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.Controllers.PaymentRecommendationNote
{
    [Route("api/[controller]")]
    [ApiController]
    public class PRNMasterController : ControllerBase
    {
        private readonly ISRVPRNMaster srvPRNMaster;
        private ISRVUsers srvUsers;
        private ISRVScheme srvScheme;
        private ISRVTSPDetail srvTSPDetail;
        private readonly ISRVTSPMaster srvTSPMaster;
        private ISRVKAMAssignment srvKam;
        private readonly ISRVDashboard srvDashboard;

        public PRNMasterController(ISRVPRNMaster srvPRNMaster, ISRVUsers srvUsers, ISRVScheme srvScheme, ISRVTSPDetail srvTSPDetail, ISRVTSPMaster srvTSPMaster, ISRVKAMAssignment srvKam, ISRVDashboard srvDashboard)
        {
            this.srvPRNMaster = srvPRNMaster;
            this.srvUsers = srvUsers;
            this.srvScheme = srvScheme;
            this.srvTSPDetail = srvTSPDetail;
            this.srvTSPMaster = srvTSPMaster;
            this.srvKam = srvKam;
            this.srvDashboard = srvDashboard;
        }

        //[HttpGet]
        //[Route("GetPRNMasterForApproval")]
        //public IActionResult GetPRNMasterForApproval()
        //{
        //    try
        //    {
        //        return Ok(srv.GetPRNMasterForApproval(Convert.ToInt32(User.Identity.Name)));
        //    }
        //    catch (Exception e)
        //    { return BadRequest(e.InnerException.ToString()); }
        //}


        [HttpGet]
        [Route("GetFiltersData")]      //Filters for KAM , Scheme and TSP
        public IActionResult GetFiltersData()
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;


                List<object> ls = new List<object>();
                ls.Add(srvKam.FetchKAMAssignmentForFilters(false));
                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    ls.Add(srvDashboard.FetchSchemesByUsers(curUserID));
                }
                else
                {
                    ls.Add(srvDashboard.FetchSchemes());
                }
                //ls.Add(srvDashboard.FetchTSPDetail());
                ls.Add(srvTSPMaster.FetchTSPMaster(false));

                return Ok(ls);
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("GetPRNMasterForApproval")]
        public IActionResult GetPRNMasterForApproval([FromBody] PRNMasterModel m)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    m.UserID = curUserID;
                }
                else
                {
                    m.UserID = 0;
                }
                return Ok(srvPRNMaster.GetPRNMasterForApproval(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

    }
}