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
    [Route("Sys/ImgType")]
    public class ImgTypeController : BaseController
    {
        private readonly ISysImgTypeService _sysImgTypeService;

        public ImgTypeController(ISysImgTypeService sysImgTypeService)
        {
            _sysImgTypeService = sysImgTypeService;
        }

        [HttpGet("Modify")]
        public IActionResult Modify(string id, int types)
        {
            var data = _sysImgTypeService.GetModelAsync(m => m.ID == id).Result.data;
            if (string.IsNullOrEmpty(id))
            {
                data.Types = types;
            }
            return View(data);
        }

        /// <summary>
        /// 添加图片类型
        /// </summary>
        /// <param name="model">SysImgType</param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<ApiResult<string>> Add([FromBody]SysImgType model)
        { 
            return await _sysImgTypeService.AddAsync(model);
        }

        /// <summary>
        /// 删除图片类型
        /// </summary>
        /// <param name="obj">string parm</param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public async Task<ApiResult<string>> Delete([FromBody]DelParams obj)
        {
            return await _sysImgTypeService.DeleteAsync(obj.ids);
        }

        /// <summary>
        /// 修改图片类型
        /// </summary>
        /// <param name="model">SysImgType</param>
        /// <returns></returns>
        [HttpPost("Modify")]
        public async Task<ApiResult<string>> Modify([FromBody]SysImgType model)
        {
            return await _sysImgTypeService.UpdateAsync(model);
        }

        /// <summary>
        /// 获得树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTree")]
        public List<TreeDto> GetTree()
        {
            return _sysImgTypeService.GetListTreeAsync().Result.data;
        }

        /// <summary>
        /// 获得图片类型列表
        /// </summary>
        /// <param name="parm">PageParm parm</param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody]PageParm parm)
        {
            return Ok(await _sysImgTypeService.GetListAsync(m => m.Types == 1, m => m.CreateTime, DbOrderEnum.Asc));
        }
    }
}