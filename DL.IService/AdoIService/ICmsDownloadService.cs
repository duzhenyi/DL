using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;

namespace DL.IService.AdoIService
{

    public interface ICmsDownloadService: IBaseService<AdoDownload>
	{
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="parm">CmsColumn</param>
        /// <returns></returns>
        PageReply<AdoDownload> GetList(PageParmRqst parm);
    }
}