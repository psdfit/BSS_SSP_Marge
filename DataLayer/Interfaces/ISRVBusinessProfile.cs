using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using DataLayer.Models.SSP;


namespace DataLayer.Interfaces
{
    public interface ISRVBusinessProfile
    {
        public DataTable FetchProfile(int UserID);
        public DataTable FetchMaster(int UserID);
        public DataTable FetchTspRegistration();
        public DataTable FetchTspRegistrationDetail(BusinessProfileModel user);
        public DataTable UpdateTspTradeValidationStatus(TradeMapModel data);

        public DataTable FetchDropDownList(string Param);
        public DataTable SaveProfile(BusinessProfileModel param);
        public DataTable SaveContactPersonProfile(BusinessProfileModel param);

       public DataTable FetchData(dynamic data,string SpName);

    }
}
