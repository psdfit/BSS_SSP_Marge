using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PSDF_BSS.Reports.Models
{
    public class ParameterModel : IEnumerable<KeyValuePair<string, object>>
    {
        public ParameterModel()
        {

        }
        public string MyType { get; set; }
        public int? Scheme { get; set; }
        public int? ProgramType { get; set; }
        public int? ProgramCategory { get; set; }
        public string ReportName { get; set; }
        public string MyReportType { get; set; }
        public int? MyschemeID { get; set; }
        public int? MySchemeTypeID { get; set; }
        public string Type { get; set; }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(p => p.Name, p => p.GetGetMethod().Invoke(this, null)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}