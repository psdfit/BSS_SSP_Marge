using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVSourceOfCurriculum
    {
        SourceOfCurriculumModel GetBySourceOfCurriculumID(int SourceOfCurriculumID);
        List<SourceOfCurriculumModel> SaveSourceOfCurriculum(SourceOfCurriculumModel SourceOfCurriculum);
        List<SourceOfCurriculumModel> FetchSourceOfCurriculum(SourceOfCurriculumModel mod);
        List<SourceOfCurriculumModel> FetchSourceOfCurriculum();
        List<SourceOfCurriculumModel> FetchSourceOfCurriculum(bool InActive);
        void ActiveInActive(int SourceOfCurriculumID, bool? InActive, int CurUserID);
    }
}
