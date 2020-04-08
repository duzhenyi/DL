using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.SysDto
{

    /// <summary>
    /// 管理员登录，获得菜单权限列表
    /// </summary>
    public class SysMenuDto
    {
        /// <summary>
        /// Desc:唯一标识ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string ID { get; set; }

        /// <summary>
        /// Desc:菜单父级ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string parentID { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string parentName { get; set; }

        /// <summary>
        /// Desc:菜单名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string name { get; set; }

        /// <summary>
        /// Desc:菜单名称标识
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string nameCode { get; set; }


        /// <summary>
        /// Desc:菜单级别
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int type { get; set; }

        /// <summary>
        /// Desc:菜单Url
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string url { get; set; }

        /// <summary>
        /// Desc:菜单图标Class
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string icon { get; set; }


        /// <summary>
        /// 菜单图标颜色
        /// </summary>
        public string iconColor { get; set; }

        /// <summary>
        /// 是否置顶桌面
        /// </summary>
        public bool isDeskTop { get; set; } = false;

        /// <summary>
        /// Desc:菜单排序
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int sort { get; set; }

        /// <summary>
        /// Desc:权限操作是否选中
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool isChecked { get; set; } = false;
    }
}
