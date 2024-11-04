using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PSDF_BSSMasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private ISRVInstructor srvInstructor = null;
        private readonly ISRVGender srvGender;
        private readonly ISRVTrade srvTrade;
        private readonly ISRVInstructorMaster srvInstructorMaster;
        private readonly ISRVInstructorChangeRequest srvInstructorChangeRequest;

        public InstructorController(ISRVInstructor srvInstructor, ISRVGender srvGender, ISRVTrade srvTrade, ISRVInstructorMaster srvInstructorMaster, ISRVInstructorChangeRequest srvInstructorChangeRequest)
        {
            this.srvInstructor = srvInstructor;
            this.srvGender = srvGender;
            this.srvTrade = srvTrade;
            this.srvInstructorMaster = srvInstructorMaster;
            this.srvInstructorChangeRequest = srvInstructorChangeRequest;
        }

        // GET: Instructor
        [HttpGet]
        [Route("GetInstructor")]
        public IActionResult GetInstructor()
        {
            try
            {
                List<object> ls = new List<object>();

                //ls.Add(srvInstructor.FetchInstructor());

                ls.Add(srvGender.FetchGender(false));
                ls.Add(srvTrade.FetchTrade(new TradeModel() { IsApproved = true }));
                ls.Add(srvInstructorMaster.FetchInstructorMaster(false));
                //ls.Add(new SRVTSPDetail().FetchTSPDetailByScheme(id));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetInstructorCNICs")]
        public IActionResult GetInstructorCNICs()
        {
            try
            {
                List<object> ls = new List<object>();

                //ls.Add();
                ls.Add(srvInstructor.FetchOldInstructorCNICs());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetInstructorByUser/{id}")]
        public IActionResult GetInstructorByUser(int id)
        {
            try
            {
                int userid = Convert.ToInt32(User.Identity.Name);
                List<object> ls = new List<object>();
                ls.Add(srvInstructor.FetchInstructorDataByUser(id));
                ls.Add(srvGender.FetchGender(false));
                ls.Add(srvTrade.FetchTrade(new TradeModel() { IsApproved = true }));
                ls.Add(srvInstructorChangeRequest.FetchNewInstructorChangeRequest(userid));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost]
        [Route("GetCRInstructorByUser")]
        public IActionResult GetCRInstructorByUser(InstructorCRFiltersModel mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvInstructor.FetchCRInstructorDataByUser(mod));
                //ls.Add(srvGender.FetchGender(false));
                ls.Add(srvTrade.FetchTrade(new TradeModel() { IsApproved = true }));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        // RD: Instructor
        [HttpGet]
        [Route("RD_Instructor")]
        public IActionResult RD_Instructor()
        {
            try
            {
                return Ok(srvInstructor.FetchInstructor(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: Instructor
        [HttpGet]
        [Route("RD_InstructorBy_ID/{id}")]
        public IActionResult RD_InstructorBy_ID(int id)
        {
            try
            {
                return Ok(srvInstructor.GetByInstructorID(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // RD: Instructor
        [HttpGet]
        [Route("RD_Instructor_ByClassID_InceptionReport/{id}")]
        public IActionResult RD_Instructor_ByClassID_InceptionReport(int id)
        {
            try
            {
                return Ok(srvInstructor.GetIRInstructorByClassID(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: Instructor
        [HttpGet]
        [Route("RD_Instructor_ByTradeID/{id}")]
        public IActionResult RD_Instructor_ByTrade(int id)
        {
            try
            {
                return Ok(srvInstructor.GetInstructorByTradeID(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: Instructor
        [HttpPost]
        [Route("RD_InstructorBy")]
        public IActionResult RD_InstructorBy(InstructorModel mod)
        {
            try
            {
                return Ok(srvInstructor.FetchInstructor(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: Instructor/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody] string str)
        {
            List<InstructorModel> ls = new List<InstructorModel>();
            try
            {
                InstructorModel[] model = JsonConvert.DeserializeObject<InstructorModel[]>(str);
                foreach (var row in model)
                {
                    row.CurUserID = Convert.ToInt32(User.Identity.Name);
                    ls.Add(srvInstructor.SaveInstructor(row));
                }
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: Instructor/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(InstructorModel d)
        {
            try
            {
                srvInstructor.ActiveInActive(d.InstrID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetInstructorsBySchemeID/{id}")]
        public IActionResult GetInstructorsBySchemeID(int id)
        {
            try
            {
                List<InstructorModel> m = new List<InstructorModel>();

                m = srvInstructor.FetchInstructorByScheme(id);

                return Ok(m);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetClassCodeByInstrID/{instrID}")]
        public IActionResult GetClassCodeByInstrID(int instrID)
        {
            try
            {
                // Call the service method to get the ClassCode
                var classCodes = srvInstructor.GetClassCodeByInstrID(instrID);
                return Ok(classCodes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}