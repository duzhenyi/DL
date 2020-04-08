using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.AdoIService
{
    public interface IAdoRecruitenlistService: IBaseService<AdoRecruitenlist>
	{
		/// <summary>
		/// 分页查询
		/// </summary>
		/// <param name="parm"></param>
		/// <returns></returns>
		Task<ApiResult<PageReply<AdoRecruitenlist>>> GetPagesAsync(PageParmRqst parm);

        /// <summary>
        /// 获取我的报名信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult<List<MyAdoRecruitenlistDto>>> GetMyRecruitListAsync(string userGuid);
    }
}