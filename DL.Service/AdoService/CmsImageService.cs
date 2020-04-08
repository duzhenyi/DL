using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;
using DL.Utils.Extensions;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace DL.Service.AdoService
{
    public class CmsImageService  : BaseService<SysImage>, ICmsImageService
    {
        //public CloudFile GetList(PageParmRqst parm)
        //{
        //    var model = new CloudFile() { Code = 200};
        //    try
        //    {
        //        var query = Db.Queryable<CmsImage>()
        //                .WhereIF(parm.where!="/",m=>m.ImgBig.Contains(parm.where))
        //                .OrderBy(m=>m.AddDate,OrderByType.Desc)
        //                .ToPageAsync(parm.page, parm.limit);
        //        var fileList = new List<ListInfo>();
        //        if (query.Result.TotalItems != 0)
        //        {
        //            foreach (var item in query.Result.Items)
        //            {
        //                fileList.Add(new ListInfo()
        //                {
        //                    Name = item.ImgBig,
        //                    Size = item.ImgSize,
        //                    Type = item.ImgType,
        //                    Time = item.AddDate
        //                });
        //            }
        //        }
        //        model.list = fileList;
        //    }
        //    catch (Exception ex)
        //    {
        //        model.Message = ApiEnum.Error.GetEnumText() + ex.Message;
        //        model.Code = (int)ApiEnum.Error;
        //    }
        //    return model;
        //}
    }
}
