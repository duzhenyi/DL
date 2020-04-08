﻿using SqlSugar;
using System;

namespace DL.Domain.Models.SysModels
{
    /// <summary>
    /// 栏目管理
    /// </summary>
    [SugarTable("Sys_Column")]
    public class SysColumn 
    {
         
		///主键 唯一编号
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }

		///栏目标题
		/// </summary>
		[SugarColumn(ColumnName = "Title")]
		public string Title { get; set; }

		///英文栏位名称
		/// </summary>
		[SugarColumn(ColumnName = "EnTitle",IsNullable = true)]
		public string EnTitle { get; set; }

		///栏位副标题
		/// </summary>
		[SugarColumn(ColumnName = "SubTitle",IsNullable = true)]
		public string SubTitle { get; set; }

		///父栏目
		/// </summary>
		[SugarColumn(ColumnName = "ParentID", IsNullable = true)]
		public string ParentID { get; set; }

		///
		/// </summary>
		[SugarColumn(ColumnName = "ParentTitle",IsNullable = true)]
		public string ParentTitle { get; set; }

		///栏位等级
		/// </summary>
		[SugarColumn(ColumnName = "Layer")]
		public int Layer { get; set; }

		///排序
		/// </summary>
		[SugarColumn(ColumnName = "Sort")]
		public int Sort { get; set; }

		///外部链接地址
		/// </summary>
		[SugarColumn(ColumnName = "LinkUrl",IsNullable = true)]
		public string LinkUrl { get; set; }

		///关键词
		/// </summary>
		[SugarColumn(ColumnName = "KeyWord",IsNullable = true)]
		public string KeyWord { get; set; }

		///描述
		/// </summary>
		[SugarColumn(ColumnName = "Description",IsNullable = true)]
		public string Description { get; set; }

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