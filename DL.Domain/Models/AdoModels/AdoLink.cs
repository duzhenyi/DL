using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    [SugarTable("Ado_Link")]
    public class AdoLink 
    {
         		/// <summary>
		///主键 唯一编号
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }
		/// <summary>
		///链接名称
		/// </summary>
		[SugarColumn(ColumnName = "Name")]
		public string Name { get; set; }
		/// <summary>
		///链接地址
		/// </summary>
		[SugarColumn(ColumnName = "Url")]
		public string Url { get; set; }
		/// <summary>
		///排序
		/// </summary>
		[SugarColumn(ColumnName = "Sort")]
		public int Sort { get; set; }
		/// <summary>
		///图标
		/// </summary>
		[SugarColumn(ColumnName = "LinkPic", IsNullable = true)]
		public string LinkPic { get; set; }
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
