using DL.Domain.PublicModels;
using System;

namespace DL.Domain.Dto.AdminDto.AdoModelsDto
{
    public class AdoAboutPageParm :PageParm
    {
         		/// <summary>
		///标题
		/// </summary>		public string title { get; set; }
		/// <summary>
		///是否启用 默认不启用
		/// </summary>		public bool isEnable { get; set; }
		/// <summary>
		///创建时间
		/// </summary>
		public string createTime { get; set; }

    }
}
