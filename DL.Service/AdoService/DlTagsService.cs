
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;

namespace DL.Service.AdoService
{ 
	public class AdoTagsService : BaseService<AdoTags>, IAdoTagsService
	{
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PageReply<AdoTags> GetList(PageParmRqst parm)
        {
            return Db.Queryable<AdoTags>() 
                .WhereIF(!string.IsNullOrEmpty(parm.key), m => m.Name.Contains(parm.key))
                .WhereIF(!string.IsNullOrEmpty(parm.where), parm.where)
                .OrderBy(m => m.TagsHits, SqlSugar.OrderByType.Desc)
                .ToPage(parm.page, parm.limit);
        }
    }
}