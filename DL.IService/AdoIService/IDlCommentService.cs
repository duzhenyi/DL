using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.AdoIService
{

    public interface IAdoCommentService : IBaseService<AdoComment>
    {
        Task<ApiResult<PageReply<AdoCommentDto>>> GetPageList(PageParmRqst parm);

        /// <summary>
        /// 前端-查询当前评论信息
        /// </summary>
        /// <param name="searchParmDto"></param>
        /// <returns></returns>
        Task<ApiResult<List<AdoCommentDto>>> GetListAsync(SearchParmDto searchParmDto);

    }
}