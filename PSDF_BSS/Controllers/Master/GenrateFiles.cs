using DataLayer.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenrateFiles : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("CreateClasses/{id}")]
        public void CreateClasses(string id)
        {
            GenrateCode.GenrateClasses(id, "MasterDataModule");
        }
    }
}