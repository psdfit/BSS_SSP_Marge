using DataLayer.Models;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVGURNCoursera
    {
        public DataTable FetchGURNCourseraTrainees(QueryFilters model);
        public DataTable GenerateGURNCoursera(QueryFilters model, out string IsGenerated);

    }
}
