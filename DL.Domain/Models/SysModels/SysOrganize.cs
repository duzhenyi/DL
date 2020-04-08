using SqlSugar;

namespace DL.Domain.Models.SysModels
{
    ///<summary>
    /// 组织表
    ///</summary>
    [SugarTable("Sys_Organize")]
    public partial class SysOrganize : BaseModel
    { 
         
        /// <summary>
        /// Desc:父节点
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ParentId { get; set; }

        /// <summary>
        /// 父名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// Desc:组织名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:深度
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int Layer { get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int Sort { get; set; }
    }
}
