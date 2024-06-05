using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassChangeRequestController : ControllerBase
    {
        ISRVClassChangeRequest srv = null;
        ISRVClass srvClass = null;
        ISRVDistrict srvDistrict = null;
        ISRVTehsil srvTehsil = null;
        public ClassChangeRequestController(ISRVClassChangeRequest srv, ISRVClass srvClass, ISRVDistrict srvDistrict, ISRVTehsil srvTehsil)
        {
            this.srv = srv;
            this.srvClass = srvClass;
            this.srvDistrict = srvDistrict;
            this.srvTehsil = srvTehsil;
        }
        // GET: ClassChangeRequest
        [HttpGet]
        [Route("GetClassChangeRequest")]
        public IActionResult GetClassChangeRequest()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchClassChangeRequest());

                ls.Add(new SRVTrade().FetchTrade(false));

                ls.Add(new SRVSourceOfCurriculum().FetchSourceOfCurriculum(false));

                ls.Add(new SRVCertificationAuthority().FetchCertificationAuthority(false));

                ls.Add(new SRVCluster().FetchCluster(false));

                ls.Add(new SRVTehsil().FetchTehsil(false));


                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        // GET: ClassChangeRequest
        [HttpGet]
        [Route("GetClassDatesChangeRequest")]
        public IActionResult GetClassDatesChangeRequest()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchClassDatesChangeRequest());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetClassesForLocationChangeByUser/{id}")]
        public IActionResult GetClassesForLocationChangeByUser(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srv.FetchClassesForLocationChangeByUser(id));
                ls.Add(srvTehsil.FetchTehsil(false));
                ls.Add(srvDistrict.FetchDistrict(false));
             
                return Ok(ls);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpGet]
        [Route("GetClassesByUser/{id}")]
        public IActionResult GetClassesByUser(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srv.FetchClassesForDatesChangeByUser(id));
                ls.Add(srvTehsil.FetchTehsil(false));
                ls.Add(srvDistrict.FetchDistrict(false));
             
                return Ok(ls);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }



        // RD: ClassChangeRequest
        [HttpGet]
        [Route("RD_ClassChangeRequest")]
        public IActionResult RD_ClassChangeRequest()
        {
            try
            {
                return Ok(srv.FetchClassChangeRequest(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: ClassChangeRequest
        [HttpPost]
        [Route("RD_ClassChangeRequestBy")]
        public IActionResult RD_ClassChangeRequestBy(ClassChangeRequestModel mod)
        {
            try
            {
                return Ok(srv.FetchClassChangeRequest(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: ClassChangeRequest/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ClassChangeRequestModel D)
        {
            
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.ClassChangeRequestID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveClassChangeRequest(D));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        // POST: ClassDatesChangeRequest/SaveClassDates
        [HttpPost]
        [Route("SaveClassDates")]
        public IActionResult SaveClassDates(ClassChangeRequestModel D)
        {
            

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.ClassChangeRequestID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveClassDatesChangeRequest(D));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        // POST: ClassChangeRequest/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ClassChangeRequestModel d)
        {
            try
            {
                srv.ActiveInActive(d.ClassChangeRequestID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetClassChangeRequestByFilter")]
        public IActionResult GetClassChangeRequestByFilter(QueryFilters filters)
        {
            try
            {
                return Ok(srv.FetchClassChangeRequestByFilter(filters));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

