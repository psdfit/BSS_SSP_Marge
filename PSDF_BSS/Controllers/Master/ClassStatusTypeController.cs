using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassStatusTypeController : ControllerBase
    {
        private ISRVClassStatusType srv = null;

        public ClassStatusTypeController(ISRVClassStatusType srv)
        {
            this.srv = srv;
        }

        // GET: ClassStatusType
        [HttpGet]
        [Route("GetClassStatusType")]
        public IActionResult GetClassStatusType()
        {
            try
            {
                return Ok(srv.FetchClassStatusType());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ClassStatusType
        [HttpGet]
        [Route("RD_ClassStatusType")]
        public IActionResult RD_ClassStatusType()
        {
            try
            {
                return Ok(srv.FetchClassStatusType(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ClassStatusType
        [HttpPost]
        [Route("RD_ClassStatusTypeBy")]
        public IActionResult RD_ClassStatusTypeBy(ClassStatusTypeModel mod)
        {
            try
            {
                return Ok(srv.FetchClassStatusType(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ClassStatusType/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ClassStatusTypeModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveClassStatusType(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ClassStatusType/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ClassStatusTypeModel d)
        {
            try
            {
               
                srv.ActiveInActive(d.StatusTypeID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}