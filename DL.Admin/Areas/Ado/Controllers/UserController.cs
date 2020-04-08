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
    /// 用户管理
    /// </summary>
    [Area("Ado")]
    [Route("Ado/User")]
    public class UserController : BaseController
    {
        private readonly IAdoUserService _adoUserService;
        public UserController(IAdoUserService adoUserService)
        {
            _adoUserService = adoUserService;
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
            return View(_adoUserService.GetByIDAsync(id).Result.data);
        }
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPages")]
        public async Task<JsonResult> GetPages(AdoUserPageParm request)
        {
            var res = await _adoUserService.GetPagesAsync(request);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "User", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> AddUser([FromBody]AdoUser model)
        { 
            return await _adoUserService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "User", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteUser([FromBody]DelParams delParams)
        {
            return await _adoUserService.DelAsync(delParams.ids);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "User", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> UpdateUser([FromBody]AdoUser parm)
        {
            return await _adoUserService.ModifyAsync(parm);
        }
    }
}