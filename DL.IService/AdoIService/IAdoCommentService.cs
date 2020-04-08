using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using DL.IService;
using DL.Domain.Dto.AdminDto.AdoModelsDto;

namespace DL.IService.AdoIService
{
    /// <summary>
    /// 用户评论
    /// </summary>
    public interface IAdoCommentService : IBaseService<AdoComment>
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(AdoComment model);

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
        Task<ApiResult<string>> ModifyAsync(AdoComment model);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<AdoComment>> GetByIDAsync(string parm);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PageReply<AdoComment>>> GetPagesAsync(AdoCommentPageParm pageParm);
    }
}
