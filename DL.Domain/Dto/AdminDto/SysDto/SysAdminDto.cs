using DL.Domain.Models.SysModels;
using System;
using System.Collections.Generic;

namespace DL.Domain.Dto.AdminDto.SysDto
{

    public class SysAdminDto : SysAdmin
    {
        public string OrganizeName { get; set; }
        public string RoleName { get; set; }
    }

    /// <summary>
    /// 管理员登录成功返回的信息
    /// </summary>
    public class SysAdminMenuDto
    {
        /// <summary>
        /// 登录后的用户信息
        /// </summary>
        public SysAdmin admin { get; set; }

        /// <summary>
        /// 当前登录用户的菜单权限
        /// </summary>
        public List<SysMenuDto> menu { get; set; }
    }
    /// <summary>
    /// 后台管理登录
    /// </summary>
    public class SysAdminLogin
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string loginname { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string password { get; set; }
    }

    /// <summary>
    /// 用户登录次数和过期时间配置
    /// </summary>
    public class SysAdminLoginConfig
    {
        /// <summary>
        /// 登录次数
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// 过期时间-分钟
        /// </summary>
        public DateTime? DelayMinute { get; set; }
    }
}
