/* **** Aamer Rehman Malik *****/

using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using PSDF_BSS.Logging;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KAMAssignmentController : ControllerBase
    {
        private ISRVKAMAssignment srv = null;

        public KAMAssignmentController(ISRVKAMAssignment srv)
        {
            this.srv = srv;
        }

        // GET: KAMAssignment
        [HttpGet]
        [Route("GetKAMAssignment")]
        public IActionResult GetKAMAssignment()
        {
            try
            {
                List<object> ls = new List<object>();

               

                ls.Add(new SRVUsers().FetchUsers(false));

                //ls.Add(new SRVTSPDetail().FetchTSPDetailForKam());
                ls.Add(srv.FetchUnAssigenedTSPMastersForKAM());
                //ls.Add(srv.FetchKAMAssignment());
                ls.Add(srv.FetchKAMAssignmentByTSPMaster());
                ls.Add(new SRVRegion().FetchRegion(false));
                ls.Add(new SRVDistrict().FetchDistrict(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpGet]
        [Route("GetTSPKAMHistory/{tspid}")]
        public IActionResult GetTSPKAMHistory(int tspid)
        {
            try
            {
                
                return Ok(srv.FetchTSPKAMHistory(tspid));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // RD: KAMAssignment
        [HttpGet]
        [Route("RD_KAMAssignment")]
        public IActionResult RD_KAMAssignment()
        {
            try
            {
                return Ok(srv.FetchKAMAssignment(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // RD: KAMAssignment
        [HttpGet]
        [Route("RD_KAMAssignmentBy")]
        public IActionResult RD_KAMAssignmentBy()
        {
            try
            {
                int userID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.FetchKAMAssignment(new KAMAssignmentModel() { UserID = userID }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        // RD: KAMAssignment
        [HttpGet]
        [Route("RD_KAMInforForTSP")]
        public IActionResult RD_KAMInforForTSP()
        {
            try
            {
                int userID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.FetchKAMInfoForTSP(new KAMAssignmentModel() { UserID = userID }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // POST: KAMAssignment/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(List<KAMAssignmentModel> D)
        {
           
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], 0);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.BatchInsert(D, D[0].UserID, Convert.ToInt32(User.Identity.Name));
                    foreach (var KAMAssignment in D)
                    {
                        srv.SendNotificationToKAMAndTSP(KAMAssignment);
                    }
                    return Ok(srv.FetchUnAssigenedTSPMastersForKAM());
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }



        }

        [HttpGet]
        [Route("RD_KAMAssignmentForFilters")]
        public IActionResult RD_KAMAssignmentForFilters()
        {
            try
            {
                return Ok(srv.FetchKAMAssignmentForFilters(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Save(KAMAssignmentModel D)
        {
            
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], 1);
            if (IsAuthrized == true)
            {
                try
                {
                    //srv.BatchInsert(D, D[0].UserID, Convert.ToInt32(User.Identity.Name));
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveKAMAssignment(D));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }


        }

        //// POST: KAMAssignment/Save
        //[HttpPost]
        //[Route("Save")]
        //public IActionResult Save(List<KAMAssignmentModel> D)
        //{
        //    try
        //    {
        //        D.CurUserID = Convert.ToInt32(User.Identity.Name);
        //        return Ok(srv.SaveKAMAssignment(D));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message.ToString());
        //    }
        //}

        // POST: KAMAssignment/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(KAMAssignmentModel d)
        {
            try
            {
                //
                srv.ActiveInActive(d.KamID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}