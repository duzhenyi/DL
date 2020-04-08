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
    /// 标签模块
    /// </summary>
    [Area("Ado")]
    [Route("Ado/Tag")]
    public class TagController : BaseController
    {
        private readonly IAdoTagService _adoTagService;
        public TagController(IAdoTagService adoTagService)
        {
            _adoTagService = adoTagService;
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
            return View(_adoTagService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoTagPageParm request)
        {
            var res = await _adoTagService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Tag", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddTag([FromBody]AdoTag model)
        { 
            return await _adoTagService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Tag", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteTag([FromBody]DelParams delParams)
        {
            return await _adoTagService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Tag", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateTag([FromBody]AdoTag parm)
        {
            return await _adoTagService.ModifyAsync(parm);
        }
    }
}