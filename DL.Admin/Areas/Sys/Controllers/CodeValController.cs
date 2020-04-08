using System.Collections.Generic;
using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using Microsoft.AspNetCore.Mvc;

namespace DL.Admin.Areas.Sys.Controllers
{
    [Area("Sys")]
    [Route("Sys/CodeVal")]
    public class CodeValController : BaseController
    {
        private readonly ISysCodeService _sysCodeService;

        public CodeValController(ISysCodeService sysCodeService)
        {
            _sysCodeService = sysCodeService;
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
            var CodeModel = _sysCodeService.GetByIDAsync(id).Result.data;
            if (string.IsNullOrEmpty(CodeModel.ID))
            {//如果传递的类型ID查询不出数据，则父级ID则为类型ID

                CodeModel.CodeTypeId = id;
            }

            return View(CodeModel);
        }

        /// <summary>
		/// 查询列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(PageParm pageParm)
        {
            var res = await _sysCodeService.GetPagesAsync(pageParm);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "CodeVal", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> Add([FromBody]SysCode model)
        {
            return await _sysCodeService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "CodeVal", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> Delete([FromBody]DelParams obj)
        {
            return await _sysCodeService.DeleteAsync(obj.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "CodeVal", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> Update([FromBody]SysCode model)
        {
            return await _sysCodeService.ModifyAsync(model);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "CodeVal", Action = "Update")]
        [Route("UpdateStatus")]
        public async Task<ApiResult<string>> UpdateStatus([FromBody]SysCode model)
        {
            return await _sysCodeService.ModifyStatusAsync(model);
        }
    }
}