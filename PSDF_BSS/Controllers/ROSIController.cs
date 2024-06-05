using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ROSIController : ControllerBase
    {
        private readonly ISRVROSI srvROSI;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVTSPDetail srvTSPDetail;
        private readonly ISRVClass srvClass;
        private readonly ISRVProgramType srvProgramType;
        private readonly ISRVSector srvSector;
        private readonly ISRVCluster srvCluster;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTrade srvTrade;
        private readonly ISRVOrganization srvOrganization;
        private readonly ISRVFundingSource srvFundingSource;
        private readonly ISRVGender srvGender;
        private readonly ISRVDuration srvDuration;

        public ROSIController(ISRVROSI srvROSI, ISRVScheme srvScheme, ISRVTSPDetail srvTSPDetail,
            ISRVClass srvClass, ISRVProgramType srvProgramType, ISRVSector srvSector,
            ISRVCluster srvCluster, ISRVDistrict srvDistrict, ISRVTrade srvTrade, ISRVOrganization srvOrganization,
            ISRVFundingSource srvFundingSource, ISRVGender srvGender, ISRVDuration srvDuration)
        {
            this.srvROSI = srvROSI;
            this.srvScheme = srvScheme;
            this.srvTSPDetail = srvTSPDetail;
            this.srvClass = srvClass;
            this.srvProgramType = srvProgramType;
            this.srvSector = srvSector;
            this.srvCluster = srvCluster;
            this.srvDistrict = srvDistrict;
            this.srvTrade = srvTrade;
            this.srvOrganization = srvOrganization;
            this.srvFundingSource = srvFundingSource;
            this.srvGender = srvGender;
            this.srvDuration = srvDuration;

        }
        //GET: ROSI
        [HttpGet]
        [Route("GetROSI")]
        public IActionResult GetROSI()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvScheme.FetchSchemeForFilter(false));
                ls.Add(srvTSPDetail.FetchTSPDetail(false));
                ls.Add(srvClass.FetchClass(false));
                ls.Add(srvProgramType.FetchProgramType(false));
                ls.Add(srvSector.FetchSector(false));
                ls.Add(srvCluster.FetchCluster(false));
                ls.Add(srvDistrict.FetchDistrict(false));
                ls.Add(srvTrade.FetchTrade(false));
                ls.Add(srvOrganization.FetchOrganization(false));
                ls.Add(srvFundingSource.FetchFundingSource(false));
                ls.Add(srvGender.FetchGender(false));
                ls.Add(srvDuration.FetchDuration(false));
                // ls.Add(srvROSI.FetchROSI());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        //GET: ROSI
        [HttpPost]
        [Route("GetROSIFiltersData")]
        public IActionResult GetROSIFiltersData(ROSIFiltersModel rosiFilters)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvScheme.FetchSchemeForROSIFilter(rosiFilters));
                ls.Add(srvTSPDetail.FetchTSPDetailForROSIFilter(rosiFilters));
                //ls.Add(srvClass.FetchClass(false));
                ls.Add(srvProgramType.FetchPTypeForROSIFilter(rosiFilters));
                ls.Add(srvSector.FetchSectorForROSIFilter(rosiFilters));
                ls.Add(srvCluster.FetchClusterForROSIFilter(rosiFilters));
                ls.Add(srvDistrict.FetchDistrictForROSIFilter(rosiFilters));
                ls.Add(srvTrade.FetchTradeForROSIFilter(rosiFilters));
                ls.Add(srvOrganization.FetchOrganization(false));
                ls.Add(srvFundingSource.FetchFundingSourceForROSIFilter(rosiFilters));
                ls.Add(srvGender.FetchGenderForROSIFilter(rosiFilters));
                ls.Add(srvDuration.FetchDurationForROSIFilter(rosiFilters));

                // ls.Add(srvROSI.FetchROSI());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        //[HttpGet]
        //[Route("FilteredROSI/{filter}")]
        //public IActionResult FilteredROSI([FromQuery(Name = "filter")]int[] filter)
        //{
        //    try
        //    {
        //        return Ok(srvROSI.FetchROSIByFilters(filter));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}


        // RD: HomeStats
        [HttpGet]
        [Route("RD_ROSI")]
        public IActionResult RD_ROSI()
        {
            try
            {
                return Ok(srvROSI.FetchROSI(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetROSIByScheme/{SchemeID}")]
        public IActionResult GetROSIByScheme(int SchemeID)
        {
            try
            {

                return Ok(srvROSI.FetchROSIByScheme(SchemeID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetROSIByTSP/{TSPID}")]
        public IActionResult GetROSIByTSP(int TSPID)
        {
            try
            {

                return Ok(srvROSI.FetchROSIByTSP(TSPID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("GetROSIByClass/{ClassID}")]
        public IActionResult GetROSIByClass(int ClassID)
        {
            try
            {

                return Ok(srvROSI.FetchROSIByClass(ClassID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetROSIByFilters")]
        public IActionResult GetROSIByFilters(ROSIFiltersModel mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvROSI.FetchROSIFilters(mod));
                //return Ok(srv.FetchMasterSheetByFilters(filter));
                //ls.Add(new SRVScheme().FetchSchemeForFilter(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetROSICalculationByFilters")]
        public IActionResult GetROSICalculationByFilters(ROSIFiltersModel mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvROSI.FetchCalculatedROSIByFilters(mod));
                //return Ok(srv.FetchMasterSheetByFilters(filter));
                //ls.Add(new SRVScheme().FetchSchemeForFilter(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetROSICalculationDataSetByFilters")]
        public IActionResult GetROSICalculationDataSetByFilters(ROSIFiltersModel mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvROSI.FetchCalculatedROSIDataSetByFilters(mod));
                //return Ok(srv.FetchMasterSheetByFilters(filter));
                //ls.Add(new SRVScheme().FetchSchemeForFilter(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: HomeStats
        [HttpPost]
        [Route("RD_ROSIBy")]
        public IActionResult RD_ROSIBy(ROSIModel mod)
        {
            try
            {
                return Ok(srvROSI.FetchROSI(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("FetchFilteredRosi")]
        public IActionResult FetchFilteredRosi(QueryFilters filters)
        {
            try
            {
                return Ok(srvROSI.FetchROSIByFilters(filters));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
