using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly ISRVDashboard srvDashboard;
        private readonly ISRVUsers srvUsers;

        public DashboardController(ISRVDashboard srvDashboard, ISRVUsers srvUsers)
        {
            this.srvDashboard = srvDashboard;
            this.srvUsers = srvUsers;
        }

        [HttpGet]
        [Route("TraineeJourney")]
        public IActionResult TraineeJourney(string type, int ID, string startDate = "", string EndDate = "")
        {
            var data = this.srvDashboard.FetchTraineeJourney(type, ID, startDate, EndDate);

            return Ok(data);
        }
        [HttpGet]
        [Route("ManagmentDashboard")]
        public IActionResult ManagementDashboard(string type, int ID, string startDate = "", string EndDate = "")
        {
            var data = this.srvDashboard.FetchManagementDashboard(type, ID, startDate, EndDate);

            return Ok(data);
        }
        [HttpGet]
        [Route("TraineeJourneySingle")]
        public IActionResult TraineeJourneySingle(string traineeCode, string traineeCNIC)
        {
            var data = this.srvDashboard.FetchTraineeJourneySingle(traineeCode, traineeCNIC);

            return Ok(data);
        }
        [HttpGet]
        [Route("ClassJourney")]
        public IActionResult ClassJourney(int ClassID)
        {
            var data = this.srvDashboard.FetchClassJourney(ClassID);

            return Ok(data);
        }
        [HttpGet]
        [Route("TraineeJourneyFilters")]
        public IActionResult TraineeJourneyFilters()
        {
            try
            {
                Dictionary<string, object> ls = new Dictionary<string, object>();
                Parallel.Invoke(
                              () => ls.Add("Trades", this.srvDashboard.FetchTrades()),
                              () => ls.Add("Clusters", this.srvDashboard.FetchClusters()),
                              () => ls.Add("Districts", this.srvDashboard.FetchDistricts()),
                              () => ls.Add("TSPs", this.srvDashboard.FetchTSPs()),
                              () => ls.Add("Schemes", this.srvDashboard.FetchSchemes()),
                              () => ls.Add("Programs", this.srvDashboard.FetchPrograms())
                              );

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchSchemes")]
        public IActionResult FetchSchemes()
        {
            try
            {
                return Ok(this.srvDashboard.FetchSchemes());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchTSPsByScheme")]
        public IActionResult FetchTSPsByScheme(int SchemeID)
        {
            try
            {
                return Ok(this.srvDashboard.FetchTSPsByScheme(SchemeID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchClassesBySchemeTSP")]
        public IActionResult FetchClassesBySchemeTSP(int SchemeID, int TspID)
        {
            try
            {
                return Ok(this.srvDashboard.FetchClassesBySchemeTSP(SchemeID, TspID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchClassesByTSP")]
        public IActionResult FetchClassesByTSP(int TspID)
        {
            try
            {
                return Ok(this.srvDashboard.FetchClassesByTSP(TspID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchSchemesByKam")]
        public IActionResult FetchSchemesByKam(int KamID)
        {
            try
            {
                return Ok(this.srvDashboard.FetchSchemesByKam(KamID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchTSPsByKamScheme")]
        public IActionResult FetchTSPsByKamScheme(int KamID, int SchemeID)
        {
            try
            {
                return Ok(this.srvDashboard.FetchTSPsByKamScheme(KamID, SchemeID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchSchemesByUsers")]
        public IActionResult FetchSchemesByUsers(int UserID)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                UserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                if (UserID == 0)
                    return Ok(this.srvDashboard.FetchSchemes());
                return Ok(this.srvDashboard.FetchSchemesByUsers(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchClassesBySchemeUser")]
        public IActionResult FetchClassesBySchemeUser(int SchemeID, int UserID)
        {
            try
            {
                return Ok(this.srvDashboard.FetchClassesBySchemeUser(SchemeID, UserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}
