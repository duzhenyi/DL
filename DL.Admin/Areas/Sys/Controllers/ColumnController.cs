using System.Collections.Generic;
using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using Microsoft.AspNetCore.Mvc;
using DL.Domain.Dto.AdminDto.SysModelsDto;
using DL.Domain.Dto.AdminDto.SysDto;

namespace DL.Admin.Areas.Sys.Controllers
{
    /// <summary>
    /// 栏目管理
    /// </summary>
    [Area("Sys")]
    [Route("Sys/Column")]
    public class ColumnController : BaseController
    {
        private readonly ISysColumnService _sysColumnService;
        public ColumnController(ISysColumnService sysColumnService)
        {
            _sysColumnService = sysColumnService;
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
            return View(_sysColumnService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(SysColumnPageParm request)
        {
            var res = await _sysColumnService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 获得树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTree")]
        public List<TreeDto> GetTree()
        {
            return _sysColumnService.GetListTreeAsync().Result.data;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Column", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddColumn([FromBody]SysColumn model)
        { 
            return await _sysColumnService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Column", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteColumn([FromBody]DelParams delParams)
        {
            return await _sysColumnService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Column", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateColumn([FromBody]SysColumn parm)
        {
            return await _sysColumnService.ModifyAsync(parm);
        }
    }
}