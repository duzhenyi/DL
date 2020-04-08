using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.AdoModels;
using DL.Domain.Models.SysModels;
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
    public class SysColumnService : BaseService<SysColumn>, ISysColumnService
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(SysColumn parm)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.Error };
            try
            {
                parm.Number = UtilsHelper.Number(10);
                //根据模板ID查询相关内容
                var mbModel = await Db.Queryable<AdoTemplate>().SingleAsync(m => m.Id == parm.TempId);
                if (mbModel != null)
                {
                    parm.TempName = mbModel.Title;
                    parm.TempUrl = mbModel.Url;
                }
                //如果描述不写，直接读取内容
                if (!string.IsNullOrEmpty(parm.Summary))
                {
                    parm.Summary = UtilsHelper.CutString(parm.Content, 160);
                }
                //生成排序数字
                var sorts = Db.Queryable<SysColumn>().OrderBy(m => m.Sort, SqlSugar.OrderByType.Desc).Take(1).First();
                if (sorts != null)
                {
                    parm.Sort = sorts.Sort + 1;
                }
                else
                {
                    parm.Sort = 1;
                }
                try
                {
                    var result = Db.Ado.UseTran(async () =>
                    {
                        var addId = Guid.NewGuid().ToString();//Db.Insertable(parm).ExecuteReturnIdentity();
                        if (parm.ParentGuid != null)
                        {
                            //说明有父级  根据父级，查询对应的模型
                            var parModel = Db.Queryable<SysColumn>().Single(m => m.Guid == parm.ParentGuid);
                            if (parModel != null)
                            {
                                parm.ClassList = parModel.ClassList + addId + ",";
                                parm.ClassLayer = parModel.ClassLayer + 1;
                                parm.Guid = addId;
                            }
                        }
                        else
                        {
                            //没有父级
                            parm.ClassList = "," + addId + ",";
                        }
                        await Db.Updateable(parm).ExecuteCommandAsync();
                    });
                    if (result.IsSuccess)
                    {
                        res.statusCode = (int)ApiEnum.Status;
                    }
                    else
                    {
                        res.msg = result.ErrorMessage;
                    }

                }
                catch (Exception ex)
                {
                    res.msg = ex.Message;
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
        public async Task<ApiResult<string>> UpdateAsync(SysColumn parm)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.Error };
            try
            {
                //先查出原来的
                var sourceModel = CmsColumnDb.GetSingle(m => m.Guid == parm.Guid);
                //根据模板ID查询相关内容
                var mbModel = AdoTemplateDb.GetSingle(m => m.Id == parm.TempId);
                if (parm.TempId != sourceModel.TempId && mbModel != null)
                {
                    parm.TempName = mbModel.Title;
                    parm.TempUrl = mbModel.Url;
                }
                else
                {
                    parm.TempName = sourceModel.TempName;
                    parm.TempUrl = sourceModel.TempUrl;
                }
                if (sourceModel.ParentGuid != parm.ParentGuid)
                {
                    //不相等更改等级
                    var parModel = CmsColumnDb.GetSingle(m => m.Guid == parm.ParentGuid);
                    if (parModel != null)
                    {
                        parm.ClassList = parModel.ClassList + parm.Guid + ",";
                        parm.ClassLayer = parModel.ClassLayer + 1;
                    }
                }
                else
                {
                    parm.ClassList = sourceModel.ClassList;
                    parm.ClassLayer = sourceModel.ClassLayer;
                }
                await Db.Updateable(parm).IgnoreColumns(m => new { m.Number, m.Sort, }).ExecuteCommandAsync();
                res.statusCode = (int)ApiEnum.Status;
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="p">父级</param>
        /// <param name="i">当前id</param>
        /// <param name="o">排序方式</param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ColSort(string p, string i, int o)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.Error };
            try
            {
                int a = 0, b = 0, c = 0;
                var list = CmsColumnDb.GetList(m => m.ParentGuid == p).OrderBy(m => m.Sort).ToList();
                if (list.Count > 0)
                {
                    var index = 0;
                    foreach (var item in list)
                    {
                        index++;
                        if (index == 1)
                        {
                            if (item.Guid == i) //判断是否是头如果上升则不做处理
                            {
                                if (o == 1) //下降一位
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    CmsColumnDb.Update(item);
                                    await Db.Updateable(item).ExecuteCommandAsync();
                                    var nitem = list[index];
                                    nitem.Sort = b;
                                    await Db.Updateable(nitem).ExecuteCommandAsync();
                                    break;
                                }
                            }
                        }
                        else if (index == list.Count)
                        {
                            if (item.Guid == i) //最后一条如果下降则不做处理
                            {
                                if (o == 0) //上升一位
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index - 2].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    await Db.Updateable(item).ExecuteCommandAsync();
                                    var nitem = list[index - 2];
                                    nitem.Sort = b;
                                    await Db.Updateable(nitem).ExecuteCommandAsync();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (item.Guid == i) //判断是否是头如果上升则不做处理
                            {
                                if (o == 1) //下降一位
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    await Db.Updateable(item).ExecuteCommandAsync();
                                    var nitem = list[index];
                                    nitem.Sort = b;
                                    await Db.Updateable(nitem).ExecuteCommandAsync();
                                    break;
                                }
                                else
                                {
                                    a = Convert.ToInt32(item.Sort);
                                    b = Convert.ToInt32(list[index - 2].Sort);
                                    c = a;
                                    a = b;
                                    b = c;
                                    item.Sort = a;
                                    await Db.Updateable(item).ExecuteCommandAsync();
                                    var nitem = list[index - 2];
                                    nitem.Sort = b;
                                    await Db.Updateable(nitem).ExecuteCommandAsync();
                                    break;
                                }
                            }
                        }
                    }
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
        /// 反向递归模块集合，可重复模块数据，最后去重
        /// </summary>
        /// <param name="prevModule">总模块</param>
        /// <param name="retmodule">返回模块</param>
        /// <param name="parentId">上级ID</param>
        private void RecursiveModule(List<SysColumn> prevModule, List<SysColumn> retmodule, string parentId)
        {
            var result = prevModule.Where(p => p.Guid == parentId);
            foreach (var item in result)
            {
                retmodule.Add(item);
                RecursiveModule(prevModule, retmodule, item.ParentGuid);
            }
        }

        /// <summary>
        /// 递归模块列表，返回按级别排序
        /// </summary>
	    public List<SysColumn> RecursiveModule(List<SysColumn> list)
        {
            List<SysColumn> result = new List<SysColumn>();
            if (list != null && list.Count > 0)
            {
                ChildModule(list, result, null);
            }
            return result;
        }
        /// <summary>
        /// 递归模块列表
        /// </summary>
        private void ChildModule(List<SysColumn> list, List<SysColumn> newlist, string parentId)
        {
            var result = list.Where(p => p.ParentGuid == parentId).OrderBy(p => p.ClassLayer).ThenBy(p => p.Sort).ToList();
            if (result.Any())
            {
                for (int i = 0; i < result.Count(); i++)
                {
                    newlist.Add(result[i]);
                    ChildModule(list, newlist, result[i].Guid);
                }
            }
        }

        /// <summary>
        /// 栏目Tree
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<ColumnTree>>> TreeAsync(int type = 1)
        {
            var res = new ApiResult<List<ColumnTree>>() { statusCode = (int)ApiEnum.Error };
            try
            {
                var list = await Db.Queryable<SysColumn>()
                    .Where(m => m.TypeID == type)
                    .Select(m => new ColumnTree()
                    {
                        Id = m.Guid,
                        ColumnId = m.ParentGuid,
                        Name = m.Title,
                        Href = m.TempUrl,
                        TempId = m.TempId,
                        Sort = m.ClassLayer
                    }).ToListAsync();

                var resList = new List<ColumnTree>();
                foreach (var item in list.Where(m => m.ColumnId == null).OrderBy(m => m.Sort))
                {
                    resList.Add(new ColumnTree()
                    {
                        Id = item.Id,
                        ColumnId = item.ColumnId,
                        Name = item.Name,
                        Href = item.Href + item.Id,
                        TempId = item.TempId,
                        Sort = item.Sort,
                        children = RecursiveModule(list, item)
                    });
                }
                res.data = resList;
                res.statusCode = (int)ApiEnum.Status;
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 递归模块列表，返回按级别排序
        /// </summary>
	    public List<ColumnTree> RecursiveModule(List<ColumnTree> list, ColumnTree model)
        {
            var nodeList = new List<ColumnTree>();
            var children = list.Where(t => t.ColumnId == model.Id).OrderBy(m => m.Sort);
            if (children.Any())
            {
                foreach (var item in children)
                {
                    nodeList.Add(new ColumnTree()
                    {
                        Id = item.Id,
                        ColumnId = item.ColumnId,
                        Name = item.Name,
                        Href = item.Href + item.Id,
                        TempId = item.TempId,
                        Sort = item.Sort,
                        children = RecursiveModule(list, item)
                    });
                }
            }
            return nodeList;
        }

        /// <summary>
        /// 模型去重，非常重要
        /// </summary>
        public class ModuleDistinct : IEqualityComparer<SysColumn>
        {
            public bool Equals(SysColumn x, SysColumn y)
            {
                return x.Guid == y.Guid;
            }

            public int GetHashCode(SysColumn obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}