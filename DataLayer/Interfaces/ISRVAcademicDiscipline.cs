using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVAcademicDiscipline
    {
        AcademicDisciplineModel GetByAcademicDisciplineID(int AcademicDisciplineID);
        List<AcademicDisciplineModel> SaveAcademicDiscipline(AcademicDisciplineModel AcademicDiscipline);
        List<AcademicDisciplineModel> FetchAcademicDiscipline(AcademicDisciplineModel mod);
        List<AcademicDisciplineModel> FetchAcademicDiscipline();
        List<AcademicDisciplineModel> FetchAcademicDiscipline(bool InActive);
        void ActiveInActive(int AcademicDisciplineID, bool? InActive, int CurUserID);
    }
}
