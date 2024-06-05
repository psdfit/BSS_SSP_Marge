using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class AdvancedSearchModel
    {
        public AdvancedSearchModel() : base() { }
        public string SearchString { get; set; }
        public string SearchTables { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string TablePKName { get; set; }
        public int TablePKValue { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public string JsonRow { get; set; }


    }
}
