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
    public class TehsilController : ControllerBase
    {
        private ISRVTehsil srv = null;

        public TehsilController(ISRVTehsil srv)
        {
            this.srv = srv;
        }

        // GET: Tehsil
        [HttpGet]
        [Route("GetTehsil")]
        public IActionResult GetTehsil()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchTehsil());

                ls.Add(new SRVDistrict().FetchDistrict(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Tehsil
        [HttpGet]
        [Route("RD_Tehsil")]
        public IActionResult RD_Tehsil()
        {
            try
            {
                return Ok(srv.FetchTehsil(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Tehsil
        [HttpPost]
        [Route("RD_TehsilBy")]
        public IActionResult RD_TehsilBy(TehsilModel mod)
        {
            try
            {
                return Ok(srv.FetchTehsil(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Tehsil/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TehsilModel D)
        {
           

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.TehsilID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveTehsil(D));
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
        [Route("CheckTehsilName")]
        public IActionResult CheckTehsilName(TehsilModel tehsil)
        {
            if (!String.IsNullOrEmpty(tehsil.TehsilName))
            {
                List<TehsilModel> u = srv.GetByTehsilName(tehsil.TehsilName);
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
                        if (u[0].TehsilID == tehsil.TehsilID)
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

        // POST: Tehsil/ActiveInActive
        [HttpPost] 
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TehsilModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.TehsilID, d.InActive, Convert.ToInt32(User.Identity.Name));
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
        // RD: Tehsil
        [HttpGet]
        [Route("GetTehsilByDistrictID")]
        public IActionResult GetTehsilByDistrictID([FromQuery]int id)
        {
            try
            {
                return Ok(srv.GetByDistrictID(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}