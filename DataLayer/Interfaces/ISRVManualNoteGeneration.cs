using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
   public interface ISRVManualNoteGeneration
    {
        DataTable FetchEligibleClassDataForPVRN(QueryFilters data);
        DataTable GeneratePVRN(QueryFilters model, out string IsGenerated);
        DataTable FetchEligibleClassDataForMRN(QueryFilters data);
        DataTable GenerateMRN(QueryFilters model, out string IsGenerated);
        DataTable FetchEligibleClassDataForPCRN(QueryFilters data);
        DataTable GeneratePCRN(QueryFilters model, out string IsGenerated);
        DataTable FetchEligibleClassDataForOTRN(QueryFilters data);
        DataTable GenerateOTRN(QueryFilters model, out string IsGenerated);


    }
}
