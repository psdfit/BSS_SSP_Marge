using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchemeController : ControllerBase
    {
        private readonly ISRVScheme srvScheme;
        private readonly ISRVProgramType srvProgramType;
        private readonly ISRVProgramCategory srvProgramCategory;
        private readonly ISRVFundingSource srvFundingSource;
        private readonly ISRVFundingCategory srvFundingCategory;
        private readonly ISRVGender srvGender;
        private readonly ISRVEducationTypes srvEducationTypes;
        private readonly ISRVOrganization srvOrganization;
        private readonly ISRVPaymentSchedule srvPaymentSchedule;
        private readonly ISRVTSPDetail srvTSPDetail;
        private readonly ISRVClass srvClass;
        private readonly ISRVInstructor srvInstructor;
        private readonly ISRVUsers srvUsers;

        public SchemeController(ISRVScheme srvScheme, ISRVProgramType srvProgramType, ISRVProgramCategory srvProgramCategory, ISRVFundingSource srvFundingSource, ISRVFundingCategory srvFundingCategory, ISRVGender srvGender, ISRVEducationTypes srvEducationTypes, ISRVOrganization srvOrganization, ISRVPaymentSchedule srvPaymentSchedule, ISRVTSPDetail srvTSPDetail, ISRVClass srvClass, ISRVInstructor srvInstructor, ISRVUsers srvUsers)
        {
            this.srvScheme = srvScheme;
            this.srvProgramType = srvProgramType;
            this.srvProgramCategory = srvProgramCategory;
            this.srvFundingSource = srvFundingSource;
            this.srvFundingCategory = srvFundingCategory;
            this.srvGender = srvGender;
            this.srvEducationTypes = srvEducationTypes;
            this.srvOrganization = srvOrganization;
            this.srvPaymentSchedule = srvPaymentSchedule;
            this.srvTSPDetail = srvTSPDetail;
            this.srvClass = srvClass;
            this.srvInstructor = srvInstructor;
            this.srvUsers = srvUsers;
        }

        // GET: Scheme
        [HttpGet]
        [Route("GetScheme")]
        public IActionResult GetScheme([FromQuery] int OID)

        {
            try
            {
                List<object> ls = new List<object>();
                // List<SchemeModel> draft = srvScheme.LoadNotSubmittedSchemes(Convert.ToInt32(User.Identity.Name)).OrderByDescending(x => x.SchemeID).ToList();
                //List<SchemeModel> submitted = srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true }).OrderByDescending(x => x.SchemeID).ToList();

                //List<SchemeModel> schemes = srvScheme.FetchScheme(new SchemeModel() { CreatedUserID = Convert.ToInt32(User.Identity.Name), OrganizationID = OID });
                List<SchemeModel> schemes = srvScheme.FetchAllScheme(new SchemeModel() { CreatedUserID = Convert.ToInt32(User.Identity.Name), OrganizationID = OID });
                //ls.Add(draft.Concat(submitted));
                ls.Add(schemes);

                ls.Add(srvProgramType.FetchProgramType(false));

                ls.Add(srvProgramCategory.FetchProgramCategory(false));

                ls.Add(srvFundingSource.FetchFundingSource(false));

                ls.Add(srvFundingCategory.FetchFundingCategory(false));

                ls.Add(srvGender.FetchGender(false));

                ls.Add(srvEducationTypes.FetchEducationTypes(false));

                //ls.Add(new SRVOrganization().FetchOrganization(false));
                ls.Add(srvOrganization.FetchOrganizationByUser(Convert.ToInt32(User.Identity.Name), OID));

                ls.Add(srvPaymentSchedule.GetPaymentSchedules());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetSchemeByUser/{id}")]
        public IActionResult GetSchemeByUser(int id)
        {
            try
            {
                return Ok(srvScheme.FetchSchemeDataByUser(id));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpGet]
        [Route("GetSchemeByKAM/{id}")]
        public IActionResult GetSchemeByKAM(int id)
        {
            try
            {
                return Ok(srvScheme.FetchSchemesByKAMUser(id));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        //[HttpGet]
        //[Route("GenerateInvoice")]
        //public IActionResult GenerateInvoice() // just for testing
        //{
        //    int SchemeID = 381;
        //    new GenerateInvoice().GenerateInvoices(SchemeID);
        //    return Ok();
        //}

        // RD: Scheme
        [HttpGet]
        [Route("RD_Scheme")]
        public IActionResult RD_Scheme()
        {
            try
            {
                return Ok(srvScheme.FetchScheme(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("RD_SchemeByOrg")]
        public IActionResult RD_SchemeByOrg([FromQuery] int OID)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                //curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                List<SchemeModel> schemes = new List<SchemeModel>();
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    schemes = srvScheme.FetchSchemesByTSPUser(curUserID);
                }
                else
                {
                    schemes = srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true, OrganizationID = OID, IsApproved = true });
                }
                return Ok(schemes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: Scheme
        [HttpGet]
        [Route("GetSchemesForAppendix")]
        public IActionResult GetSchemesForAppendix([FromQuery] int OID)
        {
            try
            {
                //List<SchemeModel> draft = srvScheme.LoadNotSubmittedSchemes(Convert.ToInt32(User.Identity.Name)).OrderByDescending(x => x.SchemeID).ToList();
                //List<SchemeModel> submitted = srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true }).OrderByDescending(x => x.SchemeID).ToList();

                List<SchemeModel> schemes = srvScheme.FetchScheme(new SchemeModel() { CreatedUserID = Convert.ToInt32(User.Identity.Name), OrganizationID = OID });

                //return Ok(draft.Concat(submitted));
                return Ok(schemes);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("GetAllSchemes")]
        public IActionResult GetAllSchemes()
        {
            try
            {
                List<SchemeModel> schemes = srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true });

                //return Ok(draft.Concat(submitted));
                return Ok(schemes);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }

        // RD: Scheme
        [HttpPost]
        [Route("RD_SchemeBy")]
        public IActionResult RD_SchemeBy(SchemeModel mod)
        {
            try
            {
                return Ok(srvScheme.FetchScheme(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetSchemeSequence")]
        public IActionResult GetSchemeSequence()
        {
            try
            {
                int seq = srvScheme.GetSchemeSequence();
                return Ok(seq);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: Scheme/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody] string D)
        {
            try
            {
                SchemeModel model = JsonConvert.DeserializeObject<SchemeModel>(D);
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvScheme.SaveScheme(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: Scheme/FinalSubmit
        [HttpGet]
        [Route("FinalSubmit")]
        public IActionResult FinalSubmit([FromQuery] int SchemeID, [FromQuery] string ProcessKey)
        {
            // int SchemeID = JsonConvert.DeserializeObject<int>(schemeID);

            try
            {
                //mod.CurUserID = Convert.ToInt32(User.Identity.Name);
                srvScheme.FinalSubmit(
                    new SchemeModel()
                    {
                        SchemeID = SchemeID,
                        FinalSubmitted = true,
                        ProcessKey = ProcessKey,
                        CurUserID = Convert.ToInt32(User.Identity.Name)
                    });
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: Scheme/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(SchemeModel d)
        {
            try
            {
                srvScheme.ActiveInActive(d.SchemeID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetSubmittedSchemes")]
        public IActionResult GetSubmittedSchemes([FromQuery] int OID)
        {
            //if (srv.CheckUserInFormApproval(Convert.ToInt32(User.Identity.Name)) == false)
            //    return Ok(false);

            try
            {
                //List<SubmittedSchemesModel> schemes = new List<SubmittedSchemesModel>();
                //schemes = srvScheme.GetSubmittedSchemes();
                //var schemes = srvScheme.FetchScheme(new SchemeModel() { IsApproved = false, FinalSubmitted = true, OrganizationID = OID });
                var schemes = srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true, OrganizationID = OID });

                return Ok(schemes);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // RD: Scheme
        [HttpGet]
        [Route("LoadNotSubmittedSchemes")]
        public IActionResult LoadNotSubmittedSchemes()
        {
            try
            {
                return Ok(srvScheme.FetchScheme(new SchemeModel() { CreatedUserID = Convert.ToInt32(User.Identity.Name), FinalSubmitted = false }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("FetchSchemeByUser")]
        public IActionResult FetchSchemeByUser(QueryFilters filters)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                filters.UserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                return Ok(srvScheme.FetchSchemeByUser(filters));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetUnverifiedTraineeEmail")]
        public IActionResult GetUnverifiedTraineeEmail(int? tspId)
        {
            try
            {
                return Ok(srvScheme.GetUnverifiedTraineeEmail(tspId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetUnverifiedTraineeEmailByKam/{KamId}")]
        public IActionResult GetUnverifiedTraineeEmailByKam(int KamId)
        {
            try
            {
                return Ok(srvScheme.GetUnverifiedTraineeEmailByKam(KamId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("FetchSchemeByUserPaged")]
        public IActionResult FetchSchemeByUserPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                filterModel.UserID = curUserID;

                List<object> ls = new List<object>();

                ls.Add(srvScheme.FetchSchemeByUserPaged(pagingModel, filterModel, out int totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region Appendix

        [HttpGet]
        [Route("GetAppendix/{id}")]
        public IActionResult GetAppendix(int id = 0)
        {
            try
            {
                List<object> ls = new List<object>();
                //ls.Add(new List<SchemeModel>() { srvScheme.GetBySchemeID(id) });
                ls.Add(srvScheme.FetchAllScheme(new SchemeModel() { SchemeID = id }));
                ls.Add(srvTSPDetail.FetchTSPDetailByScheme(id));
                ls.Add(srvClass.FetchClassByScheme(id));
                ls.Add(srvInstructor.FetchInstructorByScheme(id));
                //ls.Add(new SRVOrganization().FetchOrganizationByUser(Convert.ToInt32(User.Identity.Name)));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("DeleteDraftAppendix")]
        public IActionResult DeleteDraftAppendix([FromQuery] int SchemeID)
        {
            try
            {
                bool result = false;
                if (srvScheme.GetAllSchemeBySchemeID(SchemeID)?.FinalSubmitted == false)
                {
                    srvScheme.DeleteDraftAppendix(SchemeID);
                    result = true;
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion Appendix

        [HttpGet]
        [Route("FetchSchemeForFilter")]
        public IActionResult FetchSchemeForFilter()
        {
            try
            {
                return Ok(srvScheme.FetchSchemeForFilter(false));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //Added by Rao Ali Haider 27-05-2024
        [HttpGet]
        [Route("SSPFetchSchemeByUser")]
        public IActionResult SSPFetchSchemeByUser()
        
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvScheme.SSPFetchSchemeByUser(curUserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //-For scheme auto appendix
        [HttpGet]
        [Route("SSPFetchScheme")]
        public IActionResult SSPFetchScheme()
        {
            try
            {
                int curUserID = 0;
                List<object> ls = new List<object>();
                List<SchemeModel> schemes = srvScheme.SSPFetchSchemeByUser(curUserID);
                ls.Add(schemes);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GenerateAutoAppendix")]
        public IActionResult GenerateAutoAppendix([FromQuery] int SchemeID)
        {
            try
            {
                bool result = false;
                int curUserID = Convert.ToInt32(User.Identity.Name);
                srvScheme.GenerateAutoAppendix(SchemeID, curUserID);
                result = true;
                
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetSchemeByID")]
        public IActionResult GetSchemeByID()
        {
            try
            {
                var mod = new SchemeModel
                {
                    SchemeID = 22533
                };
                return Ok(srvScheme.FetchScheme(mod));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
    }
}