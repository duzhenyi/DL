using DL.Domain.PublicModels;
using System;

namespace DL.Domain.Dto.AdminDto.AdoModelsDto
{
    public class AdoAdvPageParm :PageParm
    {
         		/// <summary>
		///广告标题
		/// </summary>		public string title { get; set; }
		/// <summary>
		///广告位置
		/// </summary>		public string advLocationId { get; set; }
		/// <summary>
		///是否启用 默认不启用
		/// </summary>		public bool isEnable { get; set; }

    }
}
