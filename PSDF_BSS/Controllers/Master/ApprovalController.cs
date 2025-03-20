using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApprovalController : ControllerBase
    {
        private readonly ISRVApproval serviceApproval;
        private readonly ISRVApprovalHistory serviceApprovalHistory;
        private readonly ISRVApprovalProcess serviceApprovalProcess;
        private readonly ISRVUsers serviceUsers;
        public ApprovalController(ISRVApproval serviceApproval, ISRVApprovalHistory serviceApprovalHistory, ISRVApprovalProcess serviceApprovalProcess, ISRVUsers serviceUsers)
        {
            this.serviceApproval = serviceApproval;
            this.serviceApprovalHistory = serviceApprovalHistory;
            this.serviceApprovalProcess = serviceApprovalProcess;
            this.serviceUsers = serviceUsers;
        }
        [HttpPost]
        [Route("GetFactSheet")]
        //public IActionResult GetApprovalHistory(SchemeModel model)
        public IActionResult GetFactSheet(SchemeModel model)
        {
            try
            {
                return Ok(serviceApproval.GetFactSheet(model.SchemeID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        #region Approval
        [HttpGet]
        [Route("GetApproval")]
        public IActionResult GetApproval()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(serviceApproval.FetchApproval());

                ls.Add(serviceApprovalProcess.FetchApprovalProcess(false));

                ls.Add(serviceUsers.FetchUsers(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("RD_Approval")]
        public IActionResult RD_Approval()
        {
            try
            {
                return Ok(serviceApproval.FetchApproval(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("RD_ApprovalBy")]
        public IActionResult RD_ApprovalBy(ApprovalModel mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(serviceApproval.FetchApproval(mod, out string HasAutoApproval));
                ls.Add(HasAutoApproval);
                return Ok(serviceApproval.FetchApproval(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("RD_Approval")]
        public IActionResult RD_Approval(ApprovalModel mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(serviceApproval.FetchApproval(mod, out string HasAutoApproval));
                ls.Add(Convert.ToBoolean(HasAutoApproval));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("GetApproval")]
        public IActionResult GetApproval(ApprovalModel mod)
        {
            try
            {
                return Ok(serviceApproval.FetchApproval(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(List<ApprovalModel> D)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3], D[0].ApprovalD);
            if (IsAuthrized == true)
            {
                try
                {
                    //  D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    serviceApproval.BatchInsert(D, D[0].ProcessKey, Convert.ToInt32(User.Identity.Name));
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
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ApprovalModel d)
        {
                try
                {

                    serviceApproval.ActiveInActive(d.ApprovalD, d.InActive, Convert.ToInt32(User.Identity.Name));
                    return Ok(true);
                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException.ToString());
                }

        }

        [HttpGet]
        [Route("CheckPendingApprovalStep")]
        public IActionResult CheckPendingApprovalStep(string ProcessKey)
        {
            try
            {
                return Ok(serviceApproval.CheckPendingApprovalStep(ProcessKey));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetMaxPendindingStep")]
        public IActionResult MaxPendindingStep(string ProcessKey)
        {
            try
            {
                return Ok(serviceApproval.MaxPendindingStep(ProcessKey));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        #endregion Approval

        #region Approval Dialogue
        [HttpPost]
        [Route("SaveApprovalHistory")]
        public IActionResult SaveApprovalHistory(ApprovalHistoryModel model)
        {
            try
            {
                ApprovalWrapperModel wrapperModel = new ApprovalWrapperModel();
                wrapperModel.approvalHistoryModel = model;
                if (model?.IsAutoApproval == null || model?.IsAutoApproval == false)
                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                else
                    model.CurUserID = (int)EnumUsers.System;
                var result = serviceApprovalHistory.SaveApprovalHistory(ref wrapperModel);
                serviceApprovalHistory.SendApprovalNotification(wrapperModel);
                AutoApproval(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
        [HttpPost]
        [Route("SaveSRNApprovalHistory")]
        public IActionResult SaveSRNApprovalHistory(ApprovalHistoryModel model)
        {
            try
            {
                ApprovalWrapperModel wrapperModel = new ApprovalWrapperModel();
                wrapperModel.approvalHistoryModel = model;
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                var result = serviceApprovalHistory.SaveSRNApprovalHistory(ref wrapperModel);
                serviceApprovalHistory.SendSRNApprovalNotification(wrapperModel);
                AutoApproval(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("SaveGURNApprovalHistory")]
        public IActionResult SaveGURNApprovalHistory(ApprovalHistoryModel model)
        {
            try
            {
                ApprovalWrapperModel wrapperModel = new ApprovalWrapperModel();
                wrapperModel.approvalHistoryModel = model;
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                var result = serviceApprovalHistory.SaveGURNApprovalHistory(ref wrapperModel);
                serviceApprovalHistory.SendSRNApprovalNotification(wrapperModel);
                AutoApproval(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("SaveTPRNApprovalHistory")]
        public IActionResult SaveTPRNApprovalHistory(ApprovalHistoryModel model)
        {
            try
            {
                ApprovalWrapperModel wrapperModel = new ApprovalWrapperModel();
                wrapperModel.approvalHistoryModel = model;
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                var result = serviceApprovalHistory.SaveTPRNApprovalHistory(ref wrapperModel);
                serviceApprovalHistory.SendTPRNApprovalNotification(wrapperModel);
                
                AutoApproval(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("SavePVRNApprovalHistory")]
        public IActionResult SavePVRNApprovalHistory(ApprovalHistoryModel model)
        {
            try
            {
                ApprovalWrapperModel wrapperModel = new ApprovalWrapperModel();
                wrapperModel.approvalHistoryModel = model;
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                var result = serviceApprovalHistory.SavePVRNApprovalHistory(ref wrapperModel);
                serviceApprovalHistory.SendTPRNApprovalNotification(wrapperModel);

                AutoApproval(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("SaveMRNApprovalHistory")]
        public IActionResult SaveMRNApprovalHistory(ApprovalHistoryModel model)
        {
            try
            {
                ApprovalWrapperModel wrapperModel = new ApprovalWrapperModel();
                wrapperModel.approvalHistoryModel = model;
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                var result = serviceApprovalHistory.SaveMRNApprovalHistory(ref wrapperModel);
                serviceApprovalHistory.SendTPRNApprovalNotification(wrapperModel);

                AutoApproval(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("SavePCRNApprovalHistory")]
        public IActionResult SavePCRNApprovalHistory(ApprovalHistoryModel model)
        {
            try
            {
                ApprovalWrapperModel wrapperModel = new ApprovalWrapperModel();
                wrapperModel.approvalHistoryModel = model;
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                var result = serviceApprovalHistory.SavePCRNApprovalHistory(ref wrapperModel);
                serviceApprovalHistory.SendTPRNApprovalNotification(wrapperModel);

                AutoApproval(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("SaveOTRNApprovalHistory")]
        public IActionResult SaveOTRNApprovalHistory(ApprovalHistoryModel model)
        {
            try
            {
                ApprovalWrapperModel wrapperModel = new ApprovalWrapperModel();
                wrapperModel.approvalHistoryModel = model;
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                var result = serviceApprovalHistory.SaveOTRNApprovalHistory(ref wrapperModel);
                serviceApprovalHistory.SendTPRNApprovalNotification(wrapperModel);

                AutoApproval(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("GetApprovalHistory")]
        public IActionResult GetApprovalHistory(ApprovalHistoryModel model)
        {
            try
            {
                return Ok(serviceApprovalHistory.FetchApprovalHistory(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        private void AutoApproval(ApprovalHistoryModel model)
        {
            try
            {
                List<ApprovalHistoryModel> result = serviceApprovalHistory.NextApproval(model);
                if (result != null)
                {
                    foreach(ApprovalHistoryModel var in result)
                    {
                        SaveApprovalHistory(var);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpPost]
        [Route("GetTradeDate")]
        public IActionResult GetTradeDate(ApprovalHistoryModel model)
        {
            try
            {
                return Ok(serviceApprovalHistory.FetchTradeTarget(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("SaveTradeTarget")]
        public IActionResult SaveTradeTarget(List<SchemeTradeMapping> model)
        {
            try
            {
                var result = serviceApprovalHistory.SaveTradeTarget(model);
                
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdateTradeTarget")]
        public IActionResult UpdateTradeTarget(List<SchemeTradeMapping> model)
        {
            try
            {
                var result = serviceApprovalHistory.UpdateTradeTarget(model);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion Approval Dialogue
    }
}