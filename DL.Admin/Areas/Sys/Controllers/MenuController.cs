using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Admin.Models;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using DL.Utils.Cache.MemoryCache;
using DL.Utils.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DL.Admin.Areas.Sys.Controllers
{
    [Area("Sys")]
    [Route("Sys/Menu")]
    public class MenuController : BaseController
    {
        private readonly ISysMenuService _sysMenuService;
        private readonly ISysAuthorizeService _authorizeService;
        private readonly ISysCodeService _sysCodeService;
        public MenuController(ISysMenuService sysMenuService, ISysAuthorizeService authorizeService, ISysCodeService sysCodeService)
        {
            _sysMenuService = sysMenuService;
            _authorizeService = authorizeService;
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
            return View(_sysMenuService.GetByIDAsync(id).Result.data);
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Menu", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddMenu([FromBody]SysMenu model)
        {
            return await _sysMenuService.AddAsync(model);
        }

        /// <summary>
        /// 获取树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTree")]
        public List<TreeDto> GetTree()
        {
            return _sysMenuService.GetTreeAsync().Result.data;
        }

        /// <summary>
        /// 查询树表格列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeTableAsync")]
        public async Task<JsonResult> GetTreeTableAsync()
        {
            var res = await _sysMenuService.GetTreeTableAsync();
            return Json(new { code = 0, msg = "success", data = res.data });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Menu", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteMenu([FromBody]DelParams obj)
        {
            var list = UtilsHelper.StrToListString(obj.ids);
            return await _sysMenuService.DeleteAsync(m => list.Contains(m.ID));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Menu", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateMenu([FromBody]SysMenu model)
        {
            return await _sysMenuService.ModifyAsync(model);
        }

        /// <summary>
        /// 修改为置顶桌面
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ToDeskTop")]
        public async Task<ApiResult<string>> ToDeskTop([FromBody]ToDeskTopDto obj)
        {
            return await _sysMenuService.ModifyAsync(obj.ID, obj.isDeskTop);
        }


        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Menu", Action = "GetAuthorizaionMenu")]
        [Route("GetAuthorizaionMenu")]
        public async Task<ApiResult<List<SysMenuDto>>> GetAuthorizaionMenu([FromBody]DelParams obj)
        {
            return await _sysMenuService.GetMenuByRole(obj.ids);
        }

        /// <summary>
        /// 根据菜单，获得当前菜单的所有功能权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCodeByMenu")]
        public JsonResult GetCodeByMenu(string role, string menu = "all")
        {
            var res = _authorizeService.GetCodeByMenu(role, menu);
            return Json(new { code = 0, msg = "success", count = 1, res.data });
        }

        /// <summary>
        /// 获取单个角色授权菜单按钮
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMenuRoleTree")]
        public List<SysMenuRoleTreeDto> GetMenuRoleTree(string roleId)
        {
            return  _sysMenuService.GetMenuRoleTreeAsync(roleId).Result.data;
        }

        /// <summary>
        /// 提供权限查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AuthMenu")]
        public async Task<ApiResult<List<SysMenuDto>>> GetAuthMenuAsync()
        {
            var res = new ApiResult<List<SysMenuDto>>();
            var auth = await HttpContext.AuthenticateAsync();
            var userID = auth.Principal.Identities.First(u => u.IsAuthenticated).FindFirst(ClaimTypes.Sid).Value;

            res.data = MemoryCacheHelper.Get<List<SysMenuDto>>(KeyModel.AdminMenu + "_" + userID);
            if (res.data == null)
            {
                res.statusCode = (int)ApiEnum.URLExpireError;
                res.msg = "Session已过期，请重新登录";
            }
            return res;
        }


    }
}