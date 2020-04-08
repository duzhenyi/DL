using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using DL.IService;
using DL.Domain.Dto.AdminDto.AdoModelsDto;

namespace DL.IService.AdoIService
{
    /// <summary>
    /// 资源模块
    /// </summary>
    public interface IAdoDownloadService : IBaseService<AdoDownload>
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(AdoDownload model);

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
        Task<ApiResult<string>> ModifyAsync(AdoDownload model);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<AdoDownload>> GetByIDAsync(string parm);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PageReply<AdoDownload>>> GetPagesAsync(AdoDownloadPageParm pageParm);
    }
}
