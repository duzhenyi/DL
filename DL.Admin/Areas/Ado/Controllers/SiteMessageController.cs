using System.Collections.Generic;
using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;
using Microsoft.AspNetCore.Mvc;
using DL.Domain.Dto.AdminDto.AdoModelsDto;
namespace DL.Admin.Areas.Ado.Controllers
{
    /// <summary>
    /// 站内留言
    /// </summary>
    [Area("Ado")]
    [Route("Ado/SiteMessage")]
    public class SiteMessageController : BaseController
    {
        private readonly IAdoSiteMessageService _adoSiteMessageService;
        public SiteMessageController(IAdoSiteMessageService adoSiteMessageService)
        {
            _adoSiteMessageService = adoSiteMessageService;
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
            return View(_adoSiteMessageService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoSiteMessagePageParm request)
        {
            var res = await _adoSiteMessageService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "SiteMessage", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddSiteMessage([FromBody]AdoSiteMessage model)
        { 
            return await _adoSiteMessageService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "SiteMessage", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteSiteMessage([FromBody]DelParams delParams)
        {
            return await _adoSiteMessageService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "SiteMessage", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateSiteMessage([FromBody]AdoSiteMessage parm)
        {
            return await _adoSiteMessageService.ModifyAsync(parm);
        }
    }
}