using DL.Domain.PublicModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.SysDto
{
    public class SysImgPageParmDto : PageParm
    {
        /// <summary>
        /// 图片类型id
        /// </summary>
        public string typeId { get; set; }

        /// <summary>
        /// 云图片还是本地图片
        /// </summary>
        public int types { get; set; }

    }
}
