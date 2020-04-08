using SqlSugar;

namespace DL.Domain.Models.SysModels
{
    ///<summary>
    /// 权限角色管理菜单表
    ///</summary>
    [SugarTable("Sys_Permissions")]
    public partial class SysPermissions : BaseModel
    {
        /// <summary>
        /// 管理员ID
        /// </summary>
        public string AdminId { get; set; }

        /// <summary>
        /// Desc:角色ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string RoleId { get; set; }
  
        /// <summary>
        /// Desc:菜单ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string MenuId { get; set; }

        /// <summary>
        /// 授权类型1=角色-菜单 2=用户-角色
        /// 默认=1
        /// </summary>
        public int RoleType { get; set; } 
    }
}
