using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrictController : ControllerBase
    {
        private ISRVDistrict srv = null;

        public DistrictController(ISRVDistrict srv)
        {
            this.srv = srv;
        }

        // GET: District
        [HttpGet]
        [Route("GetDistrict")]
        public IActionResult GetDistrict()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchDistrict());

                ls.Add(new SRVCluster().FetchCluster(false));
                ls.Add(new SRVRegion().FetchRegion(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: District
        [HttpGet]
        [Route("RD_District")]
        public IActionResult RD_District()
        {
            try
            {
                return Ok(srv.FetchDistrict(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: District
        [HttpPost]
        [Route("RD_DistrictBy")]
        public IActionResult RD_DistrictBy(DistrictModel mod)
        {
            try
            {
                return Ok(srv.FetchDistrict(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: District/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(DistrictModel D)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3], D.DistrictID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);

                    return Ok(srv.SaveDistrict(D));
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        [HttpPost]
        [Route("CheckDistrictName")]
        public IActionResult CheckDistrictName(DistrictModel district)
        {
            if (!String.IsNullOrEmpty(district.DistrictName))
            {
                List<DistrictModel> u = srv.GetByDistrictName(district.DistrictName);
                if (u == null)
                {
                    return Ok(true);
                }
                else
                {
                    if (u.Count > 1)
                        return Ok(false);
                    else
                    {
                        if (u[0].DistrictID == district.DistrictID)
                        {
                            return Ok(true);
                        }
                        else
                            return Ok(false);
                    }
                }
            }
            else
                return BadRequest("Bad Request");
        }


        // POST: District/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(DistrictModel d)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.DistrictID, d.InActive, Convert.ToInt32(User.Identity.Name));
                    return Ok(true);
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
        // RD: RD_DistrictTSP
        [HttpGet]
        [Route("SSPRD_DistrictTSP/{programid}")]
        public IActionResult SSPRD_DistrictTSP(int programid)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SSPFetchDistrictTSP(programid, curUserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}