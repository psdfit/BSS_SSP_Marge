using System;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MobileAppDownloadController : ControllerBase
    {
        private readonly ISRVMobileAppDownload srvDownload;

        public MobileAppDownloadController(ISRVMobileAppDownload srvDownload)
        {
            this.srvDownload = srvDownload;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetMobileApp")]
        public IActionResult GetMobileApp()
        {
            try
            {
                var fileBytes = srvDownload.GetMobileApp();
                return File(fileBytes, "application/zip", "myApp.zip");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
