﻿using DataLayer.Models;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVSRNCoursera
    {
        public DataTable FetchSRNCourseraTrainees(QueryFilters model);
        public DataTable GenerateSRNCoursera(QueryFilters model, out string IsGenerated);
        public DataTable FetchVRNClasses(QueryFilters model);
        public DataTable GenerateVRN(QueryFilters model, out string IsGenerated);


    }
}