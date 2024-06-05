using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using System.IO;
using System.Text;
using DataLayer.Classes;
using DataLayer.Services;
using PSDF_BSS.Logging;

namespace PSDF_BSS.Controllers.Complaintmodule
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly ISRVTraineeProfile _srvTraineeProfile;
        private ISRVComplaint srvComplaint;
        private ISRVUsers serviceUsers;
        private ISRVTSPDetail srvTSPDetail;
        public ComplaintController(ISRVTSPDetail srv,ISRVComplaint srvComplaint, ISRVUsers serviceUsers, ISRVTraineeProfile srvTraineeProfile)
        {
            this.srvTSPDetail = srv;
            _srvTraineeProfile = srvTraineeProfile;
            this.srvComplaint = srvComplaint;
            this.serviceUsers = serviceUsers;
        }
        
       [HttpGet]
        [Route("GetComplaintTypeForCRUD")]
        public IActionResult GetComplaintTypeForCRUD()
        {
            try
            {
                List<ComplaintModel> ls = new List<ComplaintModel>();
                ls = srvComplaint.FetchComplaintTypeForCRUD();
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchUsers")]
        public IActionResult FetchUsers()
        {
            try
            {
                List<UsersModel> ls = new List<UsersModel>();
                ls = serviceUsers.FetchInternalUsers();
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("save")]
        public IActionResult save(ComplaintModel model)
        {
           

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.ComplainantID);
            if (IsAuthrized == true)
            {
                try
                {
                    string errMsg = string.Empty;
                    if (model.base64 != null)
                    {
                        byte[] bytes = Convert.FromBase64String(model.base64);
                        Guid obj = Guid.NewGuid();
                        model.FileGuid = obj + "." + model.ext;
                        var path = "\\Documents\\Complaint_Attachment\\";
                        model.FilePath = path + model.FileGuid;
                        string appRoot;
                        if (AppContext.BaseDirectory.IndexOf("bin") > 0)
                            appRoot = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin") - 1);
                        else
                            appRoot = AppContext.BaseDirectory;
                        bool exists = Directory.Exists(appRoot + path);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(appRoot + path);
                        System.IO.FileStream stream =
                        new FileStream(@"Documents\Complaint_Attachment\" + model.FileGuid, FileMode.CreateNew);
                        System.IO.BinaryWriter writer = new BinaryWriter(stream);
                        writer.Write(bytes, 0, bytes.Length);
                        writer.Close();
                    }
                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    model.ComplaintRegisterType = 1;
                    long ls = new long();
                    ls = srvComplaint.saveComplaint(model, out errMsg);
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
        [Route("GetComplaintTSubTypeByComplaintType")]
        public IActionResult GetComplaintTSubTypeByComplaintType(int? ComplaintTypeID)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvComplaint.FetchComplaintSubTypeForCRUD(ComplaintTypeID));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        } 
        [HttpGet]
        [Route("getComplianStatus")]
        public IActionResult getComplianStatus(int? ComplaintTypeID)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvComplaint.GetComplaintStatusType());
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("FetchComplaintForGridView")]
        public IActionResult FetchComplaintForGridView(int? ComplaintStatusTypeID)
        {
            try
            {
                if (ComplaintStatusTypeID == null)
                    ComplaintStatusTypeID = 0;
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = serviceUsers.GetByUserID(CurUserID).UserLevel;
                List<object> list = new List<object>();
                List<ComplaintModel> ls = new List<ComplaintModel>();
               
                if (loggedInUserLevel==4)
                {
                    list.Add(srvComplaint.FetchComplaintOfTSPSelf(CurUserID));

                }
                else if (loggedInUserLevel == 2|| loggedInUserLevel == 1)
                {
                    list.Add(srvComplaint.FetchTraineeAndTSPComplaintForKAM(CurUserID, ComplaintStatusTypeID));
                }
                else
                {
                    list.Add(srvComplaint.FetchComplaintForGridView(CurUserID, ComplaintStatusTypeID));
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ComplaintModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srvComplaint.ActiveInActive(d.ComplainantID, d.InActive, Convert.ToInt32(User.Identity.Name));
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
        [HttpPost]
        [Route("complaintStatusChange")]
        public IActionResult complaintStatusChange(ComplaintModel model)
        {
            
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.ComplainantID);
            if (IsAuthrized == true)
            {
                try
                {
                    bool ls = new bool();

                    string errMsg = string.Empty;
                    if (model.base64 != null)
                    {
                        byte[] bytes = Convert.FromBase64String(model.base64);
                        Guid obj = Guid.NewGuid();
                        model.FileGuid = obj + "." + model.ext;
                        var path = "\\Documents\\Complaint_StatusWise_Attachment\\";
                        model.FilePath = path + model.FileGuid;
                        string appRoot;
                        if (AppContext.BaseDirectory.IndexOf("bin") > 0)
                            appRoot = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin") - 1);
                        else
                            appRoot = AppContext.BaseDirectory;
                        bool exists = Directory.Exists(appRoot + path);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(appRoot + path);
                        System.IO.FileStream stream =
                        new FileStream(@"Documents\Complaint_StatusWise_Attachment\" + model.FileGuid, FileMode.CreateNew);
                        System.IO.BinaryWriter writer = new BinaryWriter(stream);
                        writer.Write(bytes, 0, bytes.Length);
                        writer.Close();
                    }

                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    srvComplaint.complaintStatusChange(model);
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
        [Route("GetComplaintHistory/{ComplainantID}")]
        public IActionResult GetComplaintHistory(int ComplainantID)
        {
            try
            {
                 int CurUserID = Convert.ToInt32(User.Identity.Name);
              //  SRVNotificationDetail.SendNotification((short)EnumNotificationType.ChangeRequest, CurUserID);
                return Ok(srvComplaint.FetchComplaintHistory(ComplainantID));

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("ComplaintAttachments")]
        public IActionResult ComplaintAttachments(ComplaintModel mod)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvComplaint.FetchComplaintAttachments(mod.ComplainantID));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("ComplaintStatusAttachments")]
        public IActionResult ComplaintStatusAttachments(ComplaintModel mod)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvComplaint.FetchComplaintStatusAttachments(mod.ComplaintStatusDetailID));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetTraineeInfoByCNIC")]
        public IActionResult GetTraineeInfoByCNIC(string TraineeCNIC)
        {
            try
            {
                ComplaintModel traineeProfile = srvComplaint.GETTraineeByCnicAndTSPUserID(TraineeCNIC, Convert.ToInt32(User.Identity.Name));
                return Ok(traineeProfile);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("GetTSPProfile")]
        public IActionResult GetTSPProfile()
        {
            try
            {
                int id = Convert.ToInt32(User.Identity.Name);
                List<TSPDetailModel> tsps = new List<TSPDetailModel>();

                tsps = srvComplaint.FetchTSPMasterDataByUser(id);

                return Ok(tsps);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
    }
}
