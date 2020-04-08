using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.SysDto
{
    /// <summary>
    /// 权限管理，授权菜单参数
    /// </summary>
    public class SysMenuAuthorization
    {
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<SysMenuDto> list { get; set; }

        /// <summary>
        /// 授权角色
        /// </summary>
        public string roleID { get; set; }
    }
}
