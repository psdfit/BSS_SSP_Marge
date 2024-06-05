using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models.SSP;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriteriaTemplateController : ControllerBase
    {
        private readonly ISRVCriteriaTemplate srvCriteria;

        public CriteriaTemplateController(
            ISRVCriteriaTemplate srvCriteria
        )
        {
            this.srvCriteria = srvCriteria;
        }

        
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(CriteriaTemplateModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false,Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvCriteria.SaveCriteriaTemplate(data));
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
        [HttpPost]
        [Route("removeMainCategory")]
        public IActionResult removeMainCategory(CriteriaMainCategory data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false,Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    return Ok(srvCriteria.RemoveMainCategory(data));
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

        [HttpPost]
        [Route("removeSubCategory")]
        public IActionResult removeSubCategory(CriteriaSubCategory data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    return Ok(srvCriteria.RemoveSubCategory(data));
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


        [HttpPost]
        [Route("FetchCriteriaTemplate")]
        public IActionResult FetchCriteriaTemplate(CriteriaTemplateModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false,Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    var criteriaHeader       = srvCriteria.FetchDataListBySPName("RD_SSPCriteriaTemplate");
                    var criteriaMainCategory = srvCriteria.FetchDataListBySPName("RD_SSPCriteriaMainCategory");
                    string[] attachmentColumns = { "Attachment" };
                    var subCatgoryData = srvCriteria.FetchDataListBySPName("RD_SSPCriteriaSubCatgoryForTemplate");
                    var criteriaSubCategory = srvCriteria.LoopinData(subCatgoryData, attachmentColumns);
                    var DataObj = new {
                        criteriaHeader=criteriaHeader,
                        criteriaMainCategory = criteriaMainCategory,
                        criteriaSubCategory = criteriaSubCategory,
                    };
                    return Ok(DataObj);
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
        [HttpPost]
        [Route("LoadCriteria")]
        public IActionResult LoadCriteria(CriteriaTemplateModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false,Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    var criteriaHeader       = srvCriteria.FetchDataListBySPName("RD_SSPCriteriaTemplate");

                    return Ok(criteriaHeader);
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

       
    }
}
