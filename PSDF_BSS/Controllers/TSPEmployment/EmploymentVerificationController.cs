/* **** Aamer Rehman Malik *****/
using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Users.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmploymentVerificationController : ControllerBase
    {
        private readonly ISRVEmploymentVerification srvEmploymentVerification ;
        private readonly ISRVTSPEmployment srvTSPEmployment;

        public EmploymentVerificationController(ISRVEmploymentVerification srvEmploymentVerification, ISRVTSPEmployment srvTSPEmployment)
        {
            this.srvEmploymentVerification = srvEmploymentVerification;
            this.srvTSPEmployment = srvTSPEmployment;
        }

        // GET: EmploymentVerification
        [HttpGet]
        [Route("GetData/{EmploymentTypeID}")]
        public IActionResult GetEmploymentVerification(int EmploymentTypeID)
        {
            try
            {
                List<object> ls = new List<object>();

                //ls.Add(srv.FetchEmploymentVerification());

                //ls.Add(new SRVTSPEmployment().FetchPlacementFormE(new TSPEmploymentModel { IsMigrated = false }));

                return Ok(new
                {
                    VerificationMethodList = new SRVEmploymentVerification().GetVerificationMethod(EmploymentTypeID),
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetFormalEmploymentList/{ClassID}")]
        public IActionResult GetFormalEmploymentList(int ClassID)
        {
            try
            {
                return Ok(new
                {
                    FormalList = srvTSPEmployment.GetFormalTSPList(ClassID).Tables[0],
                    TypeID = srvTSPEmployment.GetFormalTSPList(ClassID).Tables[1].Rows[0][0]
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetSelfEmploymentList/{ClassID}")]
        public IActionResult GetSelfEmploymentList(int ClassID)
        {
            try
            {
                return Ok(new
                {
                    SelfList = srvTSPEmployment.GetSelfTSPList(ClassID).Tables[0],
                    TypeID = srvTSPEmployment.GetFormalTSPList(ClassID).Tables[1].Rows[0][0]
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: EmploymentVerification
        [HttpGet]
        [Route("RD_EmploymentVerification")]
        public IActionResult RD_EmploymentVerification()
        {
            try
            {
                return Ok(srvEmploymentVerification.FetchEmploymentVerification(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: EmploymentVerification
        [HttpPost]
        [Route("RD_EmploymentVerificationBy")]
        public IActionResult RD_EmploymentVerificationBy(EmploymentVerificationModel mod)
        {
            try
            {
                return Ok(srvEmploymentVerification.FetchEmploymentVerification(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EmploymentVerification/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(EmploymentVerificationModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvEmploymentVerification.SaveEmploymentVerification(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EmploymentVerificationOJT/Save
        [HttpPost]
        [Route("OJTSave")]
        public IActionResult OJTSave(EmploymentVerificationModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvEmploymentVerification.SaveEmploymentVerificationOJT(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: EmploymentVerification/Save
        [HttpPost]
        [Route("SaveBulk")]
        public IActionResult SaveBulk(List<EmploymentVerificationModel> list)
        {
            try
            {
                //D.CurUserID = Convert.ToInt32(User.Identity.Name);
                foreach (var item in list)
                {
                    EmploymentVerificationModel model = new EmploymentVerificationModel();
                    model.ID = item.ID;
                    model.PlacementID = item.PlacementID;
                    model.TraineeName = item.TraineeName;
                    model.VerificationMethodID = item.VerificationMethodID;
                    model.EmploymentStatus = item.EmploymentStatus;
                    model.EmploymentType = item.EmploymentType;
                    model.District = item.District;
                    model.EmploymentTehsil = item.EmploymentTehsil;
                    model.EmploymentStartDate = item.EmploymentStartDate;
                    model.EmployerBusinessType = item.EmployerBusinessType;
                    model.Designation = item.Designation;
                    model.Department = item.Department;
                    model.EmployerName = item.EmployerName;
                    model.EmploymentAddress = item.EmploymentAddress;
                    model.EmploymentDuration = item.EmploymentDuration;
                    model.EmploymentTiming = item.EmploymentTiming;
                    model.OfficeContactNo = item.OfficeContactNo;
                    model.Salary = item.Salary;
                    model.SupervisorContact = item.SupervisorContact;
                    model.SupervisorName = item.SupervisorName;
                    model.TraineeName = item.TraineeName;
                    model.IsVerified = item.IsVerified;
                    model.Comments = item.Comments;
                    model.Attachment = item.Attachment;

                    if (!String.IsNullOrEmpty(item.Attachment))
                    {
                        var path = "\\Documents\\TSPEmployment\\";

                        var fileName = Common.AddFile(item.Attachment, path);

                        model.Attachment = fileName;
                        model.Attachment = fileName;
                    }

                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    srvEmploymentVerification.SaveEmploymentVerification(model);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EmploymentVerification/SaveOJT
        [HttpPost]
        [Route("SaveBulkOJT")]
        public IActionResult SaveBulkOJT(List<EmploymentVerificationModel> list)
        {
            try
            {
                //D.CurUserID = Convert.ToInt32(User.Identity.Name);
                foreach (var item in list)
                {
                    EmploymentVerificationModel model = new EmploymentVerificationModel();
                    model.ID = item.ID;
                    model.PlacementID = item.PlacementID;
                    model.TraineeName = item.TraineeName;
                    model.VerificationMethodID = item.VerificationMethodID;
                    model.EmploymentStatus = item.EmploymentStatus;
                    model.EmploymentType = item.EmploymentType;
                    model.District = item.District;
                    model.EmploymentTehsil = item.EmploymentTehsil;
                    model.EmploymentStartDate = item.EmploymentStartDate;
                    model.EmployerBusinessType = item.EmployerBusinessType;
                    model.Designation = item.Designation;
                    model.Department = item.Department;
                    model.EmployerName = item.EmployerName;
                    model.EmploymentAddress = item.EmploymentAddress;
                    model.EmploymentDuration = item.EmploymentDuration;
                    model.EmploymentTiming = item.EmploymentTiming;
                    model.OfficeContactNo = item.OfficeContactNo;
                    model.Salary = item.Salary;
                    model.SupervisorContact = item.SupervisorContact;
                    model.SupervisorName = item.SupervisorName;
                    model.TraineeName = item.TraineeName;
                    model.IsVerified = item.IsVerified;
                    model.Comments = item.Comments;
                    model.Attachment = item.Attachment;

                    if (!String.IsNullOrEmpty(item.Attachment))
                    {
                        var path = "\\Documents\\TSPEmployment\\";

                        var fileName = Common.AddFile(item.Attachment, path);

                        model.Attachment = fileName;
                        model.Attachment = fileName;
                    }

                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    srvEmploymentVerification.SaveEmploymentVerificationOJT(model);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("SubmitClassVerification")]
        public async Task<IActionResult> SubmitClassVerification(TSPEmploymentModel m)
        {
            try
            {
                m.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvEmploymentVerification.SubmitVerification(m.ClassID ?? 0,m.CurUserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("SubmitClassVerificationOJT")]
        public async Task<IActionResult> SubmitClassVerificationOJT(TSPEmploymentModel m)
        {
            try
            {
                m.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvEmploymentVerification.SubmitVerificationOJT(m.ClassID ?? 0,m.CurUserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("SubmitClassVerificationByCallCenter")]
        public async Task<IActionResult> SubmitClassVerificationByCallCenter(TSPEmploymentModel m)
        {
            try
            {
                m.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvEmploymentVerification.SubmitVerificationByCallCenter(m.ClassID ?? 0,m.CurUserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("SubmitClassVerificationByCallCenterOJT")]
        public async Task<IActionResult> SubmitClassVerificationByCallCenterOJT(TSPEmploymentModel m)
        {
            try
            {
                m.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvEmploymentVerification.SubmitVerificationByCallCenterOJT(m.ClassID ?? 0, m.CurUserID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EmploymentVerification/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]                                                                                                                                                                                                      
        public IActionResult ActiveInActive(EmploymentVerificationModel d)
        {
            try
            {
                srvEmploymentVerification.ActiveInActive(d.ID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateVerifyStatus")]
        public IActionResult UpdateVerifyStatus(EmploymentVerificationModel d)
        {
            try
            {
                d.CurUserID = Convert.ToInt32(User.Identity.Name);

                srvEmploymentVerification.SaveEmploymentVerification(d);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateVerifyStatusOJT")]
        public IActionResult UpdateVerifyStatusOJT(EmploymentVerificationModel d)
        {
            try
            {
                d.CurUserID = Convert.ToInt32(User.Identity.Name);

                srvEmploymentVerification.SaveEmploymentVerificationOJT(d);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EmploymentVerification/Save
        [HttpPost]
        [Route("UpdateTelephonicEmploymentVerification")]
        public IActionResult UpdateTelephonicEmploymentVerification(EmploymentVerificationModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvEmploymentVerification.UpdateTelephonicEmploymentVerification(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        // POST: EmploymentVerification/Save
        [HttpPost]
        [Route("UpdateTelephonicEmploymentVerificationOJT")]
        public IActionResult UpdateTelephonicEmploymentVerificationOJT(EmploymentVerificationModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvEmploymentVerification.UpdateTelephonicEmploymentVerificationOJT(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        [HttpGet]
        [Route("GetCallCenterVerification")]
        public IActionResult GetCallCenterVerification()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvEmploymentVerification.GetCallCenterVerificationTrainee());
                ls.Add(srvEmploymentVerification.GetCallCenterVerificationSupervisor());
                ls.Add(srvEmploymentVerification.GetCallCenterVerificationComments());
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EmploymentVerification/Save
        [HttpPost]
        [Route("TelephonicSaveBulk")]
        public IActionResult TelephonicSaveBulk(List<EmploymentVerificationModel> list)
        {
            try
            {
                //D.CurUserID = Convert.ToInt32(User.Identity.Name);
                foreach (var item in list)
                {
                    EmploymentVerificationModel model = new EmploymentVerificationModel();
                    model.ID = item.ID;
                    model.PlacementID = item.PlacementID;
                    model.TraineeName = item.TraineeName;
                    model.VerificationMethodID = item.VerificationMethodID;
                    model.EmploymentStatus = item.EmploymentStatus;
                    model.EmploymentType = item.EmploymentType;
                    model.District = item.District;
                    model.EmploymentTehsil = item.EmploymentTehsil;
                    model.EmploymentStartDate = item.EmploymentStartDate;
                    model.EmployerBusinessType = item.EmployerBusinessType;
                    model.Designation = item.Designation;
                    model.Department = item.Department;
                    model.EmployerName = item.EmployerName;
                    model.EmploymentAddress = item.EmploymentAddress;
                    model.EmploymentDuration = item.EmploymentDuration;
                    model.EmploymentTiming = item.EmploymentTiming;
                    model.OfficeContactNo = item.OfficeContactNo;
                    model.Salary = item.Salary;
                    model.SupervisorContact = item.SupervisorContact;
                    model.SupervisorName = item.SupervisorName;
                    model.TraineeName = item.TraineeName;
                    model.IsVerified = item.IsVerified;
                    model.Comments = item.Comments;
                    model.Attachment = item.Attachment;
                    model.CallCenterVerificationTraineeID = item.CallCenterVerificationTraineeID;
                    model.CallCenterVerificationSupervisorID = item.CallCenterVerificationSupervisorID;
                    model.CallCenterVerificationCommentsID = item.CallCenterVerificationCommentsID;

                    if (!String.IsNullOrEmpty(item.Attachment))
                    {
                        var path = "\\Documents\\TSPEmployment\\";

                        var fileName = Common.AddFile(item.Attachment, path);

                        model.Attachment = fileName;
                        model.Attachment = fileName;
                    }

                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    srvEmploymentVerification.UpdateTelephonicEmploymentVerification(model);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: EmploymentVerification/OJTSave
        [HttpPost]
        [Route("TelephonicSaveBulkOJT")]
        public IActionResult TelephonicSaveBulkOJT(List<EmploymentVerificationModel> list)
        {
            try
            {
                //D.CurUserID = Convert.ToInt32(User.Identity.Name);
                foreach (var item in list)
                {
                    EmploymentVerificationModel model = new EmploymentVerificationModel();
                    model.ID = item.ID;
                    model.PlacementID = item.PlacementID;
                    model.TraineeName = item.TraineeName;
                    model.VerificationMethodID = item.VerificationMethodID;
                    model.EmploymentStatus = item.EmploymentStatus;
                    model.EmploymentType = item.EmploymentType;
                    model.District = item.District;
                    model.EmploymentTehsil = item.EmploymentTehsil;
                    model.EmploymentStartDate = item.EmploymentStartDate;
                    model.EmployerBusinessType = item.EmployerBusinessType;
                    model.Designation = item.Designation;
                    model.Department = item.Department;
                    model.EmployerName = item.EmployerName;
                    model.EmploymentAddress = item.EmploymentAddress;
                    model.EmploymentDuration = item.EmploymentDuration;
                    model.EmploymentTiming = item.EmploymentTiming;
                    model.OfficeContactNo = item.OfficeContactNo;
                    model.Salary = item.Salary;
                    model.SupervisorContact = item.SupervisorContact;
                    model.SupervisorName = item.SupervisorName;
                    model.TraineeName = item.TraineeName;
                    model.IsVerified = item.IsVerified;
                    model.Comments = item.Comments;
                    model.Attachment = item.Attachment;
                    model.CallCenterVerificationTraineeID = item.CallCenterVerificationTraineeID;
                    model.CallCenterVerificationSupervisorID = item.CallCenterVerificationSupervisorID;
                    model.CallCenterVerificationCommentsID = item.CallCenterVerificationCommentsID;

                    if (!String.IsNullOrEmpty(item.Attachment))
                    {
                        var path = "\\Documents\\TSPEmployment\\";

                        var fileName = Common.AddFile(item.Attachment, path);

                        model.Attachment = fileName;
                        model.Attachment = fileName;
                    }

                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    srvEmploymentVerification.UpdateTelephonicEmploymentVerificationOJT(model);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}