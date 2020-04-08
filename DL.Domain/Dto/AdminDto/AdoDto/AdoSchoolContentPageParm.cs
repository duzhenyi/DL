using DL.Domain.PublicModels;
using System;

namespace DL.Domain.Dto.AdminDto.AdoModelsDto
{
    public class AdoSchoolContentPageParm :PageParm
    {
         
		/// <summary>
		///内容类型
		/// </summary>
		public string sysColumnId { get; set; }

		/// <summary>
		///所属学院
		/// </summary>
		public string schoolId { get; set; }

		/// <summary>
		///所属标签
		/// </summary>
		public string tagId { get; set; }

		/// <summary>
		///标题
		/// </summary>
		public string title { get; set; }

		/// <summary>
		///是否审核
		/// </summary>
		public int audit { get; set; }

		/// <summary>
		///是否启用 默认不启用
		/// </summary>
		public bool isEnable { get; set; }

    }
}
