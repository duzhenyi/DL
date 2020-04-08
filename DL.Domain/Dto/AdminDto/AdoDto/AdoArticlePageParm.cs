using DL.Domain.PublicModels;
using System;

namespace DL.Domain.Dto.AdminDto.AdoModelsDto
{
    public class AdoArticlePageParm :PageParm
    {
         		/// <summary>
		///所属栏目
		/// </summary>		public string sysColumnId { get; set; }
		/// <summary>
		///所属标签，
		/// </summary>		public string tagId { get; set; }
		/// <summary>
		///相关属性
		/// </summary>		public int? attrcheck { get; set; }
		/// <summary>
		///是否审核
		/// </summary>		public int? audit { get; set; }
		 
		/// <summary>
		///是否启用 
		/// </summary>        public bool isEnable { get; set; }

    }
}
