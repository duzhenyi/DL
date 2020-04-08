using System;
using SqlSugar;

namespace DL.Domain.Models
{
    public class BaseModel
    {
        /// <summary>
        /// 主键 唯一编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 是否启用 默认启用
        /// </summary>        
        public bool IsEnable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
