using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TakamolRecommendationNoteController:ControllerBase 
    {
        private readonly ISRVTakamolRecommendationNote srvTakamolRecommendationNote;
        private readonly ISRVTakamolRecommendationNoteDetails srvTakamolRecommendationNoteDetails;
        private readonly ISRVUsers srvUsers;

        public TakamolRecommendationNoteController(ISRVTakamolRecommendationNote srvTakamolRecommendationNote, ISRVTakamolRecommendationNoteDetails srvTakamolRecommendationNoteDetails, ISRVUsers srvUsers)
        {
            this.srvTakamolRecommendationNote = srvTakamolRecommendationNote;
            this.srvTakamolRecommendationNoteDetails = srvTakamolRecommendationNoteDetails;
            this.srvUsers = srvUsers;
        }
        [HttpPost]
        [Route("GetTakamolRecommendationNote")]
        public IActionResult GetTakamolRecommendationNote(TakamolRecommendationNoteModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvTakamolRecommendationNote.FetchTakamolRecommendationNote(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetTakamolRecommendationNoteDetails/{TakamolRecommendationNoteID}")]
        public IActionResult GetTakamolRecommendationNoteDetails(int TakamolRecommendationNoteID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvTakamolRecommendationNoteDetails.FetchTakamolRecommendationNoteDetails(new TakamolRecommendationNoteDetailsModel() { TakamolRecommendationNoteID = TakamolRecommendationNoteID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetTakamolRecommendationNoteExcelExportByIDs")]
        public IActionResult GetTakamolRecommendationNoteExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvTakamolRecommendationNoteDetails.GetTakamolRecommendationNoteExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }


        [HttpGet]
        [Route("GetTakamolRecommendationNoteDetailsFiltered/{TakamolRecommendationNoteID}")]
        public IActionResult GetTakamolRecommendationNoteDetailsFiltered(int TakamolRecommendationNoteID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvTakamolRecommendationNoteDetails.FetchTakamolRecommendationNoteDetailsFiltered(new TakamolRecommendationNoteDetailsModel() { TakamolRecommendationNoteID = TakamolRecommendationNoteID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdateTakamolRecommendationNoteDetails")]
        public IActionResult UpdateTakamolRecommendationNoteDetails(TakamolRecommendationNoteDetailsModel mod)
        {
            try
            {
                return Ok(srvTakamolRecommendationNoteDetails.UpdateTakamolRecommendationNoteDetails(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetTakamolRecommendationNoteExcelExport")]
        public IActionResult GetTakamolRecommendationNoteExcelExport(TakamolRecommendationNoteDetailsModel mod)
        {
            try
            {
                var k = srvTakamolRecommendationNoteDetails.GetTakamolRecommendationNoteExcelExport(mod);
                return Ok(k);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetVRN")]
        public IActionResult GetVRN(TakamolRecommendationNoteModel model)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserRole = srvUsers.GetByUserID(curUserID).RoleID;

                if (loggedInUserRole.Equals(Convert.ToInt32(EnumRoles.TSP)))
                {
                    model.UserID = curUserID;
                }

                return Ok(srvTakamolRecommendationNote.FetchVRN(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
