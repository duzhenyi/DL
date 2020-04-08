using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.BuilderDto
{
    public class DataFieldUIDto
    {

        /// <summary>
        /// 是否纳入搜索条件
        /// </summary>
        public bool IsSearch { get; set; } = false;

        /// <summary>
        /// 是否列表展示
        /// </summary>
        public bool IsShowList { get; set; } = true;

        /// <summary>
        /// 是否纳入表单字段
        /// </summary>
        public bool IsShowForm { get; set; } = true;
    }
}
