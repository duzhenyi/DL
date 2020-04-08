using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using DL.IService;
using DL.Domain.Dto.AdminDto.AdoModelsDto;
using DL.Domain.Dto.AdminDto.AdoDto;

namespace DL.IService.AdoIService
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdoTagService : IBaseService<AdoTag>
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(AdoTag model);

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
        Task<ApiResult<string>> ModifyAsync(AdoTag model);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<AdoTag>> GetByIDAsync(string parm);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PageReply<AdoTag>>> GetPagesAsync(AdoTagPageParm pageParm);
    }
}
