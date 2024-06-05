using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class POSAPResponseModel
    {
        public Pomodel POModel { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class Pomodel
    {
        public List<Podetail> PODetail { get; set; }
        public int DocEntry { get; set; }
        public string DocNum { get; set; }
    }

    public class Podetail
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
    }
}
