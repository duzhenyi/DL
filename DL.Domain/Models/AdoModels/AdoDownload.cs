﻿using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    /// <summary>
    /// 资源模块
    /// </summary>
    [SugarTable("Ado_Download")]
    public class AdoDownload 
    {
         
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }

		///所属栏目
		/// </summary>
		[SugarColumn(ColumnName = "SysColumnId")]
		public string SysColumnId { get; set; }

		///标题
		/// </summary>
		[SugarColumn(ColumnName = "Title")]
		public string Title { get; set; }

		///文件地址
		/// </summary>
		[SugarColumn(ColumnName = "FileUrl",IsNullable = true)]
		public string FileUrl { get; set; }

		///描述
		/// </summary>
		[SugarColumn(ColumnName = "Descption")]
		public string Descption { get; set; }

		///所属标签
		/// </summary>
		[SugarColumn(ColumnName = "TagId")]
		public string TagId { get; set; }

		///封面
		/// </summary>
		[SugarColumn(ColumnName = "FrontCoverImgUrl")]
		public string FrontCoverImgUrl { get; set; }

		///内容
		/// </summary>
		[SugarColumn(ColumnName = "DownloadContent")]
		public string DownloadContent { get; set; }

		///下载量
		/// </summary>
		[SugarColumn(ColumnName = "DownTotalCount")]
		public int DownTotalCount { get; set; }

		///访问量
		/// </summary>
		[SugarColumn(ColumnName = "Hits")]
		public int Hits { get; set; }

		///
		/// </summary>
		[SugarColumn(ColumnName = "Sort")]
		public int Sort { get; set; }

		///是否有外链
		/// </summary>
		[SugarColumn(ColumnName = "IsLink")]
		public bool IsLink { get; set; }

		///外链地址
		/// </summary>
		[SugarColumn(ColumnName = "LinkUrl",IsNullable = true)]
		public string LinkUrl { get; set; }

		///是否加密
		/// </summary>
		[SugarColumn(ColumnName = "IsEncrypt")]
		public bool IsEncrypt { get; set; }

		///加密密码
		/// </summary>
		[SugarColumn(ColumnName = "Pwd",IsNullable = true)]
		public string Pwd { get; set; }

		///是否置顶
		/// </summary>
		[SugarColumn(ColumnName = "IsTop")]
		public bool IsTop { get; set; }

		///是否最热
		/// </summary>
		[SugarColumn(ColumnName = "IsHot")]
		public bool IsHot { get; set; }

		///是否允许评论
		/// </summary>
		[SugarColumn(ColumnName = "IsComment")]
		public bool IsComment { get; set; }

		///是否审核
		/// </summary>
		[SugarColumn(ColumnName = "Audit")]
		public int Audit { get; set; }

		///是否启用 默认不启用
		/// </summary>
		[SugarColumn(ColumnName = "IsEnable")]
		public bool IsEnable { get; set; }

		///创建人
		/// </summary>
		[SugarColumn(ColumnName = "Creator")]
		public string Creator { get; set; }

		///创建时间
		/// </summary>
		[SugarColumn(ColumnName = "CreateTime")]
		public DateTime CreateTime { get; set; }

		///备注
		/// </summary>
		[SugarColumn(ColumnName = "Remark",IsNullable = true)]
		public string Remark { get; set; }

    }
}