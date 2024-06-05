using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVProgramType
    {
        ProgramTypeModel GetByPTypeID(int PTypeID);

        List<ProgramTypeModel> SaveProgramType(ProgramTypeModel ProgramType);

        List<ProgramTypeModel> FetchProgramType(ProgramTypeModel mod);

        List<ProgramTypeModel> FetchProgramType();

        List<ProgramTypeModel> FetchProgramType(bool InActive);

        void ActiveInActive(int PTypeID, bool? InActive, int CurUserID);

        public List<ProgramTypeModel> FetchPTypeForROSIFilter(ROSIFiltersModel rosiFilters);

    }
}