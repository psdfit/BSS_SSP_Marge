using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Models.IP;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace DataLayer.Services
{
    public class SRVIPDocsVerification : ISRVIPDocsVerification
    {
        public DataTable GetIPTrainees(
            int? traineeID,
            int? schemeID,
            int? tspID,
            int? classID,
            int? userID,
            int? oid
        )
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@TraineeID", traineeID ?? (object)DBNull.Value),
                new SqlParameter("@SchemeID", schemeID ?? (object)DBNull.Value),
                new SqlParameter("@TSPID", tspID ?? (object)DBNull.Value),
                new SqlParameter("@ClassID", classID ?? (object)DBNull.Value),
                new SqlParameter("@UserID", userID ?? (object)DBNull.Value),
                new SqlParameter("@OID", oid ?? (object)DBNull.Value),
            };

            DataTable dt = SqlHelper
                .ExecuteDataset(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "RD_IPTrainees",
                    param.ToArray()
                )
                .Tables[0];
            return dt;
        }

        public DataTable GetTSPDetailsByClassID(int classID)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@ClassID", classID),
            };

            DataTable dt = SqlHelper
                .ExecuteDataset(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "RD_TSPDetailByClassID_DVV",
                    param.ToArray()
                )
                .Tables[0];
            return dt;
        }

        public void SaveVisaStampingDocs(
            int traineeID,
            string traineeCode,
            string traineeName,
            int tspID,
            string classCode,
            List<string> filePaths
        )
        {
            foreach (var filePath in filePaths)
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@TraineeID", traineeID),
                    new SqlParameter("@TraineeName", traineeName),
                    new SqlParameter("@TraineeCode", traineeCode),
                    new SqlParameter("@TspID", tspID),
                    new SqlParameter("@ClassCode", classCode),
                    new SqlParameter("@FilePath", filePath),
                };

                SqlHelper.ExecuteNonQuery(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "SP_SaveVisaStampingDocs",
                    param.ToArray()
                );
            }
        }

        public DataTable GetVisaStampingDocs(int traineeID)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@TraineeID", traineeID),
            };

            DataTable dt = SqlHelper
                .ExecuteDataset(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "RD_VisaStampingTraineeDocuments",
                    param.ToArray()
                )
                .Tables[0];
            return dt;
        }

        public bool VisaStampingApproveReject(
            VisaStampingResponseModel model,
            SqlTransaction transaction = null
        )
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(
                    new SqlParameter(
                        "@VisaStampingTraineeDocumentsID",
                        model.VisaStampingTraineeDocumentsID
                    )
                );
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(
                        transaction,
                        CommandType.StoredProcedure,
                        "U_IPVSApproveReject",
                        param.ToArray()
                    );
                }
                else
                {
                    SqlHelper.ExecuteScalar(
                        SqlHelper.GetCon(),
                        CommandType.StoredProcedure,
                        "U_IPVSApproveReject",
                        param.ToArray()
                    );
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SaveMedicalCostDocs(
            int traineeID,
            string traineeCode,
            string traineeName,
            int tspID,
            string classCode,
            List<string> filePaths
        )
        {
            foreach (var filePath in filePaths)
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@TraineeID", traineeID),
                    new SqlParameter("@TraineeName", traineeName),
                    new SqlParameter("@TraineeCode", traineeCode),
                    new SqlParameter("@TspID", tspID),
                    new SqlParameter("@ClassCode", classCode),
                    new SqlParameter("@FilePath", filePath),
                };

                SqlHelper.ExecuteNonQuery(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "SP_SaveMedicalDocs",
                    param.ToArray()
                );
            }
        }

        public DataTable GetMedicalCostDocs(int traineeID)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@TraineeID", traineeID),
            };

            DataTable dt = SqlHelper
                .ExecuteDataset(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "RD_MedicalTraineeDocuments",
                    param.ToArray()
                )
                .Tables[0];
            return dt;
        }

        public bool MedicalCostApproveReject(
            MedicalCostResponseModel model,
            SqlTransaction transaction = null
        )
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(
                    new SqlParameter(
                        "@MedicalTraineeDocumentsID",
                        model.MedicalTraineeDocumentsID
                    )
                );
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(
                        transaction,
                        CommandType.StoredProcedure,
                        "U_IPMCApproveReject",
                        param.ToArray()
                    );
                }
                else
                {
                    SqlHelper.ExecuteScalar(
                        SqlHelper.GetCon(),
                        CommandType.StoredProcedure,
                        "U_IPMCApproveReject",
                        param.ToArray()
                    );
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SavePrometricCostDocs(
            int traineeID,
            string traineeCode,
            string traineeName,
            int tspID,
            string classCode,
            List<string> filePaths
        )
        {
            foreach (var filePath in filePaths)
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@TraineeID", traineeID),
                    new SqlParameter("@TraineeName", traineeName),
                    new SqlParameter("@TraineeCode", traineeCode),
                    new SqlParameter("@TspID", tspID),
                    new SqlParameter("@ClassCode", classCode),
                    new SqlParameter("@FilePath", filePath),
                };

                SqlHelper.ExecuteNonQuery(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "SP_SavePrometricDocs",
                    param.ToArray()
                );
            }
        }

        public DataTable GetPrometricCostDocs(int traineeID)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@TraineeID", traineeID),
            };

            DataTable dt = SqlHelper
                .ExecuteDataset(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "RD_PrometricTraineeDocuments",
                    param.ToArray()
                )
                .Tables[0];
            return dt;
        }

        public bool PrometricCostApproveReject(
            PrometricCostResponseModel model,
            SqlTransaction transaction = null
        )
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(
                    new SqlParameter(
                        "@PrometricTraineeDocumentsID",
                        model.PrometricTraineeDocumentsID
                    )
                );
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(
                        transaction,
                        CommandType.StoredProcedure,
                        "U_IPPCApproveReject",
                        param.ToArray()
                    );
                }
                else
                {
                    SqlHelper.ExecuteScalar(
                        SqlHelper.GetCon(),
                        CommandType.StoredProcedure,
                        "U_IPPCApproveReject",
                        param.ToArray()
                    );
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SaveOtherTrainingCostDocs(
            int traineeID,
            string traineeCode,
            string traineeName,
            int tspID,
            string classCode,
            List<string> filePaths
        )
        {
            foreach (var filePath in filePaths)
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@TraineeID", traineeID),
                    new SqlParameter("@TraineeName", traineeName),
                    new SqlParameter("@TraineeCode", traineeCode),
                    new SqlParameter("@TspID", tspID),
                    new SqlParameter("@ClassCode", classCode),
                    new SqlParameter("@FilePath", filePath),
                };

                SqlHelper.ExecuteNonQuery(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "SP_SaveOtherDocs",
                    param.ToArray()
                );
            }
        }

        public DataTable GetOtherTrainingCostDocs(int traineeID)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@TraineeID", traineeID),
            };

            DataTable dt = SqlHelper
                .ExecuteDataset(
                    SqlHelper.GetCon(),
                    CommandType.StoredProcedure,
                    "RD_OtherTraineeDocuments",
                    param.ToArray()
                )
                .Tables[0];
            return dt;
        }

        public bool OtherTrainingCostApproveReject(
            OtherTrainingCostResponseModel model,
            SqlTransaction transaction = null
        )
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(
                    new SqlParameter(
                        "@OtherTraineeDocumentsID",
                        model.OtherTraineeDocumentsID
                    )
                );
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(
                        transaction,
                        CommandType.StoredProcedure,
                        "U_IPOTApproveReject",
                        param.ToArray()
                    );
                }
                else
                {
                    SqlHelper.ExecuteScalar(
                        SqlHelper.GetCon(),
                        CommandType.StoredProcedure,
                        "U_IPOTApproveReject",
                        param.ToArray()
                    );
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
