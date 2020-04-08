using SqlSugar;
using System;

namespace DL.Domain.Models.SysModels
{
    /// <summary>
    /// 栏目管理
    /// </summary>
    [SugarTable("Sys_Column")]
    public class SysColumn 
    {
         		/// <summary>
		///主键 唯一编号
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }
		/// <summary>
		///栏目标题
		/// </summary>
		[SugarColumn(ColumnName = "Title")]
		public string Title { get; set; }
		/// <summary>
		///英文栏位名称
		/// </summary>
		[SugarColumn(ColumnName = "EnTitle",IsNullable = true)]
		public string EnTitle { get; set; }
		/// <summary>
		///栏位副标题
		/// </summary>
		[SugarColumn(ColumnName = "SubTitle",IsNullable = true)]
		public string SubTitle { get; set; }
		/// <summary>
		///父栏目
		/// </summary>
		[SugarColumn(ColumnName = "ParentID", IsNullable = true)]
		public string ParentID { get; set; }
		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "ParentTitle",IsNullable = true)]
		public string ParentTitle { get; set; }
		/// <summary>
		///栏位等级
		/// </summary>
		[SugarColumn(ColumnName = "Layer")]
		public int Layer { get; set; }
		/// <summary>
		///排序
		/// </summary>
		[SugarColumn(ColumnName = "Sort")]
		public int Sort { get; set; }
		/// <summary>
		///外部链接地址
		/// </summary>
		[SugarColumn(ColumnName = "LinkUrl",IsNullable = true)]
		public string LinkUrl { get; set; }
		/// <summary>
		///关键词
		/// </summary>
		[SugarColumn(ColumnName = "KeyWord",IsNullable = true)]
		public string KeyWord { get; set; }
		/// <summary>
		///描述
		/// </summary>
		[SugarColumn(ColumnName = "Description",IsNullable = true)]
		public string Description { get; set; }
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
