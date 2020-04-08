using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.SysDto
{
    public class ToDeskTopDto
    { 
            /// <summary>
            /// 菜单ID
            /// </summary>
            public string ID { get; set; }

            /// <summary>
            /// 是否置顶桌面
            /// </summary>
            public bool isDeskTop { get; set; }
        }
    }
