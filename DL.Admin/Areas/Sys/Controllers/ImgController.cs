using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using DL.Utils.Files;
using DL.Utils.Helper;
using DL.Utils.Log.Nlog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DL.Admin.Areas.Sys.Controllers
{
    [Area("Sys")]
    [Route("Sys/Img")]
    public class ImgController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ISysImgTypeService _sysImgTypeService;
        private readonly ISysImgService _sysImgService;
        public ImgController(ISysImgService sysImgService, ISysImgTypeService sysImgTypeService, IWebHostEnvironment environment)
        {
            _sysImgService = sysImgService;
            _sysImgTypeService = sysImgTypeService;
            _environment = environment;

        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }


        #region 本地图片

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Admin", Action = "UploadImg")]
        [Route("UploadImg")]
        public ApiResult<string> UploadImg([FromForm]IFormFile file, string typeId, int types)
        {
            var res = new ApiResult<string>();
            try
            {
                if (string.IsNullOrEmpty(typeId) || typeId == "undefined")
                {
                    res.msg = "请选择分类";
                    return res;
                }

                if (null == file)
                {
                    res.msg = "文件不可为空!";
                    NLogHelper.Error("Admin-UploadImg 文件不可为空");
                    return res;
                }
                if (file.FileName != null)
                {
                    //原文件名
                    var filename = file.FileName;
                    //扩展名
                    var fileExt = FileHelper.GetFileExt(filename);
                    //判断是否包含盘符： 文件名不允许包含冒号，如果存在，则使用新的文件名字
                    if (filename.Contains(":"))
                    {
                        filename = Guid.NewGuid() + "." + fileExt;
                    }

                    var sysType = _sysImgTypeService.GetModelAsync(m => m.ID == typeId);
                    //根据类型创建文件夹
                    var npath = _environment.WebRootPath + "/upload/" + sysType.Result.data.EnName + "/";
                    //检查物理路径是否存在 不存在则创建
                    FileCoreHelper.CreateFiles(npath);

                    if (FileCoreHelper.ExistFileInfo(npath, filename))
                    {
                        res.msg = "文件名称已存在!";
                        NLogHelper.Error("Admin-UploadImg 文件名称已存在");
                        return res;
                    }

                    res.data = "/upload/" + sysType.Result.data.EnName + "/" + filename;
                    //图片保存到数据库里面
                    var model = _sysImgService.GetModelAsync(m => m.ImgBig == res.data).Result.data;
                    if (string.IsNullOrEmpty(model.ID))
                    {
                        using (var stream = new FileStream(npath + filename, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            stream.Flush();
                        }
                        var dbRes = _sysImgService.AddAsync(new SysImage()
                        {
                            ID = Guid.NewGuid().ToString(),
                            ImgSize = file.Length,
                            ImgType = fileExt,
                            ImgBig = res.data,
                            CreateTime = DateTime.Now,
                            IsEnable = true,
                            IsCover = false,
                            Creator = "",
                            SysImgTypeId = typeId,
                            Types = types
                        });
                        if (dbRes.Result.statusCode == 200)
                        {
                            res.msg = "上传成功";
                            return res;
                        }
                    }
                    else
                    {
                        res.msg = "该图片已存在";
                    }
                }
                res.msg = "上传失败";
            }
            catch (Exception ex)
            {
                res.msg = "文件上传异常!";
                NLogHelper.Error("Admin-UploadImg" + ex.Message);
            }

            return res;
        }

        /// <summary>
        /// 获得图片列表
        /// </summary>
        /// <param name="parm">分页参数</param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ApiResult<PageReply<SysImage>>> GetList([FromBody]SysImgPageParmDto parm)
        {
            return await _sysImgService.GetList(parm);
        }

        /// <summary>
        /// 删除本地图片列表
        /// </summary>
        /// <param name="obj">文件名称</param>
        /// <returns></returns>
        [HttpPost("Delete"), AuthorizeFilter(Controller = "Img", Action = "Delete")]
        public async Task<ApiResult<string>> Delete([FromBody]DelParams delParams)
        {
            var model = _sysImgService.GetModelAsync(m => m.ID == delParams.ids);
            if (model != null)
            {
                var res = await _sysImgService.DeleteAsync(m => m.ID == delParams.ids);
                if (res.statusCode == 200)
                {//删除实际存在路径的图片
                    FileHelper.DeleteFile(model.Result.data.ImgBig);
                }
                return res;
            }
            else
            {
                return new ApiResult<string>() { statusCode = 401, msg = "图片不存在" };
            }
           
        }

        #endregion

        #region 云端图片
        ///// <summary>
        ///// 获得云端图片列表
        ///// </summary>
        ///// <param name="parm"></param>
        ///// <returns></returns>
        //[HttpPost("token")]
        //public CloudFileDto GetToken()
        //{
        //    return QiniuCloud.GetToken();
        //}

        ///// <summary>
        ///// 获得云端图片列表
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //[HttpPost("list")]
        //public CloudFileDto FileList([FromBody]QiniuListParmDto obj)
        //{
        //    return QiniuCloud.List(obj.prefix, obj.marker);
        //}

        ///// <summary>
        ///// 删除云端图片列表
        ///// </summary>
        ///// <param name="obj">文件名称</param>
        ///// <returns></returns>
        //[HttpPost("delete")]
        //public CloudFileDto DeleteList([FromBody]QiniuDelParmDto obj)
        //{
        //    return QiniuCloud.Delete(obj.filename);
        //}

        ///// <summary>
        ///// 删除云端图片列表
        ///// </summary>
        ///// <param name="obj">文件名称</param>
        ///// <returns></returns>
        //[HttpPost("upload")]
        //public IActionResult UpLoadFile([FromBody]QiniuDelByPathParmDto obj)
        //{
        //    return Ok(QiniuCloud.UploadFile(obj.prefix, obj.filepath));
        //}


        #endregion
    }
}