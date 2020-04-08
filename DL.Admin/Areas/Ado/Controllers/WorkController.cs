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
    /// 
    /// </summary>
    [Area("Ado")]
    [Route("Ado/Work")]
    public class WorkController : BaseController
    {
        private readonly IAdoWorkService _adoWorkService;
        public WorkController(IAdoWorkService adoWorkService)
        {
            _adoWorkService = adoWorkService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Modify")]
        public IActionResult Modify(string guid)
        {
            return View(_adoWorkService.GetByIDAsync(guid).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoWorkPageParm request)
        {
            var res = await _adoWorkService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Work", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddWork([FromBody]AdoWork model)
        { 
            return await _adoWorkService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Work", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteWork([FromBody]DelParams delParams)
        {
            return await _adoWorkService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Work", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateWork([FromBody]AdoWork parm)
        {
            return await _adoWorkService.ModifyAsync(parm);
        }
    }
}