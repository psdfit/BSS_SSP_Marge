/* **** Aamer Rehman Malik *****/

using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassSectionsController : ControllerBase
    {
        private ISRVClassSections srv = null;

        public ClassSectionsController(ISRVClassSections srv)
        {
            this.srv = srv;
        }

        // GET: ClassSections
        [HttpGet]
        [Route("GetClassSections")]
        public IActionResult GetClassSections()
        {
            try
            {
                return Ok(srv.FetchClassSections());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ClassSections
        [HttpGet]
        [Route("RD_ClassSections")]
        public IActionResult RD_ClassSections()
        {
            try
            {
                return Ok(srv.FetchClassSections(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: ClassSections
        [HttpPost]
        [Route("RD_ClassSectionsBy")]
        public IActionResult RD_ClassSectionsBy(ClassSectionsModel mod)
        {
            try
            {
                return Ok(srv.FetchClassSections(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ClassSections/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ClassSectionsModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveClassSections(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ClassSections/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ClassSectionsModel d)
        {
            try
            {
               
                srv.ActiveInActive(d.SectionID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}