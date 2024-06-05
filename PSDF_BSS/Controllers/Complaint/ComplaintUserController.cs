using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers.Complaint
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintUserController : ControllerBase
    {
        private readonly ISRVTraineeProfile _srvTraineeProfile;
        private ISRVComplaintUser srvComplaintuser;
        private ISRVUsers serviceUsers;
        public ComplaintUserController(ISRVComplaintUser srvComplaintuser, ISRVUsers serviceUsers, ISRVTraineeProfile srvTraineeProfile)
        {
            _srvTraineeProfile = srvTraineeProfile;
            this.srvComplaintuser = srvComplaintuser;
            this.serviceUsers = serviceUsers;
        }

        [HttpPost]
        [Route("save")]
        public IActionResult save(ComplaintModel.ComplaintUserModel model)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.ComplaintUserID);
            if (IsAuthrized == true)
            {
                try
                {
                    model.UserIDs = string.Join(",", model.UserID);
                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    bool ls = new bool();
                    ls = srvComplaintuser.saveComplaintUser(model);
                    return Ok(ls);
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
        [HttpGet]
        [Route("FetchComplaintUserForGridView")]
        public IActionResult FetchComplaintUserForGridView()
        {
            try
            {
                List<object> list = new List<object>();
                List<ComplaintModel> ls = new List<ComplaintModel>();
                list.Add(srvComplaintuser.FetchComplaintUserForGridView());
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}
