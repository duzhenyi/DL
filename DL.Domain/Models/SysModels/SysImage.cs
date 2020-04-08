using SqlSugar;

namespace DL.Domain.Models.SysModels
{
    ///<summary>
    /// 图片表
    ///</summary>
    [SugarTable("Sys_Image")]
    public partial class SysImage:BaseModel
    {
        /// <summary>
        /// Desc:图片分类ID，一个栏目可有多个图片类型
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public string SysImgTypeId { get; set; }

        /// <summary>
        /// 云图片还是本地图片
        /// </summary>
        public int Types { get; set; }

        /// <summary>
        /// Desc:图片原图
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string ImgBig { get; set; }

        /// <summary>
        /// Desc:图片缩略图
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ImgSmall { get; set; }
         
        /// <summary>
        /// Desc:文件大小
        /// Default:
        /// Nullable:True
        /// </summary>           
        public long ImgSize { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ImgType { get; set; }

        /// <summary>
        /// Desc:是否为封面
        /// Default:b'0'
        /// Nullable:False
        /// </summary>           
        public bool IsCover { get; set; }
    }
}
