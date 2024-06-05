using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVClassSections
    {
        ClassSectionsModel GetBySectionID(int SectionID);
        List<ClassSectionsModel> SaveClassSections(ClassSectionsModel ClassSections);
        List<ClassSectionsModel> FetchClassSections(ClassSectionsModel mod);
        List<ClassSectionsModel> FetchClassSections();
        List<ClassSectionsModel> FetchClassSections(bool InActive);
        void ActiveInActive(int SectionID, bool? InActive, int CurUserID);
    }
}
