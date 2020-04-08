using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.SysDto
{
    public class ListFileInfoDto
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
