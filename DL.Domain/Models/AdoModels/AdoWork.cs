using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    [SugarTable("Ado_Work")]
    public class AdoWork 
    {
         
		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }

		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "UserID")]
		public string UserID { get; set; }

		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "Title")]
		public string Title { get; set; }

		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "IndustryId")]
		public string IndustryId { get; set; }

		/// <summary>
		/// 性别要求 不限 男 女
		/// </summary>
		[SugarColumn(ColumnName = "SexType")]
		public int SexType { get; set; }

		/// <summary>
		///工作类型 '全职', '临时兼职', '短期兼职', '周末兼职','长期兼职'
		/// </summary>
		[SugarColumn(ColumnName = "WorkType")]
		public int WorkType { get; set; }

		/// <summary>
		///工资结算方式 '小时结算', '日结算', '周结算', '月结算', '完成量结算', '完工结算'
		/// </summary>
		[SugarColumn(ColumnName = "SettlementAmountType")]
		public int SettlementAmountType { get; set; }

		/// <summary>
		///学历要求
		/// </summary>
		[SugarColumn(ColumnName = "EducationType")]
		public int EducationType { get; set; }

		/// <summary>
		///岗位薪酬
		/// </summary>
		[SugarColumn(ColumnName = "Money")]
		public decimal Money { get; set; }

		/// <summary>
		///招聘人数
		/// </summary>
		[SugarColumn(ColumnName = "WorkCount")]
		public int WorkCount { get; set; }

		/// <summary>
		///工作押金
		/// </summary>
		[SugarColumn(ColumnName = "WorkDeposit")]
		public decimal WorkDeposit { get; set; }

		/// <summary>
		///商家名称
		/// </summary>
		[SugarColumn(ColumnName = "ShopName",IsNullable = true)]
		public string ShopName { get; set; }

		/// <summary>
		///联系人
		/// </summary>
		[SugarColumn(ColumnName = "LinkName")]
		public string LinkName { get; set; }

		/// <summary>
		///联系人微信
		/// </summary>
		[SugarColumn(ColumnName = "LinkWeiXin",IsNullable = true)]
		public string LinkWeiXin { get; set; }

		/// <summary>
		///联系电话
		/// </summary>
		[SugarColumn(ColumnName = "LinkTel")]
		public string LinkTel { get; set; }

		/// <summary>
		///工作时间
		/// </summary>
		[SugarColumn(ColumnName = "WorkTime")]
		public DateTime WorkTime { get; set; }

		/// <summary>
		///工作区域
		/// </summary>
		[SugarColumn(ColumnName = "WorkArea")]
		public byte WorkArea { get; set; }

		/// <summary>
		///工作地址
		/// </summary>
		[SugarColumn(ColumnName = "Address")]
		public byte Address { get; set; }

		/// <summary>
		///岗位职责
		/// </summary>
		[SugarColumn(ColumnName = "Responsibilities")]
		public byte Responsibilities { get; set; }

		/// <summary>
		///岗位要求
		/// </summary>
		[SugarColumn(ColumnName = "Requirements")]
		public byte Requirements { get; set; }

		/// <summary>
		///结束展示时间
		/// </summary>
		[SugarColumn(ColumnName = "EndShowTime")]
		public DateTime EndShowTime { get; set; }

		/// <summary>
		///是否置顶
		/// </summary>
		[SugarColumn(ColumnName = "IsTop")]
		public bool IsTop { get; set; }

		/// <summary>
		///是否最热
		/// </summary>
		[SugarColumn(ColumnName = "IsHot")]
		public bool IsHot { get; set; }

		/// <summary>
		///是否滚动
		/// </summary>
		[SugarColumn(ColumnName = "IsScroll")]
		public bool IsScroll { get; set; }

		/// <summary>
		///是否幻灯片
		/// </summary>
		[SugarColumn(ColumnName = "IsSlide")]
		public bool IsSlide { get; set; }

		/// <summary>
		///是否允许评论
		/// </summary>
		[SugarColumn(ColumnName = "IsComment")]
		public bool IsComment { get; set; }

		/// <summary>
		///所属标签
		/// </summary>
		[SugarColumn(ColumnName = "TagIds")]
		public string TagIds { get; set; }

		/// <summary>
		///审核状态
		/// </summary>
		[SugarColumn(ColumnName = "Audit")]
		public int Audit { get; set; }

		/// <summary>
		///审核人
		/// </summary>
		[SugarColumn(ColumnName = "AuditAdminId",IsNullable = true)]
		public string AuditAdminId { get; set; }

		/// <summary>
		///审核时间
		/// </summary>
		[SugarColumn(ColumnName = "AuditTime",IsNullable = true)]
		public DateTime? AuditTime { get; set; }

		/// <summary>
		///审核描述
		/// </summary>
		[SugarColumn(ColumnName = "AuditDesc",IsNullable = true)]
		public string AuditDesc { get; set; }

		/// <summary>
		///访问量
		/// </summary>
		[SugarColumn(ColumnName = "Hits")]
		public int Hits { get; set; }

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
