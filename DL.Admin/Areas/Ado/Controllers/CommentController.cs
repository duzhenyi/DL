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
    /// 用户评论
    /// </summary>
    [Area("Ado")]
    [Route("Ado/Comment")]
    public class CommentController : BaseController
    {
        private readonly IAdoUserService  _adoUserService;
        private readonly IAdoCommentService _adoCommentService;
        public CommentController(IAdoCommentService adoCommentService, IAdoUserService adoUserService)
        {
            _adoCommentService = adoCommentService;
            _adoUserService = adoUserService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.Users = _adoUserService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            return View();
        }

        [HttpGet]
        [Route("Modify")]
        public IActionResult Modify(string id)
        {
            ViewBag.Users = _adoUserService.GetListAsync(m => m.IsEnable, m => m.CreateTime, DbOrderEnum.Desc).Result.data;
            return View(_adoCommentService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoCommentPageParm request)
        {
            var res = await _adoCommentService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Comment", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddComment([FromBody]AdoComment model)
        { 
            return await _adoCommentService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Comment", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteComment([FromBody]DelParams delParams)
        {
            return await _adoCommentService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Comment", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateComment([FromBody]AdoComment parm)
        {
            return await _adoCommentService.ModifyAsync(parm);
        }
    }
}