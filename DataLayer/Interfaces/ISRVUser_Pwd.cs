using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVUser_Pwd
    {
        User_PwdModel GetBypwdID(int pwdID);
        List<User_PwdModel> SaveUser_Pwd(User_PwdModel User_Pwd);
        List<User_PwdModel> FetchUser_Pwd(User_PwdModel mod);
        List<User_PwdModel> FetchUser_Pwd();
        List<User_PwdModel> FetchUser_Pwd(bool InActive);
        void ActiveInActive(int pwdID, bool? InActive, int CurUserID);
        string GeneratePassword(int size, bool lowerCase);
        string GeneratePass(int size);
    }
}