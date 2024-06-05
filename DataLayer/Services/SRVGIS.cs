using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataLayer.Services
{
   public class SRVGIS: ISRVGIS
    {

        public List<GISModel> GetClassWithTraineeCount(GISModel model)
        {
            try
            {
                List<GISModel> ClassModel = new List<GISModel>();
                SqlParameter[] param2 = new SqlParameter[6];
                param2[0] = new SqlParameter("@SectorID", model.SectorID);
                param2[1] = new SqlParameter("@DistrictID", model.DistrictID);
                param2[2] = new SqlParameter("@TehsilID", model.TehsilID);
                param2[3] = new SqlParameter("@ClusterID", model.ClusterID);
                param2[4] = new SqlParameter("@ProgramTypeID", model.PTypeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TraineeCountAndClassInfoForGIS]", param2).Tables[0];
                ClassModel = Helper.ConvertDataTableToModel<GISModel>(dt);
                return ClassModel;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

    }
}
