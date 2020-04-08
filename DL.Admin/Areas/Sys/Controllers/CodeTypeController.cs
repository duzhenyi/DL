using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("Sys/CodeType")]
    public class CodeTypeController : BaseController
    {
        private readonly ISysCodeTypeService _sysCodeTypeService;

        public CodeTypeController(ISysCodeTypeService sysCodeTypeService)
        {
            _sysCodeTypeService = sysCodeTypeService;
        }

        [HttpGet]
        [Route("Modify")]
        public IActionResult Modify(string codetypeid)
        {
            ViewBag.SysCodeTypeDto = _sysCodeTypeService.GetByIDAsync(codetypeid).Result.data;
            return View(_sysCodeTypeService.GetListAsync().Result.data);
        }

        /// <summary>
        /// 获得字典栏目Tree列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTree")]
        public List<SysCodeTypeTree> GetTree()
        {
            return _sysCodeTypeService.GetListTreeAsync().Result.data;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "CodeType", Action = "Add")]
        [Route("Add")]
        public async Task<ApiResult<string>> Add([FromBody]SysCodeType model)
        {
            return await _sysCodeTypeService.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "CodeType", Action = "Delete")]
        [Route("Delete")]
        public async Task<ApiResult<string>> Delete([FromBody]DelParams obj)
        {
            return await _sysCodeTypeService.DeleteAsync(obj.ids);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "CodeType", Action = "Update")]
        [Route("Update")]
        public async Task<ApiResult<string>> Update([FromBody]SysCodeType model)
        {
            return await _sysCodeTypeService.ModifyAsync(model);
        }

    }
}