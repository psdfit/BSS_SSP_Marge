using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TierController : Controller
    {
        private readonly ISRVTier _srvTier;
        public TierController(ISRVTier srvTier)
        {
            _srvTier = srvTier;
        }
        [HttpGet]
        [Route("GetVoilationSummaryReport")]
        public async Task<IActionResult> GetVoilationSummaryReport()
        {
            try
            {
                
                var summary = await _srvTier.GetVoilationSummaryReport();
                return Ok(summary);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }
        [HttpGet]
        [Route("GetCompletionReport")]
        public async Task<IActionResult> GetCompletionReport()
        {
            try
            {

                var summary = await _srvTier.GetCompletionReport();
                return Ok(summary);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }
        [HttpGet]
        [Route("GetEmploymentRatioReport")]
        public async Task<IActionResult> GetEmploymentRatioReport()
        {
            try
            {

                var summary = await _srvTier.GetEmploymentRatioReport();
                return Ok(summary);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }
        [HttpGet]
        [Route("GetPerformanceAnalysisReport")]
        public async Task<IActionResult> GetPerformanceAnalysisReport()
        {
            try
            {

                var summary = await _srvTier.GetPerformanceAnalysisReport();
                return Ok(summary);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }
        [HttpGet]
        [Route("TSPPerformanceDatePeriod")]
        public async Task<IActionResult> TSPPerformanceDatePeriod()
        {
            try
            {

                var DatePeriod =  _srvTier.TSPPerformanceDatePeriod();
                return Ok(DatePeriod);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }
        [HttpPost]
        [Route("GetTSPPerformance")]
        public async Task<IActionResult> GetTSPPerformance(GetTSPPerformanceModel model)
        {
            try
            {
                var List =  _srvTier.GetTSPPerformance(model);
                return Ok(List);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }
        [HttpPost]
        [Route("FetchTradeDetailByTSP")]
        public async Task<IActionResult> FetchTradeDetailByTSP(GetTSPPerformanceModel model)
        {
            try
            {
                var List = _srvTier.FetchTradeDetailByTSP(model);
                return Ok(List);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }
    }
}