using System.Collections.Generic;
using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;
using Microsoft.AspNetCore.Mvc;
using DL.Domain.Dto.AdminDto.AdoModelsDto;
using DL.Utils.Log.Nlog;

namespace DL.Admin.Areas.Ado.Controllers
{
    /// <summary>
    /// 友情链接
    /// </summary>
    [Area("Ado")]
    [Route("Ado/Link")]
    public class LinkController : BaseController
    {
        private readonly IAdoLinkService _adoLinkService;
        public LinkController(IAdoLinkService adoLinkService)
        {
            _adoLinkService = adoLinkService;
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
            return View(_adoLinkService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoLinkPageParm request)
        {
            var res = await _adoLinkService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Link", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddLink([FromBody]AdoLink model)
        { 
            return await _adoLinkService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Link", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteLink([FromBody]DelParams delParams)
        {
            return await _adoLinkService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Link", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateLink([FromBody]AdoLink parm)
        {
            return await _adoLinkService.ModifyAsync(parm);
        } 
    }
}