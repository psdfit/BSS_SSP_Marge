using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVGenerateInvoice
    {
        void GenerateInvoices(int SchemeID, SqlTransaction transaction = null);
        List<EmailUsersModel> CreateTSPsAccounts(int SchemeID, int curUserID, SqlTransaction transaction = null);
        void GenerateEmailsToUsers(List<EmailUsersModel> user);
        void GenerateEmailsToUsersB2C(List<EmailUsersModel> user);
    }
}
