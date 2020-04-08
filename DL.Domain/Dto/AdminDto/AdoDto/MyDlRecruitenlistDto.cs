using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.AdoDto
{
    /// <summary>
    /// 我的报名信息
    /// </summary>
    public class MyAdoRecruitenlistDto
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string ID { get; set; }

        /// <summary>
        /// 招聘信息标题
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// Desc:招聘信息ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string RecruitID { get; set; }

        /// <summary>
        /// Desc:用户ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string UserID { get; set; }

        /// <summary>
        /// Desc:姓名
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:联系电话
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Tel { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime CreateTime { get; set; }
    }
}
