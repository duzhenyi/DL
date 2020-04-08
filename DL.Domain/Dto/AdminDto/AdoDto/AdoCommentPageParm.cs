using DL.Domain.PublicModels;
using System;

namespace DL.Domain.Dto.AdminDto.AdoModelsDto
{
    public class AdoCommentPageParm :PageParm
    {
         		/// <summary>
		///内容
		/// </summary>		public string descption { get; set; }
		/// <summary>
		///是否审核通过 0正在审核 1 审核通过 2 审核失败
		/// </summary>		public int? audit { get; set; }
		/// <summary>
		///是否启用 默认不启用
		/// </summary>		public bool isEnable { get; set; }

    }
}
