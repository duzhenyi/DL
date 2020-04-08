using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    /// <summary>
    /// 文章管理
    /// </summary>
    [SugarTable("Ado_Article")]
    public class AdoArticle 
    {
         		/// <summary>
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; } 

		/// <summary>
		///所属栏目
		/// </summary>
		[SugarColumn(ColumnName = "SysColumnId")]
		public string SysColumnId { get; set; }
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
		///描述
		/// </summary>
		[SugarColumn(ColumnName = "Descption")]
		public string Descption { get; set; }
		/// <summary>
		///内容
		/// </summary>
		[SugarColumn(ColumnName = "ArticleContent")]
		public string ArticleContent { get; set; }
		/// <summary>
		///作者
		/// </summary>
		[SugarColumn(ColumnName = "Author")]
		public string Author { get; set; }
		/// <summary>
		///来源
		/// </summary>
		[SugarColumn(ColumnName = "Source")]
		public string Source { get; set; }
		/// <summary>
		///是否有外链
		/// </summary>
		[SugarColumn(ColumnName = "IsLink")]
		public bool IsLink { get; set; }
		/// <summary>
		///外链地址
		/// </summary>
		[SugarColumn(ColumnName = "LinkUrl",IsNullable = true)]
		public string LinkUrl { get; set; }
		/// <summary>
		///所属标签
		/// </summary>
		[SugarColumn(ColumnName = "TagId")]
		public string TagId { get; set; }
		/// <summary>
		///排序
		/// </summary>
		[SugarColumn(ColumnName = "Sort")]
		public int Sort { get; set; }
		/// <summary>
		///封面
		/// </summary>
		[SugarColumn(ColumnName = "FrontCoverImgUrl")]
		public string FrontCoverImgUrl { get; set; }
		/// <summary>
		///视频地址
		/// </summary>
		[SugarColumn(ColumnName = "VideoUrl",IsNullable = true)]
		public string VideoUrl { get; set; }
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
		///访问量
		/// </summary>
		[SugarColumn(ColumnName = "Hits",IsNullable = true)]
		public int? Hits { get; set; }

		/// <summary>
		///SEO关键词
		/// </summary>
		[SugarColumn(ColumnName = "SeoKeyWord", IsNullable = true)]		public string SeoKeyWord { get; set; }

		/// <summary>
		///SEO描述
		/// </summary>
		[SugarColumn(ColumnName = "SeoDesc", IsNullable = true)]
		public string SeoDesc { get; set; }

		/// <summary>
		///是否审核
		/// </summary>
		[SugarColumn(ColumnName = "Audit")]
		public int Audit { get; set; }
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
