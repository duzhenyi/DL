using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.AdoIService
{
    public interface IAdoArticleService : IBaseService<AdoArticle>
	{
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="parm">CmsColumn</param>
        /// <returns></returns>
        PageReply<AdoArticle> GetList(PageParmRqst parm);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        Task<ApiResult<string>> UpdateAsync(AdoArticle parm);

        /// <summary>
        /// 转移到回收站
        /// </summary>
        /// <param name="parm">T</param>
        /// <param name="type">0=加入回收站  1=恢复</param>
        /// <returns></returns>
        Task<ApiResult<string>> GoRecycle(string parm,int type);

        /// <summary>
        /// 复制或转移
        /// </summary>
        /// <param name="parm">id集合</param>
        /// <param name="type">1=copy  2=转移</param>
        /// <param name="columnid">栏目id</param>
        /// <returns></returns>
        Task<ApiResult<string>> GoCopyOrTransfer(string parm, int type,string columnGuid);

        /// <summary>
        /// 查询网页案例和新闻
        /// </summary>
        /// <param name="parm">CmsColumn</param>
        /// <returns></returns>
        PageReply<AdoArticle> WebGetList(PageParmRqst parm, List<string> columnList);


        /// <summary>
        /// 前端-获取随机推荐文章
        /// </summary>
        /// <param name="articleParmDto"></param>
        /// <returns></returns>
        Task<ApiResult<List<AdoArticle>>> GetRandomListAsync(SearchParmDto articleParmDto);

    }
}