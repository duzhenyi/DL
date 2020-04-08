﻿using SqlSugar;
using System;

namespace DL.Domain.Models.AdoModels
{
    /// <summary>
    /// 广告模块
    /// </summary>
    [SugarTable("Ado_Adv")]
    public class AdoAdv 
    {
         
		///
		/// </summary>
		[SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
		public string ID { get; set; }

		///广告标题
		/// </summary>
		[SugarColumn(ColumnName = "Title")]
		public string Title { get; set; }

		///广告图片
		/// </summary>
		[SugarColumn(ColumnName = "ImgUrl",IsNullable = true)]
		public string ImgUrl { get; set; }

		///说明
		/// </summary>
		[SugarColumn(ColumnName = "Descption")]
		public string Descption { get; set; }

		///联系电话
		/// </summary>
		[SugarColumn(ColumnName = "Phoone",IsNullable = true)]
		public string Phoone { get; set; }

		///联系微信
		/// </summary>
		[SugarColumn(ColumnName = "WeiXin",IsNullable = true)]
		public string WeiXin { get; set; }

		///联系人
		/// </summary>
		[SugarColumn(ColumnName = "LinkName")]
		public string LinkName { get; set; }

		///广告位置
		/// </summary>
		[SugarColumn(ColumnName = "AdvLocationId")]
		public string AdvLocationId { get; set; }

		///过期时间
		/// </summary>
		[SugarColumn(ColumnName = "ExpireTime")]
		public DateTime ExpireTime { get; set; }

		///总共价格
		/// </summary>
		[SugarColumn(ColumnName = "Price")]
		public decimal Price { get; set; }

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