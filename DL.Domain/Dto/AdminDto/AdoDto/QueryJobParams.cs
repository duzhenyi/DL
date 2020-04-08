using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.AdoDto
{
    public class QueryJobParams
    {
        /// <summary>
        /// 关键字搜索
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 工作区域
        /// </summary>
        public string workArea { get; set; }

        /// <summary>
        /// 所属行业
        /// </summary>
        public string industry { get; set; }

        /// <summary>
        /// 工作类型
        /// </summary>
        public string workType { get; set; }

        /// <summary>
        /// 结算类型
        /// </summary>
        public string settlementAmount { get; set; }

        /// <summary>
        /// 酬劳
        /// </summary>
        public string workMoney { get; set; }

        /// <summary>
        /// 综合查找 0综合排序 1今日最新 2昨日发布 3本周发布 4站长推荐
        /// </summary>
        public string comprehensive { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageIndex { get; set; } = 0;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int limit { get; set; } = 20;
    }
}
