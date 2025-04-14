using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using DataLayer.Interfaces;
using DataLayer.Models.IP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PSDF_BSS.Logging;
namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IPDocsVerificationController : ControllerBase
    {
        private readonly ISRVIPDocsVerification srvIPDocsVerification;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<IPDocsVerificationController> _logger;

        public IPDocsVerificationController(
            ISRVIPDocsVerification srvIPDocsVerification,
            IWebHostEnvironment env,
            ILogger<IPDocsVerificationController> logger
            )
        {
            this.srvIPDocsVerification = srvIPDocsVerification;
            _env = env;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetIPTrainees")]
        public IActionResult GetIPTrainees([FromQuery] int? traineeID = null, [FromQuery] int? schemeID = null, [FromQuery] int? tspID = null, [FromQuery] int? classID = null, [FromQuery] int? userID = null, [FromQuery] int? oid = null)
        {
            string[] split = HttpContext.Request.Path.Value.Split("/");
            bool isAuthorized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), split[2], split[3]);

            if (isAuthorized)
            {
                try
                {
                    var result = srvIPDocsVerification.GetIPTrainees(traineeID, schemeID, tspID, classID, userID, oid);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. You are not authorized for this activity.");
            }
        }

        [HttpGet]
        [Route("GetTSPDetailsByClassID")]
        public IActionResult GetTSPDetailsByClassID(int classID)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);

            if (IsAuthrized)
            {
                try
                {
                    var result = srvIPDocsVerification.GetTSPDetailsByClassID(classID);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. You are not authorized for this activity.");
            }
        }

        [HttpPost]
        [Route("UploadVisaStampingDocs")]
        public IActionResult UploadVisaStampingDocs([FromBody] VisaStampingUploadModel model)
        {
            try
            {
                if (model == null || model.Files == null || model.Files.Count == 0)
                {
                    return BadRequest("No files received for upload.");
                }
                string baseFolder = @"C:\Users\umair.nadeem\source\repos\BSS_SSP_Marge\PSDF_BSS\Documents\VisaStamping";
                string traineeFolder = Path.Combine(baseFolder, model.ClassCode.ToString());
                if (!Directory.Exists(traineeFolder))
                {
                    Directory.CreateDirectory(traineeFolder);
                }
                List<string> savedFilePaths = new List<string>();
                foreach (var file in model.Files)
                {
                    string fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    string filePath = Path.Combine(traineeFolder, fileName);

                    // Ensure fileContent is not null or empty
                    if (string.IsNullOrEmpty(file.FileContent))
                    {
                        return BadRequest("File content is missing or empty.");
                    }
                    byte[] fileBytes = Convert.FromBase64String(file.FileContent);
                    System.IO.File.WriteAllBytes(filePath, fileBytes);

                    savedFilePaths.Add(filePath);
                }
                // Save file paths to database using Stored Procedure
                srvIPDocsVerification.SaveVisaStampingDocs(model.TraineeID, model.TraineeCode, model.TraineeName, model.TspID, model.ClassCode, savedFilePaths);
                return Ok(new { Message = "Files uploaded successfully", Paths = savedFilePaths });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UploadVisaStampingDocs");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetVisaStampingDocs/{traineeId}")]
        public IActionResult GetVisaStampingDocs(int traineeId)
        {
            try
            {
                // Fetch document details from the database
                var documents = srvIPDocsVerification.GetVisaStampingDocs(traineeId);
                if (documents == null || documents.Rows.Count == 0)
                {
                    return NotFound("No documents found for this trainee.");
                }
                List<VisaStampingResponseModel> response = new List<VisaStampingResponseModel>();
                foreach (DataRow row in documents.Rows)
                {
                    string filePath = row["VisaStampDoc"].ToString();
                    string base64Content = "";
                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                        base64Content = Convert.ToBase64String(fileBytes);
                    }
                    else
                    {
                        return NotFound($"File not found at path: {filePath}");
                    }
                    response.Add(new VisaStampingResponseModel
                    {
                        VisaStampingTraineeDocumentsID = Convert.ToInt32(row["VisaStampingTraineeDocumentsID"]),
                        TraineeID = Convert.ToInt32(row["TraineeID"]),
                        TraineeName = row["TraineeName"].ToString(),
                        TraineeCode = row["TraineeCode"].ToString(),
                        TspID = Convert.ToInt32(row["TspID"]),
                        ClassCode = row["ClassCode"].ToString(),
                        FileName = Path.GetFileName(filePath),
                        FileContentBase64 = base64Content
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetVisaStampingDocs");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpPost]
        [Route("UploadMedicalCostDocs")]
        public IActionResult UploadMedicalCostDocs([FromBody] MedicalCostUploadModel model)
        {
            try
            {
                if (model == null || model.Files == null || model.Files.Count == 0)
                {
                    return BadRequest("No files received for upload.");
                }
                string baseFolder = @"C:\Users\umair.nadeem\source\repos\BSS_SSP_Marge\PSDF_BSS\Documents\MedicalCost";
                string traineeFolder = Path.Combine(baseFolder, model.ClassCode.ToString());
                if (!Directory.Exists(traineeFolder))
                {
                    Directory.CreateDirectory(traineeFolder);
                }
                List<string> savedFilePaths = new List<string>();
                foreach (var file in model.Files)
                {
                    string fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    string filePath = Path.Combine(traineeFolder, fileName);

                    // Ensure fileContent is not null or empty
                    if (string.IsNullOrEmpty(file.FileContent))
                    {
                        return BadRequest("File content is missing or empty.");
                    }
                    byte[] fileBytes = Convert.FromBase64String(file.FileContent);
                    System.IO.File.WriteAllBytes(filePath, fileBytes);

                    savedFilePaths.Add(filePath);
                }
                // Save file paths to database using Stored Procedure
                srvIPDocsVerification.SaveMedicalCostDocs(model.TraineeID, model.TraineeCode, model.TraineeName, model.TspID, model.ClassCode, savedFilePaths);
                return Ok(new { Message = "Files uploaded successfully", Paths = savedFilePaths });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UploadMedicalCostDocs");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetMedicalCostDocs/{traineeId}")]
        public IActionResult GetMedicalCostDocs(int traineeId)
        {
            try
            {
                // Fetch document details from the database
                var documents = srvIPDocsVerification.GetMedicalCostDocs(traineeId);
                if (documents == null || documents.Rows.Count == 0)
                {
                    return NotFound("No documents found for this trainee.");
                }
                List<MedicalCostResponseModel> response = new List<MedicalCostResponseModel>();
                foreach (DataRow row in documents.Rows)
                {
                    string filePath = row["MedicalDoc"].ToString();
                    string base64Content = "";
                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                        base64Content = Convert.ToBase64String(fileBytes);
                    }
                    else
                    {
                        return NotFound($"File not found at path: {filePath}");
                    }
                    response.Add(new MedicalCostResponseModel
                    {
                        MedicalTraineeDocumentsID = Convert.ToInt32(row["MedicalTraineeDocumentsID"]),
                        TraineeID = Convert.ToInt32(row["TraineeID"]),
                        TraineeName = row["TraineeName"].ToString(),
                        TraineeCode = row["TraineeCode"].ToString(),
                        TspID = Convert.ToInt32(row["TspID"]),
                        ClassCode = row["ClassCode"].ToString(),
                        FileName = Path.GetFileName(filePath),
                        FileContentBase64 = base64Content
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMedicalCostDocs");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }



        [HttpPost]
        [Route("UploadPrometricCostDocs")]
        public IActionResult UploadPrometricCostDocs([FromBody] PrometricCostUploadModel model)
        {
            try
            {
                if (model == null || model.Files == null || model.Files.Count == 0)
                {
                    return BadRequest("No files received for upload.");
                }
                string baseFolder = @"C:\Users\umair.nadeem\source\repos\BSS_SSP_Marge\PSDF_BSS\Documents\PrometricCost";
                string traineeFolder = Path.Combine(baseFolder, model.ClassCode.ToString());
                if (!Directory.Exists(traineeFolder))
                {
                    Directory.CreateDirectory(traineeFolder);
                }
                List<string> savedFilePaths = new List<string>();
                foreach (var file in model.Files)
                {
                    string fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    string filePath = Path.Combine(traineeFolder, fileName);

                    // Ensure fileContent is not null or empty
                    if (string.IsNullOrEmpty(file.FileContent))
                    {
                        return BadRequest("File content is missing or empty.");
                    }
                    byte[] fileBytes = Convert.FromBase64String(file.FileContent);
                    System.IO.File.WriteAllBytes(filePath, fileBytes);

                    savedFilePaths.Add(filePath);
                }
                // Save file paths to database using Stored Procedure
                srvIPDocsVerification.SavePrometricCostDocs(model.TraineeID, model.TraineeCode, model.TraineeName, model.TspID, model.ClassCode, savedFilePaths);
                return Ok(new { Message = "Files uploaded successfully", Paths = savedFilePaths });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UploadPrometricCostDocs");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPrometricCostDocs/{traineeId}")]
        public IActionResult GetPrometricCostDocs(int traineeId)
        {
            try
            {
                // Fetch document details from the database
                var documents = srvIPDocsVerification.GetPrometricCostDocs(traineeId);
                if (documents == null || documents.Rows.Count == 0)
                {
                    return NotFound("No documents found for this trainee.");
                }
                List<PrometricCostResponseModel> response = new List<PrometricCostResponseModel>();
                foreach (DataRow row in documents.Rows)
                {
                    string filePath = row["PrometricDoc"].ToString();
                    string base64Content = "";
                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                        base64Content = Convert.ToBase64String(fileBytes);
                    }
                    else
                    {
                        return NotFound($"File not found at path: {filePath}");
                    }
                    response.Add(new PrometricCostResponseModel
                    {
                        PrometricTraineeDocumentsID = Convert.ToInt32(row["PrometricTraineeDocumentsID"]),
                        TraineeID = Convert.ToInt32(row["TraineeID"]),
                        TraineeName = row["TraineeName"].ToString(),
                        TraineeCode = row["TraineeCode"].ToString(),
                        TspID = Convert.ToInt32(row["TspID"]),
                        ClassCode = row["ClassCode"].ToString(),
                        FileName = Path.GetFileName(filePath),
                        FileContentBase64 = base64Content
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPrometricCostDocs");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            } 
        }


        [HttpPost]
        [Route("UploadOtherTrainingCostDocs")]
        public IActionResult UploadOtherTrainingCostDocs([FromBody] OtherTrainingCostUploadModel model)
        {
            try
            {
                if (model == null || model.Files == null || model.Files.Count == 0)
                {
                    return BadRequest("No files received for upload.");
                }
                string baseFolder = @"C:\Users\umair.nadeem\source\repos\BSS_SSP_Marge\PSDF_BSS\Documents\OtherTrainingCost";
                string traineeFolder = Path.Combine(baseFolder, model.ClassCode.ToString());
                if (!Directory.Exists(traineeFolder))
                {
                    Directory.CreateDirectory(traineeFolder);
                }
                List<string> savedFilePaths = new List<string>();
                foreach (var file in model.Files)
                {
                    string fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    string filePath = Path.Combine(traineeFolder, fileName);

                    // Ensure fileContent is not null or empty
                    if (string.IsNullOrEmpty(file.FileContent))
                    {
                        return BadRequest("File content is missing or empty.");
                    }
                    byte[] fileBytes = Convert.FromBase64String(file.FileContent);
                    System.IO.File.WriteAllBytes(filePath, fileBytes);

                    savedFilePaths.Add(filePath);
                }
                // Save file paths to database using Stored Procedure
                srvIPDocsVerification.SaveOtherTrainingCostDocs(model.TraineeID, model.TraineeCode, model.TraineeName, model.TspID, model.ClassCode, savedFilePaths);
                return Ok(new { Message = "Files uploaded successfully", Paths = savedFilePaths });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UploadOtherTrainingCostDocs");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetOtherTrainingCostDocs/{traineeId}")]
        public IActionResult GetOtherTrainingCostDocs(int traineeId)
        {
            try
            {
                // Fetch document details from the database
                var documents = srvIPDocsVerification.GetOtherTrainingCostDocs(traineeId);
                if (documents == null || documents.Rows.Count == 0)
                {
                    return NotFound("No documents found for this trainee.");
                }
                List<OtherTrainingCostResponseModel> response = new List<OtherTrainingCostResponseModel>();
                foreach (DataRow row in documents.Rows)
                {
                    string filePath = row["OtherDoc"].ToString();
                    string base64Content = "";
                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                        base64Content = Convert.ToBase64String(fileBytes);
                    }
                    else
                    {
                        return NotFound($"File not found at path: {filePath}");
                    }
                    response.Add(new OtherTrainingCostResponseModel
                    {
                        OtherTraineeDocumentsID = Convert.ToInt32(row["OtherTraineeDocumentsID"]),
                        TraineeID = Convert.ToInt32(row["TraineeID"]),
                        TraineeName = row["TraineeName"].ToString(),
                        TraineeCode = row["TraineeCode"].ToString(),
                        TspID = Convert.ToInt32(row["TspID"]),
                        ClassCode = row["ClassCode"].ToString(),
                        FileName = Path.GetFileName(filePath),
                        FileContentBase64 = base64Content
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOtherTrainingCostDocs");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }




    }
}
