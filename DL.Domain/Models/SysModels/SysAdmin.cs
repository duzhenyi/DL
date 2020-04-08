using SqlSugar;
using System;

namespace DL.Domain.Models.SysModels
{
    [SugarTable("Sys_Admin")]
    public class SysAdmin : BaseModel
    {
        /// <summary>
        /// 归属角色
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public string OrganizeId { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string RelName { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPic { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 当前登录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount { get; set; }
    }
}
