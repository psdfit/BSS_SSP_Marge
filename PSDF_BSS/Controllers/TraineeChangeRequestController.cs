using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using PSDF_BSS.Logging;
using System.IO;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TraineeChangeRequestController : ControllerBase
    {
        ISRVTraineeChangeRequest srv = null;
        public TraineeChangeRequestController(ISRVTraineeChangeRequest srv)
        {
            this.srv = srv;
        }
        // GET: TraineeChangeRequest
        [HttpPost]
        [Route("GetTraineeChangeRequest")]
        public IActionResult GetTraineeChangeRequest(TraineeChangeRequestModel td)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchTraineeChangeRequest(td));

                //ls.Add(new SRVTraineeProfile().FetchTraineeProfile(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: TraineeChangeRequest
        [HttpGet]
        [Route("RD_TraineeChangeRequest")]
        public IActionResult RD_TraineeChangeRequest()
        {
            try
            {
                return Ok(srv.FetchTraineeChangeRequest(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: TraineeChangeRequest
        [HttpPost]
        [Route("RD_TraineeChangeRequestBy")]
        public IActionResult RD_TraineeChangeRequestBy(TraineeChangeRequestModel mod)
        {
            try
            {
                return Ok(srv.FetchTraineeChangeRequest(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: TraineeChangeRequest/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TraineeChangeRequestModel D)
        {
            
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.TraineeChangeRequestID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);


                    string path = "\\Documents\\Traniee_Profiles\\TraineeDocuments\\" + D.TraineeClassCode + "\\" + D.TraineeCode;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    

                    string fileName = !String.IsNullOrEmpty(D.TraineechangeImage) ? Common.AddFile(D.TraineechangeImage, path + "\\"): Common.AddFile(D.TraineePreImage, path + "\\") ;

                    D.TraineechangeImage = fileName;


                    return Ok(srv.SaveTraineeChangeRequest(D));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);

                    //return BadRequest(e.InnerException.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
        [HttpPost]
        [Route("SaveVerifiedTrainee")]
        public IActionResult SaveVerifiedTrainee(TraineeChangeRequestModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.TraineeChangeRequestID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    string path = "\\Documents\\Traniee_Profiles\\TraineeDocuments\\" + D.TraineeClassCode + "\\" + D.TraineeCode;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    string fileName = !String.IsNullOrEmpty(D.TraineechangeImage) ? Common.AddFile(D.TraineechangeImage, path + "\\") : D.TraineePreImage;

                    D.TraineechangeImage = fileName;
                    return Ok(srv.SaveVerifiedTraineeChangeRequest(D));
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

        //// POST: TraineeChangeRequest/ActiveInActive
        //[HttpPost]
        //[Route("ActiveInActive")]
        //public IActionResult ActiveInActive(TraineeChangeRequestModel d)
        //{
        //    try
        //    {
        //        UsersModel u = Common.GetUserFromRequest(Request);
        //        srv.ActiveInActive(d.TraineeChangeRequestID, d.InActive, u.UserID);
        //        return Ok(true);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //}
    }
}

