using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DL.Admin.Filters;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using DL.Domain.Dto.AdminDto.SysDto;

namespace DL.Admin.Areas.Sys.Controllers
{
    [Area("Sys")] 
	[Route("Sys/RoleMenu")]
	public class RoleMenuController : BaseController
    {
		private readonly ISysPermissionsService _roleMenu;
		public RoleMenuController(ISysPermissionsService roleMenu)
		{
			_roleMenu = roleMenu;
		}
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
		[Route("List")]
		public async Task<JsonResult> GetPages(string key)
		{
			var res = await _roleMenu.GetListAsync(key);
			return Json(new { code = 0, msg = "success", count = 100, res.data });
		}

		/// <summary>
		/// 角色授权菜单
		/// </summary>
		/// <returns></returns>
		[HttpPost, AuthorizeFilter(Controller = "Role", Action  = "Authorize")]
		[Route("Add")]
		public async Task<ApiResult<string>> AddRoleMenu([FromBody]SysPermissions parm)
		{
			return await _roleMenu.SaveAsync(parm);
		}

		/// <summary>
		/// 角色授权菜单
		/// </summary>
		/// <returns></returns>
		[HttpPost, AuthorizeFilter(Controller = "Role", Action = "Authorize")]
		[Route("AddAuthorization")]
		public ApiResult<string> SaveAuthorization([FromBody]SysMenuAuthorization parm)
		{
			return _roleMenu.SaveAuthorization(parm.list, parm.roleID);
		}

		/// <summary>
		/// 用户授权角色
		/// </summary>
		/// <returns></returns>
		[HttpPost, AuthorizeFilter(Controller = "Admin", Action = "Authorize")]
		[Route("ToRole")]
		public async Task<ApiResult<string>> AdminToRole([FromBody]SysPermissions parm)
		{
			return await _roleMenu.ToRoleAsync(parm, parm.IsEnable);
		}

		/// <summary>
		/// 菜单授权-菜单功能
		/// </summary>
		/// <returns></returns>
		[HttpPost, AuthorizeFilter(Controller = "Role", Action = "Authorize")]
		[Route("TubtnFun")]
		public ApiResult<string> RoleMenuToFun([FromBody]SysPermissionsParm parm)
		{
			return _roleMenu.RoleMenuToFunAsync(parm);
		}
	}
}