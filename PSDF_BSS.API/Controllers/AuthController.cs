using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PSDF_BSS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PSDF_BSS.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ISRVUsers _srvUsers;
        private ISRVTSPDetail _srvTSPDetail;
        public AuthController(ISRVUsers srvUsers, ISRVTSPDetail srvTSPDetail)
        {
            _srvUsers = srvUsers;
            _srvTSPDetail = srvTSPDetail;
        }
        [AllowAnonymous]
        [HttpPost("~/api/auth/login")]
        //[Route("Login")]
        public IActionResult Login(UsersModel user)
        {
            //if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.UserPassword) && !string.IsNullOrEmpty(user.IMEI))
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.UserPassword))
            {
                UsersModel u = _srvUsers.LoginDVV(user.UserName, user.UserPassword, user.IMEI);
                if (u != null)
                {
                    if (u.UserLevel == (int)EnumUserLevel.TSP)
                    {
                        var tspDetail = _srvTSPDetail.FetchTSPDataByUser(u.UserID).Select(x => new
                        {
                            x.TSPID,
                            x.TSPName,
                            x.TSPCode,
                            ContactPersonName = x.CPName,
                            ContactPersonLandline = x.CPLandline,
                            ContactPersonEmail = x.CPEmail,
                            x.HeadName,
                            x.HeadLandline,
                            x.HeadEmail
                        });
                        Dictionary<string, object> list = new Dictionary<string, object>();
                        list.Add("Token", u.Token);
                        list.Add("TspDetails", tspDetail);
                        //list.Add("Classes", JsonConvert.SerializeObject(_srvClass.FetchClassesByUser(new QueryFilters() { UserID = u.UserID })));
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
                            StatusCode = (int)HttpStatusCode.Forbidden,
                            Data = false,
                            Message = "Invalid TSP user,Insufficient privileges."
                        });
                    }

                }
                else
                {
                    return BadRequest(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Message = "Invalid Username / Password or EMEI."
                    });
                }
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

        [AllowAnonymous]
        [HttpPost("~/api/auth/weblogin")]
        //[Route("Login")]
        public IActionResult WebLogin(UsersModel user)
        {
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.UserPassword))
            {
                UsersModel u = _srvUsers.Login(user.UserName, user.UserPassword);
                if (u != null)
                {
                    Dictionary<string, object> list = new Dictionary<string, object>();
                    list.Add("Token", u.Token);
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
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Message = "Invalid Username / Password."
                    });
                }
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
        [AllowAnonymous]
        [HttpPost("~/api/auth/crm/login")]
        //[Route("Login")]
        public IActionResult LoginForCRM(UsersModel user)
        {
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.UserPassword))
            {
                UsersModel u = _srvUsers.Login(user.UserName, user.UserPassword);
                if (u != null)
                {
                    Dictionary<string, object> list = new Dictionary<string, object>();
                    list.Add("Token", u.Token);
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
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Message = "Invalid Username / Password."
                    });
                }
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
