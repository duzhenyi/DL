﻿using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    /// <summary>
    /// 高校管理
    /// </summary>
    [SugarTable("Ado_School")]
    public class AdoSchool 
    {
         
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }

		///学校名称
		/// </summary>
		[SugarColumn(ColumnName = "Name",IsNullable = true)]
		public string Name { get; set; }

		///学校地址
		/// </summary>
		[SugarColumn(ColumnName = "Address",IsNullable = true)]
		public string Address { get; set; }

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