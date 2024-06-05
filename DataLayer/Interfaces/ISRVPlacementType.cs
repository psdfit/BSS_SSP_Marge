using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVPlacementType
    {
        PlacementTypeModel GetByPlacementTypeID(int PlacementTypeID);
        List<PlacementTypeModel> SavePlacementType(PlacementTypeModel PlacementType);
        List<PlacementTypeModel> FetchPlacementType(PlacementTypeModel mod);
        List<PlacementTypeModel> FetchPlacementType();
        List<PlacementTypeModel> FetchPlacementType(bool InActive);
        void ActiveInActive(int PlacementTypeID, bool? InActive, int CurUserID);
    }
}
