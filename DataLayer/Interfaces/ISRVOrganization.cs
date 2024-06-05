using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVOrganization
    {
        OrganizationModel GetByOID(int OID);

        List<OrganizationModel> SaveOrganization(OrganizationModel Organization);

        List<OrganizationModel> FetchOrganization(OrganizationModel mod);

        List<OrganizationModel> FetchOrganization();

        List<OrganizationModel> FetchOrganization(bool InActive);


        List<OrganizationModel> FetchOrganizationByUser(int userID,int OID);

        void ActiveInActive(int OID, bool? InActive, int CurUserID);
    }
}