using DL.Admin.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DL.Admin.Areas.Sys.Controllers
{
    [Area("Sys")] 
	[Route("Sys/Authorization")]
	public class AuthorizationController : BaseController
    {
		[HttpGet]
		[Route("Index")]
		public IActionResult Index()
        {
            return View();
        }
    }
}