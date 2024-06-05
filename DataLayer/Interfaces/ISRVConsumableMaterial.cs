using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVConsumableMaterial
    {
        ConsumableMaterialModel GetByConsumableMaterialID(int ConsumableMaterialID);
        List<ConsumableMaterialModel> SaveConsumableMaterial(ConsumableMaterialModel ConsumableMaterial);
        List<ConsumableMaterialModel> FetchConsumableMaterial(ConsumableMaterialModel mod);
        List<ConsumableMaterialModel> FetchConsumableMaterial();
        List<ConsumableMaterialModel> FetchConsumableMaterial(bool InActive);
        void ActiveInActive(int ConsumableMaterialID, bool? InActive, int CurUserID);
    }
}
