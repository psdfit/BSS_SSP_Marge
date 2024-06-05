using DataLayer.Dapper;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class SRVTrn: ISRVTrn
    {
        public IDapperConfig _dapper { get; set; }
        public SRVTrn(IDapperConfig dapper)
        {
            _dapper = dapper;
        }
        public async Task AddTRN()
        {
            var list = await _dapper.QueryAsync<Get_TRN_Classes_Model>("dbo.Get_TRN_Classes", new { }, CommandType.StoredProcedure).ConfigureAwait(true);
            if (list.Any())
            {
                var certGroup = list.GroupBy(x => x.CertAuthID).ToList();
                foreach (var item in certGroup)
                {
                    var schemeGroup = item.GroupBy(x => x.SchemeID).ToList();
                    foreach (var schemeitem in schemeGroup)
                    {

                        var trnMaster = new TRNMasterModel
                        {
                            CertAuthID = item.FirstOrDefault().CertAuthID,
                            SchemeID = item.FirstOrDefault().SchemeID,
                            CreatedDate = DateTime.Now,
                            InActive = false,
                            ProcessKey = EnumApprovalProcess.PRN_T
                        };
                        var query = $@"INSERT INTO [dbo].[TRNMaster]
                    ([ProcessKey],[CertAuthID],[SchemeID], [CreatedDate], [InActive])
                    VALUES
                    (
                     @ProcessKey,@CertAuthID,@SchemeID,@CreatedDate, @InActive
                    ); SELECT IDENT_CURRENT('TRNMaster')";

                        trnMaster.TRNMasterID = _dapper.ExecuteScalar<int>(query, trnMaster);
                        if (trnMaster.TRNMasterID > 0)
                        {

                            var appQuery = $@"INSERT INTO dbo.ApprovalHistory
                                       (
                                          ProcessKey
                                        , Step
                                        , FormID
                                        , ApproverID
                                        , Comments
                                        , ApprovalStatusID
                                        , CreatedUserID
                                        , ModifiedUserID
                                        , CreatedDate
                                        , ModifiedDate
                                        , InActive
                                       )
                                       VALUES
                                       (  '{EnumApprovalProcess.PRN_T}'
                                        , 1           
                                        , {trnMaster.TRNMasterID}
                                        , NULL        
                                        , 'Pending'  
                                        , 1           
                                        , 0           
                                        , NULL        
                                        , GETDATE()   
                                        , NULL        
                                        , 0           
                                          )";

                            var approvalEntry = _dapper.ExecuteScalar<int>(appQuery);

                            foreach (var classItem in item)
                            {
                                var trnExistQuery = $@"SELECT TRNID FROM TRN  
                                Where ClassID = @classID AND CertAuthID = @certAuthID AND InActive=0";
                                var trnExist = _dapper.Query<TRNModel>(trnExistQuery, new { @classID = classItem.ClassID, @certAuthID = classItem.CertAuthID });
                                if (trnExist.Count <= 0)
                                {
                                    var trn = new TRNModel
                                    {
                                        ProcessKey = EnumApprovalProcess.PRN_T,
                                        CertAuthID = classItem.CertAuthID,
                                        ClassID = classItem.ClassID,
                                        ClassCode = classItem.ClassCode,
                                        ClassEndDate = classItem.EndDate,
                                        ClassStartDate = classItem.StartDate,
                                        Duration = classItem.Duration,
                                        InvoiceNo = "1",
                                        TradeID = classItem.TradeID,
                                        TRNMasterID = trnMaster.TRNMasterID,
                                        AbsentUnverified = classItem.AbsentUnverified,
                                        AbsentVerified = classItem.AbsentVerified,
                                        ClaimedTrainees = 0,
                                        ContractualTrainees = classItem.TraineesPerClass,
                                        EnrolledTrainees = classItem.TotalTrainee,
                                        CNICUnverified = classItem.CNICUnVerified,
                                        CNICUnVExcesses = classItem.CNICUnVExcesses,
                                        CNICVerified = classItem.CNICVerified,
                                        CNICVExcesses = classItem.CNICVExcesses,
                                        FailedUnverified = classItem.FailedUnverified,
                                        FailedVerified = classItem.FailedVerified,
                                        PassUnverified = classItem.PassUnverified,
                                        PassVerified = classItem.PassVerified,
                                        CreatedDate = DateTime.Now,
                                        PaymentToBeReleased = classItem.TotalTraineeResult
                                    };

                                    var trnQuery = $@"INSERT INTO [dbo].[TRN]
                                    ([CertAuthID],[ClassID], [ClassCode], [ClassEndDate], [ClassStartDate]
                                    , [Duration], [InvoiceNo], [TradeID], [TRNMasterID], [ProcessKey], [AbsentUnverified], [AbsentVerified]
                                    , [CNICUnverified], [CNICUnVExcesses], [CNICVerified], [CNICVExcesses], [FailedUnverified], [FailedVerified]
                                    , [PassUnverified], [PassVerified], [ClaimedTrainees], [EnrolledTrainees], [CreatedDate], [PaymentToBeReleased])
                                    VALUES
                                    (
                                     @CertAuthID,@ClassID,@ClassCode, @ClassEndDate, @ClassStartDate, @Duration, @InvoiceNo, @TradeID
                                    , @TRNMasterID, @ProcessKey, @AbsentUnverified, @AbsentVerified, @CNICUnverified, @CNICUnVExcesses
                                    , @CNICVerified, @CNICVExcesses, @FailedUnverified, @FailedVerified, @PassUnverified, @PassVerified
                                    , @ClaimedTrainees, @EnrolledTrainees, @CreatedDate, @PaymentToBeReleased
                                    ); SELECT IDENT_CURRENT('TRN')";

                                    trn.TRNID = _dapper.ExecuteScalar<int>(trnQuery, trn);
                                    var updateClassQuery = $@"update class set IsCreatedTRN=1 where ClassID =" + trn.ClassID;

                                    _dapper.ExecuteQuery(updateClassQuery, null, CommandType.Text);

                                }
                            }
                        }

                    }

               
                }
            }
        }
        public List<TRNMasterModel> FetchTRN(QueryFilters model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@CertAuthID", model.CertAuthID));
                param.Add(new SqlParameter("@Month", model.Month.HasValue ? model.Month.Value : model.Month));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[dbo].[RD_TRNMaster]", param.ToArray()).Tables[0];
                List<TRNMasterModel> list = new List<TRNMasterModel>();
                list = Helper.ConvertDataTableToModel<TRNMasterModel>(dt);

                return (list);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TRNModel> FetchTRNDetails(int trnMasterID)
        {
            try
            {
                var list = _dapper.Query<TRNModel>("dbo.RD_TRNDetails", new { @TRNMasterID = trnMasterID }, CommandType.StoredProcedure);
                return list;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public TRNMasterModel GetCertAuthIDAndName(int trnMasterID)
        {
            try
            {
                var query = @"Select tm.TRNMasterID, ca.CertAuthName, tm.CertAuthID From TRNMaster tm
                                INNER JOIN CertificationAuthority ca ON ca.CertAuthID = tm.CertAuthID
                                Where tm.TRNMasterID = @trnMasterID";

                var trnMaster = _dapper.QuerySingle<TRNMasterModel>(query, new { @trnMasterID = trnMasterID });
                return trnMaster;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool GenerateTRN(QueryFilters model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@CertAuthID", model.CertAuthID));
                param.Add(new SqlParameter("@ClassIDs", model.ClassIDs));
                param.Add(new SqlParameter("@CurUserID", model.UserID));
                param.Add(new SqlParameter("@Month", model.Month));
                SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.GenerateTRN", param.ToArray());
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TRNModel> GetTRNExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_TRN_By_IDs", new SqlParameter("@TRNMasterIDs", ids)).Tables[0];
            
            List<TRNModel> list = new List<TRNModel>();
            list = Helper.ConvertDataTableToModel<TRNModel>(dt);
            return (list);
        }
        public List<TRNModel> GetTRNExcelExport(TRNMasterModel model)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Month", model.Month));
            param.Add(new SqlParameter("@TRNMasterID", model.TRNMasterID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.TRN_Excel_Export", param.ToArray()).Tables[0];

            List<TRNModel> list = new List<TRNModel>();
            list = Helper.ConvertDataTableToModel<TRNModel>(dt);
            return (list);
        }
    }
}
