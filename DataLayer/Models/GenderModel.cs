/* **** Aamer Rehman Malik *****/

using System;

namespace DataLayer.Models
{
    [Serializable]
    public class GenderModel : ModelBase
    {
        public GenderModel() : base()
        {
        }

        public GenderModel(bool InActive) : base(InActive)
        {
        }

        public int GenderID { get; set; }
        public string GenderName { get; set; }
    }

    public class PagingModel
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public int TotalCount { get; set; } = 0;
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string SearchColumn { get; set; }
        public string SearchValue { get; set; }
    }
}