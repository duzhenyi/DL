using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;
using DL.Utils.Extensions;
using DL.Utils.Helper;
using DL.Utils.Log.Log4net;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.Service.AdoService
{
    public class AdoArticleService : BaseService<AdoArticle>, IAdoArticleService
    {
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PageReply<AdoArticle> GetList(PageParmRqst parm)
        {
            return Db.Queryable<AdoArticle>()
                .WhereIF(parm.id != 0, m => m.ColumnGuid == parm.guid)
                .WhereIF(!string.IsNullOrEmpty(parm.key), m => m.Title.Contains(parm.key) || m.Tag.Contains(parm.key) || m.Summary.Contains(parm.key))
                .WhereIF(parm.audit == 0, m => m.Audit)
                .WhereIF(parm.audit == 1, m => !m.Audit)
                .WhereIF(parm.types == 1, m => !m.IsRecyc)
                .WhereIF(parm.types == 0, m => m.IsRecyc)
                .WhereIF(!string.IsNullOrEmpty(parm.where), parm.where)
                .OrderBy(m => m.Sort, SqlSugar.OrderByType.Desc)
                .OrderBy(m => m.EditTime, SqlSugar.OrderByType.Desc)
                .ToPage(parm.page, parm.limit);
        }

        /// <summary>
        /// 转移到回收站
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> GoRecycle(string parm, int type)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.Error };
            try
            {
                var list = UtilsHelper.StrToListString(parm);
                var dbres = 0;
                if (type == 0)
                {
                    dbres = await Db.Updateable<AdoArticle>().UpdateColumns(m => m.IsRecyc).Where(m => list.Contains(m.Guid)).ExecuteCommandAsync();
                }
                else
                {
                    dbres = await Db.Updateable<AdoArticle>().UpdateColumns(m => m.IsRecyc).Where(m => list.Contains(m.Guid)).ExecuteCommandAsync();
                }
                if (dbres == 0)
                {
                    res.msg = "删除数据失败~";
                }
                else
                {
                    res.statusCode = (int)ApiEnum.Status;
                }
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        public async Task<ApiResult<string>> UpdateAsync(AdoArticle parm)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.Error };
            try
            {
                await Db.Updateable(parm).IgnoreColumns(m => new { m.LastHitTime, m.AddTime, m.DelTime }).ExecuteCommandAsync();
                res.statusCode = (int)ApiEnum.Status;
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 复制或转移
        /// </summary>
        /// <param name="parm">id集合</param>
        /// <param name="type">1=copy  2=转移</param>
        /// <param name="columnGuid">栏目id</param>
        /// <returns></returns>
        public async Task<ApiResult<string>> GoCopyOrTransfer(string parm, int type, string columnGuid)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.Error };
            try
            {
                var list = UtilsHelper.StrToListString(parm);
                if (type == 1)
                {
                    //复制
                    var articleList = CmsArticleDb.GetList(m => list.Contains(m.Guid));
                    foreach (var item in articleList)
                    {
                        item.Guid = Guid.NewGuid().ToString();
                        item.ColumnGuid = columnGuid;
                    }
                    await Db.Insertable(articleList).ExecuteCommandAsync();
                }
                else
                {
                    //转移
                    await Db.Updateable<AdoArticle>().UpdateColumns(m => m.ColumnGuid == columnGuid).Where(m => list.Contains(m.Guid)).ExecuteCommandAsync();
                }
                res.statusCode = (int)ApiEnum.Status;
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 查询网页案例和新闻
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PageReply<AdoArticle> WebGetList(PageParmRqst parm, List<string> columnList)
        {
            return Db.Queryable<AdoArticle>()
                .WhereIF(parm.id != 0, m => m.ColumnGuid == parm.guid)
                .WhereIF(columnList.Count > 0, m => columnList.Contains(m.ColumnGuid))
                .WhereIF(!string.IsNullOrEmpty(parm.key), m => m.Title.Contains(parm.key) || m.Tag.Contains(parm.key) || m.Summary.Contains(parm.key))
                .WhereIF(parm.audit == 0, m => m.Audit)
                .WhereIF(parm.audit == 1, m => !m.Audit)
                .WhereIF(parm.types == 1, m => !m.IsRecyc)
                .WhereIF(parm.types == 0, m => m.IsRecyc)
                .WhereIF(!string.IsNullOrEmpty(parm.where), parm.where)
                .OrderBy(m => m.Sort, OrderByType.Desc)
                .OrderBy(m => m.EditTime, OrderByType.Desc)
                .ToPage(parm.page, parm.limit);
        }

        public async Task<ApiResult<List<AdoArticle>>> GetRandomListAsync(SearchParmDto articleParmDto)
        {
            var res = new ApiResult<List<AdoArticle>>();
            try
            {
                res.data = await Db.Queryable<AdoArticle>()
                               .Where(m => m.Audit && m.ColumnGuid == articleParmDto.columnGuid && m.Types == articleParmDto.option)
                               .OrderBy(m => SqlFunc.GetRandom())
                               .Take(4).ToListAsync();

            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
                Log4netHelper.Error(typeof(string), ex.Message);
            }
            return res;
        }
    }
}