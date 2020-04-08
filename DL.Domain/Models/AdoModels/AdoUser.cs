using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [SugarTable("Ado_User")]
    public class AdoUser 
    {
         		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }
		/// <summary>
		///QQ
		/// </summary>
		[SugarColumn(ColumnName = "QQ",IsNullable = true)]
		public string QQ { get; set; }
		/// <summary>
		///微信
		/// </summary>
		[SugarColumn(ColumnName = "WX",IsNullable = true)]
		public string WX { get; set; }
		/// <summary>
		///账号
		/// </summary>
		[SugarColumn(ColumnName = "LoginAccount")]
		public string LoginAccount { get; set; }
		/// <summary>
		///密码
		/// </summary>
		[SugarColumn(ColumnName = "Pwd")]
		public string Pwd { get; set; }
		/// <summary>
		///昵称
		/// </summary>
		[SugarColumn(ColumnName = "NickName")]
		public string NickName { get; set; }
		/// <summary>
		///头像
		/// </summary>
		[SugarColumn(ColumnName = "HeadPic")]
		public string HeadPic { get; set; }
		/// <summary>
		///性别
		/// </summary>
		[SugarColumn(ColumnName = "Sex")]
		public bool Sex { get; set; }
		/// <summary>
		///手机
		/// </summary>
		[SugarColumn(ColumnName = "Phone",IsNullable = true)]
		public string Phone { get; set; }
		/// <summary>
		///邮箱
		/// </summary>
		[SugarColumn(ColumnName = "Email",IsNullable = true)]
		public string Email { get; set; }
		/// <summary>
		///上次登录时间
		/// </summary>
		[SugarColumn(ColumnName = "LastLoginTime", IsNullable = true)]
		public DateTime? LastLoginTime { get; set; }
		/// <summary>
		///登录时间
		/// </summary>
		[SugarColumn(ColumnName = "LoginTime", IsNullable = true)]
		public DateTime? LoginTime { get; set; }
		/// <summary>
		/// 登录次数
		/// </summary>		[SugarColumn(ColumnName = "LoginCount", IsNullable = true)]		public long LoginCount { get; set; }

		/// <summary>
		///登录IP
		/// </summary>
		[SugarColumn(ColumnName = "LoginIp",IsNullable = true)]
		public string LoginIp { get; set; }
		/// <summary>
		///是否启用 默认不启用
		/// </summary>
		[SugarColumn(ColumnName = "IsEnable")]
		public bool IsEnable { get; set; }
		/// <summary>
		///创建人
		/// </summary>
		[SugarColumn(ColumnName = "Creator")]
		public string Creator { get; set; }
		/// <summary>
		///创建时间
		/// </summary>
		[SugarColumn(ColumnName = "CreateTime")]
		public DateTime CreateTime { get; set; }
		/// <summary>
		///备注
		/// </summary>
		[SugarColumn(ColumnName = "Remark",IsNullable = true)]
		public string Remark { get; set; }

    }
}
