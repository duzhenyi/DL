using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    /// <summary>
    /// 站内留言
    /// </summary>
    [SugarTable("Ado_SiteMessage")]
    public class AdoSiteMessage 
    {
         		/// <summary>
		///主键
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }
		/// <summary>
		///名称
		/// </summary>
		[SugarColumn(ColumnName = "Name")]
		public string Name { get; set; }
		/// <summary>
		///微信
		/// </summary>
		[SugarColumn(ColumnName = "QQWeiXin")]
		public string QQWeiXin { get; set; }
		/// <summary>
		///邮箱
		/// </summary>
		[SugarColumn(ColumnName = "Mail")]
		public string Mail { get; set; }
		/// <summary>
		///手机
		/// </summary>
		[SugarColumn(ColumnName = "Phoone")]
		public string Phoone { get; set; }
		/// <summary>
		///留言
		/// </summary>
		[SugarColumn(ColumnName = "Msg")]
		public string Msg { get; set; }
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
