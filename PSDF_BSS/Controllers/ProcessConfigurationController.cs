using System;
using DataLayer.Interfaces;
using DataLayer.Models.SSP;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessConfigurationController : ControllerBase
    {
        private readonly ISRVProcessConfiguration srvProcessConfig;

        public ProcessConfigurationController(ISRVProcessConfiguration srvProcessConfig)
        {
            this.srvProcessConfig = srvProcessConfig;
        }


        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ProcessScheduleModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvProcessConfig.Save(data));
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
        [Route("removeTaskDetail")]
        public IActionResult removeTaskDetail(TaskDetail data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    return Ok(srvProcessConfig.RemoveTaskDetail(data));
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
        public IActionResult LoadData(ProcessScheduleModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    var processScheduleDetail = srvProcessConfig.FetchDataListBySPName("RD_SSPProcessScheduleDetail");
                    var processScheduleMaster = srvProcessConfig.FetchDataListBySPName("RD_SSPProcessScheduleMaster");
                    var DataObj = new
                    {
                        processScheduleDetail = processScheduleDetail,
                        processScheduleMaster = processScheduleMaster,
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
        [Route("LoadWorkflow")]
        public IActionResult LoadWorkflow(WorkflowModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    var workflow = srvProcessConfig.FetchDataListBySPName("RD_SSWorkflow");

                    return Ok(workflow);
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
