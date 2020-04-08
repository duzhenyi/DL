using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    /// <summary>
    /// 高校内容
    /// </summary>
    [SugarTable("Ado_SchoolContent")]
    public class AdoSchoolContent 
    {
         
		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }

		/// <summary>
		///内容类型
		/// </summary>
		[SugarColumn(ColumnName = "SysColumnId")]
		public string SysColumnId { get; set; }

		/// <summary>
		///所属学院
		/// </summary>
		[SugarColumn(ColumnName = "SchoolId")]
		public string SchoolId { get; set; }

		/// <summary>
		///所属标签
		/// </summary>
		[SugarColumn(ColumnName = "TagId")]
		public string TagId { get; set; }

		/// <summary>
		///标题
		/// </summary>
		[SugarColumn(ColumnName = "Title")]
		public string Title { get; set; }

		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "TitleColor")]
		public string TitleColor { get; set; }

		/// <summary>
		///内容
		/// </summary>
		[SugarColumn(ColumnName = "Descption")]
		public string Descption { get; set; }

		/// <summary>
		///封面
		/// </summary>
		[SugarColumn(ColumnName = "FrontCoverImgUrl")]
		public string FrontCoverImgUrl { get; set; }

		/// <summary>
		///排序
		/// </summary>
		[SugarColumn(ColumnName = "Sort")]
		public int Sort { get; set; }

		/// <summary>
		///访问量
		/// </summary>
		[SugarColumn(ColumnName = "Hits",IsNullable = true)]
		public int? Hits { get; set; }

		/// <summary>
		///是否审核
		/// </summary>
		[SugarColumn(ColumnName = "Audit")]
		public int Audit { get; set; }

		/// <summary>
		///是否最火
		/// </summary>
		[SugarColumn(ColumnName = "IsHot")]
		public bool IsHot { get; set; }

		/// <summary>
		///是否推荐
		/// </summary>
		[SugarColumn(ColumnName = "IsTop")]
		public bool IsTop { get; set; }

		/// <summary>
		///是否运行评论
		/// </summary>
		[SugarColumn(ColumnName = "IsComment")]
		public bool IsComment { get; set; }

		/// <summary>
		///是否启用 默认不启用
		/// </summary>
		[SugarColumn(ColumnName = "IsEnable")]
		public bool IsEnable { get; set; }

		/// <summary>
		///创建人
		/// </summary>
		[SugarColumn(ColumnName = "Creator",IsNullable = true)]
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
