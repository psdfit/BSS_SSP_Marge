using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.API.Models
{
    public class ApiResponse 
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }

    }
}
