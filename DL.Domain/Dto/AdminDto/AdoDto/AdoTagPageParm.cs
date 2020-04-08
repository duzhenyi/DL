using DL.Domain.PublicModels;
using System;

namespace DL.Domain.Dto.AdminDto.AdoModelsDto
{
    public class AdoTagPageParm :PageParm
    {
         		/// <summary>
		///所属类型 枚举
		/// </summary>		public int tagType { get; set; }
		/// <summary>
		///是否最火
		/// </summary>		public bool isHot { get; set; }
		/// <summary>
		///是否启用 默认不启用
		/// </summary>		public bool isEnable { get; set; }
		/// <summary>
		///创建时间
		/// </summary>
		public string createTime { get; set; }

    }
}
