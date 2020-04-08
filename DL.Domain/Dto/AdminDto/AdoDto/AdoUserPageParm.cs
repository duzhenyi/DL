using DL.Domain.PublicModels;
using System;

namespace DL.Domain.Dto.AdminDto.AdoModelsDto
{
    public class AdoUserPageParm :PageParm
    {
         		/// <summary>
		///账号
		/// </summary>		public string loginAccount { get; set; }
		/// <summary>
		///昵称
		/// </summary>		public string nickName { get; set; }
		/// <summary>
		///是否启用 默认不启用
		/// </summary>		public bool isEnable { get; set; }

    }
}
