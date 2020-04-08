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
using System.Linq;
using System.Threading.Tasks;

namespace DL.Service.AdoService
{
    public class AdoClassTypeService : BaseService<AdoClassType>, IAdoClassTypeService
    {
        public async Task<ApiResult<PageReply<AdoClassType>>> GetPagesAsync(PageParmRqst parm)
        {
            var res = new ApiResult<PageReply<AdoClassType>>();
            try
            {
                res.data = await Db.Queryable<AdoClassType>()
                                   .WhereIF(!string.IsNullOrEmpty(parm.key), m => m.Title.Contains(parm.key))
                                   .OrderBy(m => m.AddTime, OrderByType.Desc)
                                   .ToPageAsync(parm.page, parm.limit);
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
                //LoggerHelper.Default.ProcessError((int)ApiEnum.Error, ex.Message);
            }
            return res;
        }


        /// <summary>
        /// 查询Tree
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<IndustryTree>>> GetListTreeAsync(int industryType = 0, bool isTop = false)
        {
            var list = await Db.Queryable<AdoClassType>()
                .WhereIF(industryType > 0, m => m.IndustryType == industryType).WhereIF(isTop, m => m.IsTop).ToListAsync();

            var treeList = new List<IndustryTree>();
            foreach (var item in list.Where(m => m.Layer == 1).OrderBy(m => m.AddTime))
            {
                //获得子级
                var children = RecursionOrganize(list, new List<IndustryTree>(), item.Guid);
                treeList.Add(new IndustryTree()
                {
                    title = item.Title,
                    value = item.Guid,
                    open = children.Count > 0,
                    children = children.Count == 0 ? null : children
                });
            }
            var res = new ApiResult<List<IndustryTree>>
            {
                statusCode = 200,
                data = treeList
            };
            return res;
        }

        /// <summary>
        /// 递归树
        /// </summary>
        /// <param name="sourceList">原数据</param>
        /// <param name="list">新集合</param>
        /// <param name="guid">父节点</param>
        /// <returns></returns>
        List<IndustryTree> RecursionOrganize(List<AdoClassType> sourceList, List<IndustryTree> list, string guid)
        {
            foreach (var row in sourceList.Where(m => m.ParentGuid == guid).OrderBy(m => m.AddTime))
            {
                var res = RecursionOrganize(sourceList, new List<IndustryTree>(), row.Guid);
                list.Add(new IndustryTree()
                {
                    title = row.Title,
                    value = row.Guid,
                    open = res.Count > 0,
                    children = res.Count > 0 ? res : null
                });
            }
            return list;
        }

        public async Task<ApiResult<string>> AddAsync(AdoClassType parm)
        {
            parm.Guid = Guid.NewGuid().ToString();
            await Db.Insertable(parm).ExecuteCommandAsync();
            if (!string.IsNullOrEmpty(parm.ParentGuid))
            {
                // 说明有父级  根据父级，查询对应的模型
                var model = AdoClassTypeDb.GetById(parm.ParentGuid);
                parm.ParentGuidList = model.ParentGuidList + parm.Guid + ",";
                parm.Layer = model.Layer + 1;
            }
            else
            {
                parm.ParentGuidList = "," + parm.Guid + ",";
            }
            //更新  新的对象
            await Db.Updateable(parm).ExecuteCommandAsync();
            var res = new ApiResult<string>
            {
                statusCode = 200,
                data = "1"
            };
            return res;
        }
    }
}