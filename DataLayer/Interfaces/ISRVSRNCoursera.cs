using DataLayer.Models;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVSRNCoursera
    {
        public DataTable FetchSRNCourseraTrainees(QueryFilters model);
        public DataTable GenerateSRNCoursera(QueryFilters model, out string IsGenerated);
        public DataTable FetchOJT(QueryFilters model);
        public DataTable GenerateSRNOJT(QueryFilters model, out string IsGenerated);
    }
}
