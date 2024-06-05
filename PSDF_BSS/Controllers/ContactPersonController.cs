using System;
using DataLayer.Models;
using DataLayer.Interfaces;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactPersonController : ControllerBase
    {
        private readonly ISRVContactPerson srvContactPerson = null;
        public ContactPersonController(ISRVContactPerson srvContactPerson)
        {
            this.srvContactPerson = srvContactPerson;
        }
        // GET: ContactPerson
        [HttpGet]
        [Route("GetContactPerson")]
        public IActionResult GetContactPerson()
        {
            try
            {

                return Ok(srvContactPerson.FetchContactPerson());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("CheckContactPersonMobile/{cp}")]
        public IActionResult CheckContactPersonMobile(string cp)
        {
            if (!String.IsNullOrEmpty(cp))
            {
                List<ContactPersonModel> u = srvContactPerson.GetByContactPersonMobile(cp);
                if (u == null)
                {
                    return Ok(u);
                }
                else
                {
                    if (u[0].ContactPersonMobile == cp)
                    {
                        return Ok(u[0]);
                    }
                    else
                        return Ok(false);
                }

            }
            else
                return BadRequest("Bad Request");
        }


        // RD: ContactPerson
        [HttpGet]
        [Route("RD_ContactPerson")]
        public IActionResult RD_ContactPerson()
        {
            try
            {
                return Ok(srvContactPerson.FetchContactPerson(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: ContactPerson
        [HttpPost]
        [Route("RD_ContactPersonBy")]
        public IActionResult RD_ContactPersonBy(ContactPersonModel mod)
        {
            try
            {
                return Ok(srvContactPerson.FetchContactPerson(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: ContactPerson/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(List<ContactPersonModel> D)
        {
            try
            {
                //  D.CurUserID = Convert.ToInt32(User.Identity.Name);
                //return Ok(srv.SaveContactPerson(D));
                srvContactPerson.BatchInsert(D, D[0].IncepReportID, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ContactPerson/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ContactPersonModel d)
        {
            try
            {
                srvContactPerson.ActiveInActive(d.ContactPersonID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

