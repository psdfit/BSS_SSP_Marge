using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.API.Models;

namespace PSDF_BSS.API.Controllers
{
    public class LookUpController : Controller
    {
        private readonly ISRVDistrict _srvDistrict;
        private readonly ISRVGender _srvGender;
        private readonly ISRVEducationTypes _srvEducationTypes;
        private readonly ISRVReligion _srvReligion;
        private readonly ISRVTehsil _srvTehsil;
        public LookUpController(ISRVDistrict srvDistrict, ISRVGender srvGender, ISRVEducationTypes srvEducationTypes, ISRVReligion srvReligion, ISRVTehsil srvTehsil)
        {
            _srvDistrict = srvDistrict;
            _srvGender = srvGender;
            _srvEducationTypes = srvEducationTypes;
            _srvReligion = srvReligion;
            _srvTehsil = srvTehsil;
        }
        //[HttpGet("~/api/LookUp/ListOfDistricts")]
        public IActionResult GetAllDistricts()
        {
            //int userID = Convert.ToInt32(User.Identity.Name);
            var list = _srvDistrict.FetchDistrict(false)
                .Select(x => new
                {
                    x.DistrictID,
                    x.DistrictName,
                    x.DistrictNameUrdu
                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = list
            });
        }
        [HttpGet("~/api/LookUp/ListOfGenders")]
        public IActionResult GetAllGenders()
        {
            //int userID = Convert.ToInt32(User.Identity.Name);
            var list = _srvGender.FetchGender(false)
                .Select(x => new
                {
                    x.GenderID,
                    x.GenderName
                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = list
            });
        }
        [HttpGet("~/api/LookUp/ListOfEducationTypes")]
        public IActionResult GetAllEducationTypes()
        {
            //int userID = Convert.ToInt32(User.Identity.Name);
            var list = _srvEducationTypes.FetchEducationTypes(false)
                .Select(x => new
                {
                    x.EducationTypeID,
                    Name = x.Education
                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = list
            });
        }

        [HttpGet("~/api/LookUp/GetTehsilByDistrictID/{districtID}")]
        public IActionResult GetTehsilByDistrictID(int districtID)
        {
            int userID = Convert.ToInt32(User.Identity.Name);
            var list = _srvTehsil.GetByDistrictIDApi(districtID)
                .Select(x => new
                {
                    x.TehsilID,
                    Name = x.TehsilName
                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = list
            });
        }

        [HttpGet("~/api/LookUp/ListDistrictsAndTehsils")]
        public IActionResult AllDistrictsAndTehsils()
        {
            int userID = Convert.ToInt32(User.Identity.Name);
            var list = _srvDistrict.AllDistrictsAndTehsils()
                .Select(x => new
                {
                    DistrictID = x.DistrictID,
                    DistrictName = x.DistrictName,
                    DistrictNameUrdu = x.DistrictNameUrdu,
                    TehsilID = x.TehsilID,
                    TehsilName = x.TehsilName,
                    TehsilNameUrdu = x.TehsilNameUrdu
                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = list
            });
        }

        [HttpGet("~/api/LookUp/Religion")]
        public IActionResult GetAllReligion()
        {
            int userID = Convert.ToInt32(User.Identity.Name);
            var list = _srvReligion.FetchReligion(false)
                .Select(x => new
                {
                    x.ReligionID,
                    Name = x.ReligionName
                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = list
            });
        }

    }
}
