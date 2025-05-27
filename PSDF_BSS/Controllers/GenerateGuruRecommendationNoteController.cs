using System;
using System.Collections.Generic;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Models.DVV;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerateGuruRecommendationNoteController : ControllerBase
    {
        private readonly ISRVGRN _srvGRN;

        public GenerateGuruRecommendationNoteController(ISRVGRN srvGRN)
        {
            _srvGRN = srvGRN;
        }

        [HttpPost]
        [Route("RD_GuruRecommendationNote")]
        public IActionResult FetchClassesForGRNCompletion([FromBody] GuruClassModel model)
        {
            string[] pathSegments = HttpContext.Request.Path.Value.Split("/");
            if (pathSegments.Length < 4)
            {
                return BadRequest("Invalid request path.");
            }

            bool isAuthorized = Authorize.CheckAuthorize(
                false,
                Convert.ToInt32(User.Identity.Name),
                pathSegments[2],
                pathSegments[3]);

            if (!isAuthorized)
            {
                return BadRequest("Access Denied. You are not authorized for this activity.");
            }

            try
            {
                var result = _srvGRN.FetchClassesForGRNCompletion(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
