using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    [SugarTable("Ado_About")]
    public class AdoAbout 
    {
         		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }
		/// <summary>
		///标题
		/// </summary>
		[SugarColumn(ColumnName = "Title")]
		public string Title { get; set; }
		/// <summary>
		///标题颜色
		/// </summary>
		[SugarColumn(ColumnName = "TitleColor",IsNullable = true)]
		public string TitleColor { get; set; }

		/// <summary>
		///标题颜色
		/// </summary>
		[SugarColumn(ColumnName = "AboutType", IsNullable = true)]
		public int AboutType { get; set; }

		/// <summary>
		///描述
		/// </summary>
		[SugarColumn(ColumnName = "Descption")]
		public string Descption { get; set; }
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
