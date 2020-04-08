using SqlSugar;

namespace DL.Domain.Models.SysModels
{
    ///<summary>
    /// 字典值
    ///</summary>
    [SugarTable("Sys_Code")]
    public partial class SysCode : BaseModel
    {
        
        /// <summary>
        /// Desc:字典值——类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CodeTypeId { get; set; }

        /// <summary>
        /// Desc:字典值——名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// 阈值
        /// </summary>
        public string Val { get; set; }

        /// <summary>
        /// Desc:字典值——排序
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int Sort { get; set; } 
    }
}
