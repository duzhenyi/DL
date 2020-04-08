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
    /// <summary>
    /// 组织部门
    /// </summary>
    [Area("Sys")]
    [Route("Sys/Organize")]
    public class OrganizeController : BaseController
    {
        private readonly ISysOrganizeService _sysOrganizeService;
        public OrganizeController(ISysOrganizeService sysOrganizeService)
        {
            _sysOrganizeService = sysOrganizeService;
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
            return View(_sysOrganizeService.GetByIDAsync(id).Result.data);
        }

        /// <summary>
        /// 获得树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTree")]
        public List<TreeDto> GetTree()
        {
            return _sysOrganizeService.GetListTreeAsync().Result.data;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(PageParm request)
        {
            var res = await _sysOrganizeService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Organize", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddOrganize([FromBody]SysOrganize model)
        { 
            return await _sysOrganizeService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Organize", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteOrganize([FromBody]DelParams delParams)
        {
            return await _sysOrganizeService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Organize", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateOrganize([FromBody]SysOrganize parm)
        {
            return await _sysOrganizeService.ModifyAsync(parm);
        }
    }
}