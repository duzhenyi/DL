using SqlSugar;

namespace DL.Domain.Models.SysModels
{
    ///<summary>
    /// 权限角色表
    ///</summary>
    [SugarTable("Sys_Role")]
    public partial class SysRole : BaseModel
    {
        /// <summary>
        /// Desc:角色名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:是否为超级管理员
        /// Default:b'0'
        /// Nullable:False
        /// </summary>           
        public bool IsSystem { get; set; }

        /// <summary>
        /// Desc: 排序
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Sort { get; set; }
    }
}
