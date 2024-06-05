
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class SchemeChangeRequestModel : ModelBase
    {
        public SchemeChangeRequestModel() : base() { }
        public SchemeChangeRequestModel(bool InActive) : base(InActive) { }

        public int SchemeChangeRequestID { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public string SchemeCode { get; set; }
        public string BusinessRuleType { get; set; }
        public int Stipend { get; set; }
        public string StipendMode { get; set; }
        public int UniformAndBag { get; set; }

        public string ProcessKey { get; set; }

        public bool IsApproved { get; set; }
        public bool IsRejected{ get; set; }


    }}
