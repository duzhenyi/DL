﻿using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    /// <summary>
    /// 广告位置
    /// </summary>
    [SugarTable("Ado_AdvLocation")]
    public class AdoAdvLocation 
    {
         
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }

		///位置名称
		/// </summary>
		[SugarColumn(ColumnName = "Name")]
		public string Name { get; set; }

		///是否启用 默认不启用
		/// </summary>
		[SugarColumn(ColumnName = "IsEnable")]
		public bool IsEnable { get; set; }

		///创建人
		/// </summary>
		[SugarColumn(ColumnName = "Creator",IsNullable = true)]
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