using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    /// <summary>
    /// 用户评论
    /// </summary>
    [SugarTable("Ado_Comment")]
    public class AdoComment 
    {
         		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }
		/// <summary>
		///评论栏目ID
		/// </summary>
		[SugarColumn(ColumnName = "ColumnId")]
		public string ColumnId { get; set; }
		/// <summary>
		///评论人ID
		/// </summary>
		[SugarColumn(ColumnName = "UserID")]
		public string UserID { get; set; }
		/// <summary>
		///内容
		/// </summary>
		[SugarColumn(ColumnName = "Descption")]
		public string Descption { get; set; }
		/// <summary>
		///回复人员ID
		/// </summary>
		[SugarColumn(ColumnName = "RepUserId",IsNullable = true)]
		public string RepUserId { get; set; }
		/// <summary>
		///回复时间
		/// </summary>
		[SugarColumn(ColumnName = "RepTime",IsNullable = true)]
		public DateTime? RepTime { get; set; }
		/// <summary>
		///审核人员
		/// </summary>
		[SugarColumn(ColumnName = "AuditAdminId",IsNullable = true)]
		public string AuditAdminId { get; set; }
		/// <summary>
		///审核时间
		/// </summary>
		[SugarColumn(ColumnName = "AuditTime",IsNullable = true)]
		public DateTime? AuditTime { get; set; }
		/// <summary>
		///是否审核通过 0正在审核 1 审核通过 2 审核失败
		/// </summary>
		[SugarColumn(ColumnName = "Audit",IsNullable = true)]
		public int? Audit { get; set; }
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
