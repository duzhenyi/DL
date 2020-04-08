using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using DL.IService;
using DL.Domain.Dto.AdminDto.SysModelsDto;
using DL.Domain.Dto.AdminDto.SysDto;

namespace DL.IService.SysIService
{
    /// <summary>
    /// 栏目管理
    /// </summary>
    public interface ISysColumnService : IBaseService<SysColumn>
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(SysColumn model);

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<ApiResult<string>> DelAsync(string ids);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyAsync(SysColumn model);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<SysColumn>> GetByIDAsync(string parm);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PageReply<SysColumn>>> GetPagesAsync(SysColumnPageParm pageParm);

        /// <summary>
        /// 获得树列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<TreeDto>>> GetListTreeAsync();
    }
}
