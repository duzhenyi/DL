using System.Collections.Generic;
using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;
using Microsoft.AspNetCore.Mvc;
using DL.Domain.Dto.AdminDto.AdoModelsDto;
using DL.IService.SysIService;

namespace DL.Admin.Areas.Ado.Controllers
{
    /// <summary>
    /// 资源模块
    /// </summary>
    [Area("Ado")]
    [Route("Ado/Download")]
    public class DownloadController : BaseController
    {
        private readonly IAdoTagService _adoTagService;
        private readonly ISysColumnService _sysColumnService;
        private readonly IAdoDownloadService _adoDownloadService;
        public DownloadController(IAdoDownloadService adoDownloadService, IAdoTagService adoTagService, ISysColumnService sysColumnService)
        {
            _adoDownloadService = adoDownloadService;
            _adoTagService = adoTagService;
            _sysColumnService = sysColumnService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.Tags = _adoTagService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            return View();
        }

        [HttpGet]
        [Route("Modify")]
        public IActionResult Modify(string id)
        {
            ViewBag.SysColums = _sysColumnService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            ViewBag.Tags = _adoTagService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            return View(_adoDownloadService.GetByIDAsync(id).Result.data);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoDownloadPageParm request)
        {
            var res = await _adoDownloadService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Download", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddDownload([FromBody]AdoDownload model)
        {
            return await _adoDownloadService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Download", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteDownload([FromBody]DelParams delParams)
        {
            return await _adoDownloadService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Download", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateDownload([FromBody]AdoDownload parm)
        {
            return await _adoDownloadService.ModifyAsync(parm);
        }
    }
}