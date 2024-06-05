using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSSMasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CentersController : ControllerBase
    {
        private readonly ISRVCenters srvCenters;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTehsil srvTehsil;

        public CentersController(ISRVCenters srvCenters, ISRVDistrict srvDistrict,ISRVTehsil srvTehsil)
        {
            this.srvCenters = srvCenters;
            this.srvDistrict = srvDistrict;
            this.srvTehsil = srvTehsil;
        }
        // GET: Centers
        [HttpGet]
        [Route("GetCenters")]
        public async Task<IActionResult> GetCenters()
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvCenters.FetchCenters());
                ls.Add(srvDistrict.FetchDistrict(false));
                ls.Add(srvTehsil.FetchTehsil(false));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: Centers
        [HttpGet]
        [Route("RD_Centers")]
        public async Task<IActionResult> RD_Centers()
        {
            try
            {
                return Ok(srvCenters.FetchCenters(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: Centers
        [HttpPost]
        [Route("RD_CentersBy")]
        public async Task<IActionResult> RD_CentersBy(CentersModel mod)
        {
            try
            {
                return Ok(srvCenters.FetchCenters(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: Centers/Save
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(CentersModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvCenters.SaveCenters(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Centers/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<IActionResult> ActiveInActive(CentersModel d)
        {
            try
            {
                srvCenters.ActiveInActive(d.CenterID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

