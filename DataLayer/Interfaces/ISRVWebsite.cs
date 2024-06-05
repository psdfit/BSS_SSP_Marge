using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVWebsite
    {
        DataTable GetDistricts(string ModifiedDate);
        DataTable GetTehsils(string ModifiedDate);
        DataTable GetSectors(string ModifiedDate);
        DataTable GetSubSectors(string ModifiedDate);
        DataTable GetTrades(string ModifiedDate);
        DataTable GetBSSTrades(string ModifiedDate);
        DataTable GetClasses(string ModifiedDate);
        DataTable GetGenders(string ModifiedDate);
        DataTable GetEducationTypes(string ModifiedDate);
        string SavePotentialTrainee(PotentialTraineesModel Trainee);
    }
}
