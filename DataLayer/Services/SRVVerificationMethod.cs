using DataLayer.Dapper;
using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Services
{
    public class SRVVerificationMethod: ISRVVerificationMethod
    {
        private readonly IDapperConfig _dapper;
        public SRVVerificationMethod(IDapperConfig dapper)
        {
            _dapper = dapper;
        }

        public List<VerificationMethodModel> FetchAll()
        {
            try
            {
                var query = @"Select VerificationMethodID, VerificationMethodType, PlacementTypeID From VerificationMethod";
                var list = _dapper.Query<VerificationMethodModel>(query);
                return list;
            }
            catch (Exception ex)
            {
                return new List<VerificationMethodModel>();
            }
            
        }

    }
}
