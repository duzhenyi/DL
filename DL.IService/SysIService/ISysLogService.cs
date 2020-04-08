using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using System.Threading.Tasks;

namespace DL.IService.SysIService
{
    /// <summary>
    /// 系统日志业务接口
    /// </summary>
    public interface ISysLogService : IBaseService<SysLog>
    {
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PageReply<SysLog>>> GetPagesAsync(PageParm parm);
        
    }
}
