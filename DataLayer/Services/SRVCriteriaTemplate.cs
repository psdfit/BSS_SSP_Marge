using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DataLayer.JobScheduler.Scheduler;
using DataLayer.Models.SSP;
using System.IO;


namespace DataLayer.Services
{
    public class SRVCriteriaTemplate : ISRVCriteriaTemplate
    {
        public SRVCriteriaTemplate()
        {
        }


        public DataTable SaveCriteriaTemplate(CriteriaTemplateModel data)
        {
            using (SqlConnection connection = new SqlConnection(SqlHelper.GetCon()))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    List<SqlParameter> parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@UserID", data.CurUserID),
                            new SqlParameter("@CriteriaHeaderID", data.CriteriaTemplateID),
                            new SqlParameter("@HeaderTitle", data.CriteriaTemplateTitle),
                            new SqlParameter("@HeaderDesc", data.Description),
                            new SqlParameter("@IsMarking", data.MarkingRequired),
                            new SqlParameter("@MaxMarks", data.MaximumMarks),
                            new SqlParameter("@IsSubmitted", data.IsSubmitted)
                        };


                    DataTable dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "AU_SSPCriteriaHeader", parameters.ToArray()).Tables[0];

                    int criteriaHeaderID = (data.CriteriaTemplateID > 0)
                        ? data.CriteriaTemplateID
                        : Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["CriteriaHeaderID"]);

                    MainCategoryBatchInsert(data.mainCategory, criteriaHeaderID, data.CurUserID, transaction);

                    transaction.Commit();

                    return dt;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void MainCategoryBatchInsert(List<CriteriaMainCategory> mainCategories, int criteriaHeaderID, int curUserID, SqlTransaction transaction = null)
        {
            foreach (var mainCategory in mainCategories)
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@UserID", curUserID),
                        new SqlParameter("@CriteriaMainCategoryID", mainCategory.CriteriaMainCategoryID),
                        new SqlParameter("@CriteriaHeaderID", criteriaHeaderID),
                        new SqlParameter("@MainCategoryTitle", mainCategory.CategoryTitle),
                        new SqlParameter("@MainCategoryDesc", mainCategory.Description),
                        new SqlParameter("@TotalMarks", mainCategory.TotalMarks)
                    };

                DataTable dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "AU_SSPCriteriaMainCategory", parameters.ToArray()).Tables[0];

                int CriteriaMainCategoryID = (mainCategory.CriteriaMainCategoryID > 0)
                    ? mainCategory.CriteriaMainCategoryID
                    : Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["CriteriaMainCategoryID"]);

                SubCategoryBatchInsert(mainCategory.subCategory, criteriaHeaderID, CriteriaMainCategoryID, curUserID, transaction);
            }
        }

        public void SubCategoryBatchInsert(List<CriteriaSubCategory> subCategories, int criteriaHeaderID, int CriteriaMainCategoryID, int userID, SqlTransaction transaction = null)
        {
            foreach (var subCategory in subCategories)
            {
                string CriteriaAttachment = SaveAttachment("CriteriaAttachment", subCategory.Attachment, "Attached", "Document");

                List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@UserID", userID),
                        new SqlParameter("@CriteriaHeaderID", criteriaHeaderID),
                        new SqlParameter("@CriteriaMainCategoryID", CriteriaMainCategoryID),
                        new SqlParameter("@CriteriaSubCategoryID", subCategory.CriteriaSubCategoryID),
                        new SqlParameter("@SubCategoryTitle", subCategory.SubCategoryTitle),
                        new SqlParameter("@SubCategoryDesc", subCategory.Description),
                        new SqlParameter("@Criteria", subCategory.Criteria),
                        new SqlParameter("@MarkedCriteria", subCategory.MarkedCriteria),
                        new SqlParameter("@MaxMarks", subCategory.MaxMarks),
                        new SqlParameter("@IsMandatory", subCategory.Mandatory),
                        new SqlParameter("@Attachment", CriteriaAttachment)
                    };

                SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "AU_SSPCriteriaSubCategory", parameters.ToArray());
            }
        }



        private static string SaveAttachment(string fileType, string attachment, string instituteName, string instituteNTN)
        {
            if (!string.IsNullOrEmpty(attachment))
            {
                string path = FilePaths.DOCUMENTS_FILE_DIR + fileType + "\\" + instituteName + "_" + instituteNTN;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string paths = path + "\\";
                return Common.AddFile(attachment, paths);
                //return path+Common.AddFile(attachment, Path.Combine(path, "\\"));
            }
            return "";
        }



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


        public DataTable DeleteTrainerDetail(int TrainerDetailID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TrainerDetailID", TrainerDetailID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Delete_TrainerDetail", param.ToArray()).Tables[0];
            return dt;
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

        public bool RemoveMainCategory(CriteriaMainCategory mainCategory)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@CriteriaMaincategoryID", mainCategory.CriteriaMainCategoryID));

            SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "SSPRemove_MainCategory", parameters.ToArray());
            return true;
        }

        public bool RemoveSubCategory(CriteriaSubCategory subCategory)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CriteriaSubCategoryID", subCategory.CriteriaSubCategoryID),
            };

            SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "SSPRemove_SubCategory", parameters.ToArray());
            return true;

        }



        public bool CriteriaApproveReject(CriteriaTemplateModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@CriteriaTemplateID", model.CriteriaTemplateID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "[AU_SSPCriteriaApproveReject]", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_SSPCriteriaApproveReject]", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool CriteriaFinalApproval(int FormId, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@CriteriaTemplateID", FormId));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "AU_SSPCriteriaFinalApproval", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPCriteriaFinalApproval", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}