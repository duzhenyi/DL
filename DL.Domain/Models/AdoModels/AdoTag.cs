﻿using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    [SugarTable("Ado_Tag")]
    public class AdoTag 
    {
         
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }

		///标签名称
		/// </summary>
		[SugarColumn(ColumnName = "TagName")]
		public string TagName { get; set; }

		///所属类型 枚举
		/// </summary>
		[SugarColumn(ColumnName = "TagType")]
		public int TagType { get; set; }

		///是否最火
		/// </summary>
		[SugarColumn(ColumnName = "IsHot")]
		public bool IsHot { get; set; }

		///标签介绍
		/// </summary>
		[SugarColumn(ColumnName = "TooltipDesc",IsNullable = true)]
		public string TooltipDesc { get; set; }

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