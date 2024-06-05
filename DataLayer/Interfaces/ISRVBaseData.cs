using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataLayer.Models.SSP;

namespace DataLayer.Interfaces
{
    public interface ISRVBaseData
    {
        public DataTable FetchBankDetailList(int UserID);
        public DataTable FetchTrainingLocationList(int UserID);
        public DataTable FetchCertificationList(int UserID);
        public DataTable FetchTradeMapList(int UserID);
        public DataTable FetchTrainerProfileList(int UserID);
        public DataTable FetchTrainerDetail(int TrainerID);
        public DataTable DeleteTrainerDetail(int TrainerDetailID);

        public DataTable SaveBankDetail(BankModel param);
        public DataTable SaveTrainingLocation(TrainingLocationModel param);
        public DataTable SaveCertification(CertificateModel param);
        public DataTable SaveTradeMapping(TradeMapModel param);
        public DataTable SaveTrainerProfile(TrainerProfileModel param);
        public DataTable SaveTrainerProfileDetail(TrainerProfileDetailModel param);


        public DataTable FetchDropDownList(string Param);   
    }
}
