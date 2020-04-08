using SqlSugar;
 
namespace DL.Domain.Models.SysModels
{
    ///<summary>
    /// 系统菜单表
    ///</summary>
    [SugarTable("Sys_Menu")]
    public partial class SysMenu : BaseModel
    {
        /// <summary>
        /// Desc:菜单名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:菜单名称标识
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string EnCode { get; set; }

        /// <summary>
        /// Desc:父级ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ParentId { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// Desc:菜单Url
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Url { get; set; }

        /// <summary>
        /// Desc:菜单图标Class
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Icon { get; set; }

        /// <summary>
        /// 按钮颜色
        /// </summary>
        public string IconColor { get; set; }

        /// <summary>
        /// Desc:菜单排序
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int Sort { get; set; }

        /// <summary>
        /// 是否保存到桌面
        /// </summary>
        public bool IsDeskTop { get; set; }

        /// <summary>
        /// 所属类型 1 模块，2 菜单，3 按钮
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 打开方式 1. 弹窗， 2. 新开窗体
        /// </summary>
        public int? OpenType { get; set; }
    }
}
