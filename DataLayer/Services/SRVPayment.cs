using Microsoft.Data.SqlClient;
using System.Data;
using DataLayer.Models.SSP;
using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Classes;
using System.Transactions;
using DataLayer.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using System.Text.RegularExpressions;


namespace DataLayer.Services
{
    public class SRVPayment : ISRVPayment
    {
        public DataTable FetchDataListBySPName(string spName)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), spName);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public DataTable LoopinData(DataTable dt, string[] attachmentColumns)
        {
            DataTable modifiedDataTable = dt.Clone();

            foreach (DataRow row in dt.Rows)
            {
                // Update all attachments in one go using the passed attachment columns array
                foreach (string attachmentColumn in attachmentColumns)
                {
                    UpdateAttachment(row, attachmentColumn);
                }

                modifiedDataTable.ImportRow(row);
            }

            return modifiedDataTable;
        }


        private void UpdateAttachment(DataRow row, string columnName)
        {
            string attachment = row[columnName].ToString();

            if (string.IsNullOrEmpty(attachment))
            {
                row[columnName] = "";
            }
            else
            {
                row[columnName] = Common.GetFileBase64(attachment);
            }
        }

        private static string SaveAttachment(string TSPName, string fileType, string attachment)
        {
            if (!string.IsNullOrEmpty(attachment))
            {
                string path = FilePaths.DOCUMENTS_FILE_DIR + TSPName + "\\" + fileType;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string paths = path + "\\";
                return Common.AddFile(attachment, paths);
            }
            return "";
        }

      
      
        public DataTable GetTSPData(int TSPID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", TSPID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTSPProfile", param.ToArray()).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
     


        public void SaveRegistrationPayment(RegistrationPaymentModel data)
        {
            try
            {
                DataTable PayProPayment= SavePayment(data.UserID,data.TotalRegistrationFee);

                var PayProID= PayProPayment.Rows[0]["ID"];
                var PayProPaymentCode= PayProPayment.Rows[0]["ConnectPayId"];

                for (int i = 0; i < data.RegisteredLocations.Count; i++)
                {
                    
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@TrainingLocationID", data.RegisteredLocations[i].TrainingLocationID));
                param.Add(new SqlParameter("@TrainingLocationFee",data.TotalRegistrationFee/data.RegisteredLocations.Count));
                param.Add(new SqlParameter("@PayProPaymentTableID", PayProID ));
                param.Add(new SqlParameter("@PayProPaymentCode", PayProPaymentCode));
                param.Add(new SqlParameter("@UserID", data.UserID ));

                SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTSPRegistrationPaymentDetail", param.ToArray());

                }

          
                //return PayProPayment;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }



        public DataTable SavePayment(int UserID,int TotalFee)
        {
            DataTable TblPayProMaxOrderNumber = FetchDataListBySPName("RD_SSPPayProMaxOrderNumber");
            var orderNumber = Convert.ToInt32(TblPayProMaxOrderNumber.Rows[0]["OrderNumber"]);
            DataTable TSP = GetTSPData(UserID);

            DateTime issueDate = DateTime.Now.Date;
            DateTime dueDate = DateTime.Today.AddDays(20).Date;

           string pattern = "[^a-zA-Z0-9\\s]"; 
           string TSPNameWithOutSpecialCh = Regex.Replace(TSP.Rows[0]["InstituteName"].ToString(), pattern, "");
           string TSPName = Regex.Replace(TSPNameWithOutSpecialCh, @"\s+", " ");


            var jsonOrder = $@"[
                    {{
                    ""MerchantId"": ""PSDF"",
                    ""MerchantPassword"": ""Live@psdf21""
                    }},{{
                    ""OrderNumber"": ""{orderNumber+999777}"",
                    ""OrderAmount"": ""{TotalFee}"",
                    ""OrderDueDate"": ""{dueDate.Day}/{dueDate.Month}/{dueDate.Year}"",
                    ""OrderAmountWithinDueDate"": ""{TotalFee}"",
                    ""OrderAmountAfterDueDate"": ""{TotalFee}"",
                    ""OrderType"": ""Service"",
                    ""IssueDate"": ""{issueDate.Day}/{issueDate.Month}/{issueDate.Year}"",
                    ""OrderExpireAfterSeconds"": ""0"",
                    ""CustomerName"": ""{TSPName}"",
                    ""CustomerMobile"": ""{TSP.Rows[0]["TspContact"].ToString().Replace("-", "")}"",
                    ""CustomerEmail"": ""{TSP.Rows[0]["TspEmail"]}"",
                    ""CustomerAddress"": """"
                    }}
                ]";


            string baseUrl = "https://api.paypro.com.pk/";
            string reqUri = baseUrl + "cpay/co?oJson=" + jsonOrder;
            WebClient client = new WebClient();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var response = client.UploadString(reqUri, jsonOrder);
            var model = JsonConvert.DeserializeObject<IList<PayProResponseModel>>(response);


            if (model[0].Status == "00")
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@OrderNumber", model[1].OrderNumber));
                param.Add(new SqlParameter("@OrderAmount", model[1].OrderAmount));
                param.Add(new SqlParameter("@OrderDueDate", dueDate));
                param.Add(new SqlParameter("@OrderType", "Service"));
                param.Add(new SqlParameter("@IssueDate", issueDate));
                param.Add(new SqlParameter("@OrderAmountWithinDueDate", model[1].OrderAmount));
                param.Add(new SqlParameter("@OrderAmountAfterDueDate", model[1].OrderAmount));
                param.Add(new SqlParameter("@Status", model[0].Status));
                param.Add(new SqlParameter("@IsFeeApplied", model[1].IsFeeApplied));
                param.Add(new SqlParameter("@ConnectPayId", model[1].ConnectPayId.ToString()));
                param.Add(new SqlParameter("@Description", model[1].Description.ToString()));
                //param.Add(new SqlParameter("@OrderStatus", "PAID"));
                param.Add(new SqlParameter("@OrderStatus", "UNPAID"));
                param.Add(new SqlParameter("@Click2Pay", "http://"));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPPayPro_PaymentDetail", param.ToArray()).Tables[0];
                return dt;
            }
            else
            {
                throw new Exception(response);
            }



        }

        public void SaveAssociationPayment(AssociationPaymentModel data)
        {
            try
            {
                DataTable PayProPayment = SavePayment(data.UserID, data.TotalAssociationFee);

                var PayProID = PayProPayment.Rows[0]["ID"];
                var PayProPaymentCode = PayProPayment.Rows[0]["ConnectPayId"];

                for (int i = 0; i < data.AssociatedTradeLot.Count; i++)
                {

                    List<SqlParameter> param = new List<SqlParameter>();

                    param.Add(new SqlParameter("@TradeLotID", data.AssociatedTradeLot[i].Tradelot));
                    param.Add(new SqlParameter("@TrainingLocationID", data.AssociatedTradeLot[i].TrainingLocation));
                    param.Add(new SqlParameter("@NoOfClasses", data.AssociatedTradeLot[i].NoOfClass));
                    param.Add(new SqlParameter("@TradeLotFee", data.TotalAssociationFee / data.NoOFClasses));
                    param.Add(new SqlParameter("@PayProPaymentTableID", PayProID));
                    param.Add(new SqlParameter("@PayProPaymentCode", PayProPaymentCode));
                    param.Add(new SqlParameter("@UserID", data.UserID));


                    SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTSPAssociationPaymentDetail", param.ToArray());

                }


                //return PayProPayment;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
