using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PSDF_BSSMasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TSPDetailSchemeMapController : ControllerBase
    {
        private readonly ISRVTSPDetailSchemeMap srvTSPDetailSchemeMap ;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVTSPDetail srvTSPDetail;

        public TSPDetailSchemeMapController(ISRVTSPDetailSchemeMap srvTSPDetailSchemeMap, ISRVScheme srvScheme, ISRVTSPDetail srvTSPDetail)
        {
            this.srvTSPDetailSchemeMap = srvTSPDetailSchemeMap;
            this.srvScheme = srvScheme;
            this.srvTSPDetail = srvTSPDetail;
        }

        // GET: TSPDetailSchemeMap
        [HttpGet]
        [Route("GetTSPDetailSchemeMap")]
        public IActionResult GetTSPDetailSchemeMap()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvTSPDetailSchemeMap.FetchTSPDetailSchemeMap());

                ls.Add(srvScheme.FetchScheme(false));

                ls.Add(srvTSPDetail.FetchTSPDetail(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TSPDetailSchemeMap
        [HttpGet]
        [Route("RD_TSPDetailSchemeMap")]
        public IActionResult RD_TSPDetailSchemeMap()
        {
            try
            {
                return Ok(srvTSPDetailSchemeMap.FetchTSPDetailSchemeMap(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TSPDetailSchemeMap
        [HttpPost]
        [Route("RD_TSPDetailSchemeMapBy")]
        public IActionResult RD_TSPDetailSchemeMapBy(TSPDetailSchemeMapModel mod)
        {
            try
            {
                return Ok(srvTSPDetailSchemeMap.FetchTSPDetailSchemeMap(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TSPDetailSchemeMap/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TSPDetailSchemeMapModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvTSPDetailSchemeMap.SaveTSPDetailSchemeMap(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TSPDetailSchemeMap/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TSPDetailSchemeMapModel d)
        {
            try
            {
               
                srvTSPDetailSchemeMap.ActiveInActive(d.ID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}