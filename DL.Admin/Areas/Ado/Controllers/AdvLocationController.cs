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
    /// 广告位置
    /// </summary>
    [Area("Ado")]
    [Route("Ado/AdvLocation")]
    public class AdvLocationController : BaseController
    {
        private readonly IAdoAdvLocationService _adoAdvLocationService;
        public AdvLocationController(IAdoAdvLocationService adoAdvLocationService)
        {
            _adoAdvLocationService = adoAdvLocationService;
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
            return View(_adoAdvLocationService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoAdvLocationPageParm request)
        {
            var res = await _adoAdvLocationService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "AdvLocation", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddAdvLocation([FromBody]AdoAdvLocation model)
        { 
            return await _adoAdvLocationService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "AdvLocation", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteAdvLocation([FromBody]DelParams delParams)
        {
            return await _adoAdvLocationService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "AdvLocation", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateAdvLocation([FromBody]AdoAdvLocation parm)
        {
            return await _adoAdvLocationService.ModifyAsync(parm);
        }
    }
}