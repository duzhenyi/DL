using System;
using System.Collections.Generic;

namespace DL.Domain.Dto.AdminDto.SysDto
{
    public class SysMenuTreeTableDto
    {

        /// <summary>
        /// 是否启用 默认启用
        /// </summary>        
        public bool isEnable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string creator { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// Desc:菜单名称标识
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string enCode { get; set; }

        /// <summary>
        /// Desc:父级ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string parentId { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string parentName { get; set; }

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
        /// 按钮颜色
        /// </summary>
        public string iconColor { get; set; }

        /// <summary>
        /// Desc:菜单排序
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int sort { get; set; }

        /// <summary>
        /// 是否保存到桌面
        /// </summary>
        public bool isDeskTop { get; set; }

        /// <summary>
        /// 所属类型 0 模块，1 菜单，2 按钮
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 打开方式 1. 弹窗， 2. 新开窗体
        /// </summary>
        public int? openType { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; } 

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool open { get; set; } = true;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createTime { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<SysMenuTreeTableDto> children { get; set; }
    }
}
