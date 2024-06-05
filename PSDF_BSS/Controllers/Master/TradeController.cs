using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        private ISRVTrade srv = null;
        private ISRVEquipmentTools srvEquipmentTools;
        private ISRVConsumableMaterial srvConsumableMaterial;
        private ISRVEducationTypes srvEducationTypes;
        private ISRVSourceOfCurriculum srvSourceOfCurriculum;
        private ISRVDuration srvDuration;
        private ISRVAcademicDiscipline srvAcademicDiscipline;
        private ISRVTradeConsumableMaterialMap srvTradeConsumableMaterial;
        private ISRVTradeDetailMap srvTradeDetail;
        private ISRVTradeEquipmentToolsMap srvTradeEquipmentTools;
        private ISRVTradeSourceOfCurriculumMap srvTradeSourceOfCurriculum;
        private ISRVCertificationCategory srvCertificationCategory;
        private ISRVCertificationAuthority srvCertAuth;
        private ISRVSector srvSector;
        private ISRVSubSector srvSubSector;
        private readonly ISRVSendEmail srvSendEmail;
       
        public TradeController(ISRVSendEmail srvSendEmail,ISRVTrade srv, ISRVEquipmentTools srvEquipmentTools,
            ISRVConsumableMaterial srvConsumableMaterial,
            ISRVEducationTypes srvEducationTypes,
            ISRVSourceOfCurriculum srvSourceOfCurriculum,
            ISRVDuration srvDuration,
            ISRVTradeConsumableMaterialMap srvTradeConsumableMaterial,
            ISRVTradeDetailMap srvTradeDetail,
            ISRVTradeEquipmentToolsMap srvTradeEquipmentTools,
            ISRVTradeSourceOfCurriculumMap srvTradeSourceOfCurriculum,
            ISRVAcademicDiscipline srvAcademicDiscipline,
            ISRVCertificationCategory srvCertificationCategory,
            ISRVCertificationAuthority srvCertAuth,
            ISRVSector srvSector,
            ISRVSubSector srvSubSector
            )
        {
            this.srv = srv;
            this.srvSendEmail = srvSendEmail;
            this.srvEquipmentTools = srvEquipmentTools;
            this.srvConsumableMaterial = srvConsumableMaterial;
            this.srvEducationTypes = srvEducationTypes;
            this.srvSourceOfCurriculum = srvSourceOfCurriculum;
            this.srvDuration = srvDuration;
            this.srvTradeConsumableMaterial = srvTradeConsumableMaterial;
            this.srvTradeDetail = srvTradeDetail;
            this.srvTradeEquipmentTools = srvTradeEquipmentTools;
            this.srvTradeSourceOfCurriculum = srvTradeSourceOfCurriculum;
            this.srvAcademicDiscipline = srvAcademicDiscipline;
            this.srvCertificationCategory = srvCertificationCategory;
            this.srvCertAuth = srvCertAuth;
            this.srvSector = srvSector;
            this.srvSubSector = srvSubSector;
        }

        // GET: Trade
        [HttpGet]
        [Route("GetTrade")]
        public IActionResult GetTrade()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchTradeForCRUD());


                ls.Add(srvSector.FetchSector(false));

                ls.Add(srvSubSector.FetchSubSector(false));

                ls.Add(srvCertificationCategory.FetchCertificationCategory(false));

                ls.Add(srvCertAuth.FetchCertificationAuthority(false));

                ls.Add(srvEquipmentTools.FetchEquipmentTools(false));

                ls.Add(srvConsumableMaterial.FetchConsumableMaterial(false));

                ls.Add(srvEducationTypes.FetchEducationTypes(false));

                ls.Add(srvSourceOfCurriculum.FetchSourceOfCurriculum(false));

                ls.Add(srvDuration.FetchDuration(false));

                ls.Add(srvAcademicDiscipline.FetchAcademicDiscipline(false));

                ls.Add(srv.FetchTradeLayer());

                //ls.Add(srvTradeDuration.FetchTradeDurationMapAll(0));

                //ls.Add(srvTradeEquipmentTools.FetchTradeEquipmentToolsMapAll(0));

                //ls.Add(srvTradeConsumableMaterial.FetchTradeConsumableMaterialMapAll(0));

                //ls.Add(srvTradeSourceOfCurriculum.FetchTradeSourceOfCurriculumMapAll(0));



                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetSubmittedTrades")]
        public IActionResult GetSubmittedTrades([FromQuery] int OID)
        {
            //if (srv.CheckUserInFormApproval(Convert.ToInt32(User.Identity.Name)) == false)
            //    return Ok(false);

            try
            {
                List<DataLayer.Models.SubmittedTradesModel> trades = new List<SubmittedTradesModel>();
                trades = srv.GetSubmittedTrades(OID);

                if (trades.Count > 0)
                    return Ok(trades);
                else
                    return Ok(null);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }



        [HttpGet]
        [Route("CheckTradeCodeScheme")]
        public IActionResult CheckTradeCodeScheme()
        {
            try
            {
                //List<object> ls = new List<object>();

                //SchemeModel scheme = srv.GetLastScheme(Convert.ToInt32(User.Identity.Name));
                //if (scheme == null)
                //{
                //    int seq = srv.GetSchemeSequence();
                //    return Ok(seq);
                //}

                //ls.Add(scheme);

                return Ok(srv.GetTradeCodeSequence());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Trade
        [HttpGet]
        [Route("RD_Trade")]
        public IActionResult RD_Trade()
        {
            try
            {
                return Ok(srv.FetchTrade(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Scheme/FinalSubmit
        [HttpGet]
        [Route("FinalSubmitTrade")]
        public IActionResult FinalSubmitTrade([FromQuery] int TradeID, [FromQuery] string ProcessKey)
        {
            // int SchemeID = JsonConvert.DeserializeObject<int>(schemeID);
            List<TradeModel> Trade = new List<TradeModel>();
           
            try
            {
                   Trade = srv.FinalSubmitTrade(new TradeModel() { TradeID = TradeID, FinalSubmitted = true, ProcessKey = ProcessKey, CurUserID = Convert.ToInt32(User.Identity.Name) });
                    var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = EnumApprovalProcess.TRD }).FirstOrDefault();
                    ApprovalModel approvalModel = new ApprovalModel();
                    ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                    approvalModel.ProcessKey = EnumApprovalProcess.NEW_TRD;
                    approvalModel.CurUserID = Convert.ToInt32(User.Identity.Name);
                    approvalModel.UserIDs = firstApproval.UserIDs;
                approvalModel.isUserMapping = true;
                srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);
               

                return Ok(Trade);
               
               
                //return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Trade
        [HttpPost]
        [Route("RD_TradeBy")]
        public IActionResult RD_TradeBy(TradeModel mod)
        {
            try
            {
                return Ok(srv.FetchTrade(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Trade/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TradeModel D)
        {
            

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.TradeID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveTrade(D));
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
        
        // POST: Trade/Save
        [HttpPost]
        [Route("SaveTradeDetail")]
        public IActionResult SaveTradeDetail(TradeModel D)
        {
            

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.TradeID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveTradeDetail(D));
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
        [Route("GetTradeMappedData/{id}")]
        public ActionResult GetTradeMappedData(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                TradeDetailMapModel Du = new TradeDetailMapModel();
                TradeEquipmentToolsMapModel Eq = new TradeEquipmentToolsMapModel();
                TradeConsumableMaterialMapModel Cs = new TradeConsumableMaterialMapModel();
                TradeSourceOfCurriculumMapModel Sc = new TradeSourceOfCurriculumMapModel();
                Du.TradeID = id;
                //Eq.TradeID = id;
                //Cs.TradeID = id;
                Sc.TradeID = id;
                ls.Add(srvTradeDetail.FetchTradeDetailMap(Du));

                ls.Add(srvTradeEquipmentTools.FetchTradeEquipmentToolsMap(Eq));

                ls.Add(srvTradeConsumableMaterial.FetchTradeConsumableMaterialMap(Cs));

                ls.Add(srvTradeSourceOfCurriculum.FetchTradeSourceOfCurriculumMap(Sc));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetTradeMapDetails/{id}")]
        public ActionResult GetTradeMapDetails(int id)
        {
            try
            {
                return Ok(new SRVTradeDetailMap().FetchTradeDetailMap(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        //[HttpGet]
        //[Route("GetDocument/{id}")]
        //public ActionResult GetDocument(string id)
        //{
        //    try
        //    {
        //        return Ok(new SRVTradeDetailMap().GetDocument(id));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }

        //[AllowAnonymous]

        //[HttpGet]
        //[Route("GetDocument/{id}")]
        //public IActionResult GetDocument(string id)
        //{
        //    try
        //    {
        //        var fileResult = new SRVTradeDetailMap().GetDocument(id);

        //        if (fileResult != null)
        //        {
        //            // You might want to set additional headers or properties, depending on your needs
        //            // For example, you might want to set Content-Disposition header for the file name
        //            // Response.Headers["Content-Disposition"] = "attachment; filename=" + fileResult.FileDownloadName;

        //            return fileResult;
        //        }
        //        else
        //        {
        //            // If the document is not found, return a 404 NotFound result
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        // Log the exception, and return a 500 Internal Server Error with the exception message
        //        // You might want to log the exception using a logging library like Serilog or log4net
        //        return StatusCode(500, e.InnerException?.ToString() ?? e.ToString());
        //    }
        //}

        //}

        // POST: Trade/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TradeModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {

                    srv.ActiveInActive(d.TradeID, d.InActive, Convert.ToInt32(User.Identity.Name));
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
        [Route("CheckTradeName")]
        public IActionResult CheckTradeName(TradeModel trade)
        {
            if (!String.IsNullOrEmpty(trade.TradeName))
            {
                List<TradeModel> u = srv.GetByTradeName(trade.TradeName);
                if (u == null)
                {
                    return Ok(true);
                }
                else
                {
                    if (u.Count > 1)
                        return Ok(false);
                    else
                    {
                        if (u[0].TradeID == trade.TradeID)
                        {
                            return Ok(true);
                        }
                        else
                            return Ok(false);
                    }
                }
            }
            else
                return BadRequest("Bad Request");
        }

        [HttpPost]
        [Route("CheckTradeCode")]
        public IActionResult CheckTradeCode(TradeModel trade)
        {
            if (!String.IsNullOrEmpty(trade.TradeCode))
            {
                List<TradeModel> u = srv.GetByTradeCode(trade.TradeCode);
                if (u == null)
                {
                    return Ok(true);
                }
                else
                {
                    if (u.Count > 1)
                        return Ok(false);
                    else
                    {
                        if (u[0].TradeID == trade.TradeID)
                        {
                            return Ok(true);
                        }
                        else
                            return Ok(false);
                    }
                }
            }
            else
                return BadRequest("Bad Request");
        }
        /// <summary>
        /// Added by Ali Haider
        /// 28-Mar-2024
        /// Change for TSP Trainee Intrest Portal
        /// </summary>
        /// <returns></returns>
        // RD: TradebyTSP
        [HttpGet]
        [Route("SSPRD_TradebyTSP/{programid}/{districtid}")]
        public IActionResult SSPRD_TradebyTSP(int programid, int districtid)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.FetchTradeTSP(programid, districtid, curUserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}