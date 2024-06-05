using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClusterController : ControllerBase
    {
        private ISRVCluster srv = null;

        public ClusterController(ISRVCluster srv)
        {
            this.srv = srv;
        }

        // GET: Cluster
        [HttpGet]
        [Route("GetCluster")]
        public IActionResult GetCluster()
        {
            try
            {
                return Ok(srv.FetchCluster());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Cluster
        [HttpGet]
        [Route("RD_Cluster")]
        public IActionResult RD_Cluster()
        {
            try
            {
                return Ok(srv.FetchCluster(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Cluster
        [HttpPost]
        [Route("RD_ClusterBy")]
        public IActionResult RD_ClusterBy(ClusterModel mod)
        {
            try
            {
                return Ok(srv.FetchCluster(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Cluster/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ClusterModel D)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3], D.ClusterID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveCluster(D));
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
        // CheckName
        [HttpPost]
        [Route("CheckName")]
        public IActionResult CheckName(ClusterModel mod)
        {
            if (!String.IsNullOrEmpty(mod.ClusterName))
            {
                int ID = mod.ClusterID;
                mod.ClusterID = 0;
                List<ClusterModel> u = srv.FetchCluster(mod);
                if (u == null || u.Count == 0)
                {
                    return Ok(true);
                }
                else
                {
                    if (u.Count > 1)
                        return Ok(false);
                    else
                    {
                        if (u[0].ClusterID == ID)
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
        // POST: Cluster/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ClusterModel d)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.ClusterID, d.InActive, Convert.ToInt32(User.Identity.Name));
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
    }
}