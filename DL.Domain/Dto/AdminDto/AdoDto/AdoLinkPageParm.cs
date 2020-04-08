using DL.Domain.PublicModels;
using System;

namespace DL.Domain.Dto.AdminDto.AdoModelsDto
{
    public class AdoLinkPageParm :PageParm
    {
         		/// <summary>
		///链接名称
		/// </summary>		public string name { get; set; }
		/// <summary>
		///链接地址
		/// </summary>		public string url { get; set; }
		/// <summary>
		///是否启用 默认不启用
		/// </summary>		public bool isEnable { get; set; }

    }
}
