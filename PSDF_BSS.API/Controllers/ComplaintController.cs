using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static DataLayer.Models.ComplaintApiModel;

namespace PSDF_BSS.API.Controllers
{
    [Route("api/crm/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly ISRVComplaint _srvComplaint;
        private readonly ISRVTraineeProfile _srvTraineeProfile;
        private readonly ISRVTSPDetail _srvTSPDetail;
        public ComplaintController(ISRVComplaint srvComplaint, ISRVTraineeProfile srvTraineeProfile, ISRVTSPDetail srvTSPDetail)
        {
            _srvComplaint = srvComplaint;
            _srvTSPDetail = srvTSPDetail;
            _srvTraineeProfile = srvTraineeProfile;
        }

        //s[AllowAnonymous]
        [HttpPost("Registration")]
        public IActionResult ComplaintRegistration(ComplaintModel model)
        {
            string errMsg = string.Empty;
            //CHECK NULL OBJECT
            try
            {
                if (model == null)
                {
                    return BadRequest(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Bad request. Did you pass valid body?"
                    });
                }

                //CHECK INDIVIDUAL PROPERTIES
                if (string.IsNullOrEmpty(model.ComplaintDescription)
                    || model.ComplaintTypeID == 0
                    || model.ComplaintSubTypeID == 0)
                {
                    return BadRequest(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Bad request. Did you pass valid body?"
                    });
                }

                //CHECK VALID SUBTYPE 
                if (model.ComplaintTypeID > 0)
                {
                    var complaintList = _srvComplaint.FetchComplaintSubTypeForCRUD(model.ComplaintTypeID);
                    bool containsItem = complaintList.Any(item => item.ComplaintSubTypeID == model.ComplaintSubTypeID);
                    if (containsItem == false)
                    {
                        return BadRequest(new ApiResponse()
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Bad request. Did you pass valid ''ComplaintSubTypeID''?"
                        });
                    }
                }

                //CHECK AND GET TRAINEE DETAILS BY CNIC AND ITS COMPLAINT TYPE
                if (string.IsNullOrEmpty(model.TraineeCNIC) && model.ComplaintTypeID == 1)
                {
                    return BadRequest(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Please Enter Trainee CNIC ."
                    });
                }
                else if (!string.IsNullOrEmpty(model.TraineeCNIC) && model.ComplaintTypeID == 1)
                {
                    //CHECK TRAINEE CNIC FORMAT
                    if (!_srvTraineeProfile.IsValidCNICFormat(model.TraineeCNIC, out errMsg))
                    {
                        return BadRequest(new ApiResponse()
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Bad request , " + errMsg
                        }); ;
                    }

                    TraineeProfileModel traineeProfile = _srvTraineeProfile.GetTraineeProfileByCNIC(model.TraineeCNIC);
                    if (traineeProfile != null)
                    {
                        model.TraineeID = traineeProfile.TraineeID;
                    }
                    else
                    {
                        return BadRequest(new ApiResponse()
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Trainee not found."
                        });
                    }

                }

                //CHECK AND GET TSP DETAILS BY TSPCODE AND ITS COMPLAINT TYPE
                if (string.IsNullOrEmpty(model.TSPCode) && model.ComplaintTypeID == 2)
                {
                    return BadRequest(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Please Enter TSP Code ."
                    });
                }
                else if (!string.IsNullOrEmpty(model.TSPCode) && model.ComplaintTypeID == 2)
                {
                    TSPDetailModel TSPDetail = _srvTSPDetail.FetchTSPByTSPCode(model.TSPCode);
                    if (TSPDetail != null)
                    {
                        model.TSPMasterID = TSPDetail.TSPMasterID;
                    }
                    else
                    {
                        return BadRequest(new ApiResponse()
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Please Enter Correct TSP Code."
                        });
                    }
                }

                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                model.Submitted = true;
                model.ComplaintRegisterType = 2;//BY CRM

                long complaint_No = _srvComplaint.saveComplaint(model, out errMsg);

                return Ok(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = complaint_No > 0 ? "Success" : errMsg,
                    Data = new
                    {
                        isSaved = complaint_No > 0 ? true : false,
                        ComplaintNo = complaint_No
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = $"Error : {ex.Message}"
                });
            }
        }

        //[AllowAnonymous]
        [HttpGet("GetTraineeProfileByCNIC/{cnic}")]
        public IActionResult GetTraineeProfileByCNIC(string cnic)
        {
            string errMsg = string.Empty;

            if (!string.IsNullOrEmpty(cnic))
            {
                try
                {
                    if (!_srvTraineeProfile.IsValidCNICFormat(cnic, out errMsg))
                    {
                        return BadRequest(new ApiResponse()
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Bad request , " + errMsg
                        }); ;
                    }

                    TraineeProfileModel traineeProfile = _srvTraineeProfile.GetTraineeProfileByCNIC(cnic);
                    if (traineeProfile == null)
                    {
                        return BadRequest(new ApiResponse()
                        {
                            StatusCode = (int)HttpStatusCode.NotFound,
                            Message = "Trainee not found."
                        }); ;
                    }

                    var obj = new
                    {
                        //traineeProfile.TraineeID,
                        traineeProfile.TraineeCode,
                        traineeProfile.TraineeName,
                        traineeProfile.FatherName,
                        traineeProfile.ContactNumber1,
                        traineeProfile.ClassCode,
                        //traineeProfile.TraineeHouseNumber,
                        //traineeProfile.TraineeMauzaTown,
                        //traineeProfile.TraineeStreetMohalla,
                        //traineeProfile.TrainingAddressLocation
                    };
                    return Ok(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Message = "Success",
                        Data = obj
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = $"Error : {ex.Message}"
                    });
                }
            }
            return BadRequest(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Bad request. Did you pass valid body?"
            });
        }

        [HttpGet("GetTspProfileByTspCode/{tspCode}")]
        public IActionResult GetTSPProfileByTspCode(string tspCode)
        {
            if (!string.IsNullOrEmpty(tspCode))
            {
                try
                {
                    TSPDetailModel TSPDetail = _srvTSPDetail.FetchTSPByTSPCode(tspCode);
                    var obj = new
                    {
                        //TSPDetail?.TSPMasterID,
                        TSPDetail?.Address,
                        TSPDetail?.TSPCode,
                        TSPDetail?.TSPName,
                    };
                    return Ok(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Message = "Success",
                        Data = obj
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = $"Error : {ex.Message}"
                    });
                }
            }
            return BadRequest(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Bad request. Did you pass valid body?"
            });
        }

        [HttpGet("GetAllComplaintTypesAndSubTypes")]
        public IActionResult GetAllComplaintTypesAndSubTypes()
        {
            var list = _srvComplaint.GetAllComplaintTypesAndSubTypes();
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = list
            });
        }
        [HttpGet("GetAllComplaintStatusTypes")]
        public IActionResult GetComplaintStatus()
        {
            var list = _srvComplaint.GetComplaintStatusType()
                 .Select(x => new
                 {
                     ComplaintStatusID = x.ComplaintStatusTypeID,
                     Name = x.ComplaintStatusType
                 });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = list
            });




        }


        [HttpGet("GetAllComplaints")]
        private IActionResult GetAllComplaints()
        {
            try
            {
                var list = _srvComplaint.FetchTraineeAndTSPComplaintForKAM(0, 0).Select(item => new
                {
                    item.ComplaintNo,
                    item.ComplaintTypeID,
                    item.ComplaintTypeName,
                    item.ComplaintSubTypeID,
                    item.ComplaintSubTypeName,
                    item.TraineeCNIC,
                    item.TraineeCode,
                    item.TraineeName,
                    item.FatherName,
                    //item.TSPMasterID,
                    item.TSPCode,
                    item.TSPName,
                    item.ComplaintStatusType,
                    item.ComplaintDescription
                });
                return Ok(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = $"Error : {ex.Message}"
                });
            }
        }

        [HttpGet("GetComplaintHistoryByCompaintNo/{complaintNo}")]
        private IActionResult GetComplaintHistoryByCompaintNo(string complaintNo)
        {

            try
            {
                List<ComplaintModel> list = _srvComplaint.GetComplaintHistoryByCompaintNoApi(complaintNo);
                var ComplaintsHistoryList = list.Select(item => new
                {
                    item.ComplaintNo,
                    ComplaintStatus = item.ComplaintStatusType,
                    Remarks = item.complaintStatusDetailComments,
                    item.CreatedDate
                });

                return Ok(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = ComplaintsHistoryList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = $"Error : {ex.Message}"
                });
            }
        }
        [HttpGet("ComplaintType")]
        private IActionResult GetComplaintType()
        {
            var list = _srvComplaint.FetchComplaintTypeForCRUD()
                 .Select(x => new
                 {
                     x.ComplaintTypeID,
                     Name = x.ComplaintTypeName
                 });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = list
            });
        }

        [HttpGet("GetComplaintSubTypeByTypeID")]
        private IActionResult GetComplaintSubTypeByTypeID(int? complaintTypeID)
        {

            if (complaintTypeID > 0 && complaintTypeID != null)
            {
                var list = _srvComplaint.FetchComplaintSubTypeForCRUD(complaintTypeID)
               .Select(x => new
               {
                   x.ComplaintSubTypeID,
                   Name = x.ComplaintSubTypeName
               });
                return Ok(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = list
                });
            }
            else
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }
        }
    }
}
