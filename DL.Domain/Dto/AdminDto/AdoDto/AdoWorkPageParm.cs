using DL.Domain.PublicModels;
using SqlSugar;
using System;

namespace DL.Domain.Dto.AdminDto.AdoModelsDto
{
    public class AdoWorkPageParm :PageParm
    {
         
		public string title { get; set; }

		public bool isEnable { get; set; }

		public string createTime { get; set; }

    }
}
