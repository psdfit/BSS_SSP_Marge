using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class CenterInspectionReportModel
    {
        public List<CenterInspectionModel> CenterInspectionModelList { get; set; }
        public List<CenterInspectionModel> CenterInspectionDataModelList { get; set; }
        public List<CenterInspectionModel> CenterInspectionDataModelList1 { get; set; }
        public List<CenterInspectionModel> CenterInspectionDataModelList2 { get; set; }
        public List<CenterInspectionModel> CenterInspectionDataModelList3 { get; set; }
        public List<CenterInspectionComplianceModel> CenterInspectionAdditionalComplianceaModelList { get; set; }
        public List<CenterInspectionTradeDetailModel> CenterInspectionTradeDetailModelList { get; set; }
        public List<CenterInspectionClassDetailModel> CenterInspectionClassDetailModelList { get; set; }
        public List<CenterInspectionNecessaryFcilitiesModel> CenterInspectionNecessaryFacilitiesModelList { get; set; }
        public List<CenterInspectionTradeToolModel> CenterInspectionTradeToolsModelList { get; set; }

    }
}
