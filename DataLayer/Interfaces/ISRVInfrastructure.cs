using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface  ISRVInfrastructure
    {
         bool SaveInfrastructure(InfrastructureSaveModel model);
         List<InfrastructureModel> GetInfrastructures();
    }
}
