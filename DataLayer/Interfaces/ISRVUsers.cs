using DataLayer.Models;
using DataLayer.Models.SSP;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVUsers
    {

        DataTable CheckNTN(SignUpModel UserNTN);
        DataTable CheckEmail(SignUpModel UserEmail);
        DataTable SignUp(SignUpModel User);
        DataTable OTPVerification(SignUpModel UserOTP);
        List<SignUpModel> SaveUser(SignUpModel User);

        UsersModel GetSession(UsersModel mod);
        UsersModel GetByUserID(int UserID, SqlTransaction transaction = null);
        UsersModel SSPGetByUserID(int UserID, SqlTransaction transaction = null);

        UsersModel GetKamIdByTspUserId(int UserID);

        List<object> SaveUsers(UsersModel Users, SqlTransaction transaction = null);

        bool UpdateUserPassword(int userID, string password);

        // bool DeleteUserLogin(int userID, string SessionID);
        List<UsersModel> DeleteUserLogin(UsersModel mod);

        List<UsersModel> FetchUsers(UsersModel mod);

        List<UsersModel> FetchPaged(PagingModel mod, out int TotCount, SqlTransaction transaction = null);

        List<UsersModel> FetchUsers();

        List<UsersModel> FetchInternalUsers();

        List<UsersModel> FetchUsers(bool InActive);

        void ActiveInActive(int UserID, bool? InActive, int CurUserID);

        UsersModel Login(string UserName, string UserPassword);
        UsersModel Login(string UserName, string UserPassword, string IPAddress);

        UsersModel SSPLogin(string UserName, string UserPassword);

        UsersModel LoginDVV(string UserName, string UserPassword, string EMEI);

        bool ChangePassword(User_PwdModel User_pwd, int UsersID);

        List<UsersModel> GetByUserName(string UserName);

        bool GetRoleRightsByFormIDAndRoleID(UsersRightsModel d);

        List<UsersModel> GetByRoleID(int RoleID);

        int LoginAttempt(string userName, string userPassword);
        int SSPLoginAttempt(string userName, string userPassword);

        string CheckPasswordAge(string userName, string userPassword);

        bool TraineeEmailsVerificationStatus(string EmailVerificationToken);

        bool CheckUserPass(string userName, string userPassword);
        bool SSP_CheckUserPass(string userName, string userPassword);
    }
}