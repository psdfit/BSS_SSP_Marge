using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVModules
    {
        ModulesModel GetByModuleID(int ModuleID);
        List<ModulesModel> SaveModules(ModulesModel Modules);
        List<ModulesModel> FetchModules(ModulesModel mod);
        List<ModulesModel> FetchModules();
        List<ModulesModel> FetchModules(bool InActive);
        void ActiveInActive(int ModuleID, bool? InActive, int CurUserID);
    }
}