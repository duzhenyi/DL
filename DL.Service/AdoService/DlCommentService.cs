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
    public class AdoCommentService : BaseService<AdoComment>, IAdoCommentService
    {
        public async Task<ApiResult<List<AdoCommentDto>>> GetListAsync(SearchParmDto searchParmDto)
        {
            var res = new ApiResult<List<AdoCommentDto>>();
            res.data = await Db.Queryable<AdoComment, AdoUser>((c, u) => new object[]
                                                   {
                                                              JoinType.Left,c.UserGuid == u.Guid
                                                   })
                      .Where(c => c.Audit == 1 && c.ColumnGuid == searchParmDto.columnGuid && searchParmDto.option == c.Option)
                      .Select((c, u) => new AdoCommentDto
                      {
                          Summary = c.Summary,
                          AddTime = c.AddTime, 
                          HeadPic = u.HeadPic,
                          AddUserName = searchParmDto.option == 0 ? "小魔仙" : u.NickName
                      })
                      .OrderBy(c => c.AddTime, OrderByType.Desc)
                      .ToListAsync();

            return res;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<AdoCommentDto>>> GetPageList(PageParmRqst parm)
        {
            var res = new ApiResult<PageReply<AdoCommentDto>>();
            res.data = await Db.Queryable<AdoComment, AdoUser>((c, u) => new object[]
                                                   {
                                                              JoinType.Left,c.UserGuid == u.Guid
                                                   })
                      .Where(c => c.Audit == 1 && c.ColumnGuid == parm.guid.ToString() && c.Option == parm.option)
                      .Select((c, u) => new AdoCommentDto
                      {
                          Summary = c.Summary,
                          AddTime = c.AddTime,
                          HeadPic = u.HeadPic,
                          AddUserName = u.NickName
                      })
                      .OrderBy(c => c.AddTime, OrderByType.Desc)
                      .ToPageAsync(parm.page, parm.limit);

            return res;

        }
    }
}