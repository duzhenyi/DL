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
    /// 高校内容
    /// </summary>
    [Area("Ado")]
    [Route("Ado/SchoolContent")]
    public class SchoolContentController : BaseController
    {
        
        private readonly IAdoSchoolService  _adoSchoolService;
        private readonly ISysColumnService _sysColumnService;
        private readonly IAdoTagService _adoTagService;
        private readonly IAdoSchoolContentService _adoSchoolContentService;
        public SchoolContentController(IAdoSchoolContentService adoSchoolContentService,
            IAdoSchoolService adoSchoolService,
            ISysColumnService sysColumnService,
            IAdoTagService adoTagService)
        {
            _adoSchoolContentService = adoSchoolContentService;
            _adoTagService = adoTagService;
            _sysColumnService = sysColumnService;
            _adoSchoolService = adoSchoolService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.Tags = _adoTagService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            ViewBag.Schools = _adoSchoolService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;

            return View();
        }

        [HttpGet]
        [Route("Modify")]
        public IActionResult Modify(string id)
        {

            ViewBag.SysColums = _sysColumnService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            ViewBag.Tags = _adoTagService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            ViewBag.Schools = _adoSchoolService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            return View(_adoSchoolContentService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoSchoolContentPageParm request)
        {
            var res = await _adoSchoolContentService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "SchoolContent", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddSchoolContent([FromBody]AdoSchoolContent model)
        { 
            return await _adoSchoolContentService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "SchoolContent", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteSchoolContent([FromBody]DelParams delParams)
        {
            return await _adoSchoolContentService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "SchoolContent", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateSchoolContent([FromBody]AdoSchoolContent parm)
        {
            return await _adoSchoolContentService.ModifyAsync(parm);
        }
    }
}