using System;
using System.Data;
using DataLayer.Interfaces;
using DataLayer.Models.SSP;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssociationController : ControllerBase
    {
        private readonly ISRVAssociation srvAssociation;

        public AssociationController(ISRVAssociation srvAssociation)
        {
            this.srvAssociation = srvAssociation;
        }


        [HttpPost]
        [Route("SaveAssociationSubmission")]
        public IActionResult SaveAssociationSubmission(AssociationSubmissionModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvAssociation.SaveAssociationSubmission(data));
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
        [Route("SaveAssociationEvaluation")]
        public IActionResult SaveAssociationEvaluation(AssociationEvaluationModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvAssociation.SaveAssociationEvaluation(data));
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
        [Route("SaveTSPAssignment")]
        public IActionResult SaveTSPAssignment(TSPAssignmentModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    srvAssociation.SaveTSPAssignment(data);
                    return Ok("{\"Status\":\"200\"}");

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
        [Route("LoadData")]
        public IActionResult LoadData()
        {
            try
            {


                var programDesignSummary = srvAssociation.FetchDataListBySPName("RD_SSPProgramDesignSummary");
                var TSPAssignment = srvAssociation.FetchDataListBySPName("RD_SSPTSPAssignment");

                var data = new
                {

                    TSPAssignment = TSPAssignment,
                    programDesignSummary = programDesignSummary,

                };

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }


        [HttpPost]
        [Route("FetchReport")]
        public IActionResult FetchReport([FromBody] dynamic param)
        {

            string[] split = HttpContext.Request.Path.Value.Split("/");
            bool isAuthorized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), split[2], split[3]);

            if (isAuthorized)
            {
                try
                {
                    string spName = param.SpName.Value;
                    int value = Convert.ToInt32(param.TspAssociationMaster.Value);

                    DataTable dataTable = srvAssociation.FetchReportBySPNameAndParam(spName, "TspAssociationMaster", value);
                    string[] attachments = new string[] { "Evidence" };
                    var dataWithAttachment = srvAssociation.LoopingData(dataTable, attachments);
                    return Ok(dataWithAttachment);
                }
                catch (Exception e)
                {
                    return BadRequest($"Error: {e.Message}");
                }
            }
            else
            {
                return BadRequest("Access Denied. You are not authorized for this activity");
            }
        }

        private object ProcessRequest(string param)
        {
            return new { Message = "Processed successfully", ParamReceived = param };
        }
    }




}
