using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PSDF_BSS.API.Controllers
{
    [ApiController]
    public class SchemeController : ControllerBase
    {
        private readonly ISRVScheme _srvScheme;
        public SchemeController(ISRVScheme srvScheme)
        {
            _srvScheme = srvScheme;
        }
        [HttpGet("~/api/scheme/getall")]
        public IActionResult GetSchemes()
        {            
            int userID = Convert.ToInt32(User.Identity.Name);
            var schemes = _srvScheme.FetchSchemeByUser_DVV(userID )
                .Select(x => new
                {
                    x.SchemeID,
                    x.SchemeName,
                    x.SchemeCode,
                    //x.tradecode,
                    //x.tradename
                }).ToList();
            //Dictionary<string, object> list = new Dictionary<string, object>();
            //list.Add("schemes", schemes);
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = schemes
            });
        }
    }
}
