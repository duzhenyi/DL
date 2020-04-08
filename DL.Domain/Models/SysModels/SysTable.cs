using SqlSugar;
using System.Collections.Generic;

namespace DL.Domain.Models.SysModels
{
    [SugarTable("Sys_Table")]
    public class SysTable : BaseModel
    {
        public int? ParentId { get; set; }
        public string TableName { get; set; }
        public string TableRelName { get; set; }
        public string ColumnCNName { get; set; }
        public string Namespace { get; set; }
        public string FolderName { get; set; }
        public string DataTableType { get; set; }
        public string EditorType { get; set; }
        public int? OrderNo { get; set; }
        public string UploadField { get; set; }
        public int? UploadMaxCount { get; set; }
        public string RichText { get; set; }
        public string ExpressField { get; set; }
        public string DBServer { get; set; }
        public string SortName { get; set; }
        public string DetailCnName { get; set; }
        public string DetailName { get; set; }
        public int? Enable { get; set; }
        public string CnName { get; set; }
    }
}
