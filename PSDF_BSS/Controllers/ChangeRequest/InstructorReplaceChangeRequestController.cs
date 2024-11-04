using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace PSDF_BSSChangeRequest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorReplaceChangeRequestController : ControllerBase
    {
        ISRVInstructorReplaceChangeRequest srv = null;
        private readonly ISRVUsers srvUsers;

        public InstructorReplaceChangeRequestController(ISRVInstructorReplaceChangeRequest srv, ISRVUsers srvUsers)
        {
            this.srv = srv;
            this.srvUsers = srvUsers;
        }
        // GET: InstructorReplaceChangeRequest
        [HttpGet]
        [Route("GetInstructorReplaceChangeRequest/{userId}")]
        public IActionResult GetInstructorReplaceChangeRequestByUserID(int userID)
        {
            try
            {
                // Get the current user ID
                // int curUserID = Convert.ToInt32(User.Identity.Name);

                // Fetch the user's level
                int loggedInUserLevel = srvUsers.GetByUserID(userID).UserLevel;

                // Declare the list to hold the data
                List<InstructorReplaceChangeRequestModel> ls = new List<InstructorReplaceChangeRequestModel>();

                InstructorCRFiltersModel model = new InstructorCRFiltersModel
                {
                    UserID = userID
                };

                // Check if the user is TSP level, fetch data accordingly
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    // Fetch schemes specific to the TSP user
                    ls.AddRange(srv.FetchInstructorReplaceChangeRequestByUserID(userID));
                }
                else
                {
                    userID = 0;
                    // Fetch schemes that are final submitted, approved, and match the organization ID
                    ls.AddRange(srv.FetchInstructorReplaceChangeRequestByUserID(userID));
                }

                // Return the list in the response
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException?.ToString() ?? e.Message);
            }
        }

        [HttpGet]
        [Route("GetInstructorReplaceChangeRequest")]
        public IActionResult GetInstructorReplaceChangeRequest()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchInstructorReplaceChangeRequest());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: InstructorReplaceChangeRequest
        [HttpGet]
        [Route("RD_InstructorReplaceChangeRequest")]
        public IActionResult RD_InstructorReplaceChangeRequest()
        {
            try
            {
                return Ok(srv.FetchInstructorReplaceChangeRequest(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: InstructorReplaceChangeRequest
        //[HttpPost]
        //[Route("RD_InstructorReplaceChangeRequestBy")]
        //public IActionResult RD_InstructorReplaceChangeRequestBy(InstructorReplaceChangeRequestModel mod)
        //{
        //    try
        //    {
        //        return Ok(srv.FetchInstructorReplaceChangeRequest(mod));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //}
        // POST: InstructorReplaceChangeRequest/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(InstructorReplaceChangeRequestModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveInstructorReplaceChangeRequest(D));
            }
            catch (Exception e)
            {
               return BadRequest(e.Message);
            }
        }

        // POST: InstructorReplaceChangeRequest/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(InstructorReplaceChangeRequestModel d)
        {
            try
            {
                srv.ActiveInActive(d.InstructorReplaceChangeRequestID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

