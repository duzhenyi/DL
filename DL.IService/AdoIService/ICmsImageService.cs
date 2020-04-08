using DL.Domain.Models.SysModels;
using DL.IService.SysIService;

namespace DL.IService.AdoIService
{
    /// <summary>
    /// 图片管理业务接口
    /// </summary>
    public interface ICmsImageService : IBaseService<SysImage>
    {
        //CloudFile GetList(PageParmRqst parm);
    }
}
