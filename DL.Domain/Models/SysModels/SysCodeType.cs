using SqlSugar;

namespace DL.Domain.Models.SysModels
{
    ///<summary>
    /// 字典类型
    ///</summary>
    [SugarTable("Sys_CodeType")]
    public partial class SysCodeType : BaseModel
    { 

        /// <summary>
        /// Desc:字典类型父级
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ParentId{ get; set; }

        /// <summary>
        /// Desc:深度
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int Layer { get; set; }

        /// <summary>
        /// Desc:字典类型名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:字典类型排序
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int Sort { get; set; }
    }
}
