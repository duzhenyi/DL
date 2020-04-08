using System.Threading.Tasks;
using DL.Admin.Filters;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using DL.Utils.Helper;
using DL.Utils.Security;
using Microsoft.AspNetCore.Mvc;

namespace DL.Admin.Areas.Sys.Controllers
{
    [Area("Sys")]
    [Route("Sys/Log")]
    public class LogController : BaseController
    {

        private readonly ISysLogService _logService;
        public LogController(ISysLogService logService)
        {
            _logService = logService;
        }
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
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
            var res = await _logService.GetPagesAsync(parm);
            return Json(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeFilter(Controller = "Log", Action = "Delete", IsLog = false)]
        [Route("Delete")]
        public async Task<ApiResult<string>> DeleteLog([FromBody]DelParams obj)
        {
            var list = UtilsHelper.StrToListString(obj.ids);
            return await _logService.DeleteAsync(m => list.Contains(m.ID));
        }
    }
}