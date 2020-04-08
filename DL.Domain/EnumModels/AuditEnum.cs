using System.ComponentModel.DataAnnotations;

namespace DL.Domain.EnumModels
{

    /// <summary>
    /// 审核类型
    /// </summary>
    public enum AuditEnum
    {
        [Display(Name = "正在审核")]
        Loading = 0,

        [Display(Name = "审核通过")]
        Pass = 1,

        [Display(Name = "审核失败")]
        Fail = 2
    }
}

