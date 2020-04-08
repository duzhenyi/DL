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
    /// 广告模块
    /// </summary>
    [Area("Ado")]
    [Route("Ado/Adv")]
    public class AdvController : BaseController
    {
        private readonly IAdoAdvService _adoAdvService;
        private readonly IAdoAdvLocationService _adoAdvLocationService;
        public AdvController(IAdoAdvService adoAdvService, IAdoAdvLocationService adoAdvLocationService)
        {
            _adoAdvService = adoAdvService;
            _adoAdvLocationService = adoAdvLocationService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.AdvLocations = _adoAdvLocationService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            return View();
        }

        [HttpGet]
        [Route("Modify")]
        public IActionResult Modify(string id)
        {
            ViewBag.AdvLocations = _adoAdvLocationService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            return View(_adoAdvService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoAdvPageParm request)
        { 
            var res = await _adoAdvService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Adv", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddAdv([FromBody]AdoAdv model)
        { 
            return await _adoAdvService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Adv", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteAdv([FromBody]DelParams delParams)
        {
            return await _adoAdvService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Adv", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateAdv([FromBody]AdoAdv parm)
        {
            return await _adoAdvService.ModifyAsync(parm);
        }
    }
}