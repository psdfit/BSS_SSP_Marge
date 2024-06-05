using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVEducationTypes
    {
        EducationTypesModel GetByID(int EducationTypeID);

        List<EducationTypesModel> SaveEducationTypes(EducationTypesModel EducationTypes);

        List<EducationTypesModel> FetchEducationTypes(EducationTypesModel mod);

        List<EducationTypesModel> FetchEducationTypes();

        List<EducationTypesModel> FetchEducationTypes(bool InActive);

        void ActiveInActive(int EducationTypeID, bool? InActive, int CurUserID);
    }
}