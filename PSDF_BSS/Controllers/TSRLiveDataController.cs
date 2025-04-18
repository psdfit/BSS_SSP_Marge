using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DataLayer.Interfaces;
using DataLayer.Services;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataLayer.Classes;
using System.Dynamic;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TSRLiveDataController : ControllerBase
    {

        private readonly ISRVTSRLiveData srvTSRLiveData;
        private readonly ISRVTraineeProfile srvTraineeProfile;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVDashboard srvDashboard;

        public TSRLiveDataController(ISRVTSRLiveData srvTSRLiveData, ISRVTraineeProfile srvTraineeProfile, ISRVUsers srvUsers, ISRVScheme srvScheme, ISRVDashboard srvDashboard)
        {
            this.srvTSRLiveData = srvTSRLiveData;
            this.srvTraineeProfile = srvTraineeProfile;
            this.srvScheme = srvScheme;
            this.srvUsers = srvUsers;
            this.srvDashboard = srvDashboard;
        }

        // GET: Schemes for TCRLiveDataSet
        [HttpGet]
        [Route("GetSchemesForTSR")]
        public IActionResult GetSchemesForTSR([FromQuery] int OID)
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    list.Add("Schemes", srvDashboard.FetchSchemesByUsers(curUserID));
                }
                else
                {
                    list.Add("Schemes", srvDashboard.FetchSchemes());
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        // GET: Schemes for TCRLiveDataSet
        [HttpGet]
        [Route("GetSchemesForGSR")]
        public IActionResult GetSchemesForGSR([FromQuery] int OID)
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    list.Add("Schemes", srvDashboard.FetchSchemesByGSRUsers(curUserID));
                }
                else
                {
                    list.Add("Schemes", srvDashboard.FetchSchemesGSR());
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // GET: TCRLiveDataSet
        [HttpGet]
        [Route("GetTCRLiveData")]
        public IActionResult GetTCRLiveData([FromQuery] int OID)
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    list.Add("Schemes", srvDashboard.FetchSchemesByUsers(curUserID));
                }
                else
                {
                    list.Add("Schemes", srvDashboard.FetchSchemes());
                }
                //list.Add("TSRLiveData", srvTSRLiveData.FetchTCRLiveDataByFilters(new int[] { 0, 0, 0, 0, OID, curUserID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: TCRLiveDataSet
        [HttpGet]
        [Route("GetTCRLiveDataScheme")]
        public IActionResult GetTCRLiveDataScheme([FromQuery] int OID)
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                
                list.Add("TSRLiveData", srvTSRLiveData.FetchTCRLiveDataByFilters(new int[] { 0, 0, 0, 0, OID, curUserID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: TSRLiveDataSet
        [HttpGet]
        [Route("GetTSRLiveData")]
        public IActionResult GetTSRLiveData([FromQuery] int OID)
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    list.Add("Schemes", srvDashboard.FetchSchemesByUsers(curUserID));
                }
                else
                {
                    list.Add("Schemes", srvDashboard.FetchSchemes());
                }
                list.Add("TSRLiveData", srvTSRLiveData.FetchTSRLiveDataByFilters(new int[] { 0, 0, 0, 0, OID, curUserID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetFilteredTSRLiveData/{filters}")]
        public IActionResult GetFilteredTSRLiveData([FromQuery(Name = "filters")] int[] filters)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                Array.Resize(ref filters, filters.Length + 1);
                filters[filters.GetUpperBound(0)] = curUserID;

                return Ok(srvTSRLiveData.FetchTSRLiveDataByFilters(filters));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetFilteredTSRData")]
        public IActionResult GetFilteredTSRData(SearchFilter filters)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                filters.UserID = curUserID;
                IEnumerable<dynamic> data = srvTSRLiveData.GetFilteredTSRData(filters).Select(
                    x =>
                    {
                        var obj = new Dictionary<string, object>();
                        filters.SelectedColumns.ForEach(column =>
                        {
                            var value = x.GetType().GetProperty(column)?.GetValue(x, null);
                            if (column == "TraineeImg" || column == "CNICImg" || column == "TraineeImg" || column == "CNICImgNADRA" || column == "ResultDocument")
                            {
                                value = (value == null || value == "") ? string.Empty : Common.GetFileBase64(value.ToString());
                            }
                            if (!obj.ContainsKey(column))
                            {
                                obj.Add(column, value);
                            }
                        });
                        return (dynamic)obj;
                    });
                // var data = srvTSRLiveData.GetFilteredTSRData(filters);//.Select(LinqHelpers.DynamicSelectGenerator<TSRLiveDataModel>(string.Join(',', filters.SelectedColumn))).ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost]
        [Route("GetFilteredGSRData")]
        public IActionResult GetFilteredGSRData(SearchFilter filters)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                filters.UserID = curUserID;
                IEnumerable<dynamic> data = srvTSRLiveData.GetFilteredGSRData(filters).Select(
                    x =>
                    {
                        var obj = new Dictionary<string, object>();
                        filters.SelectedColumns.ForEach(column =>
                        {
                            var value = x.GetType().GetProperty(column)?.GetValue(x, null);
                            if (column == "TraineeImg" || column == "CNICImg" || column == "TraineeImg" || column == "CNICImgNADRA" || column == "ResultDocument")
                            {
                                value = (value == null || value == "") ? string.Empty : Common.GetFileBase64(value.ToString());
                            }
                            if (!obj.ContainsKey(column))
                            {
                                obj.Add(column, value);
                            }
                        });
                        return (dynamic)obj;
                    });
                // var data = srvTSRLiveData.GetFilteredTSRData(filters);//.Select(LinqHelpers.DynamicSelectGenerator<TSRLiveDataModel>(string.Join(',', filters.SelectedColumn))).ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("RD_TSRPaged")]
        public IActionResult RD_TSRPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                filterModel.UserID = curUserID;

                int totalCount = 0;
                List<object> ls = new List<object>();

                ls.Add(srvTSRLiveData.FetchPaged(pagingModel, filterModel, out totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("RD_TERPaged")]
        public IActionResult RD_TERPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                filterModel.UserID = curUserID;

                int totalCount = 0;
                List<object> ls = new List<object>();

                ls.Add(srvTSRLiveData.FetchTERPaged(pagingModel, filterModel, out totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("RD_TARPaged")]
        public IActionResult RD_TARPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"]?.ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"]?.ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                filterModel.UserID = curUserID;

                int totalCount = 0;
                List<object> ls = new List<object>();

                ls.Add(srvTSRLiveData.FetchTARPaged(pagingModel, filterModel, out totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("RD_TARPagedClasswise")]
        public IActionResult RD_TARPagedClasswise([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"]?.ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"]?.ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                filterModel.UserID = curUserID;

                int totalCount = 0;
                List<object> ls = new List<object>();

                ls.Add(srvTSRLiveData.FetchTARPagedClasswise(pagingModel, filterModel, out totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("RD_GSRPaged")]
        public IActionResult RD_GSRPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                filterModel.UserID = curUserID;

                int totalCount = 0;
                List<object> ls = new List<object>();

                ls.Add(srvTSRLiveData.FetchGSRPaged(pagingModel, filterModel));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("RD_TSUPaged")]
        public IActionResult RD_TSUPaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                filterModel.UserID = curUserID;

                List<object> ls = new List<object>();

                ls.Add(srvTSRLiveData.FetchTSULiveData(pagingModel, filterModel, out int totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


    }
}
