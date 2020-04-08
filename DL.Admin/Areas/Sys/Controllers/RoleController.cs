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
    [Route("Sys/Role")]
    public class RoleController : BaseController
    {
        private readonly ISysRoleService _roleService;
        public RoleController(ISysRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 角色分配权限
        /// </summary>
        /// <param name="roid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RoleMenu")]
        public IActionResult RoleMenu(string id)
        {
            ViewBag.RoleID = id;
            return View();
        }
        /// <summary>
        /// 查询授权列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ToroleList")]
        public async Task<JsonResult> GetToRolePages(int limit, string deparmentID, string adminID)
        {
            var res = await _roleService.GetPagesToRoleAsync(limit, deparmentID, adminID);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        [HttpGet]
        [Route("Modify")]
        public IActionResult Modify(string id)
        {
            return View(_roleService.GetModelAsync(m => m.ID == id).Result.data);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(PageParm parm)
        {
            var res = await _roleService.GetPagesAsync(parm);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetList")]
        public List<SysRole> GetList()
        {
            return _roleService.GetListAsync().Result.data;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Role", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddRole([FromBody]SysRole parm)
        {
            return await _roleService.AddAsync(parm);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Role", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteRole([FromBody]DelParams obj)
        {
            return await _roleService.DelAsync(obj.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Role", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateRole([FromBody]SysRole model)
        {
            return await _roleService.ModifyAsync(model);
        }
    }
}