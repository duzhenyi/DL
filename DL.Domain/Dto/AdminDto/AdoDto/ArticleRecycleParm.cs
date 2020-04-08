namespace DL.Domain.Dto.AdminDto.AdoDto
{
    /// <summary>
    /// 转移到回收站参数/移动或者复制
    /// </summary>
    public class ArticleOptionParm
    {
        /// <summary>
        /// 文章ID集合
        /// </summary>
        public string parm { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int type { get; set; } = 0;

        /// <summary>
        /// 移动或复制的栏目
        /// </summary>
        public string columnID { get; set; } 
    }
}
