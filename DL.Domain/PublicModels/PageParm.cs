namespace DL.Domain.PublicModels
{
    public class PageParm
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        /// 每页总条数
        /// </summary>
        public int limit { get; set; } = 15;

        /// <summary>
        /// 主键-分级使用
        /// </summary>
        public string pid { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 搜索日期，可能是2个日期，通过-分隔
        /// </summary>
        public string time { get; set; } 
    }
}
