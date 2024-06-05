using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PBTEController : ControllerBase
    {
        private readonly ISRVPBTE srvPBTE ;
        private readonly ISRVTraineeResultStatusTypes srvResultStatusType;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVTSPDetail srvTSPDetail;
        private readonly ISRVClass srvClass;
        private readonly ISRVTrade srvTrade;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVSendEmail srvSendEmail;
        public PBTEController(ISRVSendEmail srvSendEmail,ISRVPBTE srvPBTE, ISRVTraineeResultStatusTypes srvResultStatusType,ISRVScheme srvScheme, ISRVTSPDetail srvTSPDetail,ISRVClass srvClass,ISRVTrade srvTrade, ISRVDistrict srvDistrict)
        {
            this.srvPBTE = srvPBTE;
            this.srvResultStatusType = srvResultStatusType;
            this.srvScheme = srvScheme;
            this.srvTSPDetail = srvTSPDetail;
            this.srvClass = srvClass;
            this.srvTrade = srvTrade;
            this.srvDistrict = srvDistrict;
            this.srvSendEmail = srvSendEmail;
        }
        // GET: PBTE
        //[HttpGet]
        //[Route("GetPBTE")]
        //public IActionResult GetPBTE()
        //{
        //    try
        //    {
        //        List<object> list = new List<object>();

        //        list.Add(srvPBTE.FetchPBTEClasses());
        //        list.Add(srvPBTE.FetchPBTETSPs());
        //        list.Add(srvPBTE.FetchPBTETrainees());
        //        list.Add(srvPBTE.FetchPBTEDropoutTrainees());
        //        list.Add(srvResultStatusType.FetchTraineeResultStatusTypes(false));
        //        list.Add(srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true, IsApproved = true }));
        //        list.Add(srvTSPDetail.FetchApprovedTSPs());
        //        list.Add(srvClass.FetchApprovedClass());
        //        list.Add(srvTrade.FetchTrade(new TradeModel() { IsApproved = true}));
        //        list.Add(srvDistrict.FetchDistrict(false));
        //        list.Add(srvPBTE.FetchNAVTTCClasses());
        //        list.Add(srvPBTE.FetchNAVTTCTSPs());
        //        list.Add(srvPBTE.FetchNAVTTCTrainees());
        //        list.Add(srvPBTE.FetchNAVTTCDropoutTrainees());
        //        list.Add(srvPBTE.FetchTradePBTE(new PBTETradeModel() { IsApproved = true }));
        //        //srv.UpdatePBTEDropoutTraineesLock();
        //        list.Add(srvPBTE.FetchPBTETraineesExamResult());
        //        list.Add(srvPBTE.FetchNAVTTCTraineesExamResult());

        //        return Ok(list);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //} 
        // GET: PBTE
        [HttpGet]
        [Route("GetPBTEFiltersData")]
        public IActionResult GetPBTEFiltersData()
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true, IsApproved = true }));
                list.Add(srvTSPDetail.FetchApprovedTSPs());
                list.Add(srvClass.FetchClassForPBTEFilter());
                list.Add(srvTrade.FetchTrade(new TradeModel() { IsApproved = true}));
                list.Add(srvDistrict.FetchDistrict(false));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("GetPBTEClasses")]
        public IActionResult GetPBTEClasses(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchPBTEClasses(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }        [HttpPost]
        [Route("GetPBTETSPs")]
        public IActionResult GetPBTETSPs(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchPBTETSPs(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("GetPBTETrainees")]
        public IActionResult GetPBTETrainees(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchPBTETrainees(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("GetPBTETraineesExamScriptData")]
        public IActionResult GetPBTETraineesExamScriptData(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchPBTETraineesExamScriptData(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetPBTETrades")]
        public IActionResult GetPBTETrades(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchTradePBTE(filters));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        } 
        [HttpPost]
        [Route("GetPBTEDropOutTrainees")]
        public IActionResult GetPBTEDropOutTrainees(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchPBTEDropoutTrainees(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("GetPBTEStatsData")]
        public IActionResult GetPBTEStatsData(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchPBTEStatsData(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("GetNAVTTCStatsData")]
        public IActionResult GetNAVTTCStatsData(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchNAVTTCStatsData(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("GetNAVTTCClasses")]
        public IActionResult GetNAVTTCClasses(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchNAVTTCClasses(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        } 
        [HttpPost]
        [Route("GetNAVTTCTSPs")]
        public IActionResult GetNAVTTCTSPs(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchNAVTTCTSPs(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        } 
        [HttpPost]
        [Route("GetNAVTTCTrainees")]
        public IActionResult GetNAVTTCTrainees(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchNAVTTCTrainees(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetNAVTTCTraineesSqlScriptData")]
        public IActionResult GetNAVTTCTraineesSqlScriptData(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchNAVTTCTraineesSqlScript(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetNAVTTCTraineesRegisterSqlScriptData")]
        public IActionResult GetNAVTTCTraineesRegisterSqlScriptData(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchNAVTTCTraineesRegisterSqlScript(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("GetNAVTTCDropOutTrainees")]
        public IActionResult GetNAVTTCDropOutTrainees(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchNAVTTCDropoutTrainees(filters));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("GetPBTETraineesExamResult")]
        public IActionResult GetPBTETraineesExamResult(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchPBTETraineesExamResult(filters));
                list.Add(srvResultStatusType.FetchTraineeResultStatusTypes(false));


                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("GetNAVTTCTraineesExamResult")]
        public IActionResult GetNAVTTCTraineesExamResult(PBTEQueryFilters filters)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchNAVTTCTraineesExamResult(filters));
                list.Add(srvResultStatusType.FetchTraineeResultStatusTypes(false));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpGet]
        [Route("GetPBTESchemes")]
        public IActionResult GetPBTESchemes()
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvPBTE.FetchPBTESchemes());

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdatePBTEClasses")]
        public IActionResult UpdatePBTEClasses(List<PBTEModel> mod)
        {
            try
            {
                srvPBTE.UpdatePBTEClasses(mod);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("UpdateNAVTTCClasses")]
        public IActionResult UpdateNAVTTCClasses(List<PBTEModel> mod)
        {
            try
            {
                srvPBTE.UpdateNAVTTCClasses(mod);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdatePBTETSPs")]
        public IActionResult UpdatePBTETSPs(List<PBTEModel> mod)
        {
            try
            {
                srvPBTE.UpdatePBTETSPs(mod);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdatePBTETrainees")]
        public IActionResult UpdatePBTETrainees(List<PBTEModel> mod)
        {
            try
            {
                srvPBTE.UpdatePBTETrainees(mod);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("UpdateNAVTTCTrainees")]
        public IActionResult UpdateNAVTTCTrainees(List<PBTEModel> mod)
        {
            try
            {
                srvPBTE.UpdateNAVTTCTrainees(mod);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("UpdatePBTETrades")]
        public IActionResult UpdatePBTETrades(List<PBTEModel> mod)
        {
            try
            {
                srvPBTE.UpdatePBTETrades(mod);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("UpdatePBTETraineesResult")]
        public IActionResult UpdatePBTETraineesResult(List<PBTEModel> mod)
        {
            try
            {
                //ApprovalModel approvalModel = new ApprovalModel();
                //ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                //approvalModel.ProcessKey = EnumApprovalProcess.EXAM_STATUS;
                //approvalModel.CurUserID = Convert.ToInt32(User.Identity.Name);
                //srvSendEmail.GenerateEmailToApprovers(approvalModel, approvalHistoryModel);
                int CurUserID= Convert.ToInt32(User.Identity.Name);
                srvPBTE.UpdatePBTETraineesResult(mod, CurUserID);
               
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }



        //// RD: PBTE
        //[HttpGet]
        //[Route("RD_PBTE")]
        //public IActionResult RD_PBTE()
        //{
        //    try
        //    {
        //        return Ok(srv.FetchPBTE(false));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //}
    }
}

