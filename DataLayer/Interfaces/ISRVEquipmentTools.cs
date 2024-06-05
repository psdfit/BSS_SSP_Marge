using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVEquipmentTools
    {
        EquipmentToolsModel GetByEquipmentToolID(int EquipmentToolID);
        List<EquipmentToolsModel> SaveEquipmentTools(EquipmentToolsModel EquipmentTools);
        List<EquipmentToolsModel> FetchEquipmentTools(EquipmentToolsModel mod);
        List<EquipmentToolsModel> FetchEquipmentTools();
        List<EquipmentToolsModel> FetchEquipmentTools(bool InActive);
        void ActiveInActive(int EquipmentToolID, bool? InActive, int CurUserID);
    }
}
