using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DL.Admin.Models;
using DL.IService.SysIService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DL.Domain.PublicModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DL.Utils.Security;
using DL.Utils.AppConfig;
using DL.Domain.Models.SysModels;
using DL.Utils.Auth;
using DL.Utils.Cache.MemoryCache;
using DL.Utils.Auth.Jwt;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.IService.AdoIService;

namespace DL.Admin.Controllers
{

    public class HomeController : Controller
    {
        private readonly ISysSiteService _siteService;
        private readonly ISysAdminService _adminService;
        private readonly ISysMenuService _sysMenuService;
        private readonly ISysLogService _sysLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public HomeController(ISysSiteService siteService,
         ISysAdminService adminService,
         ISysMenuService sysMenuService,
         ISysLogService sysLogService,
         IHttpContextAccessor httpContextAccessor)
        {
            _siteService = siteService;
            _adminService = adminService;
            _sysMenuService = sysMenuService;
            _sysLogService = sysLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            //var sysConfigVM = new SysConfigVM()
            //{
            //    Ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()
            //};
            //if (sysConfigVM.Ip.Length < 10)
            //{
            //    sysConfigVM.Ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
            //}

            //ViewBag.SysConfigVM = sysConfigVM;
            //ViewBag.Site = _siteService.GetModelAsync(m => m.ID == "78756a6c-50c8-47a5-b898-5d6d24a20327").Result.data;
            var adminID = User.Identities.First(u => u.IsAuthenticated).FindFirst(ClaimTypes.Sid).Value;
            ViewBag.Admin = _adminService.GetModelAsync(m => m.ID == adminID).Result.data;

            return View(_sysMenuService.GetPagesAsync(new PageParm() { limit = 1000 }).Result.data);

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChildMenu()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
