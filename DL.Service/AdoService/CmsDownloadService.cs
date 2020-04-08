using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;

namespace DL.Service.AdoService
{
	public class CmsDownloadService : BaseService<AdoDownload>, ICmsDownloadService
	{
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PageReply<AdoDownload> GetList(PageParmRqst parm)
        {
            return Db.Queryable<AdoDownload>()
                .WhereIF(parm.id != 0, m => m.ColumnId == parm.id)
                .WhereIF(!string.IsNullOrEmpty(parm.key), m => m.Title.Contains(parm.key) || m.Tag.Contains(parm.key) || m.Summary.Contains(parm.key))
                .WhereIF(parm.audit == 0, m => m.Audit)
                .WhereIF(parm.audit == 1, m => !m.Audit)
                .WhereIF(!string.IsNullOrEmpty(parm.where), parm.where)
                .OrderBy(m => m.Sort, SqlSugar.OrderByType.Desc)
                .OrderBy(m => m.EditDate, SqlSugar.OrderByType.Desc)
                .ToPage(parm.page, parm.limit);
        }

    }
}