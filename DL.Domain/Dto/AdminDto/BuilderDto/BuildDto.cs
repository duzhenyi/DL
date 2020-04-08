using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.BuilderDto
{
    public class BuildDto
    {

        /// <summary>
        /// 表字段
        /// </summary>
        public List<DataFieldDto> fieldDtos { get; set; }

        /// <summary>
        /// 区域名称 
        /// </summary>
        public string areaName { get; set; }
         
        /// <summary>
        /// 表名称
        /// </summary>
        public string tableName { get; set; }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string modelName { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string controllerName { get; set; }

        /// <summary>
        /// 模块别名
        /// </summary>
        public string descName { get; set; }
    }
}
