using DataLayer.Models.SSP;
using DataLayer.Models;
using DataLayer.Models.SSP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVPayment
    {

         void SaveAssociationPayment(AssociationPaymentModel data);
         void SaveRegistrationPayment(RegistrationPaymentModel data);
         DataTable FetchDataListBySPName(string SpName);
         DataTable LoopinData(DataTable dt, string[] attachmentColumns);

    }
}
