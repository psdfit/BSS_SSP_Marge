using System;
using DataLayer.Interfaces;
using DataLayer.Models.SSP;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ISRVPayment srvPayment;

        public PaymentController(ISRVPayment srvPayment)
        {
            this.srvPayment = srvPayment;
        }


        [HttpPost]
        [Route("SaveRegistrationPayment")]
        public IActionResult SaveRegistrationPayment(RegistrationPaymentModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    srvPayment.SaveRegistrationPayment(data);
                    return Ok("{\"Status\":\"200\"}");
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
         [HttpPost]
        [Route("SaveAssociationPayment")]
        public IActionResult SaveAssociationPayment(AssociationPaymentModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    srvPayment.SaveAssociationPayment(data);
                    return Ok("{\"Status\":\"200\"}");
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

     

        [HttpPost]
        [Route("LoadData")]
        public IActionResult LoadData()
        {
            try
            {


                var programDesignSummary = srvPayment.FetchDataListBySPName("RD_SSPProgramDesignSummary");
                var TSPAssignment = srvPayment.FetchDataListBySPName("RD_SSPTSPAssignment");

                var data = new
                {

                    TSPAssignment = TSPAssignment,
                    programDesignSummary = programDesignSummary,

                };

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }



    }
}
