using System.Collections.Generic;
using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;
using Microsoft.AspNetCore.Mvc;
using DL.Domain.Dto.AdminDto.AdoModelsDto;
using System;
using DL.Domain.EnumModels;
using DL.Utils.Extensions;

namespace DL.Admin.Areas.Ado.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area("Ado")]
    [Route("Ado/About")]
    public class AboutController : BaseController
    {
        private readonly IAdoAboutService _adoAboutService;
        public AboutController(IAdoAboutService adoAboutService)
        {
            _adoAboutService = adoAboutService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        { 
            return View();
        }

        [HttpGet]
        [Route("Modify")]
        public IActionResult Modify(string id)
        {
            return View(_adoAboutService.GetByIDAsync(id).Result.data);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoAboutPageParm request)
        {
            var res = await _adoAboutService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "About", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddAbout([FromBody]AdoAbout model)
        {
            return await _adoAboutService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "About", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteAbout([FromBody]DelParams delParams)
        {
            return await _adoAboutService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "About", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateAbout([FromBody]AdoAbout parm)
        {
            return await _adoAboutService.ModifyAsync(parm);
        }
    }
}