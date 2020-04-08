using System.ComponentModel.DataAnnotations;

namespace DL.Domain.EnumModels
{

    /// <summary>
    /// 关于我们类型
    /// </summary>
    public enum AboutEnum
    {
        [Display(Name = "联系站长")]
        LinkSite = 0,

        [Display(Name = "发展历史")]
        History = 1,

        [Display(Name = "官网介绍")]
        Descption = 2,

        [Display(Name = "合作伙伴")]
        Friend = 3
    }
}
