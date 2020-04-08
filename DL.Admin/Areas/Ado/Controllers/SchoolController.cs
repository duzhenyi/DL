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
    /// 高校管理
    /// </summary>
    [Area("Ado")]
    [Route("Ado/School")]
    public class SchoolController : BaseController
    {
        private readonly IAdoSchoolService _adoSchoolService;
        public SchoolController(IAdoSchoolService adoSchoolService)
        {
            _adoSchoolService = adoSchoolService;
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
            return View(_adoSchoolService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoSchoolPageParm request)
        {
            var res = await _adoSchoolService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "School", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddSchool([FromBody]AdoSchool model)
        { 
            return await _adoSchoolService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "School", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteSchool([FromBody]DelParams delParams)
        {
            return await _adoSchoolService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "School", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateSchool([FromBody]AdoSchool parm)
        {
            return await _adoSchoolService.ModifyAsync(parm);
        }
    }
}