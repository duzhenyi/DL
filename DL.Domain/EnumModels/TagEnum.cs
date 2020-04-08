using System.ComponentModel.DataAnnotations;

namespace DL.Domain.EnumModels
{
    /// <summary>
    /// 标签类型
    /// </summary>
    public enum TagEnum
    {
        [Display(Name = "高校标签")]
        School = 0,
        [Display(Name = "IT标签")]
        IT = 1,
        [Display(Name = "工具标签")]
        Tool = 2,
        [Display(Name = "其他标签")]
        Other = 3,

    }
}
