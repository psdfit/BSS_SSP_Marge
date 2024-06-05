using System;
using DataLayer.Interfaces;
using DataLayer.Models.SSP;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowController : ControllerBase
    {
        private readonly ISRVWorkflow srvWorkflow;

        public WorkflowController(ISRVWorkflow srvWorkflow )
        {
            this.srvWorkflow = srvWorkflow;
        }


        [HttpPost]
        [Route("Save")]
        public IActionResult Save(WorkflowModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvWorkflow.Save(data));
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
                    return Ok(srvWorkflow.RemoveTaskDetail(data));
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
        [Route("FetchData")]
        public IActionResult FetchData(WorkflowModel data)
        {

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    var workflow = srvWorkflow.FetchDataListBySPName("RD_SSPWorkflow");
                    var workflowTask = srvWorkflow.FetchDataListBySPName("RD_SSPWorkflowTask");
                    var sourcingType = srvWorkflow.FetchDataListBySPName("RD_SSPPlaningType");
                    var DataObj = new
                    {
                        workflow = workflow,
                        workflowTask = workflowTask,
                        sourcingType = sourcingType,
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
                    var workflow = srvWorkflow.FetchDataListBySPName("RD_SSPWorkflow");
                
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
