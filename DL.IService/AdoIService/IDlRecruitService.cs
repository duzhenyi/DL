using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using System.Threading.Tasks;

namespace DL.IService.AdoIService
{
    public interface IAdoRecruitService: IBaseService<AdoRecruit>
	{
		/// <summary>
		/// 获取数据
		/// </summary>
		/// <param name="parm"></param>
		/// <returns></returns> 
		Task<ApiResult<PageReply<AdoRecruit>>> GetPagesAsync(PageParmRqst parm);


		/// <summary>
		/// 审核数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ApiResult<string>> VerifyAsync(string ids,string desc ,string sysAdminId);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="queryJobParams"></param>
        /// <returns></returns>
        Task<ApiResult<PageReply<AdoRecruit>>> GetPageAsync(QueryJobParams queryJobParams);
    }
}