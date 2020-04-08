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
    /// 文章管理
    /// </summary>
    [Area("Ado")]
    [Route("Ado/Article")]
    public class ArticleController : BaseController
    {
        private readonly ISysColumnService _sysColumnService;
        private readonly IAdoArticleService _adoArticleService;
        private readonly IAdoTagService _adoTagService;
        public ArticleController(IAdoArticleService adoArticleService, IAdoTagService adoTagService, ISysColumnService sysColumnService)
        {
            _adoArticleService = adoArticleService;
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
            return View(_adoArticleService.GetByIDAsync(id).Result.data);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoArticlePageParm request)
        {
            var res = await _adoArticleService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Article", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddArticle([FromBody]AdoArticle model)
        {
            return await _adoArticleService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Article", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteArticle([FromBody]DelParams delParams)
        {
            return await _adoArticleService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Article", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateArticle([FromBody]AdoArticle parm)
        {
            return await _adoArticleService.ModifyAsync(parm);
        }
    }
}