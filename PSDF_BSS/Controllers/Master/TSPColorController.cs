using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class TSPColorController : ControllerBase
    {
        private ISRVTSPColor srvTSPColor;
        public TSPColorController(ISRVTSPColor srvTSPColor)
        {
            this.srvTSPColor = srvTSPColor;
        }
        [HttpGet]
        [Route("GetTSPColorMasterData")]
        public IActionResult GetTSPColorMasterData()
        {
            try
            {
                return Ok(srvTSPColor.FetchTSPMasterData());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("GetTSPColor")]
        public IActionResult GetTSPColor()
        {
            try
            {
                return Ok(srvTSPColor.FetchTSPColor());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("saveTSPColor")]
        public IActionResult saveTSPColor(TSPColorModel model)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.TSPMasterID);
            if (IsAuthrized == true)
            {
                try
                {

                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    bool ls = new bool();
                    ls = srvTSPColor.saveTSPColor(model);
                    return Ok(ls);
                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
        
        [HttpPost]
        [Route("RD_TSPMasterColorByID")]
        public IActionResult RD_TSPMasterColorByID(TSPColorModel model)
        {
            try
            {
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                //List<object> ls = new List<object>();
                //ls.Add(srvTSPColor.FetchTSPColorByID(model.CurUserID));
                return Ok(srvTSPColor.FetchTSPColorByID(model.CurUserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("RD_TSPMasterColorByFilters")]
        public IActionResult RD_TSPMasterColorByFilters(TSPColorFiltersModel model)
        {
            try
            {
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                //List<object> ls = new List<object>();
                //ls.Add(srvTSPColor.FetchTSPColorByID(model.CurUserID));
                var tspcolorResult = srvTSPColor.CheckBlacklistingCriteriaCriteria(model);
                return Ok(tspcolorResult);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetTSPColorHistory/{TSPMasterID}")]
        public IActionResult GetTSPColorHistory(int? TSPMasterID)
        {
            try
            {

                return Ok(srvTSPColor.FetchTSPColorHistory(TSPMasterID));

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}
