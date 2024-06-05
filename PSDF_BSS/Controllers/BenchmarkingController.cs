using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using PSDF_BSS.Logging;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BenchmarkingController : ControllerBase
    {
        private readonly ISRVBenchmarking srvBenchmarking ;
        private readonly ISRVDistrict srvDistrict ;
        private readonly ISRVCluster srvCluster;
        private readonly ISRVRegion srvRegion;
        private readonly ISRVYearWiseInflationRate srvInflationRate;
        private readonly ISRVTSPDetail srvTSPDetail;
        private readonly ISRVProgramType srvProgramType;

        public BenchmarkingController(ISRVBenchmarking srvBenchmarking, ISRVDistrict srvDistrict, ISRVCluster srvCluster, ISRVRegion srvRegion, ISRVYearWiseInflationRate srvInflationRate, ISRVOrganization srvOrganization, ISRVProgramType srvProgramType, ISRVTSPDetail srvTSPDetail)
        {
            this.srvBenchmarking = srvBenchmarking;
            this.srvDistrict = srvDistrict;
            this.srvCluster = srvCluster;
            this.srvRegion = srvRegion;
            this.srvInflationRate = srvInflationRate;
            this.srvTSPDetail = srvTSPDetail;
            this.srvProgramType = srvProgramType;
        }
        // GET: Benchmarking
        [HttpGet]
        [Route("GetBenchmarking")]
        public IActionResult GetBenchmarking()
        {
            try
            {
                List<object> ls = new List<object>
                {
                    srvDistrict.FetchDistrict(false),
                    srvCluster.FetchCluster(false),
                    srvRegion.FetchRegion(false),
                    srvInflationRate.FetchYearWiseInflationRate(false),
                    srvTSPDetail.FetchTSPMasterForFilters(false),
                    //srvTSPDetail.FetchTSPDetail(false),
                    srvProgramType.FetchProgramType(false)
                };

                //return Ok(srv.FetchBenchmarking());
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: Benchmarking
        [HttpGet]
        [Route("RD_Benchmarking")]
        public async Task<IActionResult> RD_Benchmarking()
        {
            try
            {
                return Ok(await Task.FromResult(srvBenchmarking.FetchBenchmarking(false)));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: Benchmarking
        [HttpPost]
        [Route("RD_BenchmarkingBy")]
        public async Task<IActionResult> RD_BenchmarkingBy(BenchmarkingModel mod)
        {
            try
            {
                return Ok(await Task.FromResult( srvBenchmarking.FetchBenchmarking(mod)));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: Benchmarking/Save
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(BenchmarkingModel D)
        {
            

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.BenchmarkingID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(await Task.FromResult(srvBenchmarking.SaveBenchmarking(D)));
                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException.ToString());
                }

            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        // POST: Benchmarking/Save
        [HttpPost]
        [Route("BenchmarkingData")]
        public async Task<IActionResult> BenchmarkingData(BenchmarkingModel mod)
        {
            try
            {
                //mod.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(await Task.FromResult(srvBenchmarking.FetchBenchmarking(mod)));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("BenchmarkingClasses")]
        public async Task<IActionResult> BenchmarkingClasses(BenchmarkingModel mod)
        {
            try
            {
                //mod.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(await Task.FromResult(srvBenchmarking.FetchBenchmarkingClasses(mod)));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


    }
}
			
        

 //// POST: Benchmarking/ActiveInActive
 //       [HttpPost]
	//	[Route("ActiveInActive")]
 //       public async Task<IActionResult> ActiveInActive(BenchmarkingModel d)
 //       {
 //           try
 //           {		
 //                   srv.ActiveInActive(d.BenchmarkingID,d.InActive, Convert.ToInt32(User.Identity.Name));
 //                  return Ok(true);
                
 //           }
 //           catch(Exception e)
 //           {
 //               return BadRequest(e.InnerException.ToString());
 //           }
 //       }
	//	}
	//	}
		
