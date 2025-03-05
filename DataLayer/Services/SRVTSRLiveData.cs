using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.Classes;
using Microsoft.Data.SqlClient;
using System.Text;

namespace DataLayer.Services
{
    public class SRVTSRLiveData : SRVBase, ISRVTSRLiveData
    {
        public List<TSRLiveDataModel> FetchTSRLiveData()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSRData").Tables[0];
                return LoopinTSRLiveData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<TSRLiveDataModel> LoopinTSRLiveData(DataTable dt)
        {
            List<TSRLiveDataModel> tsrLiveModel = new List<TSRLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                tsrLiveModel.Add(RowOfTSRLiveData(r));
            }
            return tsrLiveModel;
        }

        private TSRLiveDataModel RowOfTSRLiveData(DataRow row)
        {
            //GetFiles by Path
            string traineeImage = string.IsNullOrEmpty(row["TraineeImg"].ToString()) ? string.Empty : Common.GetFileBase64(row["TraineeImg"].ToString());
            string traineeCNICImage = string.IsNullOrEmpty(row["CNICImg"].ToString()) ? string.Empty : Common.GetFileBase64(row["CNICImg"].ToString());
            string traineeCNICImageNadra = string.IsNullOrEmpty(row.Field<string>("CNICImgNADRA")) ? string.Empty : Common.GetFileBase64(row.Field<string>("CNICImgNADRA"));

            TSRLiveDataModel tsr = new TSRLiveDataModel();
            tsr.TraineeID = row.Field<int>("TraineeID");
            tsr.TraineeCode = row.Field<string>("TraineeCode");
            tsr.TraineeName = row.Field<string>("TraineeName");
            tsr.TraineeCNIC = row.Field<string>("TraineeCNIC");
            tsr.FatherName = row.Field<string>("FatherName");
            tsr.GenderID = row.Field<int>("GenderID");
            tsr.GenderName = row.Field<string>("GenderName");
            tsr.Batch = row.Field<int>("Batch");
            tsr.TradeID = row.Field<int>("TradeID");
            tsr.TradeName = row.Field<string>("TradeName");
            tsr.SectionID = row.Field<int>("SectionID");
            tsr.SectionName = row.Field<string>("SectionName");
            tsr.TraineeStatusTypeID = row.Field<int>("TraineeStatusTypeID");
            tsr.TraineeStatusName = row.Field<string>("TraineeStatusName");
            tsr.TraineeStatusChangeDate = row.Field<DateTime?>("TraineeStatusChangeDate");
            tsr.TraineeStatusChangeReason = row.Field<string>("TraineeStatusChangeReason");
            //tsr.Shift = row.Field<DateTime?>("Shift");
            tsr.DateOfBirth = row.Field<DateTime?>("DateOfBirth");
            tsr.CNICIssueDate = row.Field<DateTime?>("CNICIssueDate");
            tsr.IsDual = row.Field<bool>("IsDual");
            tsr.UpdatedDate = row.Field<DateTime?>("UpdatedDate");
            tsr.SchemeID = row.Field<int>("SchemeID");
            tsr.TSPID = row.Field<int>("TSPID");
            tsr.TSPName = row.Field<string>("TSPName");
            tsr.ClassID = row.Field<int>("ClassID");
            tsr.ClassCode = row.Field<string>("ClassCode");
            tsr.EducationID = row.Field<int>("EducationID");
            tsr.Education = row.Field<string>("Education");
            tsr.ContactNumber1 = row.Field<string>("ContactNumber1");
            tsr.CNICVerified = row.Field<bool>("CNICVerified");
            tsr.TraineeImg = traineeImage;
            tsr.CNICImg = traineeCNICImage;
            tsr.CNICImgNADRA = traineeCNICImageNadra;
            tsr.TraineeAge = row.Field<int>("TraineeAge");
            tsr.ClassStartDate = row.Field<DateTime?>("ClassStartDate");
            tsr.CertAuthID = row.Field<int>("CertAuthID");
            tsr.IsCreatedPRNCompletion = row.Field<bool?>("IsCreatedPRNCompletion");
            tsr.ClassEndDate = row.Field<DateTime?>("ClassEndDate");
            tsr.ClassStatusName = row.Field<string>("ClassStatusName");
            tsr.ClassStatusID = row.Field<int>("ClassStatusID");
            tsr.CertAuthName = row.Field<string>("CertAuthName");
            tsr.ClusterName = row.Field<string>("ClusterName");
            tsr.KAM = row.Field<string>("KAM");
            tsr.StartDate = row.Field<DateTime>("StartDate");
            tsr.EndDate = row.Field<DateTime>("EndDate");
            tsr.ReligionName = row.Field<string>("ReligionName");
            tsr.Dvv = row.Field<string>("Dvv");
            tsr.Disability = row.Field<string>("Disability");
            tsr.VoucherHolder = row.Field<bool>("VoucherHolder");
            tsr.VoucherNumber = row.Field<string>("VoucherNumber");
            tsr.VoucherOrganization = row.Field<string>("VoucherOrganization");
            tsr.WheredidYouHearAboutUs = row.Field<string>("WheredidYouHearAboutUs");
            tsr.TraineeIndividualIncomeID = row.Field<int>("TraineeIndividualIncomeID");
            tsr.HouseHoldIncomeID = row.Field<int>("HouseHoldIncomeID");
            tsr.EmploymentStatusBeforeTrainingID = row.Field<int>("EmploymentStatusBeforeTrainingID");
            tsr.Undertaking = row.Field<bool>("Undertaking");
            tsr.GuardianNextToKinName = row.Field<string>("GuardianNextToKinName");
            tsr.GuardianNextToKinContactNo = row.Field<string>("GuardianNextToKinContactNo");
            tsr.TraineeHouseNumber = row.Field<string>("TraineeHouseNumber");
            tsr.TraineeStreetMohalla = row.Field<string>("TraineeStreetMohalla");
            tsr.TraineeMauzaTown = row.Field<string>("TraineeMauzaTown");
            tsr.TraineeDistrictID = row.Field<int>("TraineeDistrictID");
            tsr.TraineeTehsilID = row.Field<int>("TraineeTehsilID");
            tsr.AgeVerified = row.Field<bool>("AgeVerified");
            tsr.DistrictVerified = row.Field<bool>("DistrictVerified");
            tsr.TraineeVerified = row.Field<int>("TraineeVerified") == 1 ? true : false;
            tsr.IsSubmitted = row.Field<bool>("IsSubmitted");
            tsr.CNICUnVerifiedReason = row.Field<string>("CNICUnVerifiedReason");
            tsr.AgeUnVerifiedReason = row.Field<string>("AgeUnVerifiedReason");
            tsr.ResidenceUnVerifiedReason = row.Field<string>("ResidenceUnVerifiedReason");
            tsr.CNICVerificationDate = row.Field<string>("CNICVerificationDate");
            tsr.ResultStatusID = row.Field<int?>("ResultStatusID") ?? 0;
            tsr.TraineeUID = row.Field<string>("TraineeUID");
            tsr.IsMigrated = row.Field<bool>("IsMigrated");
            tsr.ResultStatusChangeDate = row.Field<DateTime?>("ResultStatusChangeDate");
            tsr.ResultStatusChangeReason = row.Field<string>("ResultStatusChangeReason");
            tsr.IsManual = row.Field<bool>("IsManual");
            tsr.SchemeCode = row.Field<string>("SchemeCode");
            tsr.SchemeName = row.Field<string>("SchemeName");
            tsr.ResultDocument = row.Field<string>("ResultDocument");
            tsr.ResultDocument = !string.IsNullOrEmpty(tsr.ResultDocument) ? Common.GetFileBase64(tsr.ResultDocument) : string.Empty;
            tsr.ResultStatusName = row.Field<string>("ResultStatusName");
            tsr.TraineeRollNumber = row.Field<string>("TraineeRollNumber");
            tsr.TSROpeningDays = row.Field<int?>("TSROpeningDays") ?? 0;
            tsr.Shift = row.Field<string>("Shift");
            tsr.TraineeDistrictName = row.Field<string>("TraineeDistrictName");
            tsr.TraineeTehsilName = row.Field<string>("TraineeTehsilName");
            tsr.ClassDistrictName = row.Field<string>("ClassDistrictName");
            tsr.ClassTehsilName = row.Field<string>("ClassTehsilName");
            tsr.TrainingAddressLocation = row.Field<string>("TrainingAddressLocation");
            tsr.ClassUID = row.Field<string>("ClassUID");
            tsr.IsVarifiedCNIC = row.Field<bool>("IsVarifiedCNIC");
            tsr.SectorName = row.Field<string>("SectorName");
            tsr.IsExtra = row.Field<bool>("IsExtra");
            tsr.TraineeEmploymentStatus = row.Field<string>("TraineeEmploymentStatus");
            tsr.TraineeEmploymentVerificationStatus = row.Field<string>("TraineeEmploymentVerificationStatus");
            if (row.Table.Columns.Contains("IsDocumentGenerated"))
                tsr.IsDocumentGenerated = row.Field<int?>("IsDocumentGenerated") ?? 0;
            return tsr;
        }

        private TSRLiveDataModel RowOfTCRLiveData(DataRow row)
        {
            ////GetFiles by Path
            //string traineeImage = string.IsNullOrEmpty(row["TraineeImg"].ToString()) ? string.Empty : Common.GetFileBase64(row["TraineeImg"].ToString());
            //string traineeCNICImage = string.IsNullOrEmpty(row["CNICImg"].ToString()) ? string.Empty : Common.GetFileBase64(row["CNICImg"].ToString());
            //string traineeCNICImageNadra = string.IsNullOrEmpty(row.Field<string>("CNICImgNADRA")) ? string.Empty : Common.GetFileBase64(row.Field<string>("CNICImgNADRA"));

            TSRLiveDataModel tsr = new TSRLiveDataModel();
            tsr.TraineeID = row.Field<int>("TraineeID");
            tsr.TraineeCode = row.Field<string>("TraineeCode");
            tsr.TraineeName = row.Field<string>("TraineeName");
            tsr.TraineeCNIC = row.Field<string>("TraineeCNIC");
            tsr.FatherName = row.Field<string>("FatherName");
            tsr.GenderID = row.Field<int>("GenderID");
            tsr.GenderName = row.Field<string>("GenderName");
            tsr.ClassEndDate = row.Field<DateTime?>("ClassEndDate");

            tsr.TraineeStatusTypeID = row.Field<int>("TraineeStatusTypeID");
            tsr.TraineeStatusName = row.Field<string>("TraineeStatusName");
            tsr.TraineeStatusChangeDate = row.Field<DateTime?>("TraineeStatusChangeDate");
            tsr.TraineeStatusChangeReason = row.Field<string>("TraineeStatusChangeReason");
            //tsr.Shift = row.Field<DateTime?>("Shift");
            tsr.CNICIssueDate = row.Field<DateTime?>("CNICIssueDate");
            tsr.SchemeID = row.Field<int>("SchemeID");
            tsr.TSPID = row.Field<int>("TSPID");
            tsr.TSPName = row.Field<string>("TSPName");
            tsr.ClassID = row.Field<int>("ClassID");
            tsr.ClassCode = row.Field<string>("ClassCode");
            tsr.ResultStatusID = row.Field<int?>("ResultStatusID") ?? 0;
            tsr.ResultStatusChangeDate = row.Field<DateTime?>("ResultStatusChangeDate");
            tsr.ResultStatusChangeReason = row.Field<string>("ResultStatusChangeReason");
            tsr.ResultDocument = row.Field<string>("ResultDocument");
            tsr.ResultDocument = !string.IsNullOrEmpty(tsr.ResultDocument) ? Common.GetFileBase64(tsr.ResultDocument) : string.Empty;
            tsr.ResultStatusName = row.Field<string>("ResultStatusName");
            tsr.TraineeEmploymentStatus = row.Field<string>("TraineeEmploymentStatus");
            tsr.TraineeEmploymentVerificationStatus = row.Field<string>("TraineeEmploymentVerificationStatus");
            if (row.Table.Columns.Contains("IsDocumentGenerated"))
                tsr.IsDocumentGenerated = row.Field<int?>("IsDocumentGenerated") ?? 0;
            return tsr;
        }

        public List<TSRLiveDataModel> FetchTSRLiveDataByFilters(int[] filters)
        {
            List<TSRLiveDataModel> list = new List<TSRLiveDataModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                int traineeID = filters[3];
                int oID = filters[4];
                int userID = filters[5];
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TSPID", tspId);
                    param[2] = new SqlParameter("@ClassID", classId);
                    param[3] = new SqlParameter("@TraineeID", traineeID);
                    param[4] = new SqlParameter("@UserID", userID);
                    param[5] = new SqlParameter("@OID", oID);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSRData", param).Tables[0];
                    list = LoopinTSRLiveData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        public List<TSRLiveDataModel> GetFilteredTSRData(SearchFilter filters)
        {
            List<TSRLiveDataModel> list = new List<TSRLiveDataModel>();
            if (filters != null)
            {
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", filters.SchemeID);
                    param[1] = new SqlParameter("@TSPID", filters.TSPID);
                    param[2] = new SqlParameter("@ClassID", filters.ClassID);
                    param[3] = new SqlParameter("@TraineeID", filters.TraineeID);
                    param[4] = new SqlParameter("@UserID", filters.UserID);
                    param[5] = new SqlParameter("@OID", filters.OID);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSRData", param).Tables[0];
                    list = LoopinTSRData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }
         public List<GSRLiveDataModel> GetFilteredGSRData(SearchFilter filters)
        {
            List<GSRLiveDataModel> list = new List<GSRLiveDataModel>();
            if (filters != null)
            {
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", filters.SchemeID);
                    param[1] = new SqlParameter("@TSPID", filters.TSPID);
                    param[2] = new SqlParameter("@ClassID", filters.ClassID);
                    param[3] = new SqlParameter("@TraineeID", filters.TraineeID);

                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GSRData", param).Tables[0];
                    list = LoopinGSRData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        public List<TSRLiveDataModel> FetchTCRLiveDataByFilters(int[] filters)
        {
            List<TSRLiveDataModel> list = new List<TSRLiveDataModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                int traineeID = filters[3];
                int oID = filters[4];
                int userID = filters[5];
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TSPID", tspId);
                    param[2] = new SqlParameter("@ClassID", classId);
                    param[3] = new SqlParameter("@TraineeID", traineeID);
                    param[4] = new SqlParameter("@UserID", userID);
                    param[5] = new SqlParameter("@OID", oID);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSRData", param).Tables[0];
                    list = LoopinTCRLiveData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        private List<TSRLiveDataModel> LoopinTSRData(DataTable dt)
        {
            List<TSRLiveDataModel> tsrLiveModel = new List<TSRLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                tsrLiveModel.Add(RowOfTSRData(r));
            }
            return tsrLiveModel;
        }
        private List<GSRLiveDataModel> LoopinGSRData(DataTable dt)
        {
            List<GSRLiveDataModel> gsrLiveModel = new List<GSRLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                gsrLiveModel.Add(RowOfGSRData(r));
            }
            return gsrLiveModel;
        }

        private List<TSRLiveDataModel> LoopinTCRLiveData(DataTable dt)
        {
            List<TSRLiveDataModel> tsrLiveModel = new List<TSRLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                tsrLiveModel.Add(RowOfTCRLiveData(r));
            }
            return tsrLiveModel;
        }

        private TSRLiveDataModel RowOfTSRData(DataRow row)
        {
            //GetFiles by Path
            //string traineeImage = string.IsNullOrEmpty(row["TraineeImg"].ToString()) ? string.Empty : Common.GetFileBase64(row["TraineeImg"].ToString());
            //string traineeCNICImage = string.IsNullOrEmpty(row["CNICImg"].ToString()) ? string.Empty : Common.GetFileBase64(row["CNICImg"].ToString());
            //string traineeCNICImageNadra = string.IsNullOrEmpty(row.Field<string>("CNICImgNADRA")) ? string.Empty : Common.GetFileBase64(row.Field<string>("CNICImgNADRA"));
            //string resultDocument = string.IsNullOrEmpty(row.Field<string>("ResultDocument")) ? string.Empty : Common.GetFileBase64(row.Field<string>("ResultDocument"));

            TSRLiveDataModel tsr = new TSRLiveDataModel();
            tsr.TraineeID = row.Field<int>("TraineeID");
            tsr.TraineeCode = row.Field<string>("TraineeCode");
            tsr.TraineeName = row.Field<string>("TraineeName");
            tsr.TraineeCNIC = row.Field<string>("TraineeCNIC");
            tsr.FatherName = row.Field<string>("FatherName");
            tsr.GenderID = row.Field<int>("GenderID");
            tsr.GenderName = row.Field<string>("GenderName");
            tsr.Batch = row.Field<int>("Batch");
            tsr.TradeID = row.Field<int>("TradeID");
            tsr.TradeName = row.Field<string>("TradeName");
            tsr.SectionID = row.Field<int>("SectionID");
            tsr.SectionName = row.Field<string>("SectionName");
            tsr.TraineeStatusTypeID = row.Field<int>("TraineeStatusTypeID");
            tsr.TraineeStatusName = row.Field<string>("TraineeStatusName");
            tsr.TraineeStatusChangeDate = row.Field<DateTime?>("TraineeStatusChangeDate");
            tsr.TraineeStatusChangeReason = row.Field<string>("TraineeStatusChangeReason");
            //tsr.Shift = row.Field<DateTime?>("Shift");
            tsr.DateOfBirth = row.Field<DateTime?>("DateOfBirth");
            tsr.CNICIssueDate = row.Field<DateTime?>("CNICIssueDate");
            tsr.IsDual = row.Field<bool>("IsDual");
            tsr.UpdatedDate = row.Field<DateTime?>("UpdatedDate");
            tsr.SchemeID = row.Field<int>("SchemeID");
            tsr.TSPID = row.Field<int>("TSPID");
            tsr.TSPName = row.Field<string>("TSPName");
            tsr.ClassID = row.Field<int>("ClassID");
            tsr.ClassCode = row.Field<string>("ClassCode");
            tsr.EducationID = row.Field<int>("EducationID");
            tsr.Education = row.Field<string>("Education");
            tsr.ContactNumber1 = row.Field<string>("ContactNumber1");
            tsr.CNICVerified = row.Field<bool>("CNICVerified");
            tsr.TraineeImg = row.Field<string>("TraineeImg");
            tsr.CNICImg = row.Field<string>("CNICImg");
            tsr.CNICImgNADRA = row.Field<string>("CNICImgNADRA");
            tsr.TraineeAge = row.Field<int>("TraineeAge");
            tsr.ClassStartDate = row.Field<DateTime?>("ClassStartDate");
            tsr.CertAuthID = row.Field<int>("CertAuthID");
            tsr.IsCreatedPRNCompletion = row.Field<bool?>("IsCreatedPRNCompletion");
            tsr.ClassEndDate = row.Field<DateTime?>("ClassEndDate");
            tsr.ClassStatusName = row.Field<string>("ClassStatusName");
            tsr.ClassStatusID = row.Field<int>("ClassStatusID");
            tsr.CertAuthName = row.Field<string>("CertAuthName");
            tsr.ClusterName = row.Field<string>("ClusterName");
            tsr.KAM = row.Field<string>("KAM");
            tsr.VoucherHolder = row.Field<bool>("VoucherHolder");
            tsr.VoucherNumber = row.Field<string>("VoucherNumber");
            tsr.VoucherOrganization = row.Field<string>("VoucherOrganization");
            tsr.WheredidYouHearAboutUs = row.Field<string>("WheredidYouHearAboutUs");
            tsr.TraineeIndividualIncomeID = row.Field<int?>("TraineeIndividualIncomeID") ?? 0;
            tsr.HouseHoldIncomeID = row.Field<int>("HouseHoldIncomeID");
            tsr.EmploymentStatusBeforeTrainingID = row.Field<int>("EmploymentStatusBeforeTrainingID");
            tsr.Undertaking = row.Field<bool>("Undertaking");
            tsr.GuardianNextToKinName = row.Field<string>("GuardianNextToKinName");
            tsr.GuardianNextToKinContactNo = row.Field<string>("GuardianNextToKinContactNo");
            tsr.TraineeHouseNumber = row.Field<string>("TraineeHouseNumber");
            tsr.TraineeStreetMohalla = row.Field<string>("TraineeStreetMohalla");
            tsr.TraineeMauzaTown = row.Field<string>("TraineeMauzaTown");
            tsr.TraineeDistrictID = row.Field<int>("TraineeDistrictID");
            tsr.TraineeTehsilID = row.Field<int>("TraineeTehsilID");
            tsr.AgeVerified = row.Field<bool>("AgeVerified");
            tsr.DistrictVerified = row.Field<bool>("DistrictVerified");
            tsr.TraineeVerified = row.Field<int>("TraineeVerified") == 1 ? true : false;
            tsr.IsSubmitted = row.Field<bool>("IsSubmitted");
            tsr.CNICUnVerifiedReason = row.Field<string>("CNICUnVerifiedReason");
            tsr.AgeUnVerifiedReason = row.Field<string>("AgeUnVerifiedReason");
            tsr.ResidenceUnVerifiedReason = row.Field<string>("ResidenceUnVerifiedReason");
            tsr.CNICVerificationDate = row.Field<string>("CNICVerificationDate");
            tsr.ResultStatusID = row.Field<int?>("ResultStatusID") ?? 0;
            tsr.TraineeUID = row.Field<string>("TraineeUID");
            tsr.IsMigrated = row.Field<bool>("IsMigrated");
            tsr.ResultStatusChangeDate = row.Field<DateTime?>("ResultStatusChangeDate");
            tsr.ResultStatusChangeReason = row.Field<string>("ResultStatusChangeReason");
            tsr.IsManual = row.Field<bool>("IsManual");
            tsr.SchemeCode = row.Field<string>("SchemeCode");
            tsr.SchemeName = row.Field<string>("SchemeName");
            tsr.ResultDocument = row.Field<string>("ResultDocument");
            tsr.ResultStatusName = row.Field<string>("ResultStatusName");
            tsr.TraineeRollNumber = row.Field<string>("TraineeRollNumber");
            tsr.TSROpeningDays = row.Field<int?>("TSROpeningDays") ?? 0;
            tsr.Shift = row.Field<string>("Shift");
            tsr.TraineeDistrictName = row.Field<string>("TraineeDistrictName");
            tsr.TraineeTehsilName = row.Field<string>("TraineeTehsilName");
            tsr.ClassDistrictName = row.Field<string>("ClassDistrictName");
            tsr.ClassTehsilName = row.Field<string>("ClassTehsilName");
            tsr.TrainingAddressLocation = row.Field<string>("TrainingAddressLocation");
            tsr.ClassUID = row.Field<string>("ClassUID");
            tsr.IsVarifiedCNIC = row.Field<bool>("IsVarifiedCNIC");
            tsr.SectorName = row.Field<string>("SectorName");
            tsr.IsExtra = row.Field<bool>("IsExtra");
            tsr.TraineeEmploymentStatus = row.Field<string>("TraineeEmploymentStatus");
            tsr.TraineeEmploymentVerificationStatus = row.Field<string>("TraineeEmploymentVerificationStatus");
            tsr.TraineeEmail = row.Field<string>("TraineeEmail");

            if (row.Table.Columns.Contains("Dvv")) //edited by ali
            {
                tsr.StartDate = row.Field<DateTime>("StartDate");
                tsr.EndDate = row.Field<DateTime>("EndDate");
                tsr.Dvv = row.Field<string>("Dvv");
                tsr.Disability = row.Field<string>("Disability");
                tsr.ReligionName = row.Field<string>("ReligionName");
                tsr.CertAuthName = row.Field<string>("CertAuthName");
                tsr.ClassStatusName = row.Field<string>("ClassStatusName");
                tsr.ProvinceName = row.Field<string>("ProvinceName");
            }
            if (row.Table.Columns.Contains("IBANNumber"))
            {
                tsr.IBANNumber = row.Field<string>("IBANNumber");
                tsr.BankName = row.Field<string>("BankName");
                tsr.Accounttitle = row.Field<string>("Accounttitle");
            }
            return tsr;
        }

        private GSRLiveDataModel RowOfGSRData(DataRow row)
        {
            GSRLiveDataModel gsr = new GSRLiveDataModel();
            gsr.TraineeCode = row.Field<string>("TraineeCode");
            gsr.TraineeName = row.Field<string>("TraineeName");
            gsr.TraineeCNIC = row.Field<string>("TraineeCNIC");
            gsr.FatherName = row.Field<string>("FatherName");
            gsr.TSPName = row.Field<string>("TSPName");
            gsr.GuruContactNumber = row.Field<string>("GuruContactNumber");
            gsr.ClassCode = row.Field<string>("ClassCode");
            gsr.GuruCNIC = row.Field<string>("GuruCNIC");
            gsr.GuruName = row.Field<string>("GuruName");

            return gsr;
        }

        public List<TSRLiveDataModel> UpdateTraineeStatusExpell(TSRLiveDataModel traineestatus)
        {

            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@TraineeStatusTypeID", 4); //Expell ID
                param[1] = new SqlParameter("@TraineeProfileID", traineestatus.CoursraTraineeIDs);
                param[2] = new SqlParameter("@Comments", traineestatus.Comment);
                param[3] = new SqlParameter("@CreatedUserID", traineestatus.CurUserID);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeStatusInGroup]", param);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return null;
        }

        public List<TSRLiveDataModel> UpdateTraineeStatusDropout(TSRLiveDataModel traineestatus)
        {

            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@TraineeStatusTypeID", 3); //Dropout ID
                param[1] = new SqlParameter("@TraineeProfileID", traineestatus.TraineeID);
                param[2] = new SqlParameter("@Comments", traineestatus.Comment);
                param[3] = new SqlParameter("@CreatedUserID", traineestatus.CurUserID);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeStatusInGroup]", param);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return null;
        }

        public List<TSRLiveDataModel> FetchCourseraPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                DataTable dt = new DataTable();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                if (filterModel.SchemeID == 1)
                { dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CourseEraInvitationNotAccepted").Tables[0]; }
                else if (filterModel.SchemeID == 2)
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CourseEra14DaysInActive", param.ToArray()).Tables[0];
                }
                else if (filterModel.SchemeID == 3)
                { dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CourseEra28DaysInActive").Tables[0]; }
                else if(filterModel.SchemeID >= 4)
                { 
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CourseEra14DaysInActive", param.ToArray()).Tables[0];
                }

                if (dt.Rows.Count > 0)
                    totalCount = dt.Rows.Count;
                else
                    totalCount = 0;
                return LoopinCourseraPaged(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<TSRLiveDataModel> LoopinCourseraPaged(DataTable dt)
        {
            List<TSRLiveDataModel> tsrLiveModel = new List<TSRLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                tsrLiveModel.Add(RowOfCourseraDatapaged(r));
            }
            return tsrLiveModel;
        }

        private TSRLiveDataModel RowOfCourseraDatapaged(DataRow row)
        {
            TSRLiveDataModel tsr = new TSRLiveDataModel();
            tsr.TraineeID = row.Field<int>("TraineeID");
            tsr.TraineeCode = row.Field<string>("TraineeCode");
            tsr.TraineeName = row.Field<string>("TraineeName");
            tsr.TraineeCNIC = row.Field<string>("TraineeCNIC");
            tsr.FatherName = row.Field<string>("FatherName");
            tsr.GenderName = row.Field<string>("GenderName");
            tsr.ContactNumber1 = row.Field<string>("ContactNumber1");
            tsr.TraineeAge = row.Field<int>("TraineeAge");
            tsr.TraineeEmail = row.Field<string>("TraineeEmail");
            tsr.InvitationDate = row.Field<DateTime>("InvitationDate");
            if (row.Table.Columns.Contains("LastActivityDate"))
            {
                tsr.LastActivityDate = row.Field<DateTime>("LastActivityDate");
                tsr.PercentageCompleted = row.Field<double>("PercentageCompleted");
                tsr.DaysSinceLastActivity = row.Field<int>("DaysSinceLastActivity");
            }
            return tsr;
        }
        public List<TSRLiveDataModel> FetchPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));
                param.Add(new SqlParameter("@OID", filterModel.OID));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSRDataPaged", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinTSRLiveDataPaged(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TERLiveDataModel> FetchTERPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                // param.Add(new SqlParameter("@UserID", filterModel.UserID));
                // param.Add(new SqlParameter("@OID", filterModel.OID));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TERPaged", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                else
                    totalCount = 0;
                return LoopinTERLiveDataPaged(dt);

               
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TARLiveDataModel> FetchTARPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                // New parameters for filtering by Month and Year
                param.Add(new SqlParameter("@Month", filterModel.Month ?? (object)DBNull.Value));
                param.Add(new SqlParameter("@Year", filterModel.Year ?? (object)DBNull.Value));
                // param.Add(new SqlParameter("@UserID", filterModel.UserID));
                // param.Add(new SqlParameter("@OID", filterModel.OID));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TAR_Paged", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                else
                    totalCount = 0;
                return LoopinTARLiveDataPaged(dt);


            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public List<TARCWLiveDataModel> FetchTARPagedClasswise(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                // New parameters for filtering by Month and Year
                param.Add(new SqlParameter("@Month", filterModel.Month ?? (object)DBNull.Value));
                param.Add(new SqlParameter("@Year", filterModel.Year ?? (object)DBNull.Value));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));
                // param.Add(new SqlParameter("@OID", filterModel.OID));
                 param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TARPagedClasswise", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                else
                    totalCount = 0;
                return LoopinTARCWLiveDataPaged(dt);


            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<GSRLiveDataModel> FetchGSRPaged(PagingModel pagingModel, SearchFilter filterModel)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GSRDataPaged", param.ToArray()).Tables[0];

                return LoopinGSRLiveDataPaged(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        private List<TSRLiveDataModel> LoopinTSRLiveDataPaged(DataTable dt)
        {
            List<TSRLiveDataModel> tsrLiveModel = new List<TSRLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                tsrLiveModel.Add(RowOfTSRLiveDatapaged(r));
            }
            return tsrLiveModel;
        } 
        private List<TERLiveDataModel> LoopinTERLiveDataPaged(DataTable dt)
        {
            List<TERLiveDataModel> terLiveModel = new List<TERLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                terLiveModel.Add(RowOfTERLiveDatapaged(r));
            }
            return terLiveModel;
        } 
        private List<TARLiveDataModel> LoopinTARLiveDataPaged(DataTable dt)
        {
            List<TARLiveDataModel> tarLiveModel = new List<TARLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                tarLiveModel.Add(RowOfTARLiveDatapaged(r));
            }
            return tarLiveModel;
        } 
        private List<TARCWLiveDataModel> LoopinTARCWLiveDataPaged(DataTable dt)
        {
            List<TARCWLiveDataModel> tarcwLiveModel = new List<TARCWLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                tarcwLiveModel.Add(RowOfTARCWLiveDatapaged(r));
            }
            return tarcwLiveModel;
        } 
        
        private List<GSRLiveDataModel> LoopinGSRLiveDataPaged(DataTable dt)
        {
            List<GSRLiveDataModel> gsrLiveModel = new List<GSRLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                gsrLiveModel.Add(RowOfGSRLiveDatapaged(r));
            }
            return gsrLiveModel;
        }

        private TSRLiveDataModel RowOfTSRLiveDatapaged(DataRow row)
        {
            TSRLiveDataModel tsr = new TSRLiveDataModel();
            tsr.TraineeID = row.Field<int>("TraineeID");
            tsr.TraineeCode = row.Field<string>("TraineeCode");
            tsr.TraineeName = row.Field<string>("TraineeName");
            tsr.TraineeCNIC = row.Field<string>("TraineeCNIC");
            tsr.FatherName = row.Field<string>("FatherName");
            tsr.GenderName = row.Field<string>("GenderName");
            tsr.ContactNumber1 = row.Field<string>("ContactNumber1");
            tsr.TraineeAge = row.Field<int>("TraineeAge");
            tsr.GuardianNextToKinName = row.Field<string>("GuardianNextToKinName");
            tsr.TraineeDistrictName = row.Field<string>("TraineeDistrictName");
            tsr.TraineeStatusName = row.Field<string>("StatusName");
            tsr.ProvinceName = row.Field<string>("ProvinceName");
            tsr.StartDate = row.Field<DateTime>("StartDate");
            tsr.EndDate = row.Field<DateTime>("EndDate");
            tsr.ClassStatusName = row.Field<string>("ClassStatusName");
            tsr.Dvv = row.Field<string>("Dvv");
            tsr.Disability = row.Field<string>("Disability");
            tsr.ReligionName = row.Field<string>("ReligionName");
            tsr.CertAuthName = row.Field<string>("CertAuthName");
            if (row.Table.Columns.Contains("IBANNumber"))
            {
                tsr.IBANNumber = row.Field<string>("IBANNumber");
                tsr.BankName = row.Field<string>("BankName");
                tsr.Accounttitle = row.Field<string>("Accounttitle");
            }
            return tsr;
        }
         private TERLiveDataModel RowOfTERLiveDatapaged(DataRow row)
        {
            TERLiveDataModel ter = new TERLiveDataModel();
            ter.TraineeCode = row.Field<string>("TraineeCode");
            ter.TraineeID = row.Field<int>("TraineeID");
            ter.TraineeName = row.Field<string>("TraineeName");
            ter.TraineeCNIC = row.Field<string>("TraineeCNIC");
            ter.FatherName = row.Field<string>("FatherName");
            ter.SchemeName = row.Field<string>("SchemeName");
            ter.ClassCode = row.Field<string>("ClassCode");
            ter.IsEnrolled = row.Field<bool>("IsEnrolled");
            return ter;
        }

        private TARLiveDataModel RowOfTARLiveDatapaged(DataRow row)
        {
            TARLiveDataModel tar = new TARLiveDataModel();

            tar.TraineeCode = row.Field<string>("TraineeCode");
            tar.TraineeID = row.Field<int>("TraineeID");
            tar.TraineeName = row.Field<string>("TraineeName");
            tar.TraineeCNIC = row.Field<string>("TraineeCNIC");
            tar.FatherName = row.Field<string>("FatherName");
            tar.SchemeName = row.Field<string>("SchemeName");
            tar.ClassID = row.Field<int>("ClassID");
            tar.ClassCode = row.Field<string>("ClassCode");
            tar.AttendanceDate = row["AttendanceDate"] == DBNull.Value ? (DateTime?)null : row.Field<DateTime>("AttendanceDate");
            tar.CheckIn = row["CheckInTime"] == DBNull.Value ? (DateTime?)null : row.Field<DateTime>("CheckInTime");
            tar.CheckOut = row["CheckOutTime"] == DBNull.Value ? (DateTime?)null : row.Field<DateTime>("CheckOutTime");
            tar.ClassStartTime = row["ClassStartTime"] == DBNull.Value ? (DateTime?)null : row.Field<DateTime>("ClassStartTime");
            tar.ClassEndTime = row["ClassEndTime"] == DBNull.Value ? (DateTime?)null : row.Field<DateTime>("ClassEndTime");
            tar.ClassTotalHours = row.Field<string>("ClassTotalHours");
            tar.EnrolledTrainees = row.Field<int>("EnrolledTrainees");
            tar.Shift = row.Field<string>("Shift");
            tar.TrainingDaysNo = row.Field<int>("TrainingDaysNo");
            tar.TrainingDays = row.Field<string>("TrainingDays");
            tar.KAM = row.Field<string>("KAM");
            tar.TraineeStatusName = row.Field<string>("TraineeStatusName");
            tar.ClassDistrictName = row.Field<string>("ClassDistrictName");
            tar.ClassStartDate = row.Field<DateTime>("ClassStartDate");
            tar.ClassEndDate = row.Field<DateTime>("ClassEndDate");
            tar.ClassStatusName = row.Field<string>("ClassStatusName");
            tar.TSPName = row.Field<string>("TSPName");
            tar.MPRTraineeStatus = row.Field<string>("MPRTraineeStatus");
            tar.TPMVisitDateMarking1 = row.IsNull("TPMVisitDateMarking1") ? (DateTime?)null : row.Field<DateTime>("TPMVisitDateMarking1");
            tar.TPMVisitDateMarking2 = row.IsNull("TPMVisitDateMarking2") ? (DateTime?)null : row.Field<DateTime>("TPMVisitDateMarking2");

            return tar;
        }
        private TARCWLiveDataModel RowOfTARCWLiveDatapaged(DataRow row)
        {
            TARCWLiveDataModel tarcw = new TARCWLiveDataModel();

            tarcw.Scheme = row.Field<string>("Scheme");
            tarcw.TSP = row.Field<string>("TSP");
            tarcw.ClassCode = row.Field<string>("ClassCode");
            tarcw.District = row.Field<string>("District");
            tarcw.ClassStartDate = row.Field<DateTime>("ClassStartDate");
            tarcw.ClassEndDate = row.Field<DateTime>("ClassEndDate");
            tarcw.AttendanceDate = row.Field<DateTime>("AttendanceDate");
            tarcw.NumberOfTrainees = row.Field<int>("NumberOfTrainees");
            tarcw.TotalTraineesPerClass = row.Field<int>("TotalTraineesPerClass");
            tarcw.OnRollCompletedTraineesPresent = row.Field<int>("OnRollCompletedTraineesPresent");
            tarcw.OnRollCompletedTraineesAbsent = row.Field<int>("OnRollCompletedTraineesAbsent");
            tarcw.DataSource = row.Field<string>("DataSource");
            tarcw.TotalOnRollCompletedTrainees = row.Field<int>("TotalOnRollCompletedTrainees");
            tarcw.OnRollCompletedTraineesRatio = Convert.ToSingle(row.Field<decimal>("OnRollCompletedTraineesRatio"));

            return tarcw;
        }


        private GSRLiveDataModel RowOfGSRLiveDatapaged(DataRow row)
        {
            GSRLiveDataModel gsr = new GSRLiveDataModel();
            gsr.TraineeCode = row.Field<string>("TraineeCode");
            gsr.TraineeName = row.Field<string>("TraineeName");
            gsr.TraineeCNIC = row.Field<string>("TraineeCNIC");
            gsr.FatherName = row.Field<string>("FatherName");
            gsr.TSPName = row.Field<string>("TSPName");
            gsr.GuruContactNumber = row.Field<string>("GuruContactNumber");
            gsr.ClassCode = row.Field<string>("ClassCode");
            gsr.GuruCNIC = row.Field<string>("GuruCNIC");
            gsr.GuruName = row.Field<string>("GuruName");
            return gsr;
        }

        public List<TSRLiveDataModel> FetchTSULiveData(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));
                param.Add(new SqlParameter("@OID", filterModel.OID));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSUDataPaged", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinTSRData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSRLiveDataModel> FetchClassLiveData(PagingModel pagingModel, SearchFilter filterModel, int UserLevel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));
                param.Add(new SqlParameter("@OID", filterModel.OID));
                param.Add(new SqlParameter("@UserLevel", UserLevel));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassDataPaged", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinClassData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<TSRLiveDataModel> LoopinClassData(DataTable dt)
        {
            List<TSRLiveDataModel> tsrLiveModel = new List<TSRLiveDataModel>();
            foreach (DataRow r in dt.Rows)
            {
                tsrLiveModel.Add(RowOfClassData(r));
            }
            return tsrLiveModel;
        }

        private TSRLiveDataModel RowOfClassData(DataRow row)
        {
            //GetFiles by Path
            //string traineeImage = string.IsNullOrEmpty(row["TraineeImg"].ToString()) ? string.Empty : Common.GetFileBase64(row["TraineeImg"].ToString());
            //string traineeCNICImage = string.IsNullOrEmpty(row["CNICImg"].ToString()) ? string.Empty : Common.GetFileBase64(row["CNICImg"].ToString());
            //string traineeCNICImageNadra = string.IsNullOrEmpty(row.Field<string>("CNICImgNADRA")) ? string.Empty : Common.GetFileBase64(row.Field<string>("CNICImgNADRA"));
            //string resultDocument = string.IsNullOrEmpty(row.Field<string>("ResultDocument")) ? string.Empty : Common.GetFileBase64(row.Field<string>("ResultDocument"));

            TSRLiveDataModel tsr = new TSRLiveDataModel();
            tsr.Batch = row.Field<int>("Batch");
            tsr.TradeID = row.Field<int>("TradeID");
            tsr.IsDual = row.Field<bool>("IsDual");
            tsr.SchemeID = row.Field<int>("SchemeID");
            tsr.TSPID = row.Field<int>("TSPID");
            tsr.TSPName = row.Field<string>("TSPName");
            tsr.ClassID = row.Field<int>("ClassID");
            tsr.ClassCode = row.Field<string>("ClassCode");
            tsr.ClassStartDate = row.Field<DateTime?>("ClassStartDate");
            tsr.IsCreatedPRNCompletion = row.Field<bool?>("IsCreatedPRNCompletion");
            tsr.ClassEndDate = row.Field<DateTime?>("ClassEndDate");
            tsr.ClassStatusName = row.Field<string>("ClassStatusName");
            tsr.ClassStatusID = row.Field<int>("ClassStatusID");
            tsr.SchemeCode = row.Field<string>("SchemeCode");
            tsr.SchemeName = row.Field<string>("SchemeName");
            tsr.Duration = row.Field<decimal>("Duration");
            return tsr;
        }

        public bool UpdateClassStatus(int ClassID, int ClassStatusID, int CurUserID, string ClassReason)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@ClassID", ClassID);
                param[1] = new SqlParameter("@UserId", CurUserID);
                //
                param[2] = new SqlParameter("@Reason", ClassReason);
                if (ClassStatusID != 6)
                {
                    param[3] = new SqlParameter("@ClassStatusID", ClassStatusID);
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[ClassAbandonedProcedure]", param);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[ClassCancellationProcedure]", param);
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