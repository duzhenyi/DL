using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using DL.Utils.Helper;
using DL.Utils.Log.Nlog;
using DL.Utils.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DL.Admin.Areas.Sys.Controllers
{
    [Area("Sys")]
    [Route("Sys/Admin")]
    public class AdminController : BaseController
    {
        private readonly ISysAdminService _sysAdminService;
        private readonly ISysRoleService _sysRoleService;
        private readonly IWebHostEnvironment _environment;
        public AdminController(ISysAdminService sysAdminService, ISysRoleService sysRoleService, IWebHostEnvironment environment)
        {
            _sysAdminService = sysAdminService;
            _sysRoleService = sysRoleService;
            _environment = environment;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("AdminModify")]
        public IActionResult AdminModify(string id)
        {
            ViewBag.Roles = _sysRoleService.GetListAsync(m => m.IsEnable, m => m.Sort, DbOrderEnum.Desc).Result.data;
            var adminModel = _sysAdminService.GetModelAsync(id).Result.data;
            //密码解密
            if (adminModel != null)
            {
                adminModel.Pwd = DES3Encrypt.DecryptString(adminModel.Pwd);
            }
            return View(adminModel);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(PageParm parm, string organizeId)
        {
            var res = await _sysAdminService.GetPagesAsync(parm, organizeId);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost, AuthorizeFilter(Controller = "Admin", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddAdmin([FromBody]SysAdmin parm)
        {
            return await _sysAdminService.AddAsync(parm);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Admin", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteAdmin([FromBody]DelParams obj)
        {
            return await _sysAdminService.DeleteAsync(obj.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Admin", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateAdmin([FromBody]SysAdmin parm)
        {
            return await _sysAdminService.ModifyAsync(parm);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Admin", Action = "UploadImg")]
        [Route("UploadImg2")]
        public ApiResult<string> UploadImg2([FromForm]IFormFile file)
        {
            var res = new ApiResult<string>();
            try
            {
                if (null == file)
                {
                    res.msg = "文件不可为空!";
                    NLogHelper.Error("Admin-UploadImg 文件不可为空");
                    return res;
                }
                if (file.FileName != null)
                {
                    var path = _environment.WebRootPath + "/upload/images/headpic/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (ImageHelper.SaveImage(file, path + file.FileName))
                    {
                        res.msg = "上传成功";
                        res.data = file.FileName;
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
        /// 上传头像
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Admin", Action = "UploadImg")]
        [Route("UploadImg")]
        public async Task<ApiResult<string>> UploadImg([FromForm]IFormFile file)
        {
            var res = new ApiResult<string>();
            try
            {
                if (null == file)
                {
                    res.msg = "文件不可为空!";
                    NLogHelper.Error("Admin-UploadImg 文件不可为空");
                    return res;
                }

                if (file.Length > 0)
                {
                    var name = Path.GetFileName(file.FileName);
                    if (name != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await file.CopyToAsync(stream);

                            // Add watermark
                            var watermarkedStream = new MemoryStream();

                            using (var img = Image.FromStream(stream))
                            {
                                if (IsIndexedPixelFormat(img.PixelFormat))
                                {//如果原图片是索引像素格式之列的，则需要转换

                                    Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                                    using (Graphics g = Graphics.FromImage(bmp))
                                    {
                                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                                        g.DrawImage(img, 0, 0);
                                    }
                                    DrawImgString(watermarkedStream, null, bmp);
                                }
                                else
                                {
                                    DrawImgString(watermarkedStream, img, null);
                                }


                                var path = _environment.WebRootPath + "/upload/images/headpic/";
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                img.Save(path + name);

                            }
                            res.msg = "上传成功";
                            res.data = name;
                            return res;
                        }
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
        /// 添加水印
        /// </summary>
        private void DrawImgString(MemoryStream watermarkedStream, Image imagae, Bitmap bitmap)
        {
            Image img;
            if (imagae != null)
            {
                img = imagae;
            }
            else
            {
                img = bitmap;
            }

            //水印操作
            using (var graphic = Graphics.FromImage(img))
            {
                var font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
                var color = Color.FromArgb(128, 255, 255, 255);
                var brush = new SolidBrush(color);
                var point = new Point(img.Width - 150, img.Height - 30);

                graphic.DrawString("www.3sha.com", font, brush, point);
                img.Save(watermarkedStream, ImageFormat.Png);
            }
        }
        /// <summary>
        /// 判断图片是否索引像素格式,是否是引发异常的像素格式
        /// </summary>
        /// <param name="imagePixelFormat">图片的像素格式</param>
        /// <returns></returns>
        private bool IsIndexedPixelFormat(PixelFormat imagePixelFormat)
        {
            PixelFormat[] pixelFormatArray = {
                                            PixelFormat.Format1bppIndexed,
                                            PixelFormat.Format4bppIndexed,
                                            PixelFormat.Format8bppIndexed,
                                            PixelFormat.Undefined,
                                            PixelFormat.DontCare,
                                            PixelFormat.Format16bppArgb1555,
                                            PixelFormat.Format16bppGrayScale
                                        };
            foreach (PixelFormat pf in pixelFormatArray)
            {
                if (imagePixelFormat == pf)
                {
                    return true;
                }
            }
            return false;
        }
    }
}