using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace PSDF_BSSRegistration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TSPDetailController : ControllerBase
    {
        private ISRVTSPDetail srvTSPDetail;
        private ISRVTSPMaster srvTspMaster;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTier srvTier;
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVUsers srvUsers;

        public TSPDetailController(ISRVTSPDetail srv, ISRVTSPMaster srvTspMaster, ISRVDistrict srvDistrict, ISRVTier srvTier, ISRVTSPMaster srvTSPMaster, ISRVUsers srvUsers)
        {
            this.srvTSPDetail = srv;
            this.srvTspMaster = srvTspMaster;
            this.srvDistrict = srvDistrict;
            this.srvTier = srvTier;
            this.srvTSPMaster = srvTSPMaster;
            this.srvUsers = srvUsers;
        }

        // GET: TSPDetail
        [HttpGet]
        [Route("GetTSPDetail")]
        public IActionResult GetTSPDetail()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvTSPDetail.FetchTSPDetail());
                ls.Add(srvDistrict.FetchDistrict(false));
                ls.Add(srvTier.FetchTier(false));
                ls.Add(srvTSPMaster.FetchTSPMaster(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }       // GET: TSPDetail

        [HttpGet]
        [Route("GetTSPs")]
        public IActionResult GetTSPs()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvTSPDetail.FetchTSPDetail());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetFilteredTSPs/{filter}")]
        public IActionResult GetFilteredTSPs([FromQuery(Name = "filter")] int[] filter)
        {
            //List<TraineeProfileModel> list = new List<TraineeProfileModel>();
            try
            {
                return Ok(srvTSPDetail.FetchTSPsByFilters(filter));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: TSPDetail
        [HttpGet]
        [Route("RD_TSPDetail")]
        public IActionResult RD_TSPDetail()
        {
            try
            {
                return Ok(srvTSPDetail.FetchTSPDetail(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTSPs/{id}")]
        public ActionResult GetTSPs(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvTSPDetail.FetchTSPListByKamUser(id));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: TSPDetail
        [HttpPost]
        [Route("RD_TSPDetailBy")]
        public IActionResult RD_TSPDetailBy(TSPDetailModel mod)
        {
            try
            {
                return Ok(srvTSPDetail.FetchTSPDetail(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Update: TSPDetail
        [HttpPost]
        [Route("Update_TSPDetail")]
        public IActionResult Update_TSPDetail(TSPDetailModel mod)
        {
            try
            {
                srvTSPDetail.UpdateTSPDetail(mod);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: TSPDetail/Save

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody] string str)
        {
            try
            {
                TSPDetailModel[] model = JsonConvert.DeserializeObject<TSPDetailModel[]>(str);
                List<TSPDetailModel> ls = new List<TSPDetailModel>();
                TSPMasterModel tspMasterModel = new TSPMasterModel();
                foreach (var item in model)
                {
                    item.CurUserID = Convert.ToInt32(User.Identity.Name);
                    ls.Add(srvTSPDetail.SaveTSPDetail(item));
                    //tspMasterModel.TSPMasterID = item.TSPID;
                    //tspMasterModel.TSPName = item.TSPName;
                    //tspMasterModel.Address = item.Address;
                    //tspMasterModel.NTN = item.NTN;
                    //tspMasterModel.PNTN = item.PNTN;
                    //tspMasterModel.GST = Convert.ToInt32(item.GST);
                    //tspMasterModel.FTN = item.FTN;
                    //tspMasterModel.InActive = false;
                    //srvTspMaster.SaveTSPMaster(tspMasterModel);
                }
                return Ok(ls);

                // D.CurUserID = Convert.ToInt32(User.Identity.Name);
                //return Ok(srv.SaveTSPDetail(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: TSPDetail/Save

        [HttpPost]
        [Route("SaveTSPMaster")]
        public IActionResult SaveTSPMaster(TSPDetailModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvTSPDetail.SaveTSPMaster(D));

                // D.CurUserID = Convert.ToInt32(User.Identity.Name);
                //return Ok(srv.SaveTSPDetail(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: TSPDetail/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TSPDetailModel d)
        {
            try
            {
                srvTSPDetail.ActiveInActive(d.TSPID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTSPSequence/{noOfSeqs}")]
        public IActionResult GetTSPSequence(int noOfSeqs)
        {
            try
            {
                int[] seqs = new int[noOfSeqs];

                for (int i = 0; i < noOfSeqs; i++)
                {
                    seqs[i] = srvTSPDetail.GetTSPSequence();
                }

                return Ok(seqs);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTSPBySchemeID/{id}")]
        public IActionResult GetTSPBySchemeID(int id)
        {
            try
            {
                List<TSPDetailModel> tsps = new List<TSPDetailModel>();

                tsps = srvTSPDetail.FetchTSPDetailByScheme(id);

                return Ok(tsps);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTSPByUser/{id}")]
        public IActionResult GetTSPByUser(int id)
        {
            try
            {
                List<TSPDetailModel> tsps = new List<TSPDetailModel>();

                tsps = srvTSPDetail.FetchTSPDataByUser(id);

                return Ok(tsps);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTSPForCRByUser/{id}")]
        public IActionResult GetTSPForCRByUser(int id)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvTSPDetail.FetchTSPDataByUser(id));
                ls.Add(srvDistrict.FetchDistrict());
                ls.Add(srvTSPDetail.FetchTSPCRDataByUser(id));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("FetchTSPByUser")]
        public IActionResult FetchTSPByUser(QueryFilters filters)
        {
            try
            {
                return Ok(srvTSPDetail.FetchTSPByUser(filters));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTSPDetailByScheme/{schemeId}")]
        public IActionResult GetTSPDetailByScheme(int schemeId)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                List<TSPDetailModel> tsps = new List<TSPDetailModel>();
                //if (curUserID > 0)
                //{
                tsps = srvTSPDetail.FetchTSPByUser(new QueryFilters() { SchemeID = schemeId, UserID = curUserID });
                //}
                //else
                //{
                //    tsps = srvTSPDetail.FetchTSPDetailByScheme(schemeId);
                //}
                return Ok(tsps);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetKamRelevantTSPDetailByScheme")]
        public IActionResult GetKamRelevantTSPDetailByScheme(QueryFilters filters)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                List<TSPDetailModel> tsps = new List<TSPDetailModel>();
                //if (curUserID > 0)
                //{
                tsps = srvTSPDetail.FetchKamRelevantTSPByScheme(new QueryFilters() { SchemeID = filters.SchemeID, UserID = curUserID });
                //}
                //else
                //{
                //    tsps = srvTSPDetail.FetchTSPDetailByScheme(schemeId);
                //}
                return Ok(tsps);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetTSPDetailForFilters")]
        public IActionResult RD_TspFilters(QueryFilters filters)
        {
            try
            {
                return Ok(srvTSPDetail.FetchKamRelevantTSPByScheme(new QueryFilters() { SchemeID = filters.SchemeID, UserID = filters.KAMID }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("FetchTSPByUserPaged")]
        public IActionResult FetchTSPByUserPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();
                List<object> ls = new List<object>();

                ls.Add(srvTSPDetail.FetchTSPByUserPaged(pagingModel, filterModel, out int totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}