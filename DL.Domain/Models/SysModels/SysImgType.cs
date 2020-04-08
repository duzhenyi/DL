using SqlSugar;

namespace DL.Domain.Models.SysModels
{
    /// <summary>
    /// 图片列表分类
    /// </summary>
    [SugarTable("Sys_ImgType")]
    public class SysImgType:BaseModel
    {
        /// <summary>
        /// Desc:所属父级
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string ParentId { get; set; }

        /// <summary>
        /// 上级名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// Desc:图片类型分类 0=本地 1=云端
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Types { get; set; }

        /// <summary>
        /// Desc:深度 
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Layer { get; set; }

        /// <summary>
        /// Desc:中文名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:英文名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string EnName { get; set; }
    }
}
